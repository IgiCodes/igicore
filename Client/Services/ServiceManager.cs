using System;
using System.Collections;
using System.Collections.Generic;
using IgiCore.Client.Diagnostics;
using IgiCore.SDK.Client;
using JetBrains.Annotations;

namespace IgiCore.Client.Services
{
	public class ServiceManager
	{
		protected readonly List<Service> Services = new List<Service>();

		public ServiceManager()
		{
			
		}

		public void Add<T>(T service) where T : Service, new()
		{
			if (service == null) throw new ArgumentNullException(nameof(service));
			if (service == default(Service)) throw new ArgumentNullException(nameof(service));

			TickHandler.Attach<T>(service.Tick);

			this.Services.Add(service);
		}

		public void Load([NotNull] Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract) throw new ArgumentException("Type must be concreate");
			if (!type.IsPublic) throw new ArgumentException("Type must be public");
			if (!type.IsSubclassOf(typeof(Service))) throw new ArgumentException("Type must inherit from Service");

			var service = Activator.CreateInstance(type, new Logger(), Client.Instance.Events) as Service;
			
			//this.Add<Service>(service);
		}
	}
}
