﻿using System.Threading;
using System.Threading.Tasks;

namespace EmbyStat.Clients.WebSocketClient
{
    public interface IWebSocketClient
    {
        Task Connect(string url, CancellationToken cancellationToken);
        Task Close(CancellationToken cancellationToken);
        bool IsWebSocketConnectionOpen();
    }
}