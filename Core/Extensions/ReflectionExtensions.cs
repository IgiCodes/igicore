using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IgiCore.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypesInNamespace(this Type type, string nameSpace) => type.GetTypeInfo().Assembly.GetTypesInNamespace(nameSpace);

	    public static IEnumerable<Type> GetTypesInNamespace(this Assembly assembly, string nameSpace) => assembly.GetTypes().Where(t => t.Namespace == nameSpace);

	    public static IEnumerable<Type> GetAllSubtypesInNamespace(this Type type) => type.GetTypesInNamespace(type.Namespace).Where(t => !t.IsAbstract && t.IsPublic && t.IsSubclassOf(type));
    }
}
