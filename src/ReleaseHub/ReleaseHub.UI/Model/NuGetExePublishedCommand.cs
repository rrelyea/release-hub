using ReleaseHub.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class NuGetExePublishedCommand : ICommand
    {
        public NuGetExePublishedCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.NuGetExePublished;

            NuGetRelease.NuGetExePublished = null;

            string sourcePathDir = NuGetRelease.SourcePath;
            string publishPathDir = NuGetRelease.PublishPath;

            string sourcePath = sourcePathDir + @"\artifacts\NuGet.exe";
            string publishPath = System.IO.Path.Combine(publishPathDir, "artifacts");
            string publishFile = System.IO.Path.Combine(publishPath, "NuGet.exe");

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {

                    // do copy
                    string message = "Publish " + publishFile + " to <TODO>\n";
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

            NuGetRelease.NuGetExePublished = desiredState;
        }
    }
}
