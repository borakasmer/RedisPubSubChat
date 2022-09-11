using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    public class ElasticCoreService<T> : IDisposable, IElasticCoreService<T> where T : class
    {
        //Elastic üzerinde Indexden bağımız Document atmaya yarar. Yoksa Index yaratır.
        public void CheckExistsAndInsertLog(T logModel, string indexName)
        {
            using (ElasticClientProvider provider = new ElasticClientProvider())
            {
                ElasticClient _client = provider.ElasticClient;
                if (!_client.Indices.Exists(indexName).Exists)
                {
                    var newIndexName = indexName + System.DateTime.Now.Ticks;

                    var indexSettings = new IndexSettings();
                    indexSettings.NumberOfReplicas = 1;
                    indexSettings.NumberOfShards = 3;

                    var response = _client.Indices.Create(newIndexName, index =>
                       index.Map<T>(m => m.AutoMap()
                              )
                      .InitializeUsing(new IndexState() { Settings = indexSettings })
                      .Aliases(a => a.Alias(indexName)));

                }
                IndexResponse responseIndex = _client.Index<T>(logModel, idx => idx.Index(indexName));
            }
        }

        public IReadOnlyCollection<ChatModel> SearchChatLog(int rowCount)
        {
            using (ElasticClientProvider provider = new ElasticClientProvider())
            {
                ElasticClient _client = provider.ElasticClient;
                string indexName = ConfigurationManager.AppSettings["ElasticIndexName"];
                var response = _client.Search<ChatModel>(s => s
                .Size(rowCount)
                .Sort(ss => ss.Descending(p => p.PostDate))

                .Index(indexName)
                );
                return response.Documents;
            }
        }

        private bool _disposedValue;

        ~ElasticCoreService() => Dispose(false);

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
