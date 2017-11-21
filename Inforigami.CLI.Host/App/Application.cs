namespace Inforigami.CLI.Host.App
{
    using System;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Inforigami.Regalo.Core;
    using Inforigami.Regalo.Messaging;

    using Newtonsoft.Json;

    public class Application
    {
        private readonly IWindsorContainer  _container;
        private readonly IInstructionParser _instructionParser;
        private readonly ICommandBuilder    _commandBuilder;
        private readonly ICommandProcessor  _commandProcessor;
        private readonly ILogger            _logger;

        private bool _stopSignalled;

        public Application(
            IWindsorContainer container,
            IInstructionParser instructionParser,
            ICommandBuilder commandBuilder,
            ICommandProcessor commandProcessor,
            ILogger logger)
        {
            if (container == null)         throw new ArgumentNullException(nameof(container));
            if (instructionParser == null) throw new ArgumentNullException(nameof(instructionParser));
            if (commandBuilder == null)    throw new ArgumentNullException(nameof(commandBuilder));
            if (commandProcessor == null)  throw new ArgumentNullException(nameof(commandProcessor));
            if (logger == null)            throw new ArgumentNullException(nameof(logger));

            _container         = container;
            _instructionParser = instructionParser;
            _commandBuilder    = commandBuilder;
            _commandProcessor  = commandProcessor;
            _logger            = logger;
        }

        public void Run()
        {
            while (!_stopSignalled)
            {
                try
                {
                    var instruction = PromptForCommand();

                    if (!string.IsNullOrEmpty(instruction))
                    {
                        try
                        {
                            using (_container.BeginScope())
                            {
                                var command = BuildCommand(instruction);
                                _commandProcessor.Process(command);
                            }
                        }
                        catch (UnknownCommandException e)
                        {
                            _logger.Error(this, "Command {0} is not defined. Try listing available commands using the \"help\" command.", e.CommandName);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(
                                this,
                                e,
                                "Unable to process instruction\r\n{0}",
                                JsonConvert.SerializeObject(instruction, Formatting.Indented));
                        }
                    }

                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    _logger.Error(this, e, "Unexpected error");
                }
            }
        }

        private object BuildCommand(string instructionText)
        {
            var instruction = _instructionParser.Parse(instructionText);

            var command = _commandBuilder.Create(instruction);

            return command;
        }

        private string PromptForCommand()
        {
            //Console.WriteLine("Enter instruction:");
            Console.Write("> ");
            return Console.ReadLine()?.Trim();
        }

        public void Stop()
        {
            _stopSignalled = true;
        }
    }
}