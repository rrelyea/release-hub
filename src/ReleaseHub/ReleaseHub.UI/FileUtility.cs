using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseHub.UI
{
    public static class FileUtility
    {

        public static void FileCopy(string sourceFileName, string destDirName)
        {
            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo fileToCopy = new FileInfo(sourceFileName);
            FileInfo destinationFile = new FileInfo(destDirName + @"\" + fileToCopy.Name);
            if (destinationFile.Exists)
            {
                FileDelete(destinationFile.FullName, false);
            }

            File.Copy(fileToCopy.FullName, destinationFile.FullName);
        }

        public static void FileDelete(string fileName, bool deleteDirIfEmpty)
        {
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                File.Delete(fileName);
            }

            DirectoryInfo dir = fileInfo.Directory;

            if (dir.Exists && deleteDirIfEmpty)
            {
                var files = dir.GetFiles("*", SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    dir.Delete(true);
                }
            }
        }

        public static void DirectoryDelete(string directoryName, bool deleteDirIfHasFiles)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryName);

            if (dir.Exists)
            {
                var files = dir.GetFiles("*", SearchOption.AllDirectories);

                if (deleteDirIfHasFiles)
                {
                    dir.Delete(true);
                }
                else if (files.Length > 0)
                {
                    // do nothing
                }
            }
        }

        // From http://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp - thx to Benny!
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
