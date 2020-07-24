using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;


namespace PlaylistMain
{
    class PathFinder : Config
    {
        public string date { get; set; }
        public string client { get; set; }
        private string nowDate;
        private string finalPath;

        public PathFinder()
        {
            date = DateTime.Now.ToString().Substring(0, 11); // default date is today
            nowDate = DateTime.Now.ToString().Substring(0, 11); // To get day.month.year
            client = "Lokalne";
            finalPath = GetPath();
        }

        private string[] GetDayMonth()
        {
            string[] dayMonth = new string[2];
            dayMonth[0] = String.Concat(date[0], date[1]);
            dayMonth[1] = String.Concat(date[3], date[4]);
            return dayMonth;
        }

        private string MatchFolder(string[] dirs, string pattern)
        {
            string path = "";
            foreach (string dir in dirs)
            {
                Match matchFolder = Regex.Match(dir, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (matchFolder.Success)
                {
                    path = dir;
                    break;
                }
            }
            return path;
        }

        private string[] GetDirs(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            return dirs;
        }

        public string GetPath()
        {
            string[] dirs;

            // Operational (temp) variables
            string _strTMP = "";
            string[] _dirs;

            // TODAY
            if (date == nowDate)
            {
                // Sun 0; Mon 1; Tue 2; Wed 3; Thu 4; Fri 5; Sat 6;
                int day = (int)DateTime.Now.DayOfWeek;
                // Sunday is assigned to number 7 in folder naming; rest days are the same as sys numbering
                if (day == 0)
                {
                    day++;
                }
                // MatchFolder(dirs, pattern_day of week_)
                finalPath = MatchFolder(GetDirs(TodayPath), String.Format(@".*0{0}.*", day));
                return finalPath;
            }
            // OTHER DAY
            else
            {
                string[] dayMonth = GetDayMonth();
                // TODO: one function for all cases as the same one is used in every cases
                switch (client)
                {
                    case "Lokalne":
                        try
                        {
                            // Get base dir for other day for Agora
                            dirs = GetDirs(LokalnePath);
                            // Get Month folder
                            _strTMP = MatchFolder(dirs, String.Format(@".*{0}_.*", dayMonth[1]));
                            // Get day folder                       
                            _dirs = GetDirs(_strTMP);
                            finalPath = MatchFolder(_dirs, String.Format(@"{0}.*", dayMonth[0]));
                        }
                        // ERROR: System.ArgumentException: 'The path is not of a legal form.'
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Nie znaleziono folderu!");
                            break;
                        }
                        break;

                    case "Agora":
                        try
                        {
                            // Get base dir for other day for Agora
                            dirs = GetDirs(AgoraPath);
                            // Get Month folder
                            _strTMP = MatchFolder(dirs, String.Format(@".*{0}_.*", dayMonth[1]));
                            // Get day folder                       
                            _dirs = GetDirs(_strTMP);
                            finalPath = MatchFolder(_dirs, String.Format(@"{0}.*", dayMonth[0]));
                        }
                        // ERROR: System.ArgumentException: 'The path is not of a legal form.'
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Nie znaleziono folderu!");
                            break;
                        }
                        break;


                    case "Eurozet":
                        try
                        {
                            // Get base dir for other day for Agora
                            dirs = GetDirs(EurozetPath);
                            // Get Month folder
                            _strTMP = MatchFolder(dirs, String.Format(@".*{0}_.*", dayMonth[1]));
                            // Get day folder                       
                            _dirs = GetDirs(_strTMP);
                            finalPath = MatchFolder(_dirs, String.Format(@"{0}.*", dayMonth[0]));
                        }
                        // ERROR: System.ArgumentException: 'The path is not of a legal form.'
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Nie znaleziono folderu!");
                            break;
                        }
                        break;
                }

                return finalPath;
            }
        }
    }
}
