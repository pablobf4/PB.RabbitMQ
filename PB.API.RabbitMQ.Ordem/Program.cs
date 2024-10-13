using PB.API.RabbitMQ.Ordem.MessageConsumer;
using PB.API.RabbitMQ.Ordem.RabbitMQSender;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<RabbitMQCheckoutConsumir>();
builder.Services.AddSingleton<IRabbitMQMessagemEnviar, RabbitMQMessagemEnviar>();

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

app.Run();
