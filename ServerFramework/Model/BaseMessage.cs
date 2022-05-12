using System;

namespace ServerFramework.Model
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
