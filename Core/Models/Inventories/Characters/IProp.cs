using System;
using IgiCore.Models.Appearance;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public interface IProp
    {
        Guid Id { get; set; }
        Prop Value { get; set; }
    }
}
