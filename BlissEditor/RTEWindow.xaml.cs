using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;

namespace BlissEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RTEWindow : Window
    {

        /* Code below is from https://wpf-tutorial.com/rich-text-controls/how-to-creating-a-rich-text-editor/ */
        public RTEWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            if (temp.ToString() == "{DependencyProperty.UnsetValue}")
            {
                cmbFontSize.Text = " ";
            }
            else
            {
                cmbFontSize.Text = temp.ToString();
            }

        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void Insert_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Builder (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                var clipboardData = Clipboard.GetDataObject();
                //BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                Uri uri = new Uri(openFileDialog.FileName, UriKind.Absolute);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = uri;

                bitmapImage.DecodePixelHeight = 200;
                bitmapImage.DecodePixelWidth = 200;

                bitmapImage.EndInit();
                Clipboard.SetImage(bitmapImage);
                rtbEditor.Paste();
                Clipboard.SetDataObject(clipboardData);

            }
        }

        private void Insert_Table(object sender, RoutedEventArgs e)
        {
            rtbEditor.BeginChange();
            var table = new Table();
            var gridLenghtConvertor = new GridLengthConverter();
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());
            table.Columns.Add(new TableColumn());


            table.RowGroups.Add(new TableRowGroup());
            for (int i = 0; i < 3; i++)
            {
                table.RowGroups[0].Rows.Add(new TableRow());
                table.RowGroups[0].Rows[i].Cells.Add(new TableCell(new Paragraph(new Run("Row" + (i + 1).ToString() + " Column1"))) /*{ BorderThickness = new Thickness(1), BorderBrush = Brushes.Black }*/);
                table.RowGroups[0].Rows[i].Cells.Add(new TableCell(new Paragraph(new Run("Row" + (i + 1).ToString() + " Column2"))) /*{ BorderThickness = new Thickness(1), BorderBrush = Brushes.Black }*/);
                table.RowGroups[0].Rows[i].Cells.Add(new TableCell(new Paragraph(new Run("Row" + (i + 1).ToString() + " Column3"))) /*{ BorderThickness = new Thickness(1), BorderBrush = Brushes.Black }*/);
            }
            rtbEditor.Document.Blocks.Add(table);
            rtbEditor.EndChange();
            rtbEditor.AppendText("newline");
        }
    }
}
