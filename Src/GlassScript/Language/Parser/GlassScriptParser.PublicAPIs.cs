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
            try
            {
                return ParseExpression();
            }
            catch (SyntaxException)
            {
                // Errors are located in the ErrorSink.
                return null;
            }
        }

        public SourceDocument ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseFile(sourceCode, tokens, GlassScriptParserOptions.Default);
        }

        public SourceDocument ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens, GlassScriptParserOptions options)
        {
            InitializeParser(sourceCode, tokens, options);
            try
            {
                return ParseDocument();
            }
            catch (SyntaxException)
            {
                return null;
            }
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseStatement(sourceCode, tokens, GlassScriptParserOptions.Default);
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens, GlassScriptParserOptions options)
        {
            InitializeParser(sourceCode, tokens, options);

            try
            {
                return ParseStatement();
            }
            catch (SyntaxException)
            {
                return null;
            }
        }
    }
}
