using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language.Syntax;

namespace GlassScript.Language.Parser
{
    public sealed partial class GlassScriptParser
    {
        public Expression ParseExpression(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            InitializeParser(sourceCode, tokens, GlassScriptParserOptions.OptionalSemicolons);

            return ParseExpression();
        }

        public SourceDocument ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseFile(sourceCode, tokens, GlassScriptParserOptions.Default);
        }

        public SourceDocument ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens, GlassScriptParserOptions options)
        {
            InitializeParser(sourceCode, tokens, options);

            return ParseDocument();
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseStatement(sourceCode, tokens, GlassScriptParserOptions.Default);
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens, GlassScriptParserOptions options)
        {
            InitializeParser(sourceCode, tokens, options);

            return ParseStatement();
        }
    }
}
