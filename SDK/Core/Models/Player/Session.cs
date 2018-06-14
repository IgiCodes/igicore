using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IgiCore.SDK.Core.Models.Player
{
	[PublicAPI]
	public class Session
	{
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 7)]
		public string IpAddress { get; set; }

		[Required]
		public DateTime Connected { get; set; }

		public DateTime? Disconnected { get; set; }

		[StringLength(200)]
		public string DisconnectReason { get; set; }

		[Required]
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; }

		public Session()
		{
			this.Connected = DateTime.UtcNow;
		}
	}
}
