using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseHub.Model
{
    public class Release : Data, ISupportInitialize
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
            set
            {
                SetField(ref branchName, value);
                CalculateNugetExeSourcePath();
                CalculateVsixSourcePath();
            }
        }

        private string sha;
        [DefaultValue(null)]
        public string Sha
        {
            get { return sha; }
            set { SetField(ref sha, value); }
        }

        private string nugetExeBuildNumber;
        [DefaultValue(null)]
        public string NugetExeBuildNumber
        {
            get { return nugetExeBuildNumber; }
            set
            {
                SetField(ref nugetExeBuildNumber, value);
                CalculatePublishPath();
                CalculateNugetExeSourcePath();
                CalculateVsixSourcePath();
            }
        }

        private string vsixBuildNumber;
        [DefaultValue(null)]
        public string VsixBuildNumber
        {
            get { return vsixBuildNumber; }
            set
            {
                SetField(ref vsixBuildNumber, value);
                CalculatePublishPath();
                CalculateNugetExeSourcePath();
                CalculateVsixSourcePath();
            }
        }

        public string GetNugetExeBuildNumber()
        {
            if (NugetExeBuildNumber == "*")
            {
                // get latest build
                if (Directory.Exists(SourcePathRoot))
                {
                    var dir = new DirectoryInfo(Path.Combine(SourcePathRoot, BranchName));
                    var buildDirs = dir.GetDirectories();
                    SortedList<int, string> orderDirs = new SortedList<int, string>();
                    foreach (var buildDir in buildDirs)
                    {
                        orderDirs.Add(Int32.Parse(buildDir.Name), buildDir.Name);
                    }
                    return orderDirs.Last().Value;
                }
                else
                {
                    return NugetExeBuildNumber;
                }

            }
            else
            {
                return NugetExeBuildNumber;
            }
        }
        public string GetVsixBuildNumber()
        {
            if (VsixBuildNumber == "*")
            {
                // get latest build
                if (Directory.Exists(SourcePathRoot))
                {
                    var dir = new DirectoryInfo(Path.Combine(SourcePathRoot, BranchName));
                    var buildDirs = dir.GetDirectories();
                    SortedList<int, string> orderDirs = new SortedList<int, string>();
                    foreach (var buildDir in buildDirs)
                    {
                        orderDirs.Add(Int32.Parse(buildDir.Name), buildDir.Name);
                    }
                    return orderDirs.Last().Value;
                }
                else
                {
                    return VsixBuildNumber;
                }

            }
            else
            {
                return VsixBuildNumber;
            }
        }

        private string version;
        [DefaultValue(null)]
        public string Version
        {
            get { return version; }
            set
            {
                SetField(ref version, value);
                CalculatePublishPath();
            }
        }

        private string versionSuffix;
        [DefaultValue(null)]
        public string VersionSuffix
        {
            get { return versionSuffix; }
            set
            {
                SetField(ref versionSuffix, value);
                CalculatePublishPath();
            }
        }

        public string VersionSuffixOrEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(VersionSuffix))
                {
                    return "";
                }
                else
                {
                    return "-" + VersionSuffix;
                }
            }
        }

        private DateTime? releaseDate;
        [DefaultValue(null)]
        public DateTime? ReleaseDate
        {
            get { return releaseDate; }
            set { SetField(ref releaseDate, value); }
        }

        private DateTime? buildDate;
        [DefaultValue(null)]
        public DateTime? BuildDate
        {
            get { return buildDate; }
            set { SetField(ref buildDate, value); }
        }

        private string publishPathRoot;
        [DefaultValue(null)]
        public string PublishPathRoot
        {
            get { return publishPathRoot; }
            set { SetField(ref publishPathRoot, value); }
        }

        private string sourcePathRoot;
        [DefaultValue(null)]
        public string SourcePathRoot
        {
            get { return sourcePathRoot; }
            set { SetField(ref sourcePathRoot, value); }
        }

        private string publishPath;
        public string PublishPath
        {
            get { return publishPath; }
        }

        public void CalculatePublishPath()
        {
            if (PublishPathRoot != null)
            {
                var versionSuffixString = string.IsNullOrEmpty(VersionSuffix) ? "" : "-" + VersionSuffix;
                publishPath = System.IO.Path.Combine(PublishPathRoot, Version + versionSuffixString);
                OnPropertyChanged("PublishPath");
            }
        }

        private string nugetExeSourcePath;
        public string NugetExeSourcePath
        {
            get { return nugetExeSourcePath; }
        }

        private string vsixSourcePath;
        public string VsixSourcePath
        {
            get { return vsixSourcePath; }
        }

        public void CalculateNugetExeSourcePath()
        {
            if (sourcePathRoot != null)
            {
                nugetExeSourcePath = System.IO.Path.Combine(sourcePathRoot, BranchName + "\\" + GetNugetExeBuildNumber());
                OnPropertyChanged("NugetExeSourcePath");
            }
        }
        public void CalculateVsixSourcePath()
        {
            if (sourcePathRoot != null)
            {
                vsixSourcePath = System.IO.Path.Combine(sourcePathRoot, BranchName + "\\" + GetVsixBuildNumber());
                OnPropertyChanged("VsixSourcePath");
            }
        }

        public void BeginInit()
        {

        }

        public void EndInit()
        {
            CalculatePublishPath();
            CalculateNugetExeSourcePath();
            CalculateVsixSourcePath();
        }
    }
}
