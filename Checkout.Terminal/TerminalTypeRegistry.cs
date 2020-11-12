using System;
using Autofac;
using Checkout.Contracts;
using Checkout.Presentation;

namespace Checkout.Terminal
{
    internal static class TerminalTypeRegistry
    {
        internal static IContainer Build(Action<ContainerBuilder> additionalRegistrationsAction = null) =>
            TypeRegistry.Build(b => RegisterTerminalTypes(b, additionalRegistrationsAction));


        private static void RegisterTerminalTypes(ContainerBuilder builder, Action<ContainerBuilder> additionalRegistrationsAction)
        {
            builder.RegisterType<BillPresenter>().AsSelf().As<IPresenter>().As<IWarningPresenter>();
            builder.RegisterType<CommandReader>().As<ICommandReader>();
            builder.RegisterType<CommandProcessor>().AsSelf();
            builder.RegisterType<MainProcess>().AsSelf();

            additionalRegistrationsAction?.Invoke(builder);
        }
    }
}
