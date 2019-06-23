using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace paint
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string nazwaPliku = "";
        static System.Windows.Media.Color col;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void czysc_Click(object sender, RoutedEventArgs e)
        {
            obszar.Strokes.Clear();
        }
        public void EskportDoJPEG(InkCanvas obszar)
        {
            double
                    x1 = obszar.Margin.Left,
                    x2 = obszar.Margin.Top,
                    x3 = obszar.Margin.Right,
                    x4 = obszar.Margin.Bottom;

            obszar.Margin = new Thickness(0, 0, 0, 0);

            Size size = new Size(obszar.Width, obszar.Height);
            obszar.Measure(size);
            obszar.Arrange(new Rect(size));

            RenderTargetBitmap renderBitmap =
             new RenderTargetBitmap(
               (int)size.Width,
               (int)size.Height,
               96,
               96,
               PixelFormats.Default);
            renderBitmap.Render(obszar);

            otworzOknoDialogoZapisz();

            try
            {
                using (FileStream fs = File.Open(nazwaPliku, FileMode.Create))
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(fs);
                }
            }
            catch (Exception e)
            {
            }
            obszar.Margin = new Thickness(x1, x2, x3, x4);
        }
        private void zapisz_Click(object sender, RoutedEventArgs e)
        {
            {
                EskportDoJPEG(obszar);
            }
        }
        void otworzOknoDialogoZapisz()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "nazwa_pliku"; 
            dlg.DefaultExt = ".jpg"; 
            dlg.Filter = "Obrazek (.jpg)|*.jpg"; 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                nazwaPliku = dlg.FileName;
            }

        }
        void ChangeColor(InkCanvas inkCanvas, System.Windows.Media.Color color)
        {

            inkCanvas.DefaultDrawingAttributes.Color = Color.FromArgb(col.A, col.R, col.G, col.B);

        }

        private static void koloDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowDialog();
            col.A = colorDialog.Color.A;
            col.B = colorDialog.Color.B;
            col.G = colorDialog.Color.G;
            col.R = colorDialog.Color.R;

        }

        private void grubosc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grubosc.Items.Count > 0 && ((ComboBoxItem)grubosc.SelectedItem).Content != null)
            {
                obszar.DefaultDrawingAttributes.Width = Convert.ToDouble(((ComboBoxItem)grubosc.SelectedItem).Content);
                obszar.DefaultDrawingAttributes.Height = Convert.ToDouble(((ComboBoxItem)grubosc.SelectedItem).Content);
            }
        }

        private void kolor_Click(object sender, RoutedEventArgs e)
        {
            {
                koloDialog();
                ChangeColor(obszar, col);
            }
        }
    } }
