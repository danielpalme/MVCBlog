using System.Reflection;
using System.Web.Mvc;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Service;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;

namespace MVCBlog.Website
{
    /// <summary>
    /// Bootstraps the IoC 'SimpleInjector'.
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            // Using Entity Framework? Please read this: http://simpleinjector.codeplex.com/discussions/363935
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void InitializeContainer(Container container)
        {
            container.RegisterPerWebRequest<IRepository, DatabaseContext>();
            container.RegisterSingle<IMessageService, EmailMessageService>();

            container.Register<ICommandHandler<DeleteCommand<BlogEntryComment>>, DeleteCommandHandler<BlogEntryComment>>();
            container.Register<ICommandHandler<UpdateCommand<BlogEntry>>, UpdateCommandHandler<BlogEntry>>();
            container.Register<ICommandHandler<UpdateCommand<BlogEntryFile>>, UpdateCommandHandler<BlogEntryFile>>();

            container.RegisterManyForOpenGeneric(
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<>).Assembly);

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(CommandLoggingDecorator<>));
        }
    }
}