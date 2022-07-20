using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        string WINDOW_NAME0 = "【原始图参考】"; //为窗口标题定义的宏 
        string WINDOW_NAME1 = "【原始图】"; //为窗口标题定义的宏
        string WINDOW_NAME2 = "【修补后的效果图】"; //为窗口标题定义的宏

        Mat srcImage = new Mat();
        Mat srcImage0 = new Mat();
        Mat srcImage1 = new Mat();
        Mat inpaintMask = new Mat();
        Point previousPoint = new Point(-1, -1);

        public Form1()
        {
            InitializeComponent();
        }

        public void on_Mouse(MouseEvent @event, int x, int y, MouseEvent flags, IntPtr userdata)
        {
            //处理鼠标左键相关消息
            if (@event == MouseEvent.LButtonUp || (flags & MouseEvent.FlagLButton) == 0)
                previousPoint = new Point(-1, -1);
	        else if (@event == MouseEvent.LButtonDown)
                previousPoint = new Point(x, y);

            //鼠标左键按下并移动，绘制出白色线条
            else if (@event == MouseEvent.MouseMove && (flags & MouseEvent.FlagLButton) > 0)
            {
		        Point pt = new Point(x, y);
		        if (previousPoint.X < 0)
                    previousPoint = pt;

                Cv2.Line(inpaintMask, previousPoint, pt, new Scalar(255,255,255), 5, LineTypes.Link8, 0);
                Cv2.Line(srcImage1, previousPoint, pt, new Scalar(255, 255, 255), 5, LineTypes.Link8, 0);
                previousPoint = pt;
                Cv2.ImShow(WINDOW_NAME1, srcImage1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 载入原图
            srcImage = Cv2.ImRead("1.jpg");
            if (srcImage.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }
            srcImage.CopyTo(srcImage0);
            srcImage.CopyTo(srcImage1);
            inpaintMask = new Mat(srcImage1.Size(), MatType.CV_8U);

            //显示原始图参考
            Cv2.ImShow(WINDOW_NAME0, srcImage0);
            //显示原始图
            Cv2.ImShow(WINDOW_NAME1, srcImage1);
            // 设置鼠标回调消息
            Cv2.SetMouseCallback(WINDOW_NAME1, new CvMouseCallback(on_Mouse));

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
                    inpaintMask = new Mat(inpaintMask.Size(), inpaintMask.Type());
                    srcImage.CopyTo(srcImage1);
                    Cv2.ImShow(WINDOW_NAME1, srcImage1);
                }

                //若检测到按键值为1或者空格，则进行处理
                if ((char)c == '1' || (char)c == ' ')
                {
                    Mat inpaintedImage = new Mat();
                    Cv2.Inpaint(srcImage1, inpaintMask, inpaintedImage, 3, InpaintMethod.Telea);
                    Cv2.ImShow(WINDOW_NAME2, inpaintedImage);
                    inpaintedImage.Dispose();
                }
            }
        }
    }
}

