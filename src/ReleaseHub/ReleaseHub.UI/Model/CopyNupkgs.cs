using ReleaseHub.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class CopyNupkgsCommand : ICommand
    {
        public CopyNupkgsCommand(NuGetRelease nugetRelease)
        {
            NuGetRelease = nugetRelease;
        }

        public NuGetRelease NuGetRelease { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            bool? desiredState = NuGetRelease.PackagesCopied;

            NuGetRelease.PackagesCopied = null;

            string vsixSourcePathDir = NuGetRelease.VsixSourcePath; // find packages that match the VSIX, in case the NuGet.exe is different version
            string publishPathDir = NuGetRelease.PublishPath;

            string nupkgsDestinationFolder = Path.Combine(publishPathDir, "nupkgs-" + NuGetRelease.Version + "." + NuGetRelease.GetVsixBuildNumber());
            string symbolsNupkgsDestinationFolder = Path.Combine(publishPathDir, "symbols.nupkgs-" + NuGetRelease.Version + "." + NuGetRelease.GetVsixBuildNumber());
            string nupkgDestinationRootFolder = Path.Combine(publishPathDir, "nupkgs");

            string nupkgSourceDir = Path.Combine(vsixSourcePathDir, "artifacts", "ReleaseNupkgs");

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    
                    foreach (var nupkg in Directory.EnumerateFiles(nupkgSourceDir))
                    {
                        // TODO: don't copy any nupkgs that we shouldn't publish

                        if (nupkg.ToLower().EndsWith(".symbols.nupkg"))
                        {
                            FileUtility.FileCopy(nupkg, symbolsNupkgsDestinationFolder);
                        }
                        else
                        {
                            FileUtility.FileCopy(nupkg, nupkgsDestinationFolder);
                        }
                    }

                    //TODO: copy Nuget.core.nupkg + Nuget.visualstudio.nupkg, etc... 
                        
                }
                else
                {
                    // undo copy
                    FileUtility.DirectoryDelete(symbolsNupkgsDestinationFolder, true);
                    FileUtility.DirectoryDelete(nupkgsDestinationFolder, true);
                    FileUtility.DirectoryDelete(nupkgDestinationRootFolder, true);
                }
            }
            else
            {
                // cancel inprogress action

            }

            NuGetRelease.PackagesCopied = desiredState;
        }
    }
}
