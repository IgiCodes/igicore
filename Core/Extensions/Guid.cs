using System;

namespace IgiCore.Core.Extensions
{
    public static class GuidGenerator
    {
        public static Guid GenerateTimeBasedGuid() => Guid.NewGuid();
    }
}
