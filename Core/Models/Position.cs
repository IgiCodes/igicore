using System.ComponentModel.DataAnnotations.Schema;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace IgiCore.Core.Models
{
    [ComplexType]
    public class Position
    {
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Z { get; set; }

        public static implicit operator Position(Vector3 vector3) => new Position { X = vector3.X, Y = vector3.Y, Z = vector3.Z };
        public static implicit operator Vector3(Position position) => new Vector3 { X = position.X ?? 0, Y = position.Y ?? 0, Z = position.Z ?? 0 };
    }
}
