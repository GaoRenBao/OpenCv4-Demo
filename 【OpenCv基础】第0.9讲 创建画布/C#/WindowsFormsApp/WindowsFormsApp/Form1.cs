using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 读取一张图片
            Mat srcImage = new Mat("a.jpg");
            // 显示图片
            //Cv2.ImShow("image", srcImage);
            // 按键检测，结束
            //Cv2.WaitKey(0);

            Bitmap map = BitmapConverter.ToBitmap(srcImage);
            pictureBox1.Image = map;
        }
    }
}
