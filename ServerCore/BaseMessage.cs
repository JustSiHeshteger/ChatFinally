using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    [Serializable]
    public class BaseMessage
    {
        public string UserName { get; set; }
        public bool ThisUser { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
