using ReleaseHub.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class CopyNuGetVsixCommand : ICommand
    {
        public CopyNuGetVsixCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.NuGetVsixCopied;

            NuGetRelease.NuGetVsixCopied = null;

            string sourcePathDir = NuGetRelease.SourcePath;
            string publishPathDir = NuGetRelease.PublishPath;

            string exeSourcePath = sourcePathDir + @"\artifacts\NuGet.exe";

            string dev14VsixFile = Path.Combine(publishPathDir, "artifacts", "dev14") + @"\NuGet.Tools.vsix";
            string dev15VsixFile = Path.Combine(publishPathDir, "artifacts", "dev15") + @"\NuGet.Tools.vsix";

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    // do copy
                    var nugetExeVersion = FileVersionInfo.GetVersionInfo(exeSourcePath);

                    DirectoryInfo wsrTcBaseDir = new DirectoryInfo(@"\\wsr-tc\drops\NuGet.Signed.AllLanguages");
                    var wsrBuilds = wsrTcBaseDir.EnumerateDirectories("build-*").ToList();

                    int dirCount = wsrBuilds.Count;

                    DirectoryInfo matchingSignedBuildDir = null;
                    for (int i = dirCount; i > 0; i--)
                    {
                        DirectoryInfo signedBuildDir = wsrBuilds[i - 1];
                        FileInfo nugetDll = new FileInfo(Path.Combine(signedBuildDir.FullName, "Signed", "temp", "NuGetTools", "14") + @"\NuGet.PackageManagement.UI.dll");
                        if (nugetDll.Exists)
                        {
                            var version = FileVersionInfo.GetVersionInfo(nugetDll.FullName);
                            if (version.FileVersion == nugetExeVersion.FileVersion)
                            {
                                //Log.Text += version.FileVersion + " == " + nugetExeVersion.FileVersion + " in " + signedBuildDir.FullName + "\n";
                                matchingSignedBuildDir = signedBuildDir;
                                break;
                            }
                        }
                    }

                    if (matchingSignedBuildDir != null)
                    {
                        Directory.CreateDirectory(Path.Combine(publishPathDir, "artifacts", "dev14"));
                        File.Copy(Path.Combine(matchingSignedBuildDir.FullName, "Signed", "VSIX", "14") + @"\NuGet.Tools.vsix", dev14VsixFile);
                        Directory.CreateDirectory(Path.Combine(publishPathDir, "artifacts", "dev15"));
                        File.Copy(Path.Combine(matchingSignedBuildDir.FullName, "Signed", "VSIX", "15") + @"\NuGet.Tools.vsix", dev15VsixFile);
                    }
                    else
                    {
                        //Log.Text += "ERROR: Signed VSIX NOT FOUND!\n";
                    }
                }
                else
                {
                    // undo copy
                    FileUtility.FileDelete(dev14VsixFile, true);
                    FileUtility.FileDelete(dev15VsixFile, true);
                }
            }
            else
            {
                // cancel inprogress action

            }

            NuGetRelease.NuGetVsixCopied = desiredState;
        }
    }
}
