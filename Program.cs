using backend.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<AutenticacaoServico>();
builder.Services.AddScoped<AlteracoesServico>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    dotenv.net.DotEnv.Fluent().WithEnvFiles(".env").Load();
    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
var HOST = System.Environment.GetEnvironmentVariable("COMPUTERNAME")
  ?? throw new InvalidOperationException("Environment variable COMPUTERNAME is not set or is innacessible!");
var allowedOrigins = new String[] {
  $"http://{HOST.ToLower()}",
  $"https://{HOST.ToLower()}",
  $"http://{HOST.ToLower()}:80",
  $"https://{HOST.ToLower()}:443",
  $"http://{HOST.ToLower()}:8080",
  $"https://{HOST.ToLower()}:4433"};
app.UseCors(options => {
  options
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
});
app.UseAuthorization();
app.UseMiddleware<AutenticacaoMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Estatistica}/{action=Index}/{id?}");
app.Run();
