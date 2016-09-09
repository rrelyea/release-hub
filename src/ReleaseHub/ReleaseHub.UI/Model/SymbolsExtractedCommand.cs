using ReleaseHub.UI;
using ReleaseHub.UI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class SymbolsExtractedCommand : ICommand
    {
        public SymbolsExtractedCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.SymbolsExtracted;

            NuGetRelease.SymbolsExtracted = null;

            string sourcePathDir = NuGetRelease.SourcePath;
            string publishPathDir = NuGetRelease.PublishPath;


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

            NuGetRelease.SymbolsExtracted = desiredState;
        }
    }
}
