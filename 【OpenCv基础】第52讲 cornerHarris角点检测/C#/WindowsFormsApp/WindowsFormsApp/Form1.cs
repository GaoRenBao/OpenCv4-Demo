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
            // 以灰度模式载入图像并显示
            Mat srcImage = Cv2.ImRead("1.jpg", 0);
            Cv2.ImShow("原始图", srcImage);

            // 进行Harris角点检测找出角点
            // OpenCvSharp3-AnyCPU 版本找不到 CornerHarris 方.
            // 必须采用 OpenCvSharp4 版本才行。
            Mat cornerStrength = new Mat();
            Cv2.CornerHarris(srcImage, cornerStrength, 2, 3, 0.01);

            // 对灰度图进行阈值操作，得到二值图并显示  
            Mat harrisCorner = new Mat();
            Cv2.Threshold(cornerStrength, harrisCorner, 0.00001, 255, ThresholdTypes.Binary);
            Cv2.ImShow("角点检测后的二值效果图", harrisCorner);

            Cv2.WaitKey(0);
        }
    }
}