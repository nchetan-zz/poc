﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisCaching
{
    public class RedisCacheLockThreadState
    {
        public int ThreadNumber { get; set; }
        public IDatabase Database { get; set; }
    }
}
