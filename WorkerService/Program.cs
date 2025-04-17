using Hangfire;
using Infra.Broker;
using Infra.Broker.Kafka;
using MassTransit;
using MassTransit.KafkaIntegration;
using Microsoft.EntityFrameworkCore;
using Outbox.Infra.Persistence;
using Outbox.Infra.Persistence.Processor;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<OutboxProcessor>();
builder.Services.AddScoped<IPublisher, Infra.Broker.Kafka.Interfaces.Publisher>();

// Hangfire setup
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {
        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetKafkaConfigurations().Host);
        });
    });

});

var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "ReferenceId", DataType = SqlDbType.NVarChar, DataLength = 100 },
        new SqlColumn { ColumnName = "TraceId", DataType = SqlDbType.NVarChar, DataLength = 100 },
        new SqlColumn { ColumnName = "SourceType", DataType = SqlDbType.NVarChar, DataLength = 50 },
        new SqlColumn { ColumnName = "Operation", DataType = SqlDbType.NVarChar, DataLength = 100 },
        new SqlColumn { ColumnName = "Payload", DataType = SqlDbType.NVarChar, DataLength = -1 },
        new SqlColumn { ColumnName = "LoggedAt", DataType = SqlDbType.DateTime2 }
    }
};

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString("LoggingDb"),
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "ErrorLogs",
                AutoCreateSqlTable = true // Since you're code-first
            },
            columnOptions: columnOptions,
            restrictedToMinimumLevel: LogEventLevel.Error
        );
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

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<OutboxProcessor>(
    "process-outbox",
    x => x.ProcessOutboxAsync(),
    Cron.Minutely // or every 10 seconds using Cron syntax
);

app.Run();
