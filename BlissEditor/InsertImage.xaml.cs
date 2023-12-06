using System.Windows;

namespace BlissEditor
{
    /// <summary>
    /// Interaction logic for InsertTable.xaml
    /// </summary>
    public partial class InsertImage : Window
    {
        public static string Hght, Wdth;
        public InsertImage()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptTableBtn_Click(object sender, RoutedEventArgs e)
        {
            Hght = hghtTextBox.Text;
            Wdth = wdthTextBox.Text;
            Close();
        }
    }
}
