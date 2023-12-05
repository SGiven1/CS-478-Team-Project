using System.Windows;

namespace BlissEditor
{
    /// <summary>
    /// Interaction logic for InsertTable.xaml
    /// </summary>
    public partial class InsertTable : Window
    {
        public static string Rows, Cols;
        public InsertTable()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptTableBtn_Click(object sender, RoutedEventArgs e)
        {
            Rows = rowsTextBox.Text;
            Cols = colTextBox.Text;
            Close();
        }
    }
}
