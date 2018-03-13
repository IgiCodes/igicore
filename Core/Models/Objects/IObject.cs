namespace IgiCore.Core.Models.Objects
{
    public interface IObject
    {
        int Hash { get; set; }
        bool IsHoldable { get; set; }
    }
}
