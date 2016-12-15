using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Reflection;
using Microsoft.Win32;
using System.Diagnostics;

namespace WpfBalls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Emgu.CV.Image<Gray, Byte> m_RawImage;
        public Emgu.CV.Image<Rgb, Byte> m_ResultImage;
        private bool m_bShowOver = true;
        private String[] m_Files;
        private uint m_FileIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //Mat image = new Mat(100, 400, DepthType.Cv8U, 3);
            //image.SetTo(new Bgr(255, 255, 255).MCvScalar);
            //CvInvoke.PutText(image, "Hello, world", new System.Drawing.Point(10, 50), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3.0, new Bgr(255.0, 0.0, 0.0).MCvScalar);

            // Build a list of posible operations
            Image<Gray,byte> img = new Image<Gray, byte>(10, 10);
            var mths = ReflectIImage.GetImageMethods(img);
            foreach(KeyValuePair<string,MethodInfo> m in mths)
            {
                Console.Write(m.Key + " : ");
                Console.WriteLine(m.Value);
            }
        }

        private void butFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Latter make true and get list
            openFileDialog.Filter = "Raw image (*.pgm)|*.pgm|png|*.png)| All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                m_Files = openFileDialog.FileNames;
                m_FileIndex = 0;
                scrFile.Maximum = m_Files.Count()-1;
                scrFile.Value = 0;
                InitNewSourceFile();
                //MainWnd.Title = System.IO.Path.GetFileName(m_Files.First());
                //m_RawImage = new Image<Gray, byte>(m_Files.First());
                //m_bShowOver = false;
                //m_ResultImage = null;
                //PrepareSource();
            }

        }

        private void butAction_Click(object sender, RoutedEventArgs e)
        {
            DoScript();
            PrepareSource();
        }

        private void PrepareSource()
        {
            // Blit together the processed image onto the starting image
            // Or any other combinations of options yet to be thought of
            if(m_bShowOver)
            {
                BitmapSource bmpRaw = BitmapSourceConvert.ToBitmapSource(m_RawImage);
                if(m_ResultImage != null)
                {
                    Image<Rgb, byte> raw = m_RawImage.Convert<Rgb,byte>();
                    raw = raw.Or(m_ResultImage);
                    DispImage.Source =  BitmapSourceConvert.ToBitmapSource(raw);
                }
                else
                    DispImage.Source = bmpRaw;
            }
            else
                DispImage.Source = BitmapSourceConvert.ToBitmapSource(m_RawImage);
        }

        private void butToggleRes_Click(object sender, RoutedEventArgs e)
        {
            m_bShowOver = !m_bShowOver;
            PrepareSource();
        }

        private void InitNewSourceFile()
        {
            MainWnd.Title = System.IO.Path.GetFileName(m_Files[m_FileIndex]);
            m_RawImage = new Image<Gray, byte>(m_Files[m_FileIndex]);
            m_bShowOver = false;
            m_ResultImage = null;
            PrepareSource();
        }

        private void DoScript()
        {
            // To be designed 
            // Intention to run a "flexible" sequence of operations
            // For now hard code
            Mat outImg = new Mat();
            Mat blank = new Mat(m_RawImage.Height, m_RawImage.Width, DepthType.Cv8S, 1);
            Mat blank2 = new Mat(m_RawImage.Height, m_RawImage.Width, DepthType.Cv8S, 1);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            CvInvoke.Canny(m_RawImage, outImg, 120, 60);
            //var c = CvInvoke.HoughCircles(m_RawImage, HoughType.Gradient, 1, 20, 95, 20, 20, 150);
            DispImage.Source = BitmapSourceConvert.ToBitmapSource(outImg);
            sw.Stop();
            long t = sw.ElapsedMilliseconds;
            MainWnd.Title += " " + t + "ms";
            //m_ResultImage = outImg.ToImage<Gray, byte>();
            Image<Gray, byte>[] data = new Image<Gray, byte>[3];
            data[1] = outImg.ToImage<Gray, byte>();
            data[2] = blank.ToImage<Gray, byte>();
            data[0] = blank.ToImage<Gray, byte>();
            m_ResultImage = new Image<Rgb, byte>(data);
            m_bShowOver = true;
        }

        private void scrFile_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            uint n = Convert.ToUInt16( e.NewValue);
            if (n < m_Files.Count())
            {
                m_FileIndex = n;
                InitNewSourceFile();
                // If script active/valid
                DoScript();
                PrepareSource();
            }
        }

    }
}
