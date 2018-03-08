using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace IgiCore.Core.Models
{
    public interface ICharacter
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        string Name { get; set; }
        bool Alive { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }
    }
}