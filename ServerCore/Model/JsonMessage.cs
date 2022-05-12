using System;
using System.Collections.Generic;

namespace ServerCore.Model
{
    [Serializable]
    internal class JsonMessage
    {
        public string Method { get; set; } //что выполнить "GETUSERS" / "GETMESSAGES"
        public BaseMessage Message { get; set; } //тело сообщения
        public List<BaseUser> Users { get; set; }
    }
}
