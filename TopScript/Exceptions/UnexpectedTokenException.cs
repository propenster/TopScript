using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException()
        {
        }

        public UnexpectedTokenException(string? message) : base(message)
        {
        }

        public UnexpectedTokenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
