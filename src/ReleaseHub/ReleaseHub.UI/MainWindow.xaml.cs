using ReleaseHub.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xaml;

namespace ReleaseHub.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string dataFile;
        List<Release> releasesData;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
            dataFile = System.Environment.ExpandEnvironmentVariables(@"%userprofile%\documents\ReleaseHub.xml");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            SaveDataFile();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // DeleteDataFile(); // TODO: remove once reasonably functional
            LoadOrCreateDataFile();
        }

        private void LoadOrCreateDataFile()
        {
            if (File.Exists(dataFile))
            {
                releasesData = (List<Release>)XamlServices.Load(dataFile);
            }
            else
            {
                releasesData = new List<Release>();
                NuGetRelease r1 = new NuGetRelease()
                {
                    Name = "3.5.0-rc1",
                    BranchName = "dev",
                    BuildNumber = "1592",
                    Version = "3.5.0",
                    VersionSuffix = "-rc1-dev15",
                    BuildDate = new DateTime(2016, 7, 14),
                };

                releasesData.Add(r1);
                NuGetRelease r2 = new NuGetRelease() { Name = "3.5.0-beta2" };
                releasesData.Add(r2);
                SaveDataFile();
            }

            this.DataContext = releasesData;
            releases.SelectedIndex = 0;
        }

        private void SaveDataFile()
        {
            XamlServices.Save(dataFile, releasesData);
        }

        private void DeleteDataFile()
        {
            if (File.Exists(dataFile))
            {
                File.Delete(dataFile);
            }
        }

    }
}
