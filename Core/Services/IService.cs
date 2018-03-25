using System;
using System.Collections.Generic;

namespace IgiCore.Core.Services
{
	public interface IService
	{
		Dictionary<string, Delegate> Events { get; }
	}
}
