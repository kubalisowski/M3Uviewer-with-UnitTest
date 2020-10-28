using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMain
{
    public class LoadOptions
    {
        private bool showComments;
        public bool ShowComments
        {
            get
            {
                return showComments;
            }
            set
            {
                showComments = value;
            }
        }

        private bool showPath;
        public bool ShowPath
        {
            get
            {
                return showPath;
            }
            set
            {
                showPath = value;
            }
        }

        public LoadOptions()
        {
            // Defaults; unchecked in UI by default
            ShowComments = false;
            ShowPath = false;
        }
        // Overload for single file load (M3UItem -> LoadSnapshot property)
        public LoadOptions(bool? showComments, bool? showPath)
        {
            // Defaults; unchecked in UI by default
            ShowComments = (bool)showComments;
            ShowPath = (bool)showPath;
        }
    }
}
