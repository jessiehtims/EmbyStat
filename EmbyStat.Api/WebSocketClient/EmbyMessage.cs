﻿namespace EmbyStat.Api.WebSocketClient
{
    public class EmbyMessage
    {
        public string MessageType { get; set; }
        public string MessageId { get; set; }
        public string Data { get; set; }
    }
}