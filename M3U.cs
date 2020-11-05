using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Diagnostics;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;

namespace PlaylistMain
{
    public class M3U
    {
        public static FileInfo[] M3Ufiles;
        // Encapsulation
        private static List<FileInfo> selectedM3U = new List<FileInfo>();
        // Var binded to loadedM3U.ItemsSource
        public ObservableCollection<object> ItemsM3U
            = new ObservableCollection<object>();

        public void FilesInfoList(string path)
        {
            DirectoryInfo DirInfo = new DirectoryInfo(path);
            M3Ufiles = DirInfo.GetFiles("*.m3u");
        }

        //// File names without extensions
        /// https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getfilename?redirectedfrom=MSDN&view=netcore-3.1#System_IO_Path_GetFileName_System_String_
        public List<string> FileNameNoExt(FileInfo[] m3u)
        {
            List<string> filesNoExt = new List<string>();
            foreach (FileInfo m in m3u)
            {
                string strFileName = m.Name.ToString();
                string extension = m.Extension.ToString();
                string strName = strFileName.Replace(extension, "");
                filesNoExt.Add(strName);
            }
            return filesNoExt;
        }
        // Overload
        public string FileNameNoExt(string m3uName)
        {
            string strNameNoExt = m3uName.Replace(".m3u", "");
            return strNameNoExt;
        }

        public static List<FileInfo> GetSelectedM3UInfo(List<string> selectedItems)
        {
            // Clear List to avoid previous, irrelevant data
            selectedM3U.Clear();
            foreach (FileInfo m3u in M3Ufiles)
            {
                foreach (string item in selectedItems)
                {
                    Match matchM3U = Regex.Match(m3u.Name.ToString(), item + ".m3u", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (matchM3U.Success)
                    {
                        selectedM3U.Add(m3u);
                    }
                }
            }

            List<FileInfo> returnSelectedM3U = selectedM3U.ToList();
            return returnSelectedM3U;
        }

        ///////// FOR: M3UItem & M3USingleItem CLASS //////////
        ///// Read full M3U file with comments
        public List<string> ReadM3U(FileInfo m3u)
        {
            List<string> Content = new List<string>();
            string[] content = File.ReadAllLines(m3u.FullName.ToString());
            foreach (string line in content)
            {
                Content.Add(line);
            }
            return Content;
        }

        public List<string> HideComments(List<string> Content)
        {
            List<string> noComments = new List<string>();
            for (int i = 0; i < Content.Count; i++)
            {
                Match matchComment = Regex.Match(Content[i], @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!matchComment.Success)
                {
                    noComments.Add(Content[i]);
                }
            }

            return noComments;
        }

        protected Dictionary<string, string> HideCommentsDict(Dictionary<string, string> Content)
        {
            Dictionary<string, string> noComments = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> entry in Content)
            {
                Match matchComment = Regex.Match(entry.Key, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!matchComment.Success)
                {
                    noComments.Add(entry.Key, entry.Value);
                }
            }

            return noComments;
        }

        protected Dictionary<string, string> HideCommentsDict(List<string> Content)
        {
            Dictionary<string, string> noComments = new Dictionary<string, string>();
            for (int i = 0; i < Content.Count; i++)
            {
                Match matchComment = Regex.Match(Content[i], @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!matchComment.Success)
                {
                    noComments.Add(Content[i], Content[i]);
                }
            }

            return noComments;
        }

        // Overload
        //public ObservableCollection<M3USingleItem> HideComments(ObservableCollection<M3USingleItem> m3USingleItems)
        //{
        //    ObservableCollection<M3USingleItem> update = new ObservableCollection<M3USingleItem>();
        //    foreach (var item in m3USingleItems)
        //    {
        //        Match matchComment = Regex.Match(item.ContentLine, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //        if (!matchComment.Success)
        //        {
        //            update.Add(item);
        //        }
        //    }

        //    return update;
        //}

        ///// Hide path
        public List<string> HidePath(List<string> lines)
        {
            List<string> content = new List<string>();
            string[] lineSplit;
            for (int i = 0; i < lines.Count; i++)
            {
                Match matchComment = Regex.Match(lines[i], @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                // Just add comment
                if (matchComment.Success == true)
                {
                    content.Add(lines[i]);
                }
                // Get only file name
                else
                {
                    lineSplit = lines[i].Split(@"\".ToCharArray());
                    content.Add(lineSplit[lineSplit.Length - 1]);
                }
            }

            return content;
        }

        protected Dictionary<string, string> HidePathDict(List<string> lines)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            string[] lineSplit;

            for (int i = 0; i < lines.Count; i++)
            {
                Match matchComment = Regex.Match(lines[i], @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                // Just add comment
                if (matchComment.Success == true)
                {
                    content.Add(lines[i], lines[i]);
                }
                // Get only file name
                else
                {
                    lineSplit = lines[i].Split(@"\".ToCharArray());
                    content.Add(lines[i], lineSplit[lineSplit.Length - 1]);
                }
            }

                return content;
            
        }

        // Overload
        //public string HidePath(string line)
        //{
        //    Match matchComment = Regex.Match(line, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //    if (matchComment.Success == true)
        //    {
        //        return line;
        //    }
        //    else
        //    {
        //        string[] noPath = line.Split(@"\".ToCharArray());
        //        return noPath[0];
        //    }
        //}
        
        //public string DisplayContentLine(List<string> lines, LoadOptions LoadOptions)
        //{
        //    string finalLine = "";
        //    foreach (string fullLine in lines)
        //    {
        //        if (LoadOptions.ShowPath == true)
        //        {
        //            finalLine = fullLine;
        //        }
        //        else
        //        {
        //            finalLine = HidePath(fullLine);
        //        }
        //    }

        //    return finalLine;
        //}
        //public string DisplayContentLine(string fullLine, LoadOptions LoadOptions)
        //{
        //    // temp variable
        //    string finalLine = fullLine;
        //    if (LoadOptions.ShowPath == true)
        //    {
        //        finalLine = fullLine;
        //    }
        //    else
        //    {
        //        finalLine = HidePath(fullLine);
        //    }
            
        //    return finalLine;
        //}
        /// M3U file
        //public List<string> DisplayContentM3U(FileInfo m3uFile, LoadOptions LoadOptions)
        //{
        //    List<string> finalContent = new List<string>();
        //    List<string> fullContent = ReadM3U(m3uFile);
        //    /// ALL
        //    if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == true)
        //    {
        //        finalContent = fullContent;
        //    }
        //    /// FILE NAMES
        //    else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == false)
        //    {
        //        finalContent = HideComments(HidePath(fullContent));
        //    }
        //    /// FULL PATHS
        //    else if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == false)
        //    {
        //        finalContent = HideComments(fullContent);
        //    }
        //    /// COMMENTS
        //    else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == true)
        //    {
        //        finalContent = HidePath(fullContent);
        //    }

        //    return finalContent;
        //}

        // New method returning dictionary
        public Dictionary<string, string> DisplayContentM3Udict(FileInfo m3uFile, LoadOptions LoadOptions)
        {
            Dictionary<string, string> finalContent = new Dictionary<string, string>();         
            List<string> readContent = ReadM3U(m3uFile);

            /// ALL
            if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == true)
            {
                foreach (string line in readContent)
                {
                    finalContent.Add(line, line);
                }
            }
            /// FILE NAMES
            else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == false)
            {
                finalContent = HideCommentsDict(HidePathDict(readContent));
            }
            /// FULL PATHS
            else if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == false)
            {
                finalContent = HideCommentsDict(readContent);
            }
            /// COMMENTS
            else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == true)
            {

                finalContent = HidePathDict(readContent);
            }

            return finalContent;
        }

        // Whole M3U content indexation
        protected Dictionary<int, string> RawContentIndex(List<string> RawContent)
        {
            Dictionary<int, string> IndexedRawContent = new Dictionary<int, string>();

            if (RawContent.Count > 0)
            {
                IndexedRawContent = RawContent.Select((val, index) => new { Index = index, Value = val })
               .ToDictionary(i => i.Index, i => i.Value);
            }

            return IndexedRawContent;
        }
    }
}