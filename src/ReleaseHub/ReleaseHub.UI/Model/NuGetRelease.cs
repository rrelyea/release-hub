using ReleaseHub.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class NuGetRelease : Release
    {
        public NuGetRelease()
        {
            NuGetExeCopied = false;
            NuGetExePublished = false;
            NuGetVsixCopied = false;
            NuGetVsixPublished = false;
            SymbolsExtracted = false;
            SymbolsPublished = false;
            PackagesCopied = false;
            PackagesFinalized = false;
            PackagesPublished = false;
        }

        private bool? nuGetExeCopied;
        [DefaultValue(false)]
        public bool? NuGetExeCopied
        {
            get { return nuGetExeCopied; }
            set { SetField(ref nuGetExeCopied, value); }
        }

        
        private ICommand packagesFinalizedCommand;
        public ICommand PackagesFinalizedCommand
        {
            get
            {
                if (packagesFinalizedCommand == null)
                {
                    packagesFinalizedCommand = new PackagesFinalizedCommand(this);
                }

                return packagesFinalizedCommand;
            }
        }
        private ICommand copyNuGetVsixCommand;
        public ICommand CopyNuGetVsixCommand
        {
            get
            {
                if (copyNuGetVsixCommand == null)
                {
                    copyNuGetVsixCommand = new CopyNuGetVsixCommand(this);
                }

                return copyNuGetVsixCommand;
            }
        }
        private ICommand nuGetVsixPublishedCommand;
        public ICommand NuGetVsixPublishedCommand
        {
            get
            {
                if (nuGetVsixPublishedCommand == null)
                {
                    nuGetVsixPublishedCommand = new NuGetVsixPublishedCommand(this);
                }

                return nuGetVsixPublishedCommand;
            }
        }
        private ICommand copyNuGetExeCommand;
        public ICommand CopyNuGetExeCommand
        {
            get
            {
                if (copyNuGetExeCommand == null)
                {
                    copyNuGetExeCommand = new CopyNuGetExeCommand(this);
                }

                return copyNuGetExeCommand;
            }
        }

        private ICommand nuGetExePublishedCommand;
        public ICommand NuGetExePublishedCommand
        {
            get
            {
                if (nuGetExePublishedCommand == null)
                {
                    nuGetExePublishedCommand = new NuGetExePublishedCommand(this);
                }

                return nuGetExePublishedCommand;
            }
        }
        private ICommand copyNupkgsCommand;
        public ICommand CopyNupkgsCommand
        {
            get
            {
                if (copyNupkgsCommand == null)
                {
                    copyNupkgsCommand = new CopyNupkgsCommand(this);
                }

                return copyNupkgsCommand;
            }
        }
        private bool? nuGetVsixCopied;
        [DefaultValue(false)]
        public bool? NuGetVsixCopied
        {
            get { return nuGetVsixCopied; }
            set { SetField(ref nuGetVsixCopied, value); }
        }

        private bool? symbolsExtracted;
        [DefaultValue(false)]
        public bool? SymbolsExtracted
        {
            get { return symbolsExtracted; }
            set { SetField(ref symbolsExtracted, value); }
        }

        private bool? packagesCopied;
        [DefaultValue(false)]
        public bool? PackagesCopied
        {
            get { return packagesCopied; }
            set { SetField(ref packagesCopied, value); }
        }

        private bool? packagesFinalized;
        [DefaultValue(false)]
        public bool? PackagesFinalized
        {
            get { return packagesFinalized; }
            set { SetField(ref packagesFinalized, value); }
        }

        private bool? nuGetExePublished;
        [DefaultValue(false)]
        public bool? NuGetExePublished
        {
            get { return nuGetExePublished; }
            set { SetField(ref nuGetExePublished, value); }
        }

        private bool? nuGetVsixPublished;
        [DefaultValue(false)]
        public bool? NuGetVsixPublished
        {
            get { return nuGetVsixPublished; }
            set { SetField(ref nuGetVsixPublished, value); }
        }

        private bool? symbolsPublished;
        [DefaultValue(false)]
        public bool? SymbolsPublished
        {
            get { return symbolsPublished; }
            set { SetField(ref symbolsPublished, value); }
        }

        private bool? packagesPublished;
        [DefaultValue(false)]
        public bool? PackagesPublished
        {
            get { return packagesPublished; }
            set { SetField(ref packagesPublished, value); }
        }
    }
}
