using MyVegas.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MyVegas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            lblStart.Content = "Start: " + START.ToString();
            lblStop.Content = "Stop: " + STOP.ToString();

            foreach (string file in Directory.EnumerateFiles("../../screenshots", "*.png"))
            {
                File.Delete(file);
            }
        }


        private Key START = Key.F1;
        private Key STOP = Key.F2;
        private int TIMER = 10;
        private int STALL_TIMER = 60;
        private DateTime LAST_CLICK;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private bool RUN = false;
        private bool objectWasFound = false;
        private DateTime SCREENSHOT_DELETION_TIMER;
        private int deletion_timer = 10;
        private string LastClick = "";
        private int LastClickCount = 0;
        private int DUPLICATE_CLICK_COUNT = 3;

        #region BGW pass through variables
        int numOfObjects = 0;
        double prob = 0;
        string objectFound = "";
        #endregion


        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tbxOutput.Text += DateTime.Now.ToString() + " Stopped processing.\n";
            btnPlay.IsEnabled = true;
            btnSettings.IsEnabled = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                tbxOutput.Text += "\n" + DateTime.Now.ToString() + " → Taking Screenshot.\n";
            }
            else if (e.ProgressPercentage == 1)
            {
                tbxOutput.Text += DateTime.Now.ToString() + " → Reading Screenshot.\n";
            }
            else if (e.ProgressPercentage == 2)
            {
                tbxOutput.Text += DateTime.Now.ToString() + " → " + numOfObjects +  " objects found.\n";
            }
            else if (e.ProgressPercentage == 3)
            {

                tbxOutput.Text += DateTime.Now.ToString() + " → " + objectFound + " was found with " + Math.Round(prob * 100) + "% probability.\n";
            }
            else if (e.ProgressPercentage == 4)
            {
                tbxOutput.Text += DateTime.Now.ToString() + " → No objects probability surpassed the threashold of " + (Object.Threashold * 100) + "%.\n";
            }
            else if (e.ProgressPercentage == 5)
            {
                tbxOutput.Text += "\n" + DateTime.Now.ToString() + " → Waiting " + TIMER + " seconds.\n";
            }
            else if (e.ProgressPercentage == 6)
            {
                tbxOutput.Text += DateTime.Now.ToString() + " → STALLED: The program hasnt detected a click in over " + STALL_TIMER + " seconds, reseting app.\n";
            }
            else if (e.ProgressPercentage == 7)
            {
                tbxOutput.Text += DateTime.Now.ToString() + " → Emptying screenshot folder.\n";
            }

            tbxOutput.ScrollToEnd();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SCREENSHOT_DELETION_TIMER = DateTime.Now;
            while (RUN)
            {
                backgroundWorker.ReportProgress(0);
                Screenshot screenshot = new Screenshot();
                backgroundWorker.ReportProgress(1);
                screenshot.ReadImage();
                numOfObjects = screenshot.objects.Count;
                backgroundWorker.ReportProgress(2);

                if (screenshot.objects.Count > 0)
                {
                    objectWasFound = false;
                    foreach (Object obj in screenshot.objects)
                    {
                        if (obj.Name == "Retry" || obj.Name == "Collect")
                        {

                            if (obj.Probability > Object.Threashold)
                            {
                                objectWasFound = true;
                                objectFound = obj.Name;
                                prob = obj.Probability;
                                backgroundWorker.ReportProgress(3);
                                SetCursor(obj.GetCenterX(), obj.GetCenterY());
                                DoMouseClick();
                                LAST_CLICK = DateTime.Now;
                                if (LastClick == obj.Name)
                                {
                                    LastClickCount++;
                                }
                                else
                                {
                                    LastClick = obj.Name;
                                    LastClickCount = 0;
                                }
                                break;
                            }
                        }
                        else if (obj.Name == "Close Store")
                        {

                            if (obj.Probability > Object.Threashold)
                            {
                                objectWasFound = true;
                                objectFound = obj.Name;
                                prob = obj.Probability;
                                backgroundWorker.ReportProgress(3);
                                SetCursor(obj.GetCenterX(), obj.GetCenterY());
                                DoMouseClick();
                                LAST_CLICK = DateTime.Now;
                                if (LastClick == obj.Name)
                                {
                                    LastClickCount++;
                                }
                                else
                                {
                                    LastClick = obj.Name;
                                    LastClickCount = 0;
                                }
                                break;
                            }
                        }
                        else if (obj.Name == "Quit")
                        {

                            if (obj.Probability > Object.Threashold)
                            {
                                objectWasFound = true;
                                objectFound = obj.Name;
                                prob = obj.Probability;
                                backgroundWorker.ReportProgress(3);
                                SetCursor(obj.GetCenterX(), obj.GetCenterY());
                                DoMouseClick();
                                LAST_CLICK = DateTime.Now;
                                if (LastClick == obj.Name)
                                {
                                    LastClickCount++;
                                }
                                else
                                {
                                    LastClick = obj.Name;
                                    LastClickCount = 0;
                                }
                                break;
                            }
                        }
                    }

                    if (!objectWasFound)
                    {
                        if (screenshot.objects[0].Probability > Object.Threashold)
                        {
                            objectFound = screenshot.objects[0].Name;
                            prob = screenshot.objects[0].Probability;
                            backgroundWorker.ReportProgress(3);
                            SetCursor(screenshot.objects[0].GetCenterX(), screenshot.objects[0].GetCenterY());
                            DoMouseClick();
                            LAST_CLICK = DateTime.Now;
                            if (LastClick == screenshot.objects[0].Name)
                            {
                                LastClickCount++;
                            }
                            else
                            {
                                LastClick = screenshot.objects[0].Name;
                                LastClickCount = 0;
                            }
                        }
                        else
                        {
                            backgroundWorker.ReportProgress(4);
                            if (HasStalled())
                            {
                                backgroundWorker.ReportProgress(6);
                                FixStall();
                            }
                        }
                    }
                }
                else
                {
                    if (HasStalled())
                    {
                        backgroundWorker.ReportProgress(6);
                        FixStall();
                    }
                }

                if (LastClickCount == DUPLICATE_CLICK_COUNT)
                {
                    backgroundWorker.ReportProgress(6);
                    FixStall();
                }
                backgroundWorker.ReportProgress(5);
                Thread.Sleep(TIMER * 500);

                TimeSpan ts = DateTime.Now.Subtract(SCREENSHOT_DELETION_TIMER);
                if (ts.TotalMinutes >= deletion_timer)
                {
                    SCREENSHOT_DELETION_TIMER = DateTime.Now;
                    string[] files = Directory.GetFiles("../../screenshots", "*.png");
                    backgroundWorker.ReportProgress(7);
                    for (int i = 0; i < (files.Length-1); i++)
                    {
                        File.Delete(files[i]);
                    }
                }

                Thread.Sleep(TIMER * 500);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (RUN)
            {
                RUN = false;
                btnPlay.Content = "Start";
                btnPlay.Background = System.Windows.Media.Brushes.Green;
                btnPlay.IsEnabled = false;
            }
            else
            {
                btnSettings.IsEnabled = false;
                LAST_CLICK = DateTime.Now;
                RUN = true;
                btnPlay.Content = "Stop";
                btnPlay.Background = System.Windows.Media.Brushes.Red;
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void Key_Pressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Key key = e.Key;
            if (key == START && !backgroundWorker.IsBusy)
            {
                btnSettings.IsEnabled = false;
                LAST_CLICK = DateTime.Now;
                RUN = true;
                btnPlay.Content = "Stop";
                btnPlay.Background = System.Windows.Media.Brushes.Red;
                backgroundWorker.RunWorkerAsync();
            }
            else if (key == STOP)
            {
                RUN = false;
                btnPlay.Content = "Start";
                btnPlay.Background = System.Windows.Media.Brushes.Green;
                btnPlay.IsEnabled = false;
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private static void SetCursor(int x, int y)
        {
            // Left boundary
            var xL = Screen.PrimaryScreen.Bounds.X;
            // Top boundary.
            var yT = Screen.PrimaryScreen.Bounds.Y;

            SetCursorPos(xL + x, yT + y);
        }

        private bool HasStalled()
        {
            TimeSpan ts = DateTime.Now.Subtract(LAST_CLICK);

            if (ts.TotalSeconds > STALL_TIMER)
            {
                return true;
            }
            return false;
        }

        private void FixStall()
        {
            int homeX = 1550;
            int homeY = 30;
            int appX = 1000;
            int appY = 1000;

            SetCursor(homeX, homeY);
            DoMouseClick();
            Thread.Sleep(4000);

            SetCursor(appX, appY);
            DoMouseClick();
            Thread.Sleep(1000);
            LAST_CLICK = DateTime.Now;
            LastClick = string.Empty;
            LastClickCount = 0;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public void DoMouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            settings.DeletionChange += Settings_DeletionChange;
            settings.ScreenshotChange += Settings_ScreenshotChange;
            settings.StallChange += Settings_StallChange;
            settings.ThreasholdChange += Settings_ThreasholdChange;
            settings set = new settings(Object.Threashold, (double)TIMER, (double)deletion_timer, (double)STALL_TIMER);
            set.ShowDialog();
        }

        private void Settings_ThreasholdChange(double threashold)
        {
            Object.Threashold = threashold;
        }

        private void Settings_StallChange(double timer)
        {
            STALL_TIMER = (int)timer;
        }

        private void Settings_ScreenshotChange(double Timer)
        {
            TIMER = (int)Timer;
        }

        private void Settings_DeletionChange(double timer)
        {
            deletion_timer = (int)timer;
        }
    }
}
