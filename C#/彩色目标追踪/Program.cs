/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        // 声明全局变量
        public static Mat image = new Mat();
        public static bool selectObject = false;
        public static int trackObject = 0;
        public static Rect selection;
        public static Point origin;

        /// <summary>
        /// 视频操作
        /// </summary>
        public static VideoCapture Cap = new VideoCapture();

        private static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            //当左键按下，框选标志为真，执行如下程序得到矩形区域selection
            if (selectObject)
            {
                selection.X = Math.Min(x, origin.X);
                selection.Y = Math.Min(y, origin.Y);
                selection.Width = Math.Abs(x - origin.X);
                selection.Height = Math.Abs(y - origin.Y);
                //矩形区域与image进行与运算，结果保存到矩形区域中
                selection &= new Rect(0, 0, image.Cols, image.Rows);
            }
            if (@event == MouseEventTypes.LButtonDown)
            {
                origin = new Point(x, y);
                selection = new Rect(x, y, 0, 0);
                selectObject = true;
            }
            if (@event == MouseEventTypes.LButtonUp)
            {
                selectObject = false;
                if (selection.Width > 0 && selection.Height > 0)
                    trackObject = -1;
            }
        }

        static void Main(string[] args)
        {
            int hsize = 16;
            float[] phranges = { 0, 180 };
            Rect trackWindow = new Rect();

            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }
            // 设置采集的图像尺寸为：640*480
            Cap.Set(VideoCaptureProperties.FrameHeight, 480);
            Cap.Set(VideoCaptureProperties.FrameWidth, 640);

            Mat frame = new Mat();
            Mat hsv = new Mat();
            Mat hue = new Mat();
            Mat mask = new Mat();
            Mat hist = new Mat();
            Mat backproj = new Mat();

            Cv2.NamedWindow("Histogram");
            Cv2.NamedWindow("CamShift Demo");

            MouseCallback GetRGBCvMouseCallback = new MouseCallback(onMouse);
            Cv2.SetMouseCallback("CamShift Demo", GetRGBCvMouseCallback);

            while (true)
            {
                //读取当前帧
                if (!Cap.Read(frame)) continue;
                if (frame.Empty()) break;

                frame.CopyTo(image); //将当前帧复制到image中
                Cv2.CvtColor(image, hsv, ColorConversionCodes.BGR2HSV); //将image转为hsv色彩空间，保存到hsv中
                if (trackObject != 0)//如果有操作，trackobject等于1或-1
                {
                    // 亮度范围设置
                    int _vmin = 10, _vmax = 256;
                    // 色彩范围检测
                    // Scalar: 色调、饱和度、亮度，第一个Scalar是最小值，第二个Scalar是最大值
                    Cv2.InRange(hsv, new Scalar(0, 30, Math.Min(_vmin, _vmax)), new Scalar(180, 256, Math.Max(_vmin, _vmax)), mask);
                    hue.Create(hsv.Size(), hsv.Depth());//创建一个与hsv尺寸和深度一样的hue

                    // 从输入中拷贝某通道到输出中特定的通道。
                    // 官方代码参考位置：./Sample-4.1.0-20190417/SamplesCS/Samples/MergeSplitSample.cs:51:
                    int[] ch = { 0, 0 };
                    Mat[] input = { hsv };
                    Mat[] output = { hue, };
                    Cv2.MixChannels(input, output, ch);

                    Rangef[] range = new Rangef[3];//三个通道，范围
                    range[0] = new Rangef(phranges[0], phranges[1]); //从0开始（含）,到180结束（不含）
                    range[1] = range[0];//通道2和通道1一样
                    range[2] = range[0];//通道3和通道1一样

                    if (trackObject < 0)//如果为-1,代表左键弹起，划定了区域
                    {
                        //hue是视频帧处理后的图像，selection是鼠标选定的矩形区域，同时创建一个感兴趣区域和一个标记感兴趣区域
                        Mat roi = new Mat(hue, selection);
                        Mat maskroi = new Mat(mask, selection);
                        Cv2.CalcHist(images: new[] { roi }, channels: new[] { 0 }, mask: maskroi, hist: hist, dims: 1,
                            histSize: new[] { 16 }, ranges: new[] { new Rangef(0, 180) });
                        Cv2.Normalize(hist, hist, 0, 255, NormTypes.MinMax);
                        trackWindow = selection;
                        trackObject = 1;
                    }

                    Mat[] arrs2 = { hue };
                    Cv2.CalcBackProject(arrs2, channels: new[] { 0 }, hist, backproj, range);
                    backproj &= mask;
                    RotatedRect trackBox = Cv2.CamShift(backproj, ref trackWindow, new TermCriteria(CriteriaTypes.Count | CriteriaTypes.Eps, 10, 1));

                    if ((trackWindow.Width * trackWindow.Height) <= 1)
                    {
                        int cols = backproj.Cols, rows = backproj.Rows, r = (Math.Min(cols, rows) + 5) / 6;
                        trackWindow = new Rect(trackWindow.X - r, trackWindow.Y - r,
                            trackWindow.X + r, trackWindow.Y + r) &
                            new Rect(0, 0, cols, rows);
                    }

                    // 投影视图
                    // Cv2.CvtColor(backproj, image, ColorConversionCodes.GRAY2BGR);

                    Cv2.Ellipse(image, trackBox, new Scalar(0, 0, 255), 3, LineTypes.AntiAlias);
                }

                if (selectObject && selection.Width > 0 && selection.Height > 0)
                {
                    Mat roi = new Mat(image, selection);
                    Cv2.BitwiseNot(roi, roi);
                }
                Cv2.ImShow("CamShift Demo", image);

                if ((char)Cv2.WaitKey(10) == 27) break;
            }
        }
    }
}
