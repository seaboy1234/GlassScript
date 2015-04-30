using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language;
using GlassScript.Language.Lexer;

namespace GlassScript.InteractiveConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GlassScriptLexer lexer = new GlassScriptLexer();

            while (true)
            {
                Console.Write("GlassScript> ");
                var program = Console.ReadLine();
                var tokens = lexer.LexFile(program);

                foreach (var token in tokens.ToArray())
                {
                    Console.WriteLine($"{token.Kind} ( \"{token.Value}\" ) ");
                }

                if (lexer.ErrorSink.Count() > 0)
                {
                    foreach (var error in lexer.ErrorSink)
                    {
                        Console.WriteLine(new string('-', Console.WindowWidth / 3));

                        WriteError(error);
                    }
                    lexer.ErrorSink.Clear();
                }

                Console.WriteLine(new string('-', Console.WindowWidth / 2));
            }
        }

        private static void WriteError(ErrorEntry error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorLeft = error.Span.Start.Column;

            if (error.Lines.Length > 1)
            {
                Console.WriteLine(error.Lines.First());
                Console.WriteLine(new string('^', error.Lines[0].Length - error.Span.Start.Column));
                for (int i = 1; i < error.Lines.Length - 1; i++)
                {
                    Console.WriteLine(error.Lines[i]);
                    Console.WriteLine(new string('^', error.Lines[i].Length));
                }
                Console.WriteLine(error.Lines.Last());
                Console.WriteLine(new string('^', error.Lines.Last().Length - error.Span.End.Column));
            }
            else
            {
                Console.WriteLine(error.Lines.First());
                Console.WriteLine(new string('^', error.Span.Length));
                Console.WriteLine($"{error.Severity} {error.Span}: {error.Message}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
