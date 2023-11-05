using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        Mat g_srcImage = new Mat();
        Mat g_grayImage = new Mat();
        Mat g_cannyMat_output = new Mat();
        int g_nThresh = 80;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 【1】载入原始图，且必须以二值图模式载入
            g_srcImage = Cv2.ImRead("../../../images/flowers2.jpg");
            Cv2.ImShow("原始图", g_srcImage);

            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            new Task(() => {
                on_ThreshChange();
            }).Start();
            Cv2.WaitKey(0);
        }

        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = hScrollBar2.Value.ToString();
        }
        private void hScrollBar2_MouseLeave(object sender, EventArgs e)
        {
            g_nThresh = hScrollBar2.Value;
        }

        private void on_ThreshChange()
        {
            int num = 0;
            while (true)
            {
                Thread.Sleep(500);
                if (num != g_nThresh)
                {
                    num = g_nThresh;
#if true
                    // 用Canny算子检测边缘
                    Cv2.Canny(g_grayImage, g_cannyMat_output, g_nThresh, g_nThresh * 2, 3);

                    // 寻找轮廓
                    Point[][] g_vContours = new Point[][] { };
                    HierarchyIndex[] g_vHierarchy;
                    Cv2.FindContours(g_cannyMat_output, out g_vContours, out g_vHierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

                    // 绘出轮廓
                    Random g_rng = new Random();
                    Mat drawing = new Mat(g_cannyMat_output.Size(), MatType.CV_8UC3);
                    for (int i = 0; i < g_vContours.Length; i++)
                    {
                        //随机生成bgr值
                        int b = g_rng.Next(255);//随机返回一个0~255之间的值
                        int g = g_rng.Next(255);//随机返回一个0~255之间的值
                        int r = g_rng.Next(255);//随机返回一个0~255之间的值
                        Cv2.DrawContours(drawing, g_vContours, i, new Scalar(b, g, r), 2, LineTypes.Link8, g_vHierarchy);
                    }
#else
                    g_grayImage.CopyTo(g_cannyMat_output);
                    Cv2.Threshold(g_cannyMat_output, g_cannyMat_output, g_nThresh, 255, ThresholdTypes.Binary);

                    // 寻找轮廓
                    Mat drawing = new Mat(g_cannyMat_output.Size(), MatType.CV_8UC3);
                    Point[][] contours = new Point[][] { };
                    HierarchyIndex[] hierarcy;
                    Cv2.FindContours(g_cannyMat_output, out contours, out hierarcy, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple, null);
                    Random g_rng = new Random();

                    // 绘出轮廓
                    for (int index = 0; index < contours.Length; index++)
                    {
                        //随机生成bgr值
                        int b = g_rng.Next(255);//随机返回一个0~255之间的值
                        int g = g_rng.Next(255);//随机返回一个0~255之间的值
                        int r = g_rng.Next(255);//随机返回一个0~255之间的值
                        Cv2.DrawContours(drawing, contours, index, new Scalar(b, g, r), -1, LineTypes.Link8, hierarcy);
                    }
#endif
                    // 输出图像到pictureBox控件
                    pictureBox1.Image = BitmapConverter.ToBitmap(drawing);
                }
            }
        }
    }
}

