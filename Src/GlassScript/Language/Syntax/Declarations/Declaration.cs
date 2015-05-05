using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;

        public string Name { get; }

        protected Declaration(SourceSpan span, string name) : base(span)
        {
            Name = name;
        }
    }
}
