using LeadProducer.Helppers;
using LeadProducer.Infrastructure;
using LeadProducer.Infrastructure.Repositories;
using LeadProducer.Services;
using LeadProducer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddDbContext<LeadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LeadDbConnection")));

builder.Services.AddScoped<IRealEstateRepository, RealEstateRepository>();

builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
