using System;
using System.Security.Cryptography;

namespace IgiCore.SDK.Core.Helpers
{
	public static class GuidGenerator
	{
		public static Guid GenerateTimeBasedGuid()
		{
			byte[] randomBytes = new byte[10];

			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(randomBytes);
			}

			byte[] timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);

			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(timestampBytes);
			}

			byte[] guidBytes = new byte[16];

			Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
			Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

			// ReSharper disable once InvertIf
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(guidBytes, 0, 4);
				Array.Reverse(guidBytes, 4, 2);
			}

			return new Guid(guidBytes);
		}
	}
}
