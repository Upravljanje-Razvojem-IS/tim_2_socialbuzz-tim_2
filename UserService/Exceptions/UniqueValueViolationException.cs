using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Exceptions
{
    [Serializable]
    public class UniqueValueViolationException: Exception
    {
        public UniqueValueViolationException(string message)
    : base(message)
        {

        }
        public UniqueValueViolationException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
