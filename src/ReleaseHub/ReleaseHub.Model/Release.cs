using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseHub.Model
{
    public class Release : Data
    {
        private string name;
        [DefaultValue(null)]
        public string Name
        {
            get { return name; }
            set { SetField(ref name, value); }
        }

        private string branchName;
        [DefaultValue(null)]
        public string BranchName
        {
            get { return branchName; }
            set { SetField(ref branchName, value); }
        }

        private string sha;
        [DefaultValue(null)]
        public string Sha
        {
            get { return sha; }
            set { SetField(ref sha, value); }
        }

        private int? nugetExebuildNumber;
        [DefaultValue(null)]
        public int? NugetExeBuildNumber
        {
            get { return nugetExebuildNumber; }
            set { SetField(ref nugetExebuildNumber, value); }
        }

        private int? vsixBuildNumber;
        [DefaultValue(null)]
        public int? VsixBuildNumber
        {
            get { return vsixBuildNumber; }
            set { SetField(ref vsixBuildNumber, value); }
        }
        private string version;
        [DefaultValue(null)]
        public string Version
        {
            get { return version; }
            set { SetField(ref version, value); }
        }

        private string versionSuffix;
        [DefaultValue(null)]
        public string VersionSuffix
        {
            get { return versionSuffix; }
            set { SetField(ref versionSuffix, value); }
        }
    }
}
