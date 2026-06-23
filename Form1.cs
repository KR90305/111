using System.ComponentModel;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            slideshowTimer = new System.Windows.Forms.Timer();
            slideshowTimer.Interval = 2000; // 2 сек
            slideshowTimer.Tick += SlideshowTimer_Tick;
            this.CenterToScreen();
            trackBar.Minimum = 1;
            trackBar.Maximum = 30;
            trackBar.Value = 10; // начальный масштаб 1.0
            currentScale = trackBar.Value / 10f;
            trackBar.Scroll += TrackBar_Scroll;
        }
        private string[] images;
        private int currentIndex = 0;
        private System.Windows.Forms.Timer slideshowTimer;
        private float currentScale = 1f;

        private Image originalImage;
        private float zoomFactor = 1.0f;
        private void LoadImages(string folderPath)
        {
            var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            images = Directory.GetFiles(folderPath)
                .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()))
                .ToArray();

            if (images.Length > 0)
            {
                currentIndex = 0;
                ShowImage();
            }
            else
            {
                MessageBox.Show("Нет изображений в выбранной папке");
            }
        }

        private void ShowImage()
        {

            try
            {
                // Загружаем изображение
                using (var img = Image.FromFile(images[currentIndex]))
                {
                    // Масштабируем изображение
                    var scaledImage = ScaleImage(img, currentScale);

                    // Освободить предыдущий образ
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                    }

                    pictureBox.Image = scaledImage;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
            }
        }
        private Bitmap ScaleImage(Image image, float scale)
        {
            int width = (int)(image.Width * scale);
            int height = (int)(image.Height * scale);
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(image, 0, 0, width, height);
            }
            return bmp;
        }
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            currentScale = trackBar.Value / 10f;
            ShowImage();
        }



        private void SlideshowTimer_Tick(object sender, EventArgs e)
        {
            btnNext.PerformClick();
        }

        private void btnSelectFolder_Click_1(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadImages(dlg.SelectedPath);
                }
            }
        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            if (images == null || images.Length == 0) return;
            currentIndex = (currentIndex + 1) % images.Length;
            ShowImage();
        }

        private void btnPrev_Click_1(object sender, EventArgs e)
        {
            if (images == null || images.Length == 0) return;
            currentIndex = (currentIndex - 1 + images.Length) % images.Length;
            ShowImage();
        }

        private void btnStartStopSlideshow_Click_1(object sender, EventArgs e)
        {
            if (slideshowTimer.Enabled)
            {
                slideshowTimer.Stop();
            }
            else
            {
                slideshowTimer.Start();
            }
        }
        int defaultwidth = 505;
        int defaultheight = 966;




    }
}