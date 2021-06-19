using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Exceptions
{
    [Serializable]
    public class ExecutionException: Exception
    {
        public ExecutionException(string message)
: base(message)
        {

        }
        public ExecutionException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
