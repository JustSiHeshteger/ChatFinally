using System;

namespace ServerCore.Model
{
    [Serializable]
    internal class JsonMessage
    {
        public string Method { get; set; } //что выполнить "GETUSERS" / "GETMESSAGES"
        public object Message { get; set; } //тело сообщения 
    }
}
