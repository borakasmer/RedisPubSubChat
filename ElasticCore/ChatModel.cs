using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    public class ChatModel
    {
        public string From { get; set; }        
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }
    }
}
