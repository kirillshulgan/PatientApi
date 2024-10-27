using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PatientContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PatientDatabase")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PatientAPI", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PatientApi.xml"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Api V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();