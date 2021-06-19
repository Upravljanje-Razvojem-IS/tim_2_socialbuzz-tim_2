using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Exceptions
{
    [Serializable]
    public class ReferentialConstraintViolationException: Exception
    {
         public ReferentialConstraintViolationException(string message)
   : base(message)
    {

    }
    public ReferentialConstraintViolationException(string message, Exception inner)
         : base(message, inner)
    {

    }
}
}
