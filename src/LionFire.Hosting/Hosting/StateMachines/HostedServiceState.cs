﻿using LionFire.StateMachines.Class;
using System;

namespace LionFire.Trading.Feeds
{
    [Flags]
    public enum HostedServiceState
    {
        [State(StartingState = true)]
        Stopped = 1 << 0,
        Starting = 1 << 1,
        Running = 1 << 2,
        Stopping = 1 << 3,
    }
}
