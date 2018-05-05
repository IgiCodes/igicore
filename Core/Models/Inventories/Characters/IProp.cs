using System;
using IgiCore.Core.Models.Appearance;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public interface IProp
    {
        Guid Id { get; set; }
        Prop Value { get; set; }
    }
}
