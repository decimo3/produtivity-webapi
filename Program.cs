using backend.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<AutenticacaoServico>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder =>
  builder.AddProvider(new LoggerDatabaseProvider())
);
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    dotenv.net.DotEnv.Fluent().WithEnvFiles(".env").Load();
    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(option => {
  option
    .WithOrigins("http://localhost:8080")
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
