using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgiCore.Client
{
    public delegate void HandleEvent<in T>(T obj);
}
