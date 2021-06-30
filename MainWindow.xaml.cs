using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.ShowDialog();
            dirPathLabel.Content = dialog.SelectedPath;
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string path = dirPathLabel.Content.ToString();
            DateTime? firstDate = Calendar1.SelectedDate;
            DateTime? lastDate = Calendar2.SelectedDate;

            if (firstDate >= lastDate)
            {
                MessageBox.Show("Конечная дата не может быть равна или меньше начальной даты.");
            }
            else
            {
                
                if (path == null || path == "Путь до директории")
                    MessageBox.Show("Вы не выбрали путь для сохранения директорий.");
                else
                {
                    int monthNumber = firstDate.Value.Month;
                    int firstDay = firstDate.Value.Day;
                    int lastDay = DateTime.DaysInMonth(firstDate.Value.Year, firstDate.Value.Month);

                    while(firstDate.Value.Month <= lastDate.Value.Month)
                    {
                        CreateMonthFolder(monthNumber, firstDay, lastDay);
                        firstDate = firstDate.Value.AddMonths(1);
                    }

                    MessageBox.Show("Директории созданы.");
                    ProcessStartInfo procInfo = new ProcessStartInfo("Explorer.exe", path);
                    Process.Start(procInfo);
                }
            }

        }

        private void CreateMonthFolder(int monthNumber, int firstDay, int lastDay)
        {
            // Get month name by number
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
            monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

            // Create month directory
            string path = System.IO.Path.Combine(dirPathLabel.Content.ToString(), monthName);
            Directory.CreateDirectory(path);

            // Create directories for days
            string dayPath;
            for (; firstDay <= lastDay; firstDay++)
            {
                dayPath = System.IO.Path.Combine(path, firstDay.ToString());
                Directory.CreateDirectory(dayPath);
            }
        }

        private void ReleaseFocusCalendar(object sender, MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        private void Calendar1_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ReleaseFocusCalendar(sender, e);
        }

        private void Calendar2_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ReleaseFocusCalendar(sender, e);
        }
    }
}
