using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    public interface IElasticCoreService<T> where T : class
    {
        public IReadOnlyCollection<ChatModel> SearchChatLog(int rowCount);
        public void CheckExistsAndInsertLog(T logMode, string indexName);
    }
}
