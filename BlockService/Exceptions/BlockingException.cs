using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Exceptions
{
    public class BlockingException: Exception
    {
        public BlockingException(string message)
            : base(message)
        {

        }
        public BlockingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
