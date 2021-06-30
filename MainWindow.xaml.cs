using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            int monthsDifference = (lastDate.Value.Year - firstDate.Value.Year) * 12 + lastDate.Value.Month - firstDate.Value.Month + 1;
            if (firstDate >= lastDate)
                MessageBox.Show("Конечная дата не может быть равна или меньше начальной даты.");
            else if (monthsDifference > 12)
                MessageBox.Show("Нельзя выбрать диапазон больше одного года.");
            else if (path == null || path == "Путь до директории")
                MessageBox.Show("Вы не выбрали путь для сохранения директорий.");
            else
            {
                DirCreator dirCreator = new DirCreator(path, firstDate, lastDate);
                dirCreator.CreateDirs();
                MessageBox.Show("Директории успешно созданы.");
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
