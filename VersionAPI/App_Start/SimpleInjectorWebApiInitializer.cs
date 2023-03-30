[assembly: WebActivator.PostApplicationStartMethod(typeof(VersionAPI.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace VersionAPI.App_Start
{
    using System.Web.Http;
    using DLL;
    using DLL.Repositories;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register<EntitiesMap>(Lifestyle.Scoped);
            container.Register<UnitOfWorkJapfaMap>(Lifestyle.Scoped);

            container.Register<EntitiesLBCosting>(Lifestyle.Scoped);
            container.Register<UnitOfWorkLBCosting>(Lifestyle.Scoped);
        }
    }
}