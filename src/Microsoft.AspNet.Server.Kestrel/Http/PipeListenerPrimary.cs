﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Server.Kestrel.Infrastructure;
using Microsoft.AspNet.Server.Kestrel.Networking;

namespace Microsoft.AspNet.Server.Kestrel.Http
{
    /// <summary>
    /// An implementation of <see cref="ListenerPrimary"/> using UNIX sockets.
    /// </summary>
    public class PipeListenerPrimary : ListenerPrimary
    {
        public PipeListenerPrimary(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        /// <summary>
        /// Creates the socket used to listen for incoming connections
        /// </summary>
        protected override UvStreamHandle CreateListenSocket(string host, int port)
        {
            var socket = new UvPipeHandle();
            socket.Init(Thread.Loop, false);
            socket.Bind(host);
            socket.Listen(Constants.ListenBacklog, ConnectionCallback, this);
            return socket;
        }

        /// <summary>
        /// Handles an incoming connection
        /// </summary>
        /// <param name="listenSocket">Socket being used to listen on</param>
        /// <param name="status">Connection status</param>
        protected override void OnConnection(UvStreamHandle listenSocket, int status)
        {
            var acceptSocket = new UvPipeHandle();
            acceptSocket.Init(Thread.Loop, false);
            listenSocket.Accept(acceptSocket);

            DispatchConnection(acceptSocket);
        }
    }
}