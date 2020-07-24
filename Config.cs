using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMain
{
    class Config
        {
            private string todayPath = @"C:\Users\Sferis\source\repos\PlaylistMain\__FOLDERS\current day\Archiwum\Public\RADIO\reklamy m3u";
            public string TodayPath
            {
                get { return todayPath; }
                set { todayPath = value; }
            }

            private string lokalnePath = @"C:\Users\Sferis\source\repos\PlaylistMain\__FOLDERS\next day\Lokalne\Archiwum\Public\RADIO\reklamy m3u\2020";
            public string LokalnePath
            {
                get { return lokalnePath; }
                set { lokalnePath = value; }
            }

            private string agoraPath = @"C:\Users\Sferis\source\repos\PlaylistMain\__FOLDERS\next day\Agora\Archiwum\Public\RADIO\reklamy\Reklamy 2020\Agora\";
            public string AgoraPath
            {
                get { return agoraPath; }
                set { agoraPath = value; }
            }

            private string eurozetPath = @"C:\Users\Sferis\source\repos\PlaylistMain\__FOLDERS\next day\Eurozet\Archiwum\Public\RADIO\reklamy\Reklamy 2020\Eurozet";
            public string EurozetPath
            {
                get { return eurozetPath; }
                set { eurozetPath = value; }
            }
        }
    }

