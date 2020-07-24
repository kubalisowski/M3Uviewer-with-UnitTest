using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMain
{
    class M3USingleItem : M3U
    {
        public string ContentLine { get; set; }
        public string Name { get; set; }
        public string fullPath { get; set; }

        ///// M3U object origin
        //public M3USingleItem(string name, string line)
        //{
        //    Name = name;
        //    ContentLine = line;
        //}
        ///// Loaded file
        /// Probably to be deleted - overload in use DRY
        public M3USingleItem(string path, string name, LoadOptions LoadOptions)
        {
            ContentLine = DisplayContentLine(path, LoadOptions);
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
