using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace IgiCore.Server.Plugins
{
	public static class Graph
	{
		public static List<ServerPluginDefinition> TopologicalSort(List<ServerPluginDefinition> nodes)
		{
			var results = new List<ServerPluginDefinition>();
			var seen = new List<ServerPluginDefinition>();
			var pending = new List<ServerPluginDefinition>();

			Visit(nodes, results, seen, pending);

			return results;
		}

		private static void Visit(List<ServerPluginDefinition> graph, List<ServerPluginDefinition> results, List<ServerPluginDefinition> dead, List<ServerPluginDefinition> pending)
		{
			foreach (var n in graph)
			{
				if (dead.Contains(n)) continue;

				if (!pending.Contains(n))
				{
					pending.Add(n);
				}
				else
				{
					Debug.WriteLine(String.Format("Cycle detected (node Data={0})", n.Definition.Name));
					return;
				}
					
				Visit(n.Definition.Server.DependencyNodes, results, dead, pending);

				if (pending.Contains(n))
				{
					pending.Remove(n);
				}

				dead.Add(n);

				results.Add(n);
			}
		}
		
	}
}
