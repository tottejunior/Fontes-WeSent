using System;
using AutoMapper;

namespace WeSent.Aplicacoes.App_Start
{
	public class MecanismoDeMapeamento
	{
		public static void Install(SimpleInjector.Container container)
		{
			// AutoMapper Profiles registration
			container.RegisterAll<Profile>(new WeSentProfile());

			// Adding AutoMapper profiles
			Mapper.Initialize(x =>
				{
					var profiles = container.GetAllInstances<Profile>();

					foreach (var profile in profiles)
					{
						x.AddProfile(profile);
					}
				});

			Mapper.AssertConfigurationIsValid();
		}
	}
}

