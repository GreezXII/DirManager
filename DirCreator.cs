using System;
using System.IO;
using System.Globalization;

namespace DirManager
{
    class DirCreator
    {
        private DateTime? FirstDate { get; set; }
        private DateTime? LastDate { get; set; }
        private string Path { get; set; }

        public DirCreator(string path, DateTime? fd, DateTime? ld)
        {
            Path = path;
            FirstDate = fd;
            LastDate = ld;
        }

        private void CreateMonthFolder(int monthNumber, int firstDay, int lastDay)
        {
            // Get month name by number
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
            monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

            // Create month directory
            string path = System.IO.Path.Combine(Path, monthName);
            Directory.CreateDirectory(path);

            // Create directories for days
            string dayPath;
            for (; firstDay <= lastDay; firstDay++)
            {
                dayPath = System.IO.Path.Combine(path, firstDay.ToString());
                Directory.CreateDirectory(dayPath);
            }
        }

        public void CreateDirs()
        {
            while(FirstDate.Value.Month <= LastDate.Value.Month)
            {
                if (FirstDate.Value.Month == LastDate.Value.Month)
                {
                    CreateMonthFolder(FirstDate.Value.Month, FirstDate.Value.Day, LastDate.Value.Day);
                }
                else
                {
                    int daysInMonth = DateTime.DaysInMonth(FirstDate.Value.Year, FirstDate.Value.Month);
                    CreateMonthFolder(FirstDate.Value.Month, FirstDate.Value.Day, daysInMonth);
                    FirstDate = FirstDate.Value.AddMonths(1);
                    FirstDate = FirstDate.Value.AddDays(1 - FirstDate.Value.Day);
                }
            }
        }
    }
}
