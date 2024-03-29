﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Exceptions
{
    [Serializable]
    public class FollowingException: Exception
    {
        public FollowingException(string message)
              : base(message)
        {

        }
        public FollowingException(string message, Exception inner)
               : base(message, inner)
        {

        }
    }
}
