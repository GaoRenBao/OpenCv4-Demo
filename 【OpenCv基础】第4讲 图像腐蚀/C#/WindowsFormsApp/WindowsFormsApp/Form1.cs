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
            Mat srcImage = Cv2.ImRead("girl.jpg");
            
            // 在窗口中显示原画
            Cv2.ImShow("原图", srcImage);

            // 进行腐蚀操作
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(){Width=15,Height=15}); 
            Mat dstImage = new Mat();
            Cv2.Erode(srcImage, dstImage, element);

            // 输出图像到pictureBox控件
            Bitmap map = BitmapConverter.ToBitmap(dstImage);
            pictureBox1.Image = map;
            
            // 弹窗显示图像
            using (new Window("效果", dstImage))
            {
                Cv2.WaitKey();
            }
        }
    }
}
