namespace Inforigami.CLI.Host.Infrastructure.IoC
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Inforigami.CLI.Host.App;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindsorContainer>()
                         .Instance(container));

            container.Register(
                Component.For<IInstructionParser>()
                         .ImplementedBy<InstructionParser>());

            container.Register(
                Component.For<IArgumentParser>()
                         .ImplementedBy<ArgumentParser>());

            container.Register(
                Component.For<IStringTokenizer>()
                         .ImplementedBy<QuotedStringTokenizer>());

            container.Register(
                Component.For<ICommandBuilder>()
                         .ImplementedBy<ReflectionCommandBuilder>());

            container.Register(
                Component.For<ICommandTypeProvider>()
                         .ImplementedBy<DirectoryScanningCommandTypeProvider>());

            container.Register(
                Component.For<Application>());
        }
    }
}