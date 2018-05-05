using System;
using IgiCore.Core.Models.Appearance;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public interface IComponent
    {
        Guid Id { get; set; }
        Component Component { get; set; }
    }
}
