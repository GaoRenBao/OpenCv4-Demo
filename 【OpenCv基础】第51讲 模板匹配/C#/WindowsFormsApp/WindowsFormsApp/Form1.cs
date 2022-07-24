using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private string WINDOW_NAME1 = "【原始图片】";
        private string WINDOW_NAME2 = "【匹配窗口】";

        Mat g_srcImage = new Mat();
        Mat g_templateImage = new Mat();
        Mat g_resultImage = new Mat();
        int g_nMatchMethod = 0;
        int g_nMaxTrackbarNum = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //【1】载入原图像和模板块
            g_srcImage = Cv2.ImRead("1.jpg");
            g_templateImage = Cv2.ImRead("2.jpg");

            //【2】创建窗口
            Cv2.NamedWindow(WINDOW_NAME1, WindowFlags.AutoSize);
            Cv2.NamedWindow(WINDOW_NAME2, WindowFlags.AutoSize);

            //【3】创建滑动条并进行一次初始化
            int v = Cv2.CreateTrackbar("方法:", WINDOW_NAME1, ref g_nMatchMethod, g_nMaxTrackbarNum, on_Matching);
            on_Matching(0, IntPtr.Zero);
            Cv2.WaitKey(0);
        }

        public void on_Matching(int pos, IntPtr userData)
        {
            //【1】给局部变量初始化
            Mat srcImage = new Mat();
            g_srcImage.CopyTo(srcImage);

            //【2】初始化用于结果输出的矩阵
            int resultImage_rows = g_srcImage.Rows - g_templateImage.Rows + 1;
            int resultImage_cols = g_srcImage.Cols - g_templateImage.Cols + 1;
            g_resultImage = new Mat(new Size(resultImage_rows, resultImage_cols), MatType.CV_32FC1);
            g_resultImage.SetIdentity(new Scalar(0));

            //【3】进行匹配和标准化
            Cv2.MatchTemplate(g_srcImage, g_templateImage, g_resultImage, (TemplateMatchModes)g_nMatchMethod);
            Cv2.Normalize(g_resultImage, g_resultImage, 0, 1, NormTypes.MinMax, -1);

            //【4】通过函数 minMaxLoc 定位最匹配的位置
            double minValue = 0;
            double maxValue = 0;
            Point minLocation = new Point();
            Point maxLocation = new Point();
            Point matchLocation = new Point();
            Cv2.MinMaxLoc(g_resultImage, out minValue, out maxValue, out minLocation, out maxLocation);

            //【5】对于方法 SQDIFF 和 SQDIFF_NORMED, 越小的数值有着更高的匹配结果. 而其余的方法, 数值越大匹配效果越好
            if ((TemplateMatchModes)g_nMatchMethod == TemplateMatchModes.SqDiff ||
                (TemplateMatchModes)g_nMatchMethod == TemplateMatchModes.SqDiffNormed)
            {
                matchLocation = minLocation;
            }
            else
            {
                matchLocation = maxLocation;
            }

            //【6】绘制出矩形，并显示最终结果
            Cv2.Rectangle(srcImage, matchLocation,
                new Point(matchLocation.X + g_templateImage.Cols, matchLocation.Y + g_templateImage.Rows),
                new Scalar(0, 0, 255), 2, LineTypes.Link8, 0);
            Cv2.Rectangle(g_resultImage, matchLocation,
                new Point(matchLocation.X + g_templateImage.Cols, matchLocation.Y + g_templateImage.Rows),
                new Scalar(0, 0, 255), 2, LineTypes.Link8, 0);

            Cv2.ImShow(WINDOW_NAME1, srcImage);
            Cv2.ImShow(WINDOW_NAME2, g_resultImage);
        }
    }
}