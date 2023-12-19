using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public class AstProgram
    {
        public List<Statement> Statements { get; set; } = new List<Statement>();


        public override string ToString()
        {
            return string.Format("[ {0} ]", string.Join(", ", Statements));
        }
    }
    

    
}
