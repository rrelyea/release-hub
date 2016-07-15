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
        }

        private bool? nuGetExeCopied;
        [DefaultValue(false)]
        public bool? NuGetExeCopied
        {
            get { return nuGetExeCopied; }
            set { SetField(ref nuGetExeCopied, value); }
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

        private bool nuGetVsixCopied;
        [DefaultValue(false)]
        public bool NuGetVsixCopied
        {
            get { return nuGetVsixCopied; }
            set { SetField(ref nuGetVsixCopied, value); }
        }

        private bool symbolsExtracted;
        [DefaultValue(false)]
        public bool SymbolsExtracted
        {
            get { return symbolsExtracted; }
            set { SetField(ref symbolsExtracted, value); }
        }

        private bool packagesFinalized;
        [DefaultValue(false)]
        public bool PackagesFinalized
        {
            get { return packagesFinalized; }
            set { SetField(ref packagesFinalized, value); }
        }

        private bool nuGetExePublished;
        [DefaultValue(false)]
        public bool NuGetExePublished
        {
            get { return nuGetExePublished; }
            set { SetField(ref nuGetExePublished, value); }
        }

        private bool nuGetVsixPublished;
        [DefaultValue(false)]
        public bool NuGetVsixPublished
        {
            get { return nuGetVsixPublished; }
            set { SetField(ref nuGetVsixPublished, value); }
        }

        private bool symbolsPublished;
        [DefaultValue(false)]
        public bool SymbolsPublished
        {
            get { return symbolsPublished; }
            set { SetField(ref symbolsPublished, value); }
        }

        private bool packagesPublished;
        [DefaultValue(false)]
        public bool PackagesPublished
        {
            get { return packagesPublished; }
            set { SetField(ref packagesPublished, value); }
        }

    }
}
