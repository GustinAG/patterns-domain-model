using System;
using Autofac;

namespace Checkout.Terminal
{
    internal static class Program
    {
        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            // Based on: https://autofaccn.readthedocs.io/en/latest/resolve/index.html
            var container = TerminalTypeRegistry.Build();

            using var scope = container.BeginLifetimeScope();
            var process = scope.Resolve<MainProcess>();
            process.Run();
        }
    }
}
