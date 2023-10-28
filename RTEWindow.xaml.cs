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
        	private enum EmfToWmfBitsFlags {

			EmfToWmfBitsFlagsDefault = 0x00000000,

			EmfToWmfBitsFlagsEmbedEmf = 0x00000001,

			EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,

			EmfToWmfBitsFlagsNoXORClip = 0x00000004
		};


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
            cmbFontSize.Text = temp.ToString();
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

        public static void SetInnerMargins(this TextBoxBase textBox, int left, int top, int right, int bottom)
        {
            var rect = textBox.GetFormattingRect();

            var newRect = new Rectangle(left, top, rect.Width - left - right, rect.Height - top - bottom);
            textBox.SetFormattingRect(newRect);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }

        [DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [DllImport(@"user32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref Rectangle lParam);

        private const int EmGetrect = 0xB2;
        private const int EmSetrect = 0xB3;

        private static void SetFormattingRect(this TextBoxBase textbox, Rectangle rect)
        {
            var rc = new RECT(rect);
            SendMessageRefRect(textbox.Handle, EmSetrect, 0, ref rc);
        }   

        private static Rectangle GetFormattingRect(this TextBoxBase textbox)
        {
            var rect = new Rectangle();
            SendMessage(textbox.Handle, EmGetrect, (IntPtr) 0, ref rect);
            return rect;
        }

        private struct RtfColorDef 
        {
			public const string Black = @"\red0\green0\blue0";
            public const string Maroon = @"\red128\green0\blue0";
			public const string Green = @"\red0\green128\blue0";
			public const string Olive = @"\red128\green128\blue0";
			public const string Navy = @"\red0\green0\blue128";
			public const string Purple = @"\red128\green0\blue128";
			public const string Teal = @"\red0\green128\blue128";
			public const string Gray = @"\red128\green128\blue128";
			public const string Silver = @"\red192\green192\blue192";
			public const string Red = @"\red255\green0\blue0";
			public const string Lime = @"\red0\green255\blue0";
			public const string Yellow = @"\red255\green255\blue0";
			public const string Blue = @"\red0\green0\blue255";
			public const string Fuchsia = @"\red255\green0\blue255";
			public const string Aqua = @"\red0\green255\blue255";
			public const string White = @"\red255\green255\blue255";
		}

        public ExRichTextBox(RtfColor _textColor) : this() 
        {
			textColor = _textColor;
		}

		private struct RtfFontFamilyDef 
        {
			public const string Unknown = @"\fnil";
			public const string Roman = @"\froman";
			public const string Swiss = @"\fswiss";
			public const string Modern = @"\fmodern";
			public const string Script = @"\fscript";
			public const string Decor = @"\fdecor";
			public const string Technical = @"\ftech";
			public const string BiDirect = @"\fbidi";
		}

        public void AppendTextAsRtf(string _text, Font _font) 
        {
			AppendTextAsRtf(_text, _font, textColor);
		}

        public void InsertTExtAsRtf(string _text) 
        {
            InsertTExtAsRtf(_text, this.Font);
        }

        private void Insert_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Builder (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg]";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                var clipboardData = Clipboard.GetDataObject();
                //BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                Uri uri = new Uri(openFileDialog.FileName, UriKind.Absolute);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = uri;

                bitmapImage.DecodePixelHeight = 150;
                bitmapImage.DecodePixelWidth = 200;

                bitmapImage.EndInit();
                Clipboard.SetImage(bitmapImage);
                rtbEditor.Paste();
                Clipboard.SetDataObject(clipboardData);

            }
        }

        /// <summary>
        /// All of the insert image methods below came from a previous GitHub project.
        /// The repository of information is linked below for credit and reference.
        /// https://github.com/brunurd/RTFExporter
        /// The file used specifically was the ExRichTextBox.cs.
        /// </summary>
        /// <param name="_image"></param>

        public void InsertImage(Image _image) {

			StringBuilder _rtf = new StringBuilder();

			_rtf.Append(RTF_HEADER);

			_rtf.Append(GetFontTable(this.Font));

			_rtf.Append(InsertJpgImage(_image));

			_rtf.Append(RTF_IMAGE_POST);

			this.SelectedRtf = _rtf.ToString();
		}

		private static string GetWidthAndHeight(Image image, float dpiX, float dpiY)
		{
			float width = (float)image.Width / dpiX;
			float height = (float)image.Height / dpiY;

			int picw = (int)(width * 2540);
			int pich = (int)(height * 2540);

			int picwgoal = (int)(width * 1440);
			int pichgoal = (int)(height * 1440);

			return "\\picw" + picw + "\\pich" + pich + "\\picwgoal" + picwgoal + "\\pichgoal" + pichgoal;
		}

        public static string InsertJpgImage(Image image)
		{
			byte[] buffer;

			using (var stream = new MemoryStream())
			{
				image.Save(stream, ImageFormat.Jpeg);
				buffer = stream.ToArray();
			}

			string hex = BitConverter.ToString(buffer, 0).Replace("-", string.Empty);

			string widthAndHeight = GetWidthAndHeight(image, image.HorizontalResolution, image.VerticalResolution);

			return "{\\pict\\jpegblip" + widthAndHeight + " " + hex + "}";
		}

        private string GetImagePrefix(Image _image) {

			StringBuilder _rtf = new StringBuilder();

			int picw = (int)Math.Round((_image.Width / xDpi) * HMM_PER_INCH);

			int pich = (int)Math.Round((_image.Height / yDpi) * HMM_PER_INCH);

			int picwgoal = (int)Math.Round((_image.Width / xDpi) * TWIPS_PER_INCH);

			int pichgoal = (int)Math.Round((_image.Height / yDpi) * TWIPS_PER_INCH);

			_rtf.Append(@"{\pict\wmetafile8");
			_rtf.Append(@"\picw");
			_rtf.Append(picw);
			_rtf.Append(@"\pich");
			_rtf.Append(pich);
			_rtf.Append(@"\picwgoal");
			_rtf.Append(picwgoal);
			_rtf.Append(@"\pichgoal");
			_rtf.Append(pichgoal);
			_rtf.Append(" ");

			return _rtf.ToString();
		}

        private string GetRtfImage(Image _image) {

			StringBuilder _rtf = null;

			MemoryStream _stream = null;

			Graphics _graphics = null;

			Metafile _metaFile = null;

			IntPtr _hdc;

			try {
				_rtf = new StringBuilder();
				_stream = new MemoryStream();

				using(_graphics = this.CreateGraphics()) {

					_hdc = _graphics.GetHdc();

					_metaFile = new Metafile(_stream, _hdc);

					_graphics.ReleaseHdc(_hdc);
				}

				using(_graphics = Graphics.FromImage(_metaFile)) {

					_graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));

				}

				IntPtr _hEmf = _metaFile.GetHenhmetafile();
                uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, MM_ANISOTROPIC,
					EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

				byte[] _buffer = new byte[_bufferSize];

				uint _convertedSize = GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, MM_ANISOTROPIC,
					EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

				for(int i = 0; i < _buffer.Length; ++i) {
					_rtf.Append(String.Format("{0:X2}", _buffer[i]));
				}

				return _rtf.ToString();
			}
			finally {
				if(_graphics != null)
					_graphics.Dispose();
				if(_metaFile != null)
					_metaFile.Dispose();
				if(_stream != null)
					_stream.Close();
			}
		}

        public void InsertTable(int rows, int cols, int width)
		{
			StringBuilder sringTableRtf = new StringBuilder();

			sringTableRtf.Append(@"{\rtf1 ");

			int cellWidth;

			sringTableRtf.Append(@"\trowd");

			int inch_width = (int)Math.Round((width / xDpi) * HMM_PER_INCH);

			for (int i = 0; i < rows; i++)
            {
				sringTableRtf.Append(@"\trowd");

				for (int j = 0; j < cols; j++)
                {
					cellWidth = (j + 1) * inch_width;

					sringTableRtf.Append(@"\cellx" + cellWidth.ToString());
				}

				sringTableRtf.Append(@"\intbl \cell \row");
			}
			sringTableRtf.Append(@"\pard");
			sringTableRtf.Append(@"}");

			this.SelectedRtf = sringTableRtf.ToString();
		}
    }
}
