using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DateTimeConverter
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            Stream iconResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("DateTimeConverter.Images.Spaghetti.ico");
            this.Icon = BitmapFrame.Create(iconResource);

            this.lblGithub.MouseLeftButtonUp += LblGithub_MouseLeftButtonUp;
        }

        private void LblGithub_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/alonrotem/DateTimeConverter");
        }
    }
}
