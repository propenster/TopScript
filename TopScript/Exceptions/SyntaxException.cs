using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript.Exceptions
{
    public class SyntaxException : Exception
    {
        public SyntaxException(string? message) : base(message)
        {
        }

        public SyntaxException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
