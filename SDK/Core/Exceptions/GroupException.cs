using JetBrains.Annotations;

namespace IgiCore.SDK.Core.Exceptions
{
	[PublicAPI]
	public class GroupException : ModelException
	{
		public GroupException(string message) : base(message) { }
	}
}
