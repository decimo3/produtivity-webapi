using backend.Models;
using OfficeOpenXml;
namespace backend.Services;
public static class FileManager
{
  public static StreamReader Sanitizacao(StreamReader stream)
  {
    var memory = new MemoryStream();
    var writer = new StreamWriter(memory);
    int character = 0;
    bool insideField = false;
    while ((character = stream.Read()) != -1)
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
  public static List<Servico> Relatorio(StreamReader stream)
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
          Desloca_estima = TimeOnly.TryParse(values[69], out TimeOnly desloca_est) ? desloca_est : null,
          duracao_estima = TimeOnly.TryParse(values[70], out TimeOnly duracao_est) ? duracao_est : null,
          codigos = (values[44] != String.Empty) ? values[44] : values[59].Split(' ')[0],
          status = (values[3] == "não concluído" && values[59] == "20.5") ? "concluído" : values[3],
        });
      }
    }
    return servicos;
  }
  public static List<Composicao> Composicao(Stream stream)
  {
    var composicoes = new List<Composicao>();
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using (var reader = new ExcelPackage(stream))
    {
      if(reader.Workbook.Worksheets.Count > 1)
      {
        throw new InvalidOperationException("O arquivo enviado tem mais de uma planilha!");
      }
      var worksheet = reader.Workbook.Worksheets[0];
      var colCount = worksheet.Dimension.Columns;
      if(colCount != 13)
      {
        var maisoumenos = (colCount > 13) ? "mais" : "menos";
        throw new InvalidOperationException($"O arquivo enviado tem colunas a {maisoumenos} que o padrão!");
      }
      var rowCount = worksheet.Dimension.Rows;
      for(int row = 2; row < rowCount; row++)
      {
        var composicao = new Composicao
        {
          dia = DateOnly.FromDateTime(worksheet.GetValue<DateTime>(row, 1)),
          adesivo = worksheet.GetValue<Int32>(row, 2),
          placa = worksheet.GetValue<string>(row, 3)
            .Replace("-", "")
            .Replace(" ", ""),
          recurso = worksheet.GetValue<string>(row, 4).Trim(),
          atividade = Enum.Parse<Atividade>(worksheet.GetValue<string>(row, 5)
            .Replace("RELIGA POSTO", "AVANCADO")
            .Replace("RELIGA CAMINHÃO", "CAMINHAO")
            .Replace("EMERGÊNCIA", "EMERGENCIA")),
          motorista = worksheet.GetValue<string>(row, 6),
          id_motorista = worksheet.GetValue<Int32>(row, 7),
          ajudante = worksheet.GetValue<string>(row, 8),
          id_ajudante = worksheet.GetValue<Int32>(row, 9),
          telefone = Int64.Parse(worksheet.GetValue<string>(row, 10)
            .Replace("-", "")
            .Replace(" ", "")),
          id_supervisor = worksheet.GetValue<Int32>(row, 11),
          supervisor = worksheet.GetValue<string>(row, 12),
          regional = Enum.Parse<Regional>(worksheet.GetValue<string>(row, 13)
            .Replace("CAMPO GRANDE", "OESTE")
            .Replace("JACAREPAGUA", "OESTE")),
        };
        composicoes.Add(composicao);
        // var validation_result = new List<ValidationResult>();
        // var validation_context = new ValidationContext(composicao);
        // if(Validator.TryValidateObject(composicao, validation_context, validation_result, validateAllProperties: true))
        // {
        //   composicoes.Add(composicao);
        // }
      }
    }
    return composicoes;
  }
}
