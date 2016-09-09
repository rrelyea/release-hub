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

            string sourcePathDir = NuGetRelease.SourcePath;
            string publishPathDir = NuGetRelease.PublishPath;

            string destinationFolder = Path.Combine(publishPathDir, "nupkgs", "fromBuild-withBuildNum");
            string nupkgDestinationRootFolder = Path.Combine(publishPathDir, "nupkgs");

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    // do copy
                    FileUtility.DirectoryCopy(Path.Combine(sourcePathDir, "nupkgs"),
                        destinationFolder,
                        copySubDirs: true);

                    //TODO: copy Nuget.core.nupkg + Nuget.visualstudio.nupkg, etc... 
                        
                }
                else
                {
                    // undo copy
                    FileUtility.DirectoryDelete(destinationFolder, true);
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
