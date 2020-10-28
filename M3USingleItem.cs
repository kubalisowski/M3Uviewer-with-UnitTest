using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMain
{
    public class M3USingleItem : M3U
    {
        public string ContentLine { get; set; }
        public string Name { get; set; }
        public string fullPath { get; set; }

        //// For single file
        public M3USingleItem(string path, string name, bool? ShowComments)
        {
            if (ShowComments == false)
            {
                List<string> temp = HidePath(new List<string>() { path });
                ContentLine = temp[0];
            }
            else
            {
                ContentLine = path;
            }

            Name = name;
            fullPath = path; // related to RawContent (List<string>) in M3USingleItem class
        }

        //// For M3U lines (M3UItem -> LoadSnapshot property)
        public M3USingleItem(string path, string name, Dictionary<string, bool?> LoadSnapshot)
        {

            ContentLine = DisplayContentLine(path, new LoadOptions(LoadSnapshot["ShowComments"], LoadSnapshot["ShowPath"]));
            Name = name;
            fullPath = path; // related to RawContent (List<string>) in M3USingleItem class
        }

        public M3USingleItem(string line, string name)
        {
            ContentLine = line;
            Name = name;
            fullPath = "none";
        }

        // NICE ARRAY FOREACH METHOD
        //var ItemsM3UCollection = new ObservableCollection<string>();
        //ItemsM3U.ForEach(x => ItemsM3UCollection.Add(x.ToString()));
    }
}
