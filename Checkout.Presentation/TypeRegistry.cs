using System;
using Autofac;
using Checkout.AppService;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Repositories;

namespace Checkout.Presentation
{
    public static class TypeRegistry
    {
        // Based on: https://autofaccn.readthedocs.io/en/latest/getting-started/index.html
        public static IContainer Build(Action<ContainerBuilder> additionalRegistrationsAction = null)
        {
            var builder = new ContainerBuilder();

            // Presentation:
            builder.RegisterType<Invoker>().AsSelf();
            builder.RegisterType<StartCommand>().AsSelf();
            builder.RegisterType<ScanCommand>().AsSelf();
            builder.RegisterType<CancelCommand>().AsSelf();
            builder.RegisterType<StopCommand>().AsSelf();

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
