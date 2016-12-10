using ReleaseHub.UI;
using ReleaseHub.UI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Input;

namespace ReleaseHub.Model
{
    public class PackagesFinalizedCommand : ICommand
    {
        public PackagesFinalizedCommand(NuGetRelease nugetRelease)
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
            bool? desiredState = NuGetRelease.PackagesFinalized;

            NuGetRelease.PackagesFinalized = null;

           // string vsixSourcePathDir = NuGetRelease.VsixSourcePath;
            string publishPathDir = NuGetRelease.PublishPath;

            string destinationFolder = Path.Combine(publishPathDir, "nupkgs", "nupkgsToPublish");
            string destinationSymbolFolder = Path.Combine(publishPathDir, "nupkgs", "symbolNupkgsToPublish");

            // DO WORK
            if (desiredState.HasValue)
            {
                if (desiredState.Value)
                {
                    // do copy

                    string releaseVersion = NuGetRelease.Version + NuGetRelease.VersionSuffixOrEmpty;
                    FinalizePackages(releaseVersion, NuGetRelease.PublishPath, "nupkgs");
                }
                else
                {
                    // undo copy
                    FileUtility.DirectoryDelete(destinationFolder, true);
                    FileUtility.DirectoryDelete(destinationSymbolFolder, true);
                }
            }
            else
            {
                // cancel inprogress action

            }

            NuGetRelease.PackagesFinalized = desiredState;
        }

        public static int FinalizePackages(string newVersion, string sourceBasePath, string folderName)
        {
            string sourceDir = null;
            string packageDestDir = null;
            string symbolsPackageDestDir = null;
            string symbolsAndBinariesDestDir = null;
            bool createPackages = false;
            bool copySymbols = false;
            sourceDir = Path.Combine(sourceBasePath, "nupkgs", "fromBuild-withBuildNum");
            packageDestDir = Path.Combine(sourceBasePath, "nupkgs", "nupkgsToPublish");
            symbolsPackageDestDir = Path.Combine(sourceBasePath, "nupkgs", "symbolNupkgsToPublish");
            symbolsAndBinariesDestDir = Path.Combine(sourceBasePath, "debugging", "symbols");

            createPackages = folderName.ToLower().Contains("nupkgs");
            copySymbols = folderName.ToLower().Contains("symbols");
            // TODO: for myget...we want everything in one folder.

            ///symbolsDestDir = Path.Combine(sourceBasePath, "nupkgsToPublishWithSymbols");

            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);

            if (createPackages)
            {
                DirectoryInfo destDirInfo = EmptyOrCreate(packageDestDir);
                DirectoryInfo symbolsDirInfo = EmptyOrCreate(symbolsPackageDestDir);

                System.Console.WriteLine("Source Dir: " + sourceDir);
                System.Console.WriteLine("Destination Dir: " + packageDestDir);
            }

            int fileCount = 0;

            bool OutputVersions = true;
            foreach (var file in sourceDirInfo.GetFiles("NuGet.*"))
            {
                if (file.Name.StartsWith("NuGet.Test.Utility") ||
                    file.Name.StartsWith("NuGet.Test.Server") ||
                    file.Name.StartsWith("NuGet.Protocol.Test.Utility"))
                {
                    // these don't get published.
                    continue;
                }

                bool isCommandLinePackage = file.Name.StartsWith("NuGet.CommandLine.") && !file.Name.StartsWith("NuGet.CommandLine.XPlat");
                bool isRTM = newVersion.ToLower().Contains("-rtm-final");
                if (isCommandLinePackage && !isRTM)
                {
                    // DO NOT PUBLISH COMMANDLINE PACKAGE TO NUGET.ORG UNLESS FINAL RELEASE.
                    // Otherwise, "nuget -update" run on CI, etc.. -- will grab a pre-release.
                    continue;
                }

                // calculate destination, filename, etc...
                const string nupkgExtension = ".nupkg";
                const string symbolsExtension = ".symbols";

                bool isSymbols = file.Name.ToLower().Contains(symbolsExtension);
                var originalIdAndVersion = FindIdAndVersion(file);

                string releaseVersion = newVersion;
                if (releaseVersion.EndsWith("-rtm-final"))
                {
                    releaseVersion = releaseVersion.Substring(0, releaseVersion.Length - "-rtm-final".Length);
                }

                string newPackageFullName = originalIdAndVersion.Id + "." + releaseVersion;

                string destPackagePath = null;

                if (!isSymbols)
                {
                    string destinationFileName = newPackageFullName + nupkgExtension;
                    destPackagePath = Path.Combine(packageDestDir, destinationFileName);
                }
                else
                {
                    string destinationFileName = newPackageFullName + symbolsExtension + nupkgExtension;
                    destPackagePath = Path.Combine(symbolsPackageDestDir, destinationFileName);
                }

                if (createPackages)
                {
                    // Copy the nupkg to the nupkg directory to ship with the proper filename
                    File.Copy(file.FullName, destPackagePath);
                }

                fileCount++;

                // modify the .nuspec
                ZipArchiveEntry originalNuSpecEntry = null;

                if (OutputVersions)
                {
                    System.Console.WriteLine("Source Version: " + originalIdAndVersion.Version);
                    System.Console.WriteLine("Destination Version: " + newVersion);
                    OutputVersions = false;
                }

                if (createPackages)
                {
                    using (FileStream zipToEdit = new FileStream(destPackagePath, FileMode.Open, FileAccess.ReadWrite))
                    using (ZipArchive zipArchive = new ZipArchive(zipToEdit, ZipArchiveMode.Update))
                    {
                        // Find nuspec and create new nuspec content
                        originalNuSpecEntry = FindNuspecEntry(zipArchive);

                        if (originalNuSpecEntry != null)
                        {
                            var nuspecConversionResult = ReadNuSpecAndReplaceVersionAndDependencyVersion(originalNuSpecEntry,
                                                                                                         originalIdAndVersion.Version,
                                                                                                         releaseVersion,
                                                                                                         isVerbose: !isSymbols);

                            // Replace the .nuspec file the new nuspec content
                            if (nuspecConversionResult.WasSuccessful)
                            {
                                DeleteAndRecreateNuspec(zipArchive, originalNuSpecEntry, nuspecConversionResult);
                            }
                            else
                            {
                                Console.WriteLine("RemoveBuildNumber failed");
                                return -1;
                            }
                        }
                    }
                }

                // copy .dll + .pdb to symbols path
                if (copySymbols && isSymbols)
                {
                    CopySymbolsFromPackagesToSymbolsDir(destPackagePath, symbolsAndBinariesDestDir);
                }
            }

            if (copySymbols)
            {
                string vsixFullPath = Path.Combine(sourceBasePath, "artifacts") + "\\" + "NuGet.Tools.vsix";
                CopyDllsFromVsixToMatchSymbols(vsixFullPath, symbolsAndBinariesDestDir);
            }

            System.Console.WriteLine("NuPkg Count: " + fileCount);
            return 0;
        }

        private static void CopySymbolsFromPackagesToSymbolsDir(string sourcePackagePath, string symbolsAndBinariesDestDir)
        {
            using (var fs = new FileStream(sourcePackagePath, FileMode.Open))
            {
                using (var za = new ZipArchive(fs))
                {
                    foreach (var entry in za.Entries)
                    {
                        string[] segments = entry.FullName.Split('/');
                        int segmentCount = segments.Length;

                        string subDir = null;
                        if (segments[0] == "lib")
                        {
                            subDir = segments[segmentCount - 2] + "\\";
                        }

                        string fileName = segments[segmentCount - 1];

                        if (fileName.ToLower().EndsWith(".dll") ||
                            fileName.ToLower().EndsWith(".exe") ||
                            fileName.ToLower().EndsWith(".pdb")
                            )
                        {
                            var streamToCopy = entry.Open();
                            Directory.CreateDirectory(symbolsAndBinariesDestDir + "\\" + subDir);
                            entry.ExtractToFile(symbolsAndBinariesDestDir + "\\" + subDir + fileName);
                        }
                    }
                }
            }
        }

        private static void CopyDllsFromVsixToMatchSymbols(string sourcePackagePath, string symbolsAndBinariesDestDir)
        {
            using (var fs = new FileStream(sourcePackagePath, FileMode.Open))
            {
                using (var za = new ZipArchive(fs))
                {
                    foreach (var entry in za.Entries)
                    {
                        string[] segments = entry.FullName.Split('/');
                        string fileName = segments[segments.Length - 1];
                        if (fileName.ToLower().EndsWith(".dll"))
                        {
                            int lastDot = fileName.LastIndexOf('.');
                            string shortFileName = fileName.Substring(0, lastDot);

                            // only extract if matching pdb is there.
                            if (File.Exists(symbolsAndBinariesDestDir + "\\" + shortFileName + ".pdb"))
                            {
                                var streamToCopy = entry.Open();
                                entry.ExtractToFile(symbolsAndBinariesDestDir + "\\" + fileName);
                            }
                        }
                    }
                }
            }
        }

        private static void CopyBinariesAndSymbols(string symbolsAndBinariesDestDir, DirectoryInfo tfmDir)
        {
            string tfmName = tfmDir.Name;
            string binDest = Path.Combine(symbolsAndBinariesDestDir, tfmName);
            var binDestInfo = new DirectoryInfo(binDest);

            foreach (var fileInfo in tfmDir.GetFiles())
            {
                string fileExt = fileInfo.Extension.ToLower();
                if (fileExt == ".dll" || fileExt == ".exe" || fileExt == ".pdb")
                {
                    if (!binDestInfo.Exists)
                    {
                        binDestInfo.Create();
                    }

                    fileInfo.CopyTo(binDestInfo + "\\" + fileInfo.Name);
                }
            }
        }

        private static DirectoryInfo EmptyOrCreate(string destDir)
        {
            DirectoryInfo destDirInfo = new DirectoryInfo(destDir);
            if (destDirInfo.Exists)
            {
                destDirInfo.Delete(true);
            }

            destDirInfo.Create();

            return destDirInfo;
        }

        private static PackageInfo FindIdAndVersion(FileInfo file)
        {
            PackageInfo packageInfo = new PackageInfo();
            bool idDone = false;
            string fileName = file.Name;
            string[] nameTokens = fileName.Split('.');
            foreach (var token in nameTokens)
            {
                int result;
                bool isInt = int.TryParse(token.Substring(0, 1), out result);
                if (!isInt)
                {
                    if (idDone)
                    {
                        break;
                    }

                    packageInfo.Id = (packageInfo.Id == null) ? token : packageInfo.Id + "." + token;
                }
                else
                {
                    idDone = true;
                    packageInfo.Version = (packageInfo.Version == null) ? token : packageInfo.Version + "." + token;
                }
            }

            return packageInfo;
        }

        private static void OutputHelp()
        {
            System.Console.WriteLine("RemoveBuildNumber <sourceBasePath> [<nupkgPublishDirectory>]");
            System.Console.WriteLine(" <sourceBasePath> - build root dir (will search for <sourceBasePath>\\nupkg\\*.nupkg)");
            System.Console.WriteLine("  -- will put modified nupkgs in <sourceBasePath>\\nupkgsToPublish");
            System.Console.WriteLine("  -- will put modified symbols nupkgs in <sourceBasePath>\\nupkgsToPublishWithSymbols");
            System.Console.WriteLine(" <nupkgPublishDirectory> - if specified, will put modified nupkgs in <nupkgPublishDirectory>");
        }

        private static ZipArchiveEntry FindNuspecEntry(ZipArchive zipArchive)
        {
            // find the nuspec file and create the modified nuspec file in memory.
            foreach (var entry in zipArchive.Entries)
            {
                if (entry.FullName.ToLower().EndsWith(".nuspec"))
                {
                    return entry;
                }
            }

            return null;
        }

        private static ConvertVersionResult ReadNuSpecAndReplaceVersionAndDependencyVersion(ZipArchiveEntry entry,
                                                                                            string oldVersion,
                                                                                            string newVersion,
                                                                                            bool isVerbose)
        {
            var result = new ConvertVersionResult() { WasSuccessful = false, NuSpecLines = new List<string>() };
            bool inDependencies = false;
            using (StreamReader sr = new StreamReader(entry.Open()))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string newLine = line.Replace(oldVersion, newVersion);
                    result.NuSpecLines.Add(newLine);

                    if (isVerbose)
                    {
                        if (line.Contains("<dependencies>"))
                        {
                            inDependencies = true;
                        }


                        if (line.Contains("<id>") || inDependencies)
                        {
                            bool wasLineChanged = line != newLine;
                            string changeMarker = wasLineChanged ? "* " : "  ";
                            Console.WriteLine(changeMarker + line);
                        }

                        if (line.Contains("</dependencies>"))
                        {
                            inDependencies = false;
                        }
                    }

                }
            }

            result.WasSuccessful = true;
            return result;
        }

        private static void DeleteAndRecreateNuspec(ZipArchive zipArchive, ZipArchiveEntry originalNuspecEntry, ConvertVersionResult result)
        {
            originalNuspecEntry.Delete();

            var newEntry = zipArchive.CreateEntry(originalNuspecEntry.Name);
            using (StreamWriter sw = new StreamWriter(newEntry.Open()))
            {
                foreach (var line in result.NuSpecLines)
                {
                    sw.WriteLine(line);
                }
            }
        }

    }
}
