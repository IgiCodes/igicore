using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace IgiCore.Core.Models
{
    public interface IUser
    {
        Guid Id { get; set; }
        string SteamId { get; set; }
        string Name { get; set; }
    }
}
