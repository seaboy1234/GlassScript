using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Lexer
{
    public sealed class GlassScriptLexer
    {
        private StringBuilder _builder;
        private int _column;
        private int _index;
        private int _line;
        private string _sourceCode;
        private SourceLocation _tokenStart;

        private char _ch => _sourceCode.CharAt(_index);

        private char _last => Peek(-1);

        private char _next => Peek(1);

        public GlassScriptLexer()
        {
            _builder = new StringBuilder();
        }

        private void Consume()
        {
            _builder.Append(_ch);
            _index++;
            _column++;
        }

        private Token CreateToken(TokenKind kind)
        {
            string contents = _builder.ToString();
            SourceLocation end = new SourceLocation(_index, _line, _column);
            SourceLocation start = _tokenStart;

            _tokenStart = end;
            _builder.Clear();

            return new Token(kind, contents, start, end);
        }

        private char Peek(int ahead)
        {
            return _sourceCode.CharAt(_index + ahead);
        }
    }
}
