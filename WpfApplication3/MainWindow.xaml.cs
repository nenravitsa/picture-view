using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Forms;
using System.Windows.Threading;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bw;
        List<string> picName = new List<string>();
        List<BitmapImage> pics = new List<BitmapImage>();
        public MainWindow()
        {
            InitializeComponent();
            path.MouseDoubleClick += path_DoubleClick;
            picsListBox.MouseUp += picsListBox_MouseUp;
            slider.ValueChanged += Slider_ValueChanged;
            leftButton.Click += LeftButton_Click;
            rightButton.Click += RightButton_Click;
        }
        public void path_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog way = new FolderBrowserDialog();
            DialogResult result = way.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path.Text = way.SelectedPath;
                
                picName = (Directory.GetFiles(path.Text, "*jpg")).ToList();
                ShowImage(picName[0]);
                progress.Value = 0;
                progress.Maximum = picName.Count;
                bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += bw_DoWork;
                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerAsync();
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (container != null)
            {
                container.Height = slider.Value;
                container.Width = slider.Value;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            foreach (var path in picName)
            {
                pics.Add(new BitmapImage(new Uri(path)));
                ((BackgroundWorker)sender).ReportProgress(++i);
                Thread.Sleep(50);
            
            }

        }

        private void getMeta()
        {
            //Stream imageStreamSource = new FileStream(picsListBox.SelectedItem.ToString(), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //    BitmapDecoder decoder = BitmapDecoder.Create(imageStreamSource, BitmapCreateOptions.PreservePixelFormat,
            //        BitmapCacheOption.Default);
            //    InPlaceBitmapMetadataWriter pngInplace = decoder.Frames[0].CreateInPlaceBitmapMetadataWriter();

            //        if (pngInplace.CameraModel != null)
            //            about.Content = pngInplace.CameraModel.ToString();
            //        else
            //            about.Content = "not found meta";
            //imageStreamSource.Flush();
            //imageStreamSource.Close();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
            picsListBox.Items.Add(picName[e.ProgressPercentage - 1]);
        }

        private void ShowImage(string path)
        {
            BitmapImage newImage = new BitmapImage();
            newImage.BeginInit();
            newImage.UriSource = new Uri(path);
            newImage.EndInit();
            container.Source = newImage;
        }

        public void picsListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowImage(picsListBox.SelectedItem.ToString());
            getMeta();
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            var prev = picsListBox.SelectedIndex - 1;
            ShowImage((picsListBox.Items[prev]).ToString());
            picsListBox.SelectedIndex = prev;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            var next = 0;
            if ((picsListBox.SelectedIndex >= 0) && (picsListBox.SelectedIndex < (picsListBox.Items.Count - 1)))
            {
                next = picsListBox.SelectedIndex + 1;
                ShowImage((picsListBox.Items[next]).ToString());
                picsListBox.SelectedIndex = next;
            }

        }

    }
}
