using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MyVegas
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        public delegate void Threashold(double threashold);

        public static event Threashold ThreasholdChange;

        public delegate void Screenshot(double Timer);

        public static event Screenshot ScreenshotChange;

        public delegate void Deletion(double timer);

        public static event Deletion DeletionChange;

        public delegate void Stall(double timer);

        public static event Stall StallChange;

        public settings(double threashold, double screenshotTimer, double screenshotDeletion, double stallTimer)
        {
            InitializeComponent();

            sldrThreashold.Value = threashold*100;
            lblThreashold.Content = threashold*100 +"%";

            sdlrScreenshot.Value = screenshotTimer;
            lblScreenshot.Content = screenshotTimer + " seconds";

            sdlrDeletion.Value = screenshotDeletion;
            lblDeletion.Content = screenshotDeletion+" minutes";

            sdlrStall.Value = stallTimer;
            lblStall.Content = stallTimer + " seconds";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ThreasholdChange(Math.Round(sldrThreashold.Value/100, 2));
            ScreenshotChange(Math.Round(sdlrScreenshot.Value));
            DeletionChange(Math.Round(sdlrDeletion.Value));
            StallChange(Math.Round(sdlrStall.Value));
            this.Close();
        }

        private void threashold_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblThreashold != null)
            {
                lblThreashold.Content = Math.Round(sldrThreashold.Value)+"%";
            }
        }

        private void screenshot_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblScreenshot != null)
            {
                lblScreenshot.Content = Math.Round(sdlrScreenshot.Value) + " seconds";
            }
        }

        private void deletion_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblDeletion != null)
            {
                lblDeletion.Content = Math.Round(sdlrDeletion.Value) + " minutes";
            }
        }

        private void stall_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblStall != null)
            {
                lblStall.Content = Math.Round(sdlrStall.Value) + " seconds";
            }
        }
    }
}
