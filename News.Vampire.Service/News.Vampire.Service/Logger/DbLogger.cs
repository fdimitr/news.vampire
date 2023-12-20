using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Models;
using Npgsql;
using NpgsqlTypes;

namespace News.Vampire.Service.Logger
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;

        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        /// <summary>  
        /// Whether to log the entry.  
        /// </summary>  
        /// <param name="logLevel"></param>  
        /// <returns></returns>  
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error || logLevel == LogLevel.Critical || logLevel == LogLevel.Warning;
        }


        /// <summary>  
        /// Used to log the entry.  
        /// </summary>  
        /// <typeparam name="TState"></typeparam>  
        /// <param name="logLevel">An instance of <see cref="LogLevel"/>.</param>  
        /// <param name="eventId">The event's ID. An instance of <see cref="EventId"/>.</param>  
        /// <param name="state">The event's state.</param>  
        /// <param name="exception">The event's exception. An instance of <see cref="Exception" /></param>  
        /// <param name="formatter">A delegate that formats </param>  
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                // Don't log the entry if it's not enabled.  
                return;
            }

            var threadId = Thread.CurrentThread.ManagedThreadId; // Get the current thread ID to use in the log file.   

            // Store record.  
            // Add to database.  

            // LogLevel  
            // ThreadId  
            // EventId  
            // Exception Message (use formatter)  
            // Exception Stack Trace  
            // Exception Source  

            var values = new JObject();

            if (_dbLoggerProvider.Options.LogFields?.Any() ?? false)
            {
                foreach (var logField in _dbLoggerProvider.Options.LogFields)
                {
                    switch (logField)
                    {
                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            {
                                values["LogLevel"] = logLevel.ToString();
                            }

                            break;
                        case "ThreadId":
                            values["ThreadId"] = threadId;
                            break;
                        case "EventId":
                            values["EventId"] = eventId.Id;
                            break;
                        case "EventName":
                            if (!string.IsNullOrWhiteSpace(eventId.Name))
                            {
                                values["EventName"] = eventId.Name;
                            }

                            break;
                        case "Message":
                            if (exception != null && !string.IsNullOrWhiteSpace(formatter(state, exception)))
                            {
                                values["Message"] = formatter(state, exception);
                            }

                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                            {
                                values["ExceptionMessage"] = exception?.Message;
                            }

                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                            {
                                values["ExceptionStackTrace"] = exception?.StackTrace;
                            }

                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                            {
                                values["ExceptionSource"] = exception?.Source;
                            }

                            break;
                    }
                }
            }

            using var connection = new NpgsqlConnection(_dbLoggerProvider.Options.ConnectionString);
            try
            {
                connection.Open();
            }
            catch(NpgsqlException e)
            {
                if (e.SqlState != null && e.SqlState.StartsWith(PostgresErrorCodes.InvalidCatalogName))
                {
                    // Possible reason: Database doesn't created yet
                    return;
                }
            }

            using var command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;

            command.CommandText = $"INSERT INTO public.\"{_dbLoggerProvider.Options.LogTable}\" (\"Values\", \"Created\") VALUES (@Values, @Created)";
            command.Parameters.Add(new NpgsqlParameter("@Values", NpgsqlDbType.Jsonb)
            {
                Value = JsonConvert.SerializeObject(values, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    Formatting = Formatting.None
                })
            });
            command.Parameters.Add(new NpgsqlParameter("@Created", DateTime.UtcNow));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
