using System;
using System.IO;

namespace Checkout.Terminal.Tests
{
    // Based on: https://www.codeproject.com/articles/501610/getting-console-output-within-a-unit-test
    public sealed class ConsoleOutput : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput() => _stringWriter.ToString();

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}
