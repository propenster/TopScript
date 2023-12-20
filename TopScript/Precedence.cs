using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public enum Precedence
    {
        Lowest,
        Statement,
        Assign,
        AndOr,
        LessThanGreaterThan,
        Equals,
        Sum,
        Product,
        Pow,
        Prefix,
        Call,
    }
}
