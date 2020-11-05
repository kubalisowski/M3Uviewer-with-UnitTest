using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlaylistMain
{
    public class SaveM3U
    {
        public void Save(M3UItem M3U, ObservableCollection<M3USingleItem> singleItems)
        {
            StreamWriter Stream = new StreamWriter(M3U.M3UInfo.FullName);

            for (int i = 0; i < singleItems.Count; i++)
            {
                if (i == singleItems.Count - 1)
                {
                    Stream.Write(singleItems[i].fullPath);
                }
                else
                {
                    Stream.Write(singleItems[i].fullPath + "\n");
                }
            }

            Stream.Close();
        }
    }
}
