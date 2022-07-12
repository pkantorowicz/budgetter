﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using Budgetter.BuildingBlocks.Infrastructure;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Budgetter.Infrastructure.Configuration.Mediation;

public class MediationModule : Module
{
    private readonly List<Assembly> _assemblies;

    public MediationModule(List<Assembly> assemblies)
    {
        _assemblies = assemblies;
    }

    protected override void Load(ContainerBuilder builder)
    {
        _assemblies.Add(ThisAssembly);

        var allConstructorFinder = new AllConstructorFinder();

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterSource(new ScopedContravariantRegistrationSource(
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)));

        var mediatorOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)
        };

        foreach (var mediatorOpenType in mediatorOpenTypes)
            builder
                .RegisterAssemblyTypes(Assemblies
                    .GetAssemblies()
                    .ToArray())
                .AsClosedTypesOf(mediatorOpenType)
                .AsImplementedInterfaces()
                .FindConstructorsWith(allConstructorFinder);

        builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            })
            .InstancePerLifetimeScope();
    }

    private class ScopedContravariantRegistrationSource : IRegistrationSource
    {
        private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
        private readonly List<Type> _types = new();

        public ScopedContravariantRegistrationSource(params Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var isGenericType = types.All(x => x.IsGenericTypeDefinition);

            if (!isGenericType)
                throw new ArgumentException("Supplied types should be generic type definitions");

            _types.AddRange(types);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var components = _source.RegistrationsFor(service, registrationAccessor);

            foreach (var c in components)
            {
                var types = c.Target.Services
                    .OfType<TypedService>()
                    .Select(x => x.ServiceType.GetGenericTypeDefinition());

                if (types.Any(_types.Contains)) yield return c;
            }
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
    }
}