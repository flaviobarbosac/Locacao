using Locacao.Domain.Model;
using Locacao.Infraestructure;
using Locacao.Services.Inteface;
using Locacao.Services;
using Microsoft.EntityFrameworkCore;
using Locacao.Repository.Interface;
using Locacao.Repository;
using Locacao.API.Configuration;
using System.Reflection;
using RabbitMQ.Client;
using Locacao.Services.Interface;
using Locacao.Services.Publisher;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Politicas de autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrDeliveryMan", policy =>
        policy.RequireRole("Admin", "DeliveryMan"));
});

/*
// Configuração do RabbitMQ
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddSingleton(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = rabbitMqSettings["HostName"],
        UserName = rabbitMqSettings["guest"],
        Password = rabbitMqSettings["guest"]
    };
    return factory.CreateConnection();
});
*/

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));

//builder.Services.AddScoped<IBaseServices<Motorcycle>, BaseServices<Motorcycle>>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<IBaseServices<Deliveryman>, BaseServices<Deliveryman>>();
builder.Services.AddScoped<IBaseServices<MotorcycleRegistrationEvent>, BaseServices<MotorcycleRegistrationEvent>>();
builder.Services.AddScoped<IBaseServices<Rental>, BaseServices<Rental>>();
builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddScoped<IBaseRepository<Motorcycle>, BaseRepository<Motorcycle>>();
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IBaseRepository<Deliveryman>, BaseRepository<Deliveryman>>();
builder.Services.AddScoped<IBaseRepository<MotorcycleRegistrationEvent>, BaseRepository<MotorcycleRegistrationEvent>>();
builder.Services.AddScoped<IBaseRepository<Rental>, BaseRepository<Rental>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<MotorcycleEventPublisher>();

builder.Services.AddScoped<JwtService>();

//builder.Services.AddSingleton<MotorcycleEventConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*
// Iniciar o consumidor do RabbitMQ
var consumer = app.Services.GetRequiredService<MotorcycleEventConsumer>();
consumer.StartConsuming();
*/

app.Run();

