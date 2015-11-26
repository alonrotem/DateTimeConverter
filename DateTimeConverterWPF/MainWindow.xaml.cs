using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DateTimeConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimer;
        private readonly string dateTimeFormatStr = "dddd dd/MM/yyyy HH:mm";
        private double intervalMinutesFromLocalTime = 0;

        public MainWindow()
        {
            InitializeComponent();

            Stream iconResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("DateTimeConverter.Images.Clock.ico");
            this.Icon = BitmapFrame.Create(iconResource);

            this.menuItemExit.Click += MenuItemExit_Click;
            this.menuItemClear.Click += MenuItemClear_Click;
            this.menuItemGithub.Click += MenuItemGithub_Click;
            this.menuItemAbout.Click += MenuItemAbout_Click;

            this.dateTimeOnRemoteComputer.Value = DateTime.Now;
            this.dateTimeOnRemoteComputer.Format = Xceed.Wpf.Toolkit.DateTimeFormat.Custom;
            this.dateTimeOnRemoteComputer.FormatString = this.dateTimeFormatStr;
            this.dateTimeOnRemoteComputer.ValueChanged += DateTimeOnRemoteComputer_ValueChanged;

            this.dateTimeOfEvent.Value = DateTime.Now;
            this.dateTimeOfEvent.Format = Xceed.Wpf.Toolkit.DateTimeFormat.Custom;
            this.dateTimeOfEvent.FormatString = this.dateTimeFormatStr;
            this.dateTimeOfEvent.ValueChanged += DateTimeOfEvent_ValueChanged;

            this.UpdateCurrentTime();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void MenuItemGithub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/alonrotem/DateTimeConverter");
        }

        void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            this.dateTimeOnRemoteComputer.Value = DateTime.Now;
            this.dateTimeOfEvent.Value = DateTime.Now;
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            About aboutWin = new About();
            aboutWin.ShowDialog();
        }

        private void DateTimeOfEvent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.RecalculateTimes();
        }

        private void DateTimeOnRemoteComputer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.RecalculateTimes();
        }

        private void RecalculateTimes()
        {
            TimeSpan? diff = this.dateTimeOnRemoteComputer.Value - DateTime.Now;
            intervalMinutesFromLocalTime = diff.Value.TotalMinutes;
            StringBuilder intervalStrB = new StringBuilder();
            bool appendHyphen = false;
            if (diff.Value.Days != 0)
            {
                if (Math.Abs(diff.Value.Days) == 1)
                    intervalStrB.AppendFormat("יום אחד");
                else
                {
                    appendHyphen = true;
                    intervalStrB.AppendFormat("{0} ימים", Math.Abs(diff.Value.Days));
                }
            }
            if (diff.Value.Hours != 0)
            {
                if (intervalStrB.Length > 0)
                    intervalStrB.Append(", ");
                else
                {
                    if (Math.Abs(diff.Value.Hours) != 1)
                        appendHyphen = true;
                }


                if (Math.Abs(diff.Value.Hours) == 1)
                    intervalStrB.AppendFormat("שעה אחת");
                else
                    intervalStrB.AppendFormat("{0} שעות", Math.Abs(diff.Value.Hours));
            }
            if (diff.Value.Minutes != 0)
            {
                if (intervalStrB.Length > 0)
                    intervalStrB.Append(", ");
                else
                {
                    if (Math.Abs(diff.Value.Minutes) != 1)
                        appendHyphen = true;
                }

                if (Math.Abs(diff.Value.Minutes) == 1)
                    intervalStrB.AppendFormat("דקה אחת");
                else
                    intervalStrB.AppendFormat("{0} דקות", Math.Abs(diff.Value.Minutes));
            }
            if (intervalStrB.Length > 0)
            {
                intervalStrB.Insert(0, ((diff.Value.TotalMinutes >= 0) ? "מאחר ב" : "מפגר ב") + ((appendHyphen)?"-": "") );
            }

            this.lblTimeDiff.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            if (intervalStrB.Length > 0)
            {
                this.lblTimeDiff.Content = intervalStrB.ToString();
                if (this.dateTimeOfEvent.Value != null)
                    this.lblRealTimeOfEvent.Content = string.Format("{0:" + dateTimeFormatStr + "}",
                        this.dateTimeOfEvent.Value.Value.AddMinutes(this.intervalMinutesFromLocalTime));
                else
                    this.lblRealTimeOfEvent.Content = "בחר זמן אירוע";
            }
            else
            {
                this.lblTimeDiff.Content = "אין הבדלי זמן";
                this.lblRealTimeOfEvent.Content = "אין הבדלי זמן";
            }

        }

        private void UpdateCurrentTime()
        {
            this.lblCurrentTime.Content = string.Format("{0:" + dateTimeFormatStr + "}", DateTime.Now);
            this.dateTimeOnRemoteComputer.Value = DateTime.Now.AddMinutes(this.intervalMinutesFromLocalTime);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateCurrentTime();
        }

        //void DatePicker_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var dp = sender as DatePicker;
        //    if (dp == null) return;

        //    var tb = GetChildOfType<DatePickerTextBox>(dp);
        //    if (tb == null) return;

        //    var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
        //    if (wm == null) return;

        //    wm.Content = "בחר תאריך";
        //}


        //public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        //{
        //    if (depObj == null) return null;

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(depObj, i);

        //        var result = (child as T) ?? GetChildOfType<T>(child);
        //        if (result != null) return result;
        //    }
        //    return null;
        //}
    }
}
