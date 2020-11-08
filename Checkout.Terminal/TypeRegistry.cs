using System;
using Autofac;
using Checkout.AppService;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Repositories;

namespace Checkout.Terminal
{
    internal static class TypeRegistry
    {
        // Based on: https://autofaccn.readthedocs.io/en/latest/getting-started/index.html
        internal static IContainer Build(Action<ContainerBuilder> additionalRegistrationsAction = null)
        {
            var builder = new ContainerBuilder();

            // Presentation:
            builder.RegisterType<BillPresenter>().AsSelf();
            builder.RegisterType<CommandReader>().As<ICommandReader>();
            builder.RegisterType<CommandProcessor>().AsSelf();
            builder.RegisterType<MainProcess>().AsSelf();

            // Service:
            builder.RegisterType<CheckoutService>().As<ICheckoutService>().SingleInstance();

            // Domain:
            builder.RegisterType<OutChecker>().AsSelf();

            // Infrastructure:
            builder.RegisterType<ProductRepository>().As<IProductRepository>();

            // Additional:
            additionalRegistrationsAction?.Invoke(builder);

            return builder.Build();
        }
    }
}
