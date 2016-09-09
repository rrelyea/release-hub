using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReleaseHub.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ReleaseHub.UI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ReleaseHub.UI;assembly=ReleaseHub.UI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FileTextBox/>
    ///
    /// </summary>
    public class FileTextBox : TextBox
    {
        static FileTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileTextBox), new FrameworkPropertyMetadata(typeof(FileTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button b = this.Template.FindName("showFolder", this) as Button;
            b.Click += B_Click;
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Text))
            {
                Process.Start(@"c:\windows\explorer", Text);
            }
        }


    }
}
