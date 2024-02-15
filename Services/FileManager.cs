using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using backend.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyModel.Resolution;
using OfficeOpenXml;
namespace backend.Services;
public class FileManager
{
  private readonly IFormFile file;
  private readonly Database database;
  private readonly Stream stream;
  public FileManager(Database database, IFormFile file)
  {
    this.database = database;
    this.file = file;
    this.stream = this.file.OpenReadStream();
  }
  private StreamReader Sanitizacao()
  {
    var reader = new StreamReader(stream);
    var memory = new MemoryStream();
    var writer = new StreamWriter(memory);
    int character = 0;
    bool insideField = false;
    while ((character = reader.Read()) != -1)
    {
      var letra = (char)character;
      if (letra == '"') insideField = !insideField; // inverte a variável
      if (letra == '\n' && insideField == true)
      {
        writer.Write(' ');
        continue;
      }
      if (letra == ',' && insideField == true)
      {
        writer.Write('.');
        continue;
      }
      if (letra == '–' && insideField == true)
      {
        writer.Write('-');
        continue;
      }
      writer.Write(letra);
    }
    writer.Flush();
    memory.Position = 0;
    using (var fileStream = new FileStream($"./AppData/relatorios_ofs/{file.FileName}", FileMode.Create, FileAccess.Write))
    {
      memory.CopyTo(fileStream);
    }
    memory.Position = 0;
    return new StreamReader(memory, System.Text.Encoding.UTF8);
  }
  public List<Servico> Relatorio()
  {
    string ext = Path.GetExtension(file.FileName);
    if (ext != ".csv") throw new InvalidOperationException("Tipo de arquivo inválido!");
    using var streamsanitizado = Sanitizacao();
    var servicos = new List<Servico>();
    {
      while (!streamsanitizado.EndOfStream)
      {
        var line = streamsanitizado.ReadLine();
        if(line == null) continue;
        line = line.Replace("\"", "");
        var values = line.Split(',');
        if(values[0] == "Recurso") continue;
        TipoStatus status;
        switch(values[3]) {
          case "não concluído": status = TipoStatus.rejeitado; break;
          case "concluído": status = TipoStatus.concluido; break;
          case "em rota": status = TipoStatus.deslocando; break;
          default: status = Enum.Parse<TipoStatus>(values[3]); break;
        }
        TipoInstalacao? fases;
        switch(values[64]) {
          case "Monofásico": fases = TipoInstalacao.Monofasico; break;
          case "Bifásico": fases = TipoInstalacao.Bifasico; break;
          case "Trifásico": fases = TipoInstalacao.Trifasico; break;
          default: fases = null; break;
        }
        if(values[59].Split(' ')[0] == "20.5") status = TipoStatus.concluido;
        var codigos = (values[44] != String.Empty) ? values[44] : values[59].Split(' ')[0];
        DateTime? vencimento = null;
        var re = new Regex(@"(?<dia>\d{2})/(?<mes>\d{2})/(?<ano>\d{2}) (?<hor>\d{2}):(?<min>\d{2})");
        if(re.IsMatch(values[16])) {
          var ma = re.Match(values[16]);
          var ano = Int32.Parse(ma.Groups["ano"].Value) + 2_000;
          var mes = Int32.Parse(ma.Groups["mes"].Value);
          var dia = Int32.Parse(ma.Groups["dia"].Value);
          var hor = Int32.Parse(ma.Groups["hor"].Value);
          var min = Int32.Parse(ma.Groups["min"].Value);
          vencimento = new DateTime(year: ano, month: mes, day: dia, hour: hor, minute: min, second: 0, kind: DateTimeKind.Utc);
        }
        var abreviado = abreviacao(values[0]);
        var id = (abreviado == String.Empty) ? String.Empty : String.Concat(DateTime.Parse(values[1]).ToString("yyyyMMdd"), abreviado);
        servicos.Add(new Servico {
          filename = file.FileName,
          recurso = values[0],
          dia = DateOnly.Parse(values[1]),
          serial = Int32.Parse(values[2]),
          status = status, // values[3] foi préviamente verificado e assinalado a variavel
          nome_do_cliente = values[4],
          endereco_destino = values[5],
          cidade_destino = values[6],
          // values[7] = "Estado" é desnecessário
          codigo_postal = values[8],
          // values[9] = telefone é sempre vazio
          // values[10] = celular é sempre vazio
          // values[11] = email é sempre vazio
          hora_inicio = TimeOnly.TryParse(values[12], out TimeOnly inicio) ? inicio : null,
          hora_final = TimeOnly.TryParse(values[13], out TimeOnly final) ? final : null,
          // values[14] = "inicio-fim" é desnecessário
          // values[15] = "inicio SLA" é sempre o mesmo
          vencimento = vencimento, // values[16] foi préviamente verificado e assinalado a variavel
          duracao_feito = TimeSpan.TryParse(values[17], out TimeSpan duracao) ? duracao : null,
          desloca_feito = TimeSpan.TryParse(values[18], out TimeSpan desloca) ? desloca : null,
          // values[19] = "tipo atividade" é sempre o mesmo
          tipo_atividade = values[20],
          servico = Int32.TryParse(values[21], out Int32 nota) ? nota : null,
          area_trabalho = values[24],
          // values[22..43] "multiplos" é desnecessário
          codigos = codigos, // values[44, 59] foi préviamente verificado e assinalado a variavel
          id_viatura = values[45],
          id_motorista = Int32.TryParse(values[46], out Int32 id_mot) ? id_mot : null,
          id_ajudante = Int32.TryParse(values[47], out Int32 id_aux) ? id_aux : null,
          id_tecnico = Int32.TryParse(values[48], out Int32 id_tec) ? id_tec : null,
          observacao = values[49],
          // values[50] "descrição para dano" é desnecessária
          bairro_destino = values[51],
          // values[52..54] "multiplos" é desnecessário
          instalacao = Int32.TryParse(values[55], out Int32 inst) ? inst : null,
          complemento_destino = values[56],
          referencia_destino = values[57],
          // values[58] "intervalo de tempo" é desnecessário
          // values[59] "motivo rejeicao" já foi combinado com values[44] para gerar a variável "codigos"
          tipo_servico = values[60],
          // values[61] = "reserva de atividade" é desnecessário
          debitos_cliente = Double.TryParse(values[62], out Double deb) ? deb : null,
          // values[63..68] = "multiplos" é desnecessário
          tipo_instalacao = fases,
          desloca_estima = TimeSpan.TryParse("00:" + values[69], out TimeSpan desloca_est) ? desloca_est : null,
          duracao_estima = TimeSpan.TryParse("00:" + values[70], out TimeSpan duracao_est) ? duracao_est : null,
          // values[71..75] = "multiplos" é desnecessário
          identificador = id,
          abreviacao = abreviado
        });
      }
    }
    return servicos;
  }
  public List<Composicao> Composicao()
  {
    var composicoes = new List<Composicao>();
    String? temp, test;
    Int32 coluna;
    if (Path.GetExtension(file.FileName) != ".xlsx")
      throw new InvalidOperationException("Tipo de arquivo inválido!");
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using (var reader = new ExcelPackage(stream))
    {
      if(reader.Workbook.Worksheets.Count > 1)
        throw new InvalidOperationException("O arquivo enviado tem mais de uma planilha!");
      var worksheet = reader.Workbook.Worksheets[0];
      var colCount = worksheet.Dimension.Columns;
      var primeira_linha = new String[20];
      primeira_linha[0] = colCount.ToString();
      for(int col = 1; col <= colCount; col++)
      {
        temp = worksheet.GetValue<String>(1, col);
        if (temp == null || temp == String.Empty) continue;
        primeira_linha[col] = temp.ToLower();
      }
      var cabecalho = Cabecalho(primeira_linha);
      var rowCount = worksheet.Dimension.Rows;
      for(int row = 2; row < rowCount; row++)
      {
        var composicao = new Composicao();

        temp = worksheet.GetValue<string>(row, 1);
        test = this.is_valid(temp, row, "Dia", ExpectedType.Date);
        if (test == null) composicao.dia = DateOnly.FromDateTime(DateTime.Parse(temp));
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 2);
        test = this.is_valid(temp, row, "Adesivo", ExpectedType.Number);
        if (test == null) composicao.adesivo = Int32.Parse(temp);
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 3);
        test = this.is_valid(temp, row, "Placa", ExpectedType.Text);
        if (test == null) composicao.placa = temp.Replace("-", "").Replace(" ", "");
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 4);
        test = this.is_valid(temp, row, "Recurso", ExpectedType.Text);
        var re = new System.Text.RegularExpressions.Regex("^([A-Z]{4,})(?: - [A-z]{3,})?( [-|–] Equipe )([0-9]{3})$");
        if(test == null)
        {
          if(!re.IsMatch(temp.Trim())) composicao.validacao.Add("O recurso digitado é inválido!");
          composicao.recurso = temp.Trim();
        }
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 5);
        test = this.is_valid(temp, row, "Atividade", ExpectedType.Enum);
        if(test == null)
          composicao.atividade = Enum.Parse<Atividade>(temp
            .Replace("RELIGA POSTO", "AVANCADO")
            .Replace("RELIGA CAMINHÃO", "CAMINHAO")
            .Replace("EMERGÊNCIA", "EMERGENCIA")
            .Replace("CORTE EXTRA", "CORTE")
            .Replace("ESTOQUE DE CORTADOS", "ESTOQUE"));
        else composicao.validacao.Add(test);

        composicao.tipo_viatura = (composicao.atividade == Atividade.CAMINHAO) ? TipoViatura.PESADO : TipoViatura.LEVE;

        var func = new Funcionario();

        temp = worksheet.GetValue<string>(row, 6);
        test = this.is_valid(temp, row, "Motorista", ExpectedType.Text);
        if(test == null)
        {
          composicao.motorista = temp.Trim();
          func.nome_colaborador = temp.Trim();
        }
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 7);
        test = this.is_valid(temp, row, "Mat. Mot.", ExpectedType.Number);
        if(test == null)
        {
          composicao.id_motorista = Int32.Parse(temp);
          func.matricula = Int32.Parse(temp);
        }
        else composicao.validacao.Add(test);

        func.funcao = TipoFuncionario.ELETRICISTA;
        if(!this.if_exist(func)) composicao.validacao.Add($"{func.matricula}: {func.nome_colaborador} não foi encontrado na lista ou nome não corresponde a matrícula!");

        func = new Funcionario();

        temp = worksheet.GetValue<string>(row, 8);
        test = this.is_valid(temp, row, "Ajudante", ExpectedType.Text);
        if (test == null)
        {
          composicao.ajudante = temp.Trim();
          func.nome_colaborador = temp.Trim();
        }
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 9);
        test = this.is_valid(temp, row, "Mat. Aju.", ExpectedType.Number);
        if(test == null)
        {
          composicao.id_ajudante = Int32.Parse(temp);
          func.matricula = Int32.Parse(temp);
        }
        else composicao.validacao.Add(test);

        func.funcao = TipoFuncionario.ELETRICISTA;
        if (!this.if_exist(func)) composicao.validacao.Add($"{func.matricula}: {func.nome_colaborador} não foi encontrado na lista ou nome não corresponde a matrícula!");

        temp = worksheet.GetValue<string>(row, 10);
        test = this.is_valid(temp, row, "Telefone", ExpectedType.Number);
        if(test == null)
        {
          temp = temp.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
          if(temp.Length == 9) temp = "21" + temp;
          if(temp.Length != 11) composicao.validacao.Add("A quantidade de dígitos do telefone está errada!");
          else composicao.telefone = Int64.Parse(temp);
        }
        else composicao.validacao.Add("O número de telefone não é válido!");

        func = new Funcionario();

        temp = worksheet.GetValue<string>(row, 11);
        test = this.is_valid(temp, row, "Mat. Sup.", ExpectedType.Number);
        if(test == null)
        {
          composicao.id_supervisor = Int32.Parse(temp);
          func.matricula = Int32.Parse(temp);
        }
        else composicao.validacao.Add(test);

        temp = worksheet.GetValue<string>(row, 12);
        test = this.is_valid(temp, row, "Supervisor", ExpectedType.Text);
        if(test == null)
        {
          composicao.supervisor = temp.Trim();
          func.nome_colaborador = temp.Trim();
        }
        else composicao.validacao.Add(test);

        func.funcao = TipoFuncionario.SUPERVISOR;
        if(!this.if_exist(func)) composicao.validacao.Add($"{func.matricula}: {func.nome_colaborador} não foi encontrado na lista ou nome não corresponde a matrícula!");

        temp = worksheet.GetValue<string>(row, 13);
        test = this.is_valid(temp, row, "Atividade", ExpectedType.Enum);
        if(test == null)
          composicao.regional = Enum.Parse<Regional>(temp
            .Replace("CAMPO GRANDE", "OESTE")
            .Replace("JACAREPAGUA", "OESTE"));
        else composicao.validacao.Add(test);

        if((composicao.dia != DateOnly.MinValue) && (composicao.recurso != null))
        {
          composicao.abreviacao = abreviacao(composicao.recurso);
          composicao.identificador = composicao.dia.ToString("yyyyMMdd") + composicao.abreviacao;
        }

        composicoes.Add(composicao);
      }
    }
    return composicoes;
  }
  private string? is_valid(string? arg, int linha, string campo, ExpectedType expectedType)
  {
    if(arg == null) return $"O campo {campo} não foi preenchido!";
    arg = arg.Trim();
    switch(expectedType)
    {
      case ExpectedType.Date:
        if(!DateTime.TryParse(arg, out DateTime diaTexto)) return "A data não pode ser reconhecida!";
      break;
      case ExpectedType.Time:
        if(!TimeOnly.TryParse(arg, out TimeOnly hrs)) return "A hora não pode ser reconhecida!";
      break;
      case ExpectedType.Number:
        arg = arg.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
        if(!Int64.TryParse(arg, out Int64 num)) return "A número contém caracteres inválidos!";
      break;
      case ExpectedType.Text:
        if(arg.Length < 5) return "O texto está incompleto ou vazio!";
      break;
      case ExpectedType.Enum:
        String[] enums = {"BAIXADA", "CAMPO GRANDE", "JACAREPAGUA", "CORTE", "RELIGA", "RELIGA POSTO", "RELIGA CAMINHÃO", "EMERGÊNCIA", "ESTOQUE DE CORTADOS", "CORTE EXTRA"};
        if(!enums.Contains(arg)) return "O texto encontrado não corresponde com o padrão!";
      break;
      case ExpectedType.Placa:
        var re = new System.Text.RegularExpressions.Regex("^[0-9A-Z]{3}-[0-9A-Z]{4}$");
        if (!re.IsMatch(arg)) return "O padrão da placa não está sendo obedecido! (I2E-AAAA)";
      break;
    }
    return null;
  }
  private bool if_exist(Funcionario funcionario)
  {
    var func = this.database.funcionario.Find(funcionario.matricula);
    if(func == null) return false;
    if(!(func.nome_colaborador.ToLower() == funcionario.nome_colaborador.ToLower())) return false;
    if(!(func.funcao == funcionario.funcao)) return false;
    return true;
  }
  private Int32 if_exist(String funcionario, TipoFuncionario tipoFuncionario)
  {
    var func = this.database.funcionario.Where(o => o.nome_colaborador.ToLower() == funcionario.ToLower()).SingleOrDefault();
    if(func == null) return 0;
    if(!(func.funcao == tipoFuncionario)) return 0;
    return func.matricula;
  }
  private enum ExpectedType {Text, Number, Date, Time, Enum, Placa}
  private String abreviacao(String recurso)
  {
    var re1 = new System.Text.RegularExpressions.Regex("^([A-Z]{3,})");
    var re2 = new System.Text.RegularExpressions.Regex("([A-Z][a-z]{1,})");
    var re3 = new System.Text.RegularExpressions.Regex("([0-9]{3})");
    if(!re3.IsMatch(recurso)) return String.Empty;
    var abreviado = "";
    recurso = recurso.Replace('–', '-');
    abreviado += re1.Match(recurso).Value;
    if(re2.Match(recurso).Value == "Religa") abreviado += 'R';
    if(re2.Match(recurso).Value == "Corte") abreviado += 'C';
    abreviado += re3.Match(recurso).Value;
    return abreviado;
  }
  private Dictionary<String, Int32> Cabecalho(String[] primeira_linha)
  {
    Dictionary<String, Int32> cabecalho = new();
    var apontador = 1;
    do
    {
      switch(primeira_linha[apontador])
      {
        case null: break;
        case "data": cabecalho.Add("data", apontador); break;
        case "recurso": cabecalho.Add("recurso", apontador); break;
        case "serviço": cabecalho.Add("recurso", apontador); break;
        case "bdi": cabecalho.Add("recurso", apontador); break;
        case "placa": cabecalho.Add("placa", apontador); break;
        case "adesivo light": cabecalho.Add("adesivo", apontador); break;
        case "justificada": cabecalho.Add("justificada", apontador); break;
        case "regional": cabecalho.Add("regional", apontador); break;
        case "área de atuação": cabecalho.Add("regional", apontador); break;
        case "situação": cabecalho.Add("situacao", apontador); break;
        case "atividade": cabecalho.Add("atividade", apontador); break;
        case "alavanca": cabecalho.Add("atividade", apontador); break;
        case "matrícula 01": cabecalho.Add("id_motorista", apontador); break;
        case "executor 01": cabecalho.Add("motorista", apontador); break;
        case "matrícula 02": cabecalho.Add("id_ajudante", apontador); break;
        case "executor 02": cabecalho.Add("ajudante", apontador); break;
        case "telefone": cabecalho.Add("telefone", apontador); break;
        case "telefone equipe": cabecalho.Add("telefone", apontador); break;
        case "matricula sup": cabecalho.Add("id_supervisor", apontador); break;
        case "supervisor empreiteira": cabecalho.Add("supervisor", apontador); break;
        case "qtr": cabecalho.Add("controlador", apontador); break;
        case "técnico light": cabecalho.Add("tecnico", apontador); break;
        default: throw new InvalidOperationException("O nome da coluna não foi reconhecido!");
      }
      apontador += 1;
    } while (apontador < primeira_linha.Length);
    return cabecalho;
  }
}
