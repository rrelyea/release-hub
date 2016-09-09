using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseHub.UI.Model
{
    internal class ConvertVersionResult
    {
        public bool WasSuccessful { get; set; }
        public List<string> NuSpecLines { get; set; }
    }
}
