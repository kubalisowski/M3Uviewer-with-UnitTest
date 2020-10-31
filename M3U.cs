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
        // Overload
        public ObservableCollection<M3USingleItem> HideComments(ObservableCollection<M3USingleItem> m3USingleItems)
        {
            ObservableCollection<M3USingleItem> update = new ObservableCollection<M3USingleItem>();
            foreach (var item in m3USingleItems)
            {
                Match matchComment = Regex.Match(item.ContentLine, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!matchComment.Success)
                {
                    update.Add(item);
                }
            }

            return update;
        }

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
        // Overload
        public string HidePath(string line)
        {
            Match matchComment = Regex.Match(line, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (matchComment.Success == true)
            {
                return line;
            }
            else
            {
                string[] noPath = line.Split(@"\".ToCharArray());
                return noPath[0];
            }
        }
        


        /////// Combine above
        ////// Single file
        /// TODO: BUG!
        /// BUG HERE + PROPER DISPLAY (LOADPTIONS) - EXCEL IN RESOURCES
        //public string DisplayContentLine(string fullLine, LoadOptions LoadOptions)
        //{
        //    // temp variable
        //    string finalLine = fullLine;
        //    List<string> fullContent = new List<string>();
        //    fullContent.Add(fullLine);
        //    /// ALL
        //    //Console.WriteLine(LoadOptions.ShowComments);
        //    //Console.WriteLine(LoadOptions.ShowPath);
        //    if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == true)
        //    {
        //        finalLine = fullContent[0];
        //        //Console.WriteLine(fullContent[0]);
        //    }
        //    /// NO PATH NO COMMENT
        //    /// TODO: There is some bug; to be fixed
        //    else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == false)
        //    {
        //        var tmp = HidePath(fullContent);
        //        //Console.WriteLine(tmp.Count());
        //        var tmp2 = HideComments(tmp);
        //        Console.WriteLine(tmp2.Count());
        //        if (tmp2.Count() > 0)
        //        {
        //            foreach (string item in tmp2)
        //            {
        //                finalLine = item;
        //                Console.WriteLine(item);
        //            }
        //        }
        //    }
        //    /// FULL PATHS
        //    /// /// TODO: There is some bug; to be fixed
        //    else if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == false)
        //    {
        //        fullLine = HideComments(fullContent)[0];
        //    }
        //    /// COMMENTS
        //    else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == true)
        //    {
        //        fullLine = HidePath(fullContent)[0];
        //    }

        //    return fullLine;
        //}
        public string DisplayContentLine(List<string> lines, LoadOptions LoadOptions)
        {
            string finalLine = "";
            foreach (string fullLine in lines)
            {
                if (LoadOptions.ShowPath == true)
                {
                    finalLine = fullLine;
                }
                else
                {
                    finalLine = HidePath(fullLine);
                }
            }

            return finalLine;
        }
        public string DisplayContentLine(string fullLine, LoadOptions LoadOptions)
        {
            // temp variable
            string finalLine = fullLine;
            if (LoadOptions.ShowPath == true)
            {
                finalLine = fullLine;
            }
            else
            {
                finalLine = HidePath(fullLine);
            }
            /// ALL
            //if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == true)
            //{
            //    finalLine = fullLine;
            //}
            ///// NO PATH NO COMMENT
            ////else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == false)
            ////{
            ////    var finalLine = HidePath(fullLine);
            ////    //finalLine = HideComments(tmp);
            ////}
            ///// YES PATH NO COMMENTS
            //else if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == false)
            //{
            //    finalLine = HideComments(fullLine);
            //}
            ///// NO PATH YES COMMENTS
            //else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == true)
            //{
            //    finalLine = HidePath(fullLine);
            //}

            return finalLine;
        }
        /// M3U file
        public List<string> DisplayContentM3U(FileInfo m3uFile, LoadOptions LoadOptions)
        {
            List<string> finalContent = new List<string>();
            List<string> fullContent = ReadM3U(m3uFile);
            /// ALL
            if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == true)
            {
                finalContent = fullContent;
            }
            /// FILE NAMES
            else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == false)
            {
                finalContent = HideComments(HidePath(fullContent));
            }
            /// FULL PATHS
            else if (LoadOptions.ShowPath == true && LoadOptions.ShowComments == false)
            {
                finalContent = HideComments(fullContent);
            }
            /// COMMENTS
            else if (LoadOptions.ShowPath == false && LoadOptions.ShowComments == true)
            {
                finalContent = HidePath(fullContent);
            }

            return finalContent;
        }

        /// TBD ???
        /// Single file
        //public string DisplayContent(string line, LoadOptions LoadOptions)
        //{
        //    string ContentLine;
        //    List<string> tempContent = new List<string>();
        //    List<string> tempLine = new List<string>();
        //    tempLine.Add(line);

        //    /// ALL
        //    if (LoadOptions.ShowPath == true)
        //    {
        //        tempContent.Add(line);
        //    }
        //    /// FILE NAME
        //    else if (LoadOptions.ShowPath == false)
        //    {
        //        tempContent = HideComments(HidePath(tempLine));
        //    }

        //    ContentLine = tempContent[0];
        //    return ContentLine;
        //}
        ///////// Above method overload for each cases mirror List<string> DisplayContent


        //private Dictionary<int, string> CommentIndexer(ObservableCollection<M3USingleItem> actualItemsM3U, LoadOptions LoadOptions, List<M3UItem> M3UItems)
        //{
        //    /// <index, comment> -- comments to be put at the same indexes as they are originally in M3U file
        //    Dictionary<int, string> indexComment = new Dictionary<int, string>();
        //    /// Only one M3U file can be edited at the time, but the list is used for further changes
        //    foreach (M3UItem M3U in M3UItems)
        //    {
        //        foreach (string line in M3U.RawContent)
        //        {
        //            Match matchComment = Regex.Match(line, @"#.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //            if (matchComment.Success)
        //            {
        //                indexComment.Add(M3U.RawContent.IndexOf(line), line);
        //            }
        //        }
        //    }

        //    return indexComment;
        //}


        ///// Re-load M3U displayed content
        //public ObservableCollection<M3USingleItem> ReloadEditBox(ObservableCollection<M3USingleItem> ObsItemsM3U, LoadOptions LoadOptions, List<M3UItem> M3UItems)
        //{
        //    ObservableCollection<M3USingleItem> ReloadedItems = new ObservableCollection<M3USingleItem>();
        //    /// <index, comment> -- comments to be put at the same list indexes as they are originally in M3U file
        //    Dictionary<int, string> indexComment = new Dictionary<int, string>();

        //    foreach (M3USingleItem single_item in ObsItemsM3U)
        //    {
        //        /// Loaded file
        //        if (single_item.Name == "AddedFile")
        //        {
        //            single_item.ContentLine = DisplayContentLine(single_item.fullPath, LoadOptions);
        //            ReloadedItems.Add(single_item);
        //        }
        //        /// M3U origin
        //        else
        //        {
        //            /// Show path
        //            if (LoadOptions.ShowPath == true)
        //            {
        //                single_item.ContentLine = DisplayContentLine(single_item.fullPath, LoadOptions);
        //                ReloadedItems.Add(single_item);
        //            }
        //            /// Restore comments - put them in the same list index as it is in M3U file
        //            if (LoadOptions.ShowComments == true)
        //            {
        //                indexComment = CommentIndexer(ObsItemsM3U, LoadOptions, M3UItems);
        //            }
        //        }
        //    }

        //    if (indexComment.Count() > 0)
        //    {
        //        foreach (var dict in indexComment)
        //        {
        //            ReloadedItems.Insert(dict.Key, new M3USingleItem(dict.Value, M3UItems[0].Name));
        //        }
        //    }

        //    return ReloadedItems;
        //}
    }
}
    ////////////////////////////////

