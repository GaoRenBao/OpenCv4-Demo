using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        string WINDOW_NAME1 = "【程序窗口】";         //为窗口标题定义的宏 
        string WINDOW_NAME2 = "【分水岭算法效果图】"; //为窗口标题定义的宏

        Mat srcImage = new Mat();
        Mat grayImage = new Mat();
        static Mat g_maskImage = new Mat();
        static Mat g_srcImage = new Mat();
        Point prevPt = new Point(-1, -1);

        public Form1()
        {
            InitializeComponent();
        }

        public void on_Mouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            //处理鼠标不在窗口中的情况
            if (x < 0 || x >= g_srcImage.Cols || y < 0 || y >= g_srcImage.Rows)
                return;

            //处理鼠标左键相关消息
            if (@event == MouseEventTypes.LButtonUp || ((int)flags & (int)MouseEventTypes.LButtonDown) == 0)
		        prevPt = new Point(-1, -1);
	        else if (@event == MouseEventTypes.LButtonDown)
		        prevPt = new Point(x, y);

            //鼠标左键按下并移动，绘制出白色线条
            else if (@event == MouseEventTypes.MouseMove && ((int)flags & (int)MouseEventTypes.LButtonDown) > 0)
            {
		        Point pt = new Point(x, y);
		        if (prevPt.X < 0)
                    prevPt = pt;

                Cv2.Line(g_maskImage, prevPt, pt, new Scalar(255,255,255), 5, LineTypes.Link8, 0);
                Cv2.Line(g_srcImage, prevPt, pt, new Scalar(255, 255, 255), 5, LineTypes.Link8, 0);
		        prevPt = pt;
                Cv2.ImShow(WINDOW_NAME1, g_srcImage);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 载入原图
            g_srcImage = Cv2.ImRead("1.jpg");
            if (g_srcImage.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }

            Cv2.ImShow(WINDOW_NAME1, g_srcImage);
            g_srcImage.CopyTo(srcImage);
            Cv2.CvtColor(g_srcImage, g_maskImage, ColorConversionCodes.BGR2GRAY); // 灰度
            Cv2.CvtColor(g_maskImage, grayImage, ColorConversionCodes.GRAY2BGR);  // 修改颜色空间
            g_maskImage.SetIdentity(new Scalar(0));

            //【2】设置鼠标操作回调函数
            Cv2.NamedWindow(WINDOW_NAME1);
            Cv2.SetMouseCallback(WINDOW_NAME1, new MouseCallback(on_Mouse));

            myTask();
        }

        private void myTask()
        {
            RNG rng = new RNG(12345);
            while (true)
            {
                //获取键值
                int c = Cv2.WaitKey(0);

                //按键键值为2时，恢复源图
                if ((char)c == '2')
                {
                    g_maskImage = new Mat(g_maskImage.Size(), g_maskImage.Type());
                    srcImage.CopyTo(g_srcImage);
                    Cv2.ImShow(WINDOW_NAME1, g_srcImage);
                }

                //若检测到按键值为1或者空格，则进行处理
                if ((char)c == '1' || (char)c == ' ')
                {
                    // 定义一些参数
                    int i, j, compCount = 0;
                    Point[][] contours;
                    HierarchyIndex[] hierarchy;
                    //寻找轮廓
                    Cv2.FindContours(g_maskImage, out contours, out hierarchy, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);

                    //轮廓为空时的处理
                    if (contours.Length == 0)
                        continue;

                    //拷贝掩膜
                    Mat maskImage = new Mat(g_maskImage.Size(), MatType.CV_32S);
                    maskImage.SetIdentity(new Scalar(0));

                    // 循环绘制出轮廓
                    for (int index = 0; index >= 0; index = hierarchy[index].ToVec4i().Item0, compCount++)
                    {
                        //绘制轮廓
                        Cv2.DrawContours(maskImage, contours, index, new Scalar(compCount+1), -1, LineTypes.Link8, hierarchy);
                    }

                    //compCount为零时的处理
                    if (compCount == 0)
                        continue;

                    //生成随机颜色
                    List<Vec3b> colorTab = new List<Vec3b>();
                    for (i = 0; i < compCount; i++)
                    {
                        byte b = (byte)rng.Uniform(100, 255);
                        byte g = (byte)rng.Uniform(100, 255);
                        byte r = (byte)rng.Uniform(100, 255);
                        colorTab.Add(new Vec3b(b, g, r));
                    }

                    Cv2.Watershed(srcImage, maskImage);

                    //双层循环，将分水岭图像遍历存入watershedImage中
                    Mat watershedImage = new Mat(maskImage.Size(), MatType.CV_8UC3);
                    for (i = 0; i < maskImage.Rows; i++)
                    {
                        for (j = 0; j < maskImage.Cols; j++)
                        {
                            int index = maskImage.At<int>(i, j);
                            if (index == -1)
                                watershedImage.Set<Vec3b>(i, j, new Vec3b(255, 255, 255));
                            else if (index <= 0 || index > compCount)
                                watershedImage.Set<Vec3b>(i, j, new Vec3b(0, 0, 0));
                            else
                                watershedImage.Set<Vec3b>(i, j, colorTab[index - 1]);
                        }
                    }

                    //混合灰度图和分水岭效果图并显示最终的窗口
                    watershedImage = watershedImage * 0.5 + grayImage * 0.5;
                    Cv2.ImShow(WINDOW_NAME2, watershedImage);
                    maskImage.Dispose();
                    watershedImage.Dispose();
                }
            }
        }
    }
}

