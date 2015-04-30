using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language.Lexer;
using Xunit;

namespace GlassScript.Tests
{
    public class LexerTests
    {
        private GlassScriptLexer lexer = new GlassScriptLexer();

        [Fact]
        public void LexerScansComment()
        {
            const string program = "/*Hi*///Hello, World!!!";
            var tokens = lexer.LexFile(program).ToArray();

            Assert.All(tokens, token => Assert.True(token.Catagory == Language.TokenCatagory.Comment));
            Assert.True(tokens.First().Kind == Language.TokenKind.BlockComment);
            Assert.True(tokens.Last().Kind == Language.TokenKind.LineComment);
        }

        [Theory]
        [InlineData("9float")]
        [InlineData("2data")]
        [InlineData("\"")]
        public void LexerShouldOutputError(string value)
        {
            var token = lexer.LexFile(value).First();

            Assert.True(lexer.ErrorSink.Count() == 1);

            lexer.ErrorSink.Clear();

            Assert.True(lexer.ErrorSink.Count() == 0);
        }

        [Theory]
        [InlineData("1f")]
        [InlineData(".1f")]
        [InlineData(".1")]
        [InlineData("3.2e+10")]
        [InlineData(".1e-32")]
        [InlineData("0.0e0")]
        [InlineData(".1e0")]
        public void LexerShouldScanFloats(string value)
        {
            var token = lexer.LexFile(value).First();

            Assert.True(token.Kind == Language.TokenKind.FloatLiteral);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("21")]
        [InlineData("0130")]
        public void LexerShouldScanInteger(string value)
        {
            var token = lexer.LexFile(value).First();

            Assert.True(token.Kind == Language.TokenKind.IntegerLiteral);
        }

        [Theory]
        [InlineData("[]{}()", Language.TokenCatagory.Grouping)]
        [InlineData("<>===-*/+^%&!|||&&", Language.TokenCatagory.Operator)]
        [InlineData(".,;:", Language.TokenCatagory.Punctuation)]
        [InlineData("var hello _09", Language.TokenCatagory.Identifier)]
        public void TokensGroup(string value, Language.TokenCatagory catagory)
        {
            var tokens = lexer.LexFile(value).Where(g => g.Catagory != Language.TokenCatagory.WhiteSpace).ToArray();

            Assert.All(tokens, token => Assert.True(token.Catagory == catagory));
            Assert.DoesNotContain(tokens, token => token.Kind == Language.TokenKind.Error);
        }
    }
}
