
using NoDaysOffApp.Features.Core;
using NoDaysOffApp.Features.Dashboards;
using NoDaysOffApp.Features.DashboardTiles;
using NoDaysOffApp.Features.Tiles;
using MediatR;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using NoDaysOffApp.Features.BodyParts;
using NoDaysOffApp.Features.CompletedScheduledExercises;
using NoDaysOffApp.Features.Days;
using NoDaysOffApp.Features.Exercises;
using NoDaysOffApp.Features.ScheduledExercises;
using NoDaysOffApp.Features.Athletes;

namespace NoDaysOffApp
{
    public class UnityConfiguration
    {
        public static IUnityContainer GetContainer()
        {
            var container = new UnityContainer();            
            container.AddMediator<UnityConfiguration>();

            container.RegisterType<HttpClient>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(x => {

                    return new HttpClient();
                }));

            container.RegisterType<ICache>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(x => {
                    return new MemoryCache();
                }));


            container.RegisterType<IDashboardsEventBusMessageHandler, DashboardsEventBusMessageHandler>();
            container.RegisterType<IDashboardTilesEventBusMessageHandler, DashboardTilesEventBusMessageHandler>();
            container.RegisterType<ITilesEventBusMessageHandler, TilesEventBusMessageHandler>();

            container.RegisterType<IAthletesEventBusMessageHandler, AthletesEventBusMessageHandler>();
            container.RegisterType<IBodyPartsEventBusMessageHandler,BodyPartsEventBusMessageHandler>();
            container.RegisterType<ICompletedScheduledExercisesEventBusMessageHandler,CompletedScheduledExercisesEventBusMessageHandler>();
            container.RegisterType<IDaysEventBusMessageHandler,DaysEventBusMessageHandler>();
            container.RegisterType<IExercisesEventBusMessageHandler,ExercisesEventBusMessageHandler>();
            container.RegisterType<IScheduledExercisesEventBusMessageHandler,ScheduledExercisesEventBusMessageHandler>();

            return container;
        }
    }

    public static class UnityContainerExtension
    {

        public static IUnityContainer AddMediator<T>(this IUnityContainer container)
        {
            var classes = AllClasses.FromAssemblies(typeof(T).Assembly)
                .Where(x => x.Name.Contains("Controller") == false
                && x.Name.Contains("Attribute") == false
                && x.Name.EndsWith("Hub") == false
                && x.Name.EndsWith("Message") == false
                && x.Name.EndsWith("Cache") == false
                && x.Name.EndsWith("EventBusMessageHandler") == false
                && x.FullName.Contains("Model") == false)
                .ToList();

            return container.RegisterClassesTypesAndInstances(classes);
        }

        public static IUnityContainer AddMediator<T1, T2>(this IUnityContainer container)
        {
            var classes = AllClasses.FromAssemblies(typeof(T1).Assembly)
                .Where(x => x.Name.Contains("Controller") == false
                && x.Name.Contains("Attribute") == false
                && x.Name.EndsWith("Message") == false
                && x.FullName.Contains("Data.Model") == false)
                .ToList();

            classes.AddRange(AllClasses.FromAssemblies(typeof(T2).Assembly)
                .Where(x => x.Name.Contains("Controller") == false 
                && x.FullName.Contains("Data.Model") == false)
                .ToList());

            return container.RegisterClassesTypesAndInstances(classes);
        }

        public static IUnityContainer RegisterClassesTypesAndInstances(this IUnityContainer container, IList<Type> classes)
        {
            container.RegisterClasses(classes);
            container.RegisterType<IMediator, Mediator>();
            container.RegisterInstance<SingleInstanceFactory>(t => container.IsRegistered(t) ? container.Resolve(t) : null);
            container.RegisterInstance<MultiInstanceFactory>(t => container.ResolveAll(t));
            return container;
        }

        public static void RegisterClasses(this IUnityContainer container, IList<Type> types)
            => container.RegisterTypes(types, WithMappings.FromAllInterfaces, container.GetName, container.GetLifetimeManager);

        public static bool IsNotificationHandler(this IUnityContainer container, Type type)
            => type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(INotificationHandler<>) || x.GetGenericTypeDefinition() == typeof(IAsyncNotificationHandler<>)));

        public static LifetimeManager GetLifetimeManager(this IUnityContainer container, Type type)
            => container.IsNotificationHandler(type) ? new ContainerControlledLifetimeManager() : null;

        public static string GetName(this IUnityContainer container, Type type)
            => container.IsNotificationHandler(type) ? string.Format("HandlerFor" + type.Name) : string.Empty;
    }
}
