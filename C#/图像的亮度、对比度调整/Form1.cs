using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public double g_nContrastValue; //对比度值
        public double g_nBrightValue;  //亮度值
        public Mat g_srcImage, g_dstImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 读入用户提供的图像
            g_srcImage = Cv2.ImRead("../../../images/flowers.jpg");
            if (g_srcImage.Data == null)
            {
                MessageBox.Show("Oh，no，读取g_srcImage图片错误~！");
                return;
            }
            g_dstImage = new Mat(g_srcImage.Size(), g_srcImage.Type());

            //设定对比度和亮度的初值
            g_nContrastValue = 80;
            g_nBrightValue = 80;
            hScrollBar1.Value = 80;
            hScrollBar2.Value = 80;

            Cv2.ImShow("【原始图窗口】", g_srcImage);

            Task t = new Task(() =>
            {
                ContrastAndBright();
            });
            t.Start();
        }

        // 对比度
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            g_nContrastValue = (double)hScrollBar1.Value;
            label3.Text = g_nContrastValue.ToString();
        }

        // 亮度
        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            g_nBrightValue = (double)hScrollBar2.Value;
            label4.Text = g_nBrightValue.ToString();
        }

        private void ContrastAndBright()
        {
            while (true)
            {
                for (int y = 0; y < g_srcImage.Rows; y++)
                {
                    for (int x = 0; x < g_srcImage.Cols; x++)
                    {
                        Vec3b color = new Vec3b
                        {
                            Item0 = Saturate_cast(g_srcImage.Get<Vec3b>(y, x).Item0 * (g_nContrastValue * 0.01) + g_nBrightValue), // B
                            Item1 = Saturate_cast(g_srcImage.Get<Vec3b>(y, x).Item1 * (g_nContrastValue * 0.01) + g_nBrightValue), // G
                            Item2 = Saturate_cast(g_srcImage.Get<Vec3b>(y, x).Item2 * (g_nContrastValue * 0.01) + g_nBrightValue)  // R
                        };
                        g_dstImage.Set(y, x, color);
                    }
                }
                // 输出图像到pictureBox控件
                pictureBox1.Image = BitmapConverter.ToBitmap(g_dstImage);
            }
        }

        //要确保运算后的像素值在正确的范围内
        private byte Saturate_cast(double n)
        {
            if (n <= 0)
            {
                return 0;
            }
            else if (n > 255)
            {
                return 255;
            }
            else
            {
                return (byte)n;
            }
        }
    }
}

