using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Core.Extensions
{
	public static class PedExtensions
	{
		public static Vector3 GetPositionInFront(this Ped ped, float distance) => ped.Position.TranslateDir(ped.Heading + 90, distance);
	}
}
