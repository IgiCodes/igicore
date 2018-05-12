using System;

namespace IgiCore.Core.Models
{
    public class Model
    {
        public Guid Id { get; set; }
        public DateTime? Deleted { get; set; }
        public DateTime Created { get; set; }
    }
}
