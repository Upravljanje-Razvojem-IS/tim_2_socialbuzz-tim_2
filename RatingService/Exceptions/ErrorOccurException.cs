using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Exceptions
{
    public class ErrorOccurException: Exception
    {
        public ErrorOccurException(string message)
            : base(message)
        {

        }
        public ErrorOccurException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
