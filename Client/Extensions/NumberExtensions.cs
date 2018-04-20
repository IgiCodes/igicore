namespace IgiCore.Client.Extensions
{
	public static class NumberExtensions
	{
		public static string Pluralize(this string str, int value, string extention = "s")
		{
			return value == 1 ? $"{value} {str}" : $"{value} {str}{extention}";
		}
		public static string Pluralize(this string str, double value, string extention = "s")
		{
			return (int)value == 1 ? $"{value} {str}" : $"{value} {str}{extention}";
		}
	}
}
