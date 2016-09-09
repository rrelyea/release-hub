using ReleaseHub.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class NuGetVsixPublishedCommand : ICommand
    {
        public NuGetVsixPublishedCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.NuGetVsixPublished;

            NuGetRelease.NuGetVsixPublished = null;

            string sourcePathDir = NuGetRelease.SourcePath;
            string publishPathDir = NuGetRelease.PublishPath;

            string source14Path = sourcePathDir + @"\artifacts\Dev14\NuGet.Tools.vsix";
            string source15Path = sourcePathDir + @"\artifacts\Dev15\NuGet.Tools.vsix";
            string publish14Path = System.IO.Path.Combine(publishPathDir, "artifacts", "dev14");
            string publish15Path = System.IO.Path.Combine(publishPathDir, "artifacts", "dev15");
            string publish14File = System.IO.Path.Combine(publish14Path, "NuGet.Tools.vsix");
            string publish15File = System.IO.Path.Combine(publish15Path, "NuGet.Tools.vsix");

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    string message = "Publish " + publish14File + " to <TODO>\n";
                    message += "Publish " + publish15File + " to <TODO>\n";
                    Message mWin = new Message(message);
                    mWin.ShowDialog();
                }
                else
                {
                    // undo copy
                }
            }
            else
            {
                // cancel inprogress action

            }

            NuGetRelease.NuGetVsixPublished = desiredState;
        }
    }
}
