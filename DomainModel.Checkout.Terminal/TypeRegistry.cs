using System;
using Autofac;
using DomainModel.AppService;
using DomainModel.Contracts;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using DomainModel.Repositories;

namespace DomainModel.Checkout.Terminal
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
