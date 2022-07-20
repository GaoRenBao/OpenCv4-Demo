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
			Mat image = Cv2.ImRead("1.jpg");

			// 显示原图
			Cv2.ImShow("【原图】", image);

			// 进行中值滤波操作
			Mat out1 = new Mat();
			Cv2.MedianBlur(image, out1, 7);

			// 进行双边滤波操作
			Mat out2 = new Mat();
			Cv2.BilateralFilter(image, out2, 25, 25 * 2, 25 / 2);

			// 显示效果图
			Cv2.ImShow("中值滤波【效果图】", out1);
			Cv2.ImShow("双边滤波【效果图】", out2);

			Cv2.WaitKey(0);
		}
    }
}

