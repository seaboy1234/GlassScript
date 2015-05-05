using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Parser
{
    public sealed class GlassScriptParserOptions
    {
        public static readonly GlassScriptParserOptions Default = new GlassScriptParserOptions();
        public static readonly GlassScriptParserOptions OptionalSemicolons = new GlassScriptParserOptions { EnforceSemicolons = false };

        public bool AllowRootStatements { get; set; }

        public bool EnforceSemicolons { get; set; }

        public GlassScriptParserOptions()
        {
            EnforceSemicolons = true;
            AllowRootStatements = true;
        }
    }
}
