using MTLCRISTALVK18BACK.Contexts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar límite de carga de archivos grandes
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // Permitir archivos de hasta 100 MB
});

// Configuración de DbContext
string connectionString = builder.Configuration.GetConnectionString("MTLCRISTALContexts");
builder.Services.AddDbContext<MTLCRISTALContexts>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(300)));

// Configurar CORS para permitir solicitudes desde la aplicación Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Cambia si Angular está en otro puerto o dominio
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar política CORS antes de autenticación y autorización
app.UseCors("AllowAngularApp");

// Middleware para deshabilitar caché en respuestas de autenticación (opcional)
app.Use(async (context, next) =>
{
    // Solo deshabilitar caché para las rutas que requieren autenticación
    if (context.Request.Path.StartsWithSegments("/api/usersadmin"))
    {
        context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
        context.Response.Headers.Add("Pragma", "no-cache");
        context.Response.Headers.Add("Expires", "0");
        context.Response.Headers.Add("Surrogate-Control", "no-store");
    }
    await next();
});



app.MapControllers();

app.Run();
