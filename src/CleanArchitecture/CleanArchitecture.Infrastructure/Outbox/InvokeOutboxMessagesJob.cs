
using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace CleanArchitecture.Infrastructure.Outbox
{
    [DisallowConcurrentExecution] //para evitar que hallan errores de concurrencia
    internal sealed class InvokeOutboxMessagesJob : IJob
    {
        public static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
                    };

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /*dapper le va a aentregar la data a publisher, el publisher la va a envolver y la va a enviar a una clase de tipo handler, el handler implemnta la logica que queremos*/
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly OutboxOptions _outboxOptions;

        private readonly ILogger<InvokeOutboxMessagesJob> _logger;

        public InvokeOutboxMessagesJob(ISqlConnectionFactory sqlConnectionFactory, IPublisher publisher, IDateTimeProvider dateTimeProvider, IOptions<OutboxOptions> outboxOptions, ILogger<InvokeOutboxMessagesJob> logger)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _publisher = publisher;
            _dateTimeProvider = dateTimeProvider;
            _outboxOptions = outboxOptions.Value;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Iniciando el proceso de outbox messages");
            using var connection = _sqlConnectionFactory.CreateConnection();
            using var transaction = connection.BeginTransaction();

            var sql = @"
                SELECT  id,content
                FROM outbox_messages 
                WHERE processed_on_utc IS NULL 
                ORDER BY ocurred_on_utc ASC
                LIMIT @BatchSize
                FOR UPDATE    
                ";

            var records = (await connection.QueryAsync<OutboxMessageData>(sql, new { _outboxOptions.BatchSize }, transaction)).ToList();

            foreach (var record in records)
            {
                Exception? exception = null;
                try
                {
                    IDomainEvent? domaninEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        record.Content,
                        jsonSerializerSettings);
                    await _publisher.Publish(domaninEvent!, context.CancellationToken);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Se produjo una excepcion en el outbox messaje  {MessageId}", record.Id);
                    exception = ex;
                }
                await UpdateOutboxMessageAsync(connection, transaction, record, exception);
            }

            transaction.Commit();
            _logger.LogInformation("Finalizando el proceso de outbox messages, se procesaron {Count} mensajes", records.Count);

        }

        private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction, OutboxMessageData record, Exception? exception)
        {
            const string sql = @"
                UPDATE outbox_messages 
                SET processed_on_utc = @ProcessedOnUtc, 
                    error = @Exception 
                WHERE id = @Id";
            var parameters = new
            {
                record.Id,
                ProcessedOnUtc = _dateTimeProvider.currentTime,
                Exception = exception?.ToString()
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }
    }

    public record OutboxMessageData(Guid Id, string Content);
}
