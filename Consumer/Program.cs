using Consumer.Consumers;
using Infra.Broker.Kafka;
using MassTransit;
using MassTransit.KafkaIntegration;
using MassTransit.Transports.Fabric;
using Messages;
using Microsoft.EntityFrameworkCore;
using Outbox.Infra.Persistence;
using Outbox.Infra.Persistence.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddScoped(typeof(InboxFilter<>));
builder.Services.AddMassTransit(x =>
{

    x.UsingInMemory();

    x.AddRider(rider =>
    {
        rider.AddConsumersFromNamespaceContaining<TestMessageConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetKafkaConfigurations().Host);
            k.TopicEndpoint<TestMessage>(typeof(TestMessage).GetMessageTopic(), nameof(TestMessage).Replace("Message", "").ToLower() + "-group", e =>
            {
                e.ConfigureConsumer<TestMessageConsumer>(context);
                e.UseConsumeFilter(typeof(InboxFilter<>), context);
                e.CreateIfMissing();
            });
        });

    });
});

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
