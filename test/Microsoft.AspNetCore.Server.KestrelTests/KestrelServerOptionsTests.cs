// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel;
using Xunit;

namespace Microsoft.AspNetCore.Server.KestrelTests
{
    public class KestrelServerOptionsTests
    {
        [Fact]
        public void NoDelayDefaultsToTrue()
        {
            var o1 = new KestrelServerOptions();
            o1.Listen(IPAddress.Loopback, 0);
            o1.Listen(IPAddress.Loopback, 0, d =>
            {
                d.NoDelay = false;
            });

            Assert.True(o1.ListenOptions[0].NoDelay);
            Assert.False(o1.ListenOptions[1].NoDelay);
        }

        [Fact]
        public void SetThreadCountUsingProcessorCount()
        {
            // Ideally we'd mock Environment.ProcessorCount to test edge cases.
            var expected = Clamp(Environment.ProcessorCount >> 1, 1, 16);

            var information = new KestrelServerOptions();

            Assert.Equal(expected, information.ThreadCount);
        }

        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}