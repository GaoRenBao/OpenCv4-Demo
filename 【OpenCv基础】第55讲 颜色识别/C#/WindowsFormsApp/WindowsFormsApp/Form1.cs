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

        /// <summary>
        /// 颜色识别(除红色外的其他单色)
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        private Mat ColorFindContours(Mat srcImage,
            int iLowH, int iHighH, 
            int iLowS, int iHighS, 
            int iLowV, int iHighV)
        {
            Mat bufImg = new Mat();
            Mat imgHSV = new Mat();
            //转为HSV
            Cv2.CvtColor(srcImage, imgHSV, ColorConversionCodes.BGR2HSV);
            Cv2.InRange(imgHSV, 
                new Scalar(iLowH, iLowS, iLowV),
                new Scalar(iHighH, iHighS, iHighV), bufImg);
            return bufImg;
        }

        /// <summary>
        /// 颜色识别（红色）
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        private Mat ColorFindContours2(Mat srcImage)
        {
            Mat des1 = ColorFindContours(srcImage,
              350 / 2, 360 / 2,         // 色调最小值~最大值
              (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
              (int)(255 * 0.60), 255);  // 亮度最小值~最大值

            Mat des2 = ColorFindContours(srcImage,
               0, 16 / 2,               // 色调最小值~最大值
              (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
              (int)(255 * 0.60), 255);  // 亮度最小值~最大值

            return des1 + des2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mat g_srcImage = Cv2.ImRead("2.jpg");
            Cv2.ImShow("原始图", g_srcImage);

            Mat des = ColorFindContours(g_srcImage,
                45 / 2, 60 / 2,           // 色调最小值~最大值
                (int)(255 * 0.60), 255,   // 饱和度最小值~最大值
                (int)(255 * 0.90), 255);  // 亮度最小值~最大值

            Cv2.ImShow("识别结果", des);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mat g_srcImage = Cv2.ImRead("2.jpg");
            Cv2.ImShow("原始图", g_srcImage);
            Mat des = ColorFindContours2(g_srcImage);
            Cv2.ImShow("识别结果", des);
        }
    }
}