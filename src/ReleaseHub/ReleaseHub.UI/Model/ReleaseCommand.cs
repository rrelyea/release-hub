using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class CopyNuGetExeCommand : ICommand
    {
        public CopyNuGetExeCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.NuGetExeCopied;

            NuGetRelease.NuGetExeCopied = null;

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    // do copy

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

            NuGetRelease.NuGetExeCopied = desiredState;
        }
    }
}
