using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseHub.UI.Model
{
    internal class PackageInfo
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public string FullName
        {
            get
            {
                return Id + "." + Version;
            }
        }

    }
}
