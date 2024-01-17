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
// var PORT = System.Environment.GetEnvironmentVariable("WEB_PORT")
//   ?? throw new InvalidOperationException("Environment variable WEB_PORT is not set or is innacessible!");
var allowedOrigins = new String[] {$"http://{HOST}:80", $"http://{HOST}:8080", $"https://{HOST}:7103", $"http://{HOST}:5131"};
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
