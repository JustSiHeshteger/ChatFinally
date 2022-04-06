using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.Model
{
    [Serializable]
    public class BaseMessage
    {
        public string UserName { get; set; }
        public bool ThisUser { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
    }
}
