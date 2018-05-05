using IgiCore.Core.Models.Inventories;

namespace IgiCore.Core.Models.Objects
{
    public interface IStorable
    {
        int Size { get; set; }
        bool IsStored { get; set; }
        Container Container { get; set; }
    }
}
