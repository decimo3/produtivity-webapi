using System.Linq.Expressions;
using backend.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyModel.Resolution;
using OfficeOpenXml;
namespace backend.Services;
public class FileManager
{
  private readonly IFormFile file;
  public readonly List<ErroValidacao> erros;
  public readonly List<IEntity> list;
  public FileManager(IFormFile file)
  {
    this.erros = new();
    this.list = new();
    this.file = file;
    if(this.is_valid())
    {
      if(true_if_xls_false_if_csv_null_if_neither() == true)
        this.list = Composicao(this.file.OpenReadStream()).Cast<IEntity>().ToList();
      if(true_if_xls_false_if_csv_null_if_neither() == false)
        this.list = Relatorio(this.file.OpenReadStream()).Cast<IEntity>().ToList();
    }
  }
  private bool is_valid()
  {
    if(file.Length == 0)
    {
      erros.Add(new ErroValidacao(0, null, null, "O arquivo enviado está vazio!"));
      return false;
    }
    if(true_if_xls_false_if_csv_null_if_neither() is null)
    {
      erros.Add(new ErroValidacao(0, null, null, "O formato de arquivo é inválido!"));
      return false;
    }
    return true;
  }
  private bool? true_if_xls_false_if_csv_null_if_neither()
  {
    string ext = Path.GetExtension(file.FileName);
    if(ext == ".xlsx") return true;
    if(ext == ".csv") return false;
    return null;
  }
  public StreamReader Sanitizacao(Stream stream)
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
        writer.Write(' ');
        continue;
      }
      writer.Write(letra);
    }
    writer.Flush();
    memory.Position = 0;
    return new StreamReader(memory);
  }
  public List<Servico> Relatorio(Stream stream)
  {
    using var streamsanitizado = Sanitizacao(stream);
    var servicos = new List<Servico>();
    {
      while (!streamsanitizado.EndOfStream)
      {
        var line = streamsanitizado.ReadLine();
        if(line == null) continue;
        line = line.Replace("\"", "");
        var values = line.Split(',');
        if(values[0] == "Recurso") continue;
        if(!Int32.TryParse(values[21], out int nota)) continue;
        servicos.Add(new Servico {
          recurso = values[0],
          dia = DateOnly.Parse(values[1]),
          indentificador = Int32.Parse(values[2]),
          hora_inicio = TimeOnly.TryParse(values[12], out TimeOnly inicio) ? inicio : null,
          hora_final = TimeOnly.TryParse(values[13], out TimeOnly final) ? final : null,
          duracao_feito = TimeOnly.TryParse(values[17], out TimeOnly duracao) ? duracao : null,
          desloca_feito = TimeOnly.TryParse(values[18], out TimeOnly desloca) ? desloca : null,
          tipo_atividade = values[20],
          servico = nota,
          observacao = values[49],
          instalacao = Int32.TryParse(values[55], out int inst) ? inst : 0,
          tipo_servico = values[60],
          Desloca_estima = TimeOnly.TryParse("00:" + values[69], out TimeOnly desloca_est) ? desloca_est : null,
          duracao_estima = TimeOnly.TryParse("00:" + values[70], out TimeOnly duracao_est) ? duracao_est : null,
          codigos = (values[44] != String.Empty) ? values[44] : values[59].Split(' ')[0],
          status = (values[3] == "não concluído" && values[59].Split(' ')[0] == "20.5") ? "concluído" : values[3],
        });
      }
    }
    return servicos;
  }
  public List<Composicao> Composicao(Stream stream)
  {
    var composicoes = new List<Composicao>();
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using (var reader = new ExcelPackage(stream))
    {
      if(reader.Workbook.Worksheets.Count > 1)
      {
        this.erros.Add(new ErroValidacao(0, null, null, "O arquivo enviado tem mais de uma planilha!"));
        return composicoes;
      }
      var worksheet = reader.Workbook.Worksheets[0];
      var colCount = worksheet.Dimension.Columns;
      if(colCount != 13)
      {
        var maisoumenos = (colCount > 13) ? "mais" : "menos";
        this.erros.Add(new ErroValidacao(0, null, null, $"O arquivo enviado tem colunas a {maisoumenos} que o padrão!"));
        return composicoes;
      }
      var rowCount = worksheet.Dimension.Rows;
      string temp;
      for(int row = 2; row < rowCount; row++)
      {
        var composicao = new Composicao();
        var is_valid = true;
        temp = worksheet.GetValue<DateTime>(row, 1).ToString();
        if(this.is_valid(temp, row, "Dia", ExpectedType.Date))
          composicao.dia = DateOnly.Parse(temp[0..9]);
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 2);
        if(this.is_valid(temp, row, "Adesivo", ExpectedType.Number))
          composicao.adesivo = Int32.Parse(temp);
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 3);
        if(this.is_valid(temp, row, "Placa", ExpectedType.Text))
          composicao.placa = temp.Replace("-", "").Replace(" ", "");
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 4);
        if(this.is_valid(temp, row, "Recurso", ExpectedType.Text))
          composicao.recurso = temp.Trim();
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 5);
        if(this.is_valid(temp, row, "Atividade", ExpectedType.Enum))
          composicao.atividade = Enum.Parse<Atividade>(temp
            .Replace("RELIGA POSTO", "AVANCADO")
            .Replace("RELIGA CAMINHÃO", "CAMINHAO")
            .Replace("EMERGÊNCIA", "EMERGENCIA"));
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 6);
        if(this.is_valid(temp, row, "Motorista", ExpectedType.Text))
          composicao.motorista = temp.Trim();
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 7);
        if(this.is_valid(temp, row, "Mat. Mot.", ExpectedType.Number))
          composicao.id_motorista = Int32.Parse(temp);
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 8);
        if(this.is_valid(temp, row, "Ajudante", ExpectedType.Text))
          composicao.ajudante = temp.Trim();
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 9);
        if(this.is_valid(temp, row, "Mat. Aju.", ExpectedType.Number))
          composicao.id_ajudante = Int32.Parse(temp);
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 10);
        if(this.is_valid(temp, row, "Telefone", ExpectedType.Number))
        {
          temp = temp.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
          if(temp.Length == 9) temp = "5521" + temp;
          if(temp.Length == 11) temp = "55" + temp;
          if(temp.Length != 13)
          {
            this.erros.Add(new ErroValidacao(row, "Telefone", temp, "O número de telefone não é válido!"));
            is_valid = false;
          }
          else composicao.telefone = Int64.Parse(temp);
        }
        else is_valid = false;
        temp = worksheet.GetValue<string>(row, 11);
        if(this.is_valid(temp, row, "Mat. Sup.", ExpectedType.Number))
          composicao.id_supervisor = Int32.Parse(temp);
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 12);
        if(this.is_valid(temp, row, "Supervisor", ExpectedType.Text))
          composicao.supervisor = temp.Trim();
          else is_valid = false;
        temp = worksheet.GetValue<string>(row, 13);
        if(this.is_valid(temp, row, "Atividade", ExpectedType.Enum))
          composicao.regional = Enum.Parse<Regional>(temp
            .Replace("CAMPO GRANDE", "OESTE")
            .Replace("JACAREPAGUA", "OESTE"));
        if(is_valid) composicoes.Add(composicao);
      }
    }
    return composicoes;
  }
  private bool is_valid(string? arg, int linha, string campo, ExpectedType expectedType)
  {
    if(arg == null)
    {
      this.erros.Add(new ErroValidacao(linha, campo, arg, "O valor não foi preenchido!"));
      return false;
    }
    switch(expectedType)
    {
      case ExpectedType.Date:

        if(!DateOnly.TryParse(arg[0..9], out DateOnly dia))
        {
          this.erros.Add(new ErroValidacao(linha, campo, arg, "A data não pode ser reconhecida!"));
          return false;
        }
        return true;
      case ExpectedType.Time:
        if(!TimeOnly.TryParse(arg, out TimeOnly hrs))
        {
          this.erros.Add(new ErroValidacao(linha, campo, arg, "A hora não pode ser reconhecida!"));
          return false;
        }
        return true;
      case ExpectedType.Number:
        arg = arg.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
        if(!Int64.TryParse(arg, out Int64 num))
        {
          this.erros.Add(new ErroValidacao(linha, campo, arg, "A número contém caracteres inválidos!"));
          return false;
        }
        return true;
      case ExpectedType.Text:
        if(arg.Length < 5)
        {
          this.erros.Add(new ErroValidacao(linha, campo, arg, "O texto está incompleto ou vazio!"));
          return false;
        }
        return true;
      case ExpectedType.Enum:
        String[] enums = {"BAIXADA", "CAMPO GRANDE", "JACAREPAGUA", "CORTE", "RELIGA", "RELIGA POSTO", "RELIGA CAMINHÃO", "EMERGÊNCIA"};
        if(!enums.Contains(arg))
        {
          this.erros.Add(new ErroValidacao(linha, campo, arg, "O texto encontrado não corresponde com o padrão!"));
          return false;
        }
        return true;
      default:
        return false;
    }
  }
  private enum ExpectedType {Text, Number, Date, Time, Enum}
}
