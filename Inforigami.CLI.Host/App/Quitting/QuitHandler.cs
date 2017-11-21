namespace Inforigami.CLI.Host.App.Quitting
{
    using System;

    using Inforigami.Regalo.Messaging;

    public class QuitHandler : ICommandHandler<Quit>
    {
        private readonly Application _app;

        public QuitHandler(Application app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            _app = app;
        }

        public void Handle(Quit command)
        {
            _app.Stop();
        }
    }
}