using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PlaylistMain
{
    public class M3UItem : M3U
    {
        //// ENCAPSULATION ////
        private FileInfo m3uInfo;
        public FileInfo M3UInfo
        {
            get
            {
                return m3uInfo;
            }
            set
            {
                m3uInfo = value;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private Dictionary<string, bool?> loadSnapshot = new Dictionary<string, bool?>
        {
            { "ShowComments", null },
            { "ShowPath", null }
        };
        public Dictionary<string, bool?> LoadSnapshot
        {
            get
            {
                return loadSnapshot;
            }
            set
            {
                loadSnapshot = value;
            }
        }

        private List<string> formatedContent;
        public List<string> FormatedContent
        {
            get
            {
                return formatedContent;
            }
            set
            {
                formatedContent = value;
            }
        }

        private List<string> rawContent;
        public List<string> RawContent
        {
            get
            {
                return rawContent;
            }
            set
            {
                rawContent = value;
            }
        }


        ///// CONSTRUCTORS
        // Main M3U file (with all content)
        public M3UItem(FileInfo m3uFile, LoadOptions LoadOptions)
        {
            Name = FileNameNoExt(m3uFile.Name);
            M3UInfo = m3uFile;
            RawContent = ReadM3U(m3uFile); // related to fullPath(string) in M3USingleItem class
            FormatedContent = DisplayContentM3U(m3uFile, LoadOptions);
            LoadSnapshot["ShowComments"] = LoadOptions.ShowComments;
            LoadSnapshot["ShowPath"] = LoadOptions.ShowPath;
        }
        // Overload for LoadSnapshot
        public M3UItem(FileInfo m3uFile, Dictionary<string, bool?> loadSnapshot)
        {
            Name = FileNameNoExt(m3uFile.Name);
            M3UInfo = m3uFile;
            RawContent = ReadM3U(m3uFile); // related to fullPath(string) in M3USingleItem class
            FormatedContent = DisplayContentM3U(m3uFile, new LoadOptions(loadSnapshot["ShowComments"], loadSnapshot["ShowPath"]));
            LoadSnapshot = loadSnapshot;
        }
    }
}
