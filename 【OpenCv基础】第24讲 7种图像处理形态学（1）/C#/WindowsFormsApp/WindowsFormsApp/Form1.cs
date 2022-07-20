using OpenCvSharp;
using System;
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
			// 载入原图
			Mat srcImage = Cv2.ImRead("1.jpg");

			// 显示原图
			Cv2.ImShow("【原图】", srcImage);

			// 定义核大小
			Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(15, 15));

			// 进行腐蚀操作 
			Mat out1 = new Mat();
			Cv2.Erode(srcImage, out1, element);

			// 进行膨胀操作 
			Mat out2 = new Mat();
			Cv2.Dilate(srcImage, out2, element);

			// 显示效果图
			Cv2.ImShow("腐蚀【效果图】", out1);
			Cv2.ImShow("膨胀【效果图】", out2);

			Cv2.WaitKey(0);
		}
    }
}

