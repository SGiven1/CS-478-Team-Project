using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
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
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;
using Image = System.Windows.Controls.Image;
using System.Net;

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
            cmbFontFamily.ItemsSource = System.Windows.Media.Fonts.SystemFontFamilies.OrderBy(f => f.Source);
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
            cmbFontSize.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            cmbFontColor.SelectedItem = temp;
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

        private void cmbFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontColor.SelectedItem != null)
            {
               rtbEditor.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, cmbFontColor.SelectedItem);
            }
        }

        private void Insert_Image(object sender, RoutedEventArgs e)
        {
            //Code below and for Image_Height and TextRangeExt class used from:
            /* https://stackoverflow.com/questions/72234599/how-to-dynamically-resize-image-in-richtextbox-with-event-or-zooming */
            var image = new System.Windows.Controls.Image();
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp;*.jpg;*.gif)|*.bmp;*.jpg;*.gif|All files (*.*)|*.* "
            };
            if (dlg.ShowDialog() == true)
            {
                var imgsrc = new BitmapImage();
                imgsrc.BeginInit();
                imgsrc.StreamSource = File.Open(dlg.FileName, FileMode.Open);
                imgsrc.EndInit();

                image.Source = imgsrc; 

                var para = new Paragraph();
                para.Inlines.Add(image);
                rtbEditor.Document.Blocks.Add(para);
            }
        }

        private void Insert_Table(object sender, RoutedEventArgs e)
        {
            // Maximum number of rows or cols that can be created.
            int maxCount = 30;

            BlissEditor.InsertTable tableDialog = new InsertTable();
            tableDialog.ShowDialog();

            int rowCount = Convert.ToInt32(InsertTable.Rows);
            int colCount = Convert.ToInt32(InsertTable.Cols);

            if (rowCount > maxCount || colCount > maxCount)
            {
                MessageBox.Show("Try a number below 30!");
                return;
            }

            rtbEditor.BeginChange();
            var table = new Table();

            // Customize table theme
            table.Background = Brushes.LightGray;
            table.BorderBrush = Brushes.DarkGray;
            table.BorderThickness = new Thickness(1);

            for (int i = 0; i < colCount; i++)
            {
                table.Columns.Add(new TableColumn());
            }

            table.RowGroups.Add(new TableRowGroup());
            for (int i = 0; i < rowCount; i++)
            {
                var row = new TableRow();
                for (int j = 0; j < colCount; j++)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run($"Row {i + 1} Column {j + 1}")));

                    // Customize cell borders
                    cell.BorderBrush = Brushes.Black;
                    cell.BorderThickness = new Thickness(1);

                    row.Cells.Add(cell);
                }
                table.RowGroups[0].Rows.Add(row);
            }

            rtbEditor.Document.Blocks.Add(table);

            // Add a newline (new paragraph) after the table
            rtbEditor.Document.Blocks.Add(new Paragraph());
            rtbEditor.EndChange();
        }

        private void Export_PDF(object sender, RoutedEventArgs e)
        {
            /* https://github.com/QuestPDF/QuestPDF/blob/97153ad83d36f1174f51d981a5131948661aea4a/Source/QuestPDF.Examples/LicenseSetup.cs#L7 */
            QuestPDF.Settings.License = LicenseType.Community;

            if (rtbEditor != null)
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4); // may want different options, but defaults to standard A4
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                            x.Item().Text(range.Text); // only plaintext for right now
                        });
                    });
                });

                var pdfContent = document.GeneratePdf();
                var filePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\BlissExport.pdf";
                System.IO.File.WriteAllBytes(filePath, pdfContent);
                MessageBox.Show("Exported: " + filePath);
            }
        }

        private void NewRtfNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create new tab item
            TabItem newNoteTab = new TabItem();
            // Create RichText box
            RichTextBox richTextBox = new RichTextBox();

            // Name tab and set content
            newNoteTab.Header = "New RTF Note";
            newNoteTab.Content = richTextBox;

            // Add the new TabItem to the TabControl
            RTETabControl.Items.Add(newNoteTab);
        }

        private void NewMdNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create a new TabItem
            TabItem newMdNoteTab = new TabItem();

            // Create a Grid to contain RichTextBox and WebBrowser
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            // Create a RichTextBox for Markdown editing
            RichTextBox mdRichTextBox = new RichTextBox();
            mdRichTextBox.TextChanged += TextBox_TextChanged;

            // Create a WebBrowser for rendering HTML
            WebBrowser mdWebBrowser = new WebBrowser();

            // Add the RichTextBox and WebBrowser to the Grid
            Grid.SetColumn(mdRichTextBox, 0);
            Grid.SetColumn(mdWebBrowser, 1);
            grid.Children.Add(mdRichTextBox);
            grid.Children.Add(mdWebBrowser);

            // Set the content and header of the TabItem
            newMdNoteTab.Header = "New Markdown Note";
            newMdNoteTab.Content = grid;

            // Add the new TabItem to the TabControl
            RTETabControl.Items.Add(newMdNoteTab);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Handle Markdown text changed event
            RichTextBox mdRichTextBox = (RichTextBox)sender;

            // Get the Markdown content
            var md = new TextRange(mdRichTextBox.Document.ContentStart, mdRichTextBox.Document.ContentEnd).Text;

            // Convert Markdown to HTML and display it in the WebBrowser
            var html = Markdig.Markdown.ToHtml(md);
            try
            {
                // Assume every element is properly set and exists
                if (mdRichTextBox.Parent is Grid grid && grid.Children.Count > 1 && grid.Children[1] is WebBrowser webBrowser)
                {
                    webBrowser.NavigateToString(html);
                }
            }
            // Add boilerplate if nothing is in richtext box
            catch (System.ArgumentNullException)
            {
                webBrowser.NavigateToString("<html><body></body></html>");
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        //private void Image_Height(object sender, MouseButtonEventArgs e)
        //{
        //    if (sender is RichTextBox rtbEditor && !rtbEditor.Selection.IsEmpty)
        //    {
        //        foreach (System.Windows.Controls.Image img in rtbEditor.Selection.FindImages())
        //        {
        //            img.Height *= 1.25;
        //        }
        //    }
        //}
    }

    //public static class TextRangeExt
    //{
    //    public static IList<Image> FindImages(this TextRange range)
    //    {
    //        IList<Image> images = new List<Image>();
    //        for (var position = range.Start;
    //            position != null && position.CompareTo(range.End) <= 0;
    //            position = position.GetNextContextPosition(LogicalDirection.Forward))
    //        {
    //            if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart
    //                && position.GetAdjacentElement(LogicalDirection.Forward) is InlineUIContainer uic && uic.Child is Image img)
    //            {
    //                images.Add(img);
    //            }
    //        }
    //        return images;
    //    }
    //}
}
