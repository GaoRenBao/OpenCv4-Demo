using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

// https://shimat.github.io/opencvsharp_docs/html/d69c29a1-7fb1-4f78-82e9-79be971c3d03.htm

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
            // 读取图像
            Mat srcImage = Cv2.ImRead("girl.jpg");
            // 显示原图
            Cv2.ImShow("原图", srcImage);
            // 均值滤波
            Mat dstImage = new Mat();
            Cv2.Blur(srcImage, dstImage, new OpenCvSharp.Size() { Width = 7, Height = 7 });
            // 在pictureBox1中显示效果图
            Bitmap map = BitmapConverter.ToBitmap(dstImage);
            pictureBox1.Image = map;
            // 弹窗显示效果图
            using (new Window("效果", dstImage))
            {
                Cv2.WaitKey();
            }
        }
    }
}
