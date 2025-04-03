using Microsoft.AspNetCore.Server.Kestrel.Core;
using TestXmlToJsonConverter.RecordsHandler;
using TestXmlToJsonConverter.XmlHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Environment.SetEnvironmentVariable("Directory", builder.Configuration["Directory"]);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
});
builder.Services.AddTransient<IXmlHandler, XmlHandler>();
builder.Services.AddTransient<IRecordsHandler, RecordsHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://127.0.0.1:9091");
