using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    public class ElasticClientProvider : IDisposable
    {
        public ElasticClientProvider()
        {
            ElasticSearchHost = ConfigurationManager.AppSettings["ElasticSearchHost"];
            ElasticClient = CreateClient();
        }

        private ElasticClient CreateClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .BasicAuthentication(ConfigurationManager.AppSettings["ElasticSearchUser"],
                ConfigurationManager.AppSettings["ElasticSearchPassword"])
                .DisablePing()
                .DisableDirectStreaming(true)
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            return new ElasticClient(connectionSettings);
        }
        public ElasticClient ElasticClient { get; }
        public string ElasticSearchHost { get; set; }

        private bool _disposedValue;

        ~ElasticClientProvider() => Dispose(false);

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
    }
}
