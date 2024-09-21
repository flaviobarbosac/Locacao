using Locacao.Domain.Model;
using Locacao.Infraestructure;
using Locacao.Services.Inteface;
using Locacao.Services;
using Microsoft.EntityFrameworkCore;
using Locacao.Repository.Interface;
using Locacao.Repository;
using Locacao.API.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Configure DbContext
builder.Services.AddDbContext<LocacaoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger configuration
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddScoped<IBaseServices<Motorcycle>, BaseServices<Motorcycle>>();
builder.Services.AddScoped<IBaseServices<Deliveryman>, BaseServices<Deliveryman>>();
builder.Services.AddScoped<IBaseServices<MotorcycleRegistrationEvent>, BaseServices<MotorcycleRegistrationEvent>>();
builder.Services.AddScoped<IBaseServices<Rental>, BaseServices<Rental>>();

builder.Services.AddScoped<IBaseRepository<Motorcycle>, BaseRepository<Motorcycle>>();
builder.Services.AddScoped<IBaseRepository<Deliveryman>, BaseRepository<Deliveryman>>();
builder.Services.AddScoped<IBaseRepository<MotorcycleRegistrationEvent>, BaseRepository<MotorcycleRegistrationEvent>>();
builder.Services.AddScoped<IBaseRepository<Rental>, BaseRepository<Rental>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

