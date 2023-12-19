using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public record Token(TokenKind kind, object literal)
    {
        public override string ToString()
        {
            return string.Format("Token {{ kind: {0}, literal: \"{1}\" }}, ", kind, literal);
        }
    }
    
}
