using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Interfaces;
using Pedidos.Infra.Database;
using Pedidos.Infra.Database.Context;
using Pedidos.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

//geração do XML Documentação Swegger
var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(xmlPath));



//Configurando o DBContext InMemory Database
builder.Services.AddDbContext<ApiContext>( options => options.UseInMemoryDatabase("TestDB"));

//Injetando as dependencias
builder.Services.AddTransient<IProductCommand, ProductCommand>();
builder.Services.AddTransient<IOrderCommand, OrderCommand>();


var app = builder.Build();



//executando o SeedData
using( var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
    SeedData.Seed(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
