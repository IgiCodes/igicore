using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.SDK.Core.Models
{
	[PublicAPI]
	[ComplexType]
	public class Position
	{
		public Position() { }

		public Position(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		[Required]
		// TODO: Range
		public float X { get; set; }

		[Required]
		// TODO: Range
		public float Y { get; set; }

		[Required]
		// TODO: Range
		public float Z { get; set; }

		public override string ToString() => $"X: {this.X}, Y: {this.Y}, Z: {this.Z}";

		protected bool Equals(Position other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((Position) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Position a, Position b)
		{
			if ((object)a == null) return (object)b == null;
			return a.Equals(b);
		}

		public static bool operator !=(Position a, Position b)
		{
			return !(a == b);
		}
	}
}
