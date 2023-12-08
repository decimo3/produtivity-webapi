namespace backend.Services;
using backend.Models;
public class JwtTokenGenerator
{

  public static string GenerateJwtToken(Funcionario funcionario)
  {
    var segredo = System.Environment.GetEnvironmentVariable("SECRET_KEY");
    if (segredo is null) throw new InvalidOperationException("Environment variable SECRET_KEY is not set!");
    var header = new { alg = "HS256", typ = "JWT" };
    var headerBase64 = EncodeBase64Url(JsonSerialize(header));
    var payload = new
    {
      sub = funcionario.matricula,
      role = funcionario.funcao,
      iat = UnixTimestamp(DateTime.Now),
      exp = UnixTimestamp(DateTime.Now.AddDays(7))
    };
    var payloadBase64 = EncodeBase64Url(JsonSerialize(payload));
    var signature = EncodeBase64Url(HmacSHA256($"{headerBase64}.{payloadBase64}", segredo));
    return $"{headerBase64}.{payloadBase64}.{signature}";
  }
  private static string JsonSerialize(object obj)
  {
    return System.Text.Json.JsonSerializer.Serialize(obj);
  }
  private static string EncodeBase64Url(string value)
  {
    var bytes = System.Text.Encoding.UTF8.GetBytes(value);
    return Convert.ToBase64String(bytes)
      .TrimEnd('=')
      .Replace('+', '-')
      .Replace('/', '_');
  }
  private static string EncodeBase64Url(byte[] value)
  {
    return Convert.ToBase64String(value)
      .TrimEnd('=')
      .Replace('+', '-')
      .Replace('/', '_');
  }
  private static byte[] HmacSHA256(string data, string key)
  {
    using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(key));
    return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
  }
  private static long UnixTimestamp(DateTime date)
  {
    return (long)(date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
  }
}

