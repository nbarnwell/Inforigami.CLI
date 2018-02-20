using Castle.MicroKernel.Registration;

namespace Inforigami.CLI.Host
{
    using System;

    using Castle.Windsor;
    using Castle.Windsor.Installer;

    using Inforigami.CLI.Host.App;

    public static class Program
    {
        private static object _consoleLock = new object();
        private static string _prevStatus = "";

        static void Main(string[] args)
        {
            WriteStatus("Loading components...");

            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(new AssemblyFilter(Environment.CurrentDirectory)));

            WriteWelcomePage(container);

            WriteStatus("Starting...");
            var app = container.Resolve<Application>();
            WriteStatus("");
            app.Run();
        }

        private static void WriteWelcomePage(IWindsorContainer container)
        {
            lock (_consoleLock)
            {
                var welcomePages = container.ResolveAll<IWelcomePage>();

                foreach (var welcomePage in welcomePages)
                {
                    welcomePage.Display();
                }
            }
        }

        private static void WriteStatus(string status)
        {
            lock (_consoleLock)
            {
                var pos = Tuple.Create(Console.CursorLeft, Console.CursorTop);
                Console.SetCursorPosition(0, 0);
                if (_prevStatus.Length > status.Length)
                {
                    Console.WriteLine(
                        status + new string(' ', _prevStatus.Length - status.Length));
                }
                else
                {
                    Console.WriteLine(status);
                }

                _prevStatus = status;
                Console.SetCursorPosition(pos.Item1, pos.Item2);
            }
        }
    }
}
