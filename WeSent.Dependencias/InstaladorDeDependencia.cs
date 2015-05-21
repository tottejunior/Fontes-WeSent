
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;
using WeSent.Aplicacoes;
using WeSent.Aplicacoes.App_Start;
using WeSent.Negocios;
using WeSent.INegocios;
using WeSent.IAplicacoes;
using WeSent.IRepositorios;
using WeSent.Repositorios;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SessionManager = WeSent.Dependencias.Helper.SessionManager;

namespace WeSent.Dependencias
{
	public class InstaladorDeDependencia
	{
		public static Container RegisterContainer(Assembly assembly)
		{
			var container = new Container();

			//Applications
			var appTypes = OpenGenericBatchRegistrationExtensions.GetTypesToRegister(
				container, typeof(IAplicacaoBase), Assembly.GetAssembly(typeof(AplicacaoBase)));

			foreach (Type implementationType in appTypes)
			{
				Type serviceType =
					implementationType.GetInterfaces().Single(i => !i.IsGenericType && i.Name.Contains(implementationType.Name));

				container.Register(serviceType, implementationType, Lifestyle.Transient);
			}

			//Business
			var businessTypes = OpenGenericBatchRegistrationExtensions.GetTypesToRegister(
				container, typeof(INegocioBase), Assembly.GetAssembly(typeof(NegocioBase)));

			foreach (Type implementationType in businessTypes)
			{
				Type serviceType =
					implementationType.GetInterfaces().Single(i => !i.IsGenericType && i.Name.Contains(implementationType.Name));

				container.Register(serviceType, implementationType, Lifestyle.Transient);
			}

			//Repositories
			var repositoryTypes = OpenGenericBatchRegistrationExtensions.GetTypesToRegister(
				container, typeof(IRepository<,>), Assembly.GetAssembly(typeof(RepositorioBase<,>)));

			foreach (Type implementationType in repositoryTypes)
			{
				Type serviceType =
					implementationType.GetInterfaces().Single(i => !i.IsGenericType && i.Name.Contains(implementationType.Name));

				container.Register(serviceType, implementationType, Lifestyle.Transient);
			}

			//Session
			container.Register(() => SessionManager.Instance.OpenSession(), Lifestyle.Singleton);

			// This is an extension method from the integration package.
			//container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			container.RegisterMvcControllers(assembly);

			// This is an extension method from the integration package as well.
			container.RegisterMvcIntegratedFilterProvider();

			MecanismoDeMapeamento.Install(container);

			container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

			return container;
		}
	}
}