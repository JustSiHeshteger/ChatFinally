using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.Model
{
    internal class MessageInfo
    {
        public DateTime DateTime { get; set; }
        public string ClientName { get; set; }
        public string Message { get; set; }
    }

    internal class Message
    {
        public ICollection<MessageInfo> Messages { get; set; }
    }
}
