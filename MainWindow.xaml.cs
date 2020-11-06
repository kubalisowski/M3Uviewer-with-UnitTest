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
            groupBox.Header = AppStrings.Items["PathsHeader"];
            Agora.Content = AppStrings.Items["Path1"];
            Eurozet.Content = AppStrings.Items["Path2"];
            Lokalne.Content = AppStrings.Items["Path3"];
            btnLoadM3U.Content = AppStrings.Items["LoadM3U"];
            showComments.Content = AppStrings.Items["ShowComments"];
            showPath.Content = AppStrings.Items["ShowPath"];
            btnLoadFile.Content = AppStrings.Items["LoadFile"];
            btnSave.Content = AppStrings.Items["Save"];
            btnUp.Content = AppStrings.Items["Up"];
            btnDown.Content = AppStrings.Items["Down"];
            btnRemove.Content = AppStrings.Items["Remove"];
        }

        ///// OBJECTS /////
        PathFinder PathFinder = new PathFinder();
        M3U M3U = new M3U();
        LoadOptions LoadOptions = new LoadOptions();
        SaveM3U SaveM3U = new SaveM3U();
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

                foreach (KeyValuePair<string, string> entry in m3UItem.FormatedContent)
                {
                    M3USingleItems.Add(new M3USingleItem(entry.Key, entry.Value, m3UItem.Name));
                }

                //foreach (string line in m3UItem._FormatedContent)
                //{
                //    M3USingleItems.Add(new M3USingleItem(line, m3UItem.Name));
                //}
            }

            btnLoadFile.IsEnabled = true;
        }

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
                    M3USingleItems.Add(new M3USingleItem(/*FileDialog.FileName*/path, "AddedFile", M3UItems[0].LoadSnapshot["ShowPath"]));
                }
            }
        }

        private void MoveItem(string direction)
        {
            if (editM3U.SelectedIndex == -1 || editM3U.Items.Count < 2)
            {
                btnUp.IsEnabled = false;
                btnDown.IsEnabled = false;
                return;
            }

            var selectedIndex = editM3U.SelectedIndex;
            var itemToMove = M3USingleItems[selectedIndex];

            void Move(int directValue)
            {
                M3USingleItems.RemoveAt(selectedIndex);
                M3USingleItems.Insert(selectedIndex + directValue, itemToMove);
                editM3U.SelectedIndex = selectedIndex + directValue;
            }

            switch (direction)
            {
                case "up":
                    if (selectedIndex > 0)
                    {
                        Move(-1);
                    }
                    break;

                case "down":
                    if (selectedIndex + 1 < M3USingleItems.Count)
                    {
                        Move(1);
                    }
                    break;
            }
        }

        private void RemoveItem()
        {
            if (editM3U.SelectedIndex == -1)
            {
                return;
            }           
            else if (editM3U.Items.Count > 0)
            {
                var selectedIndex = editM3U.SelectedIndex;
                M3USingleItems.RemoveAt(selectedIndex);
                editM3U.SelectedIndex = selectedIndex - 1;
            }
            //else
            //{
            //    btnRemove.IsEnabled = false;
            //}
        }
        //////////////////////////

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
            btnSave.IsEnabled = true;
            fileLocationInfoBox.Text = M3UItems[0].M3UInfo.FullName;

        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            loadFile();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (editM3U.SelectedIndex != 1)
                btnUp.IsEnabled = true;

            MoveItem("up");
            // UP
            //var selectedIndex = editM3U.SelectedIndex;

            //if (selectedIndex > 0)
            //{
            //    var itemToMoveUp = M3USingleItems[selectedIndex];
            //    M3USingleItems.RemoveAt(selectedIndex);
            //    M3USingleItems.Insert(selectedIndex - 1, itemToMoveUp);
            //    editM3U.SelectedIndex = selectedIndex - 1;
            //}
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (editM3U.SelectedIndex != 1)
                btnDown.IsEnabled = true;

            MoveItem("down");
            // DOWN
            //var selectedIndex = editM3U.SelectedIndex;

            //if (selectedIndex + 1 < M3USingleItems.Count)
            //{
            //    var itemToMoveDown = M3USingleItems[selectedIndex];
            //    M3USingleItems.RemoveAt(selectedIndex);
            //    M3USingleItems.Insert(selectedIndex + 1, itemToMoveDown);
            //    editM3U.SelectedIndex = selectedIndex + 1;
            //}
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveM3U.Save(M3UItems[0], M3USingleItems);
            msgBox.Text = AppStrings.Items["FileSaved"] + DateTime.Now.ToString("HH:mm");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //if (M3USingleItems.Count < 1)
            //{
            //    btnUp.IsEnabled = false;
            //    btnDown.IsEnabled = false;
            //}
            //if (editM3U.SelectedIndex == -1)
            //{
            //    btnRemove.IsEnabled = false;
            //}
            if (editM3U.Items.Count < 1)
            {
                btnRemove.IsEnabled = false;
            }
            RemoveItem();
        }

        ///// LISTVIEW | LISTBOX /////
        private void filesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                btnLoadM3U.IsEnabled = true;
            }
        private void editM3U_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUp.IsEnabled = true;
            btnDown.IsEnabled = true;
            btnRemove.IsEnabled = true;
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

        /// TEXTBOX
        
        private void fileLocationInfoBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            Clipboard.SetText(fileLocationInfoBox.Text);
            MessageBox.Show("CHUJ!");
        }

        private void fileLocationInfoBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

