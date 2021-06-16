using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Exceptions
{
    public class ForeignKeyConstraintViolationException: Exception
    {
        public ForeignKeyConstraintViolationException(string message)
   : base(message)
        {

        }
        public ForeignKeyConstraintViolationException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
