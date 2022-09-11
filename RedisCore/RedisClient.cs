using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using StackExchange.Redis;
using System.Runtime.InteropServices;

namespace RedisPubSubChat
{
    public class RedisClient : IDisposable
    {
        public ConnectionMultiplexer connection;
        public string Channel = "Joe";
        public string Channel2 = "Elie";
        public RedisClient()
        {
            string connectionString = ConfigurationManager.AppSettings["RedisConnectionString"];
            var options = ConfigurationOptions.Parse(connectionString);
            options.Password = ConfigurationManager.AppSettings["RedisPassword"];
            connection = ConnectionMultiplexer.Connect(options);
        }

        private bool _disposedValue;

        ~RedisClient() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        public ISubscriber GetSubscriber()
        {
            return connection.GetSubscriber();
        }
    }
}
