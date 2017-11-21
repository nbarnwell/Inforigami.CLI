namespace Inforigami.CLI.Host.Infrastructure.IoC
{
    using System.Linq;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    using Inforigami.Regalo.Core;
    using Inforigami.Regalo.Messaging;

    public class RegaloCoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILogger>()
                         .ImplementedBy<RegaloLogger>());

            container.Register(
                Component.For<ICommandProcessor>()
                         .ImplementedBy<CommandProcessor>());

            container.Register(
                Component.For<IEventBus>()
                         .ImplementedBy<EventBus>());

            container.Register(
                Component.For<INoHandlerFoundStrategyFactory>()
                         .ImplementedBy<NoHandlerFoundStrategyFactory>());

            container.Register(
                Classes.FromAssemblyInThisApplication()
                       .BasedOn(typeof(ICommandHandler<>))
                       .WithServiceBase()
                       .LifestyleScoped());

            container.Register(
                Classes.FromAssemblyInThisApplication()
                       .BasedOn(typeof(IEventHandler<>))
                       .WithServiceBase()
                       .LifestyleScoped());

            Resolver.Configure(
               container.Resolve,
               type => container.ResolveAll(type).Cast<object>(),
               container.Release);

            Conventions.SetRetryableEventHandlingExceptionFilter((o, ex) => true);
            Conventions.SetBehaviourWhenNoEventHandlerFound(NoMessageHandlerBehaviour.Throw);
            Conventions.SetBehaviourWhenNoFailedEventHandlerFound(NoMessageHandlerBehaviour.Warn);
        }
    }
}