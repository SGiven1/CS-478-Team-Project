using System;
using System.Collections.ObjectModel;
using system.Windows.Media;

namespace  RTEWindow 
{

    class FontList : ObservableCollection<string> 
    {
        public FontList() 
        {
            foreach (FontFamily f in FontList.SystemFontFamilies) 
            {
                System.Drawing.Font font = new System.Drawing.Font(f.ToString(), 8);
                this.Add(font.Name);
            }
        }
    }
 
}