using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Data;
using Microsoft.Win32;
using System.Windows.Forms.VisualStyles;
using System.Runtime.CompilerServices;

namespace PlaylistMain
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            editM3U.ItemsSource = M3USingleItems;
        }

        ///// OBJECTS /////
        PathFinder PathFinder = new PathFinder();
        M3U M3U = new M3U();
        LoadOptions LoadOptions = new LoadOptions();
        ///////////////////

        /// VARIABLES ////
        public static ObservableCollection<M3USingleItem> M3USingleItems = new ObservableCollection<M3USingleItem>();
        public static List<M3UItem> M3UItems = new List<M3UItem>();
        /////////////////

        ///// METHODS /////
        // Reload ListBox with filenames
        private void reloadListBox(PathFinder PathFinder, M3U M3U, ListBox filesListBox)
        {
            // Get path to certain day
            string path = PathFinder.GetPath();
            // Get m3u files info (name with ext, full path etc.)
            M3U.FilesInfoList(path);
            // Converting List<string> to array
            string[] files = M3U.FileNameNoExt(M3U.M3Ufiles).ToArray();
            // Prepare listbox and fill it
            filesListBox.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                filesListBox.Items.Add(files[i]);
            }
        }

        // Load today folder as default
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            reloadListBox(PathFinder, M3U, filesListBox);
        }

        // Get selected M3U
        public List<string> selectedItems()
        {
            // Linq used - https://stackoverflow.com/questions/22950545/fill-string-array-with-listbox-selected-items-in-c-sharp
            List<string> items = (from string s in filesListBox.SelectedItems select s).ToList();

            return items;
        }

        /////////// MOVED TO LOAD CLASS
        //// Display selected M3U(s) content
        private void loadM3U()
        {
            M3USingleItems.Clear();
            M3UItems.Clear();
            List<FileInfo> selectedM3Uinfo = M3U.GetSelectedM3UInfo(selectedItems());
            // Class M3UItem to create objects to be bind to ListBox
            foreach (FileInfo file in selectedM3Uinfo)
            {
                M3UItem m3UItem = new M3UItem(file, LoadOptions);
                M3UItems.Add(m3UItem);

                foreach (string line in m3UItem.FormatedContent)
                {
                    M3USingleItems.Add(new M3USingleItem(line, m3UItem.Name));
                }
            }

            btnLoadFile.IsEnabled = true;
        }
        /////////// MOVED TO LOAD CLASS
        /// CRAP - NEED TO BE CORRECTED
        // TODO: Change to private after test?
        public void loadFile()
        {
            // https://www.c-sharpcorner.com/UploadFile/mahesh/openfiledialog-in-wpf/
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Multiselect = true;
            FileDialog.DefaultExt = ".m3u";
            FileDialog.Filter = "M3U (.m3u)|*.m3u|All files (*.*)|*.*";
            bool? result = FileDialog.ShowDialog();
            //... equals: Nullable<bool> result = LoadFile.ShowDialog();

            if (result == true)
            {
                foreach (string path in FileDialog.FileNames)
                {
                    M3USingleItems.Add(new M3USingleItem(/*FileDialog.FileName*/path, "AddedFile", M3UItems[0].LoadSnapshot["ShowComments"]));
                }
            }
        }
        /////////////////////////


        //////// CONTROLS ////////

        ///// CALENDAR /////
        private void monthCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
            {
                // To get day.month.year only: 01.02.2020
                PathFinder.date = monthCalendar.SelectedDate.ToString().Substring(0, 11);
                reloadListBox(PathFinder, M3U, filesListBox);
            }

        ///// RADIO /////
        private void radioAgora_Checked(object sender, RoutedEventArgs e)
        {
            PathFinder.client = Agora.Name;
            reloadListBox(PathFinder, M3U, filesListBox);
        }

        private void radioEurozet_Checked(object sender, RoutedEventArgs e)
        {
            PathFinder.client = Eurozet.Name;
            reloadListBox(PathFinder, M3U, filesListBox);
        }

        private void radioLokalne_Click(object sender, RoutedEventArgs e)
        {
            PathFinder.client = Lokalne.Name;
            reloadListBox(PathFinder, M3U, filesListBox);
        }

        ///// BUTTONS /////
        private void btnLoadM3U_Click(object sender, RoutedEventArgs e)
        {
            loadM3U();
        }

        ///// LISTVIEW /////
        private void filesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                btnLoadM3U.IsEnabled = true;
            }

        ///// CHECKBOXES | LOAD OPTIONS /////
        /// Show Comments
        private void showComments_Checked(object sender, RoutedEventArgs e)
        {
            LoadOptions.ShowComments = (bool)showComments.IsChecked;
        }

        /// Show Path
        private void showPath_Checked(object sender, RoutedEventArgs e)
        {
            LoadOptions.ShowPath = (bool)showPath.IsChecked;
        }


        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            loadFile();
        }
    }
}

