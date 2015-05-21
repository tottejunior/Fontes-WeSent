using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WeSent.Dependencias.Helper
{
	public sealed class SessionManager
	{
		private const string Sessionkey = "NHIBERNATE.SESSION";

		private static readonly SessionManager instance = new SessionManager();

		private static ISessionFactory _sessionFactory;
		private static ISession _session; //this session is not used in web
		//private readonly Configuration _configuration;

		//internal static bool IsWeb { get { return (HttpContext.Current != null); } }

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static SessionManager()
		{
		}

		private SessionManager()
		{
		}

		public static SessionManager Instance
		{
			get
			{
				return instance;
			}
		}

		public ISessionFactory SessionFactory
		{
			get { return _sessionFactory ?? (_sessionFactory = InicializarBancoDeDados()); }
		}

		public ISession OpenSession()
		{
			var session = SessionFactory.OpenSession();

			HttpContext.Current.Items.Add(Sessionkey, session);

			return session;
		}

		public ISession Session
		{
			get
			{
				var session = HttpContext.Current.Items[Sessionkey] as ISession;

				return session != null && session.IsOpen ? session : OpenSession();
			}
		}

		private static ISessionFactory InicializarBancoDeDados()
		{
			const string nomeAssemblyDados = "WeSent.Repositorios";

			var c = Fluently.Configure();
			c.Database(OracleDataClientConfiguration.Oracle10
				.ConnectionString(x => x.FromConnectionStringWithKey("oracle"))
				.Driver<NHibernate.Driver.OracleClientDriver>()
				.DefaultSchema("TESTEDSV"));
			c.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(nomeAssemblyDados)));
			c.ExposeConfiguration(x => x.SetProperty("current_session_context_class", "web"));
			return c.BuildSessionFactory();
		}

		private static void BuildSchema(Configuration config)
		{
			//Create
			//new SchemaExport(config).Create(true, true);x

			////Update
			//new SchemaUpdate(config).Execute(true, true);
		}
	}
}

