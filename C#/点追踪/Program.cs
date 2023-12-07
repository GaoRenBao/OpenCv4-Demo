/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：点追踪
博客：http://www.bilibili996.com/Course?id=4954326000071
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace demo
{
    internal class Program
    {
        /// <summary>
        /// 视频操作
        /// </summary>
        public static VideoCapture Cap = new VideoCapture();
        public static Point2f point;
        public static bool addRemovePt = false;

        static void Main(string[] args)
        {
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }
            // 设置采集的图像尺寸为：640*480
            //Cap.Set(VideoCaptureProperties.FrameWidth, 640);
            //Cap.Set(VideoCaptureProperties.FrameHeight, 480);
            //Cap.Set(VideoCaptureProperties.Exposure, -3); // 曝光

            Mat frame = new Mat();
            Mat image = new Mat();
            Mat gray = new Mat();
            Mat prevGray = new Mat();
            bool needToInit = true;
            bool nightMode = false;
            Point2f[] points1 = null;
            Point2f[] points2 = null;
            const int MAX_COUNT = 500;
            TermCriteria criteria = new TermCriteria(CriteriaTypes.MaxIter | CriteriaTypes.Eps, 20, 0.03);

            Cv2.NamedWindow("CamShift Demo");
            Cv2.SetMouseCallback("CamShift Demo", new MouseCallback(onMouse));
            var window = new Window("CamShift Demo");

            while (true)
            {
                if (Cap.Read(frame))
                {
                    frame.CopyTo(image);
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                    //if (nightMode)
                    //  image = Scalar::all(0);

                    if (needToInit)
                    {
                        // 自动初始化
                        //points2 = Cv2.GoodFeaturesToTrack(gray, 500, 0.01, 10, new Mat(), 3, false, 0.04);

                        // 像素级检测特征点
                        Point2f[] po = Cv2.GoodFeaturesToTrack(gray, MAX_COUNT, 0.01, 10, new Mat(), 3, false, 0.04);
                        // 亚像素级检测
                        points2 = Cv2.CornerSubPix(gray, po, new Size(10, 10), new Size(-1, -1), criteria);
                        addRemovePt = false;
                    }
                    else if (points1 != null && points1.Length > 0)
                    {
                        byte[] status;
                        float[] err;
                        if (prevGray.Empty())
                            gray.CopyTo(prevGray);
                        //光流金字塔，输出图二的特征点
                        points2 = new Point2f[points1.Length];
                        Cv2.CalcOpticalFlowPyrLK(prevGray, gray, points1, ref points2, out status, out err);

                        int i, k;
                        for (i = k = 0; i < points2.Length; i++)
                        {
                            if (addRemovePt)
                            {
                                // C# 里没有计算范数的函数Norm，我这里直接按距离算了
                                Point2f p = point;
                                Point2f p2 = points2[i];
                                double a = Math.Sqrt(Math.Abs(p.X - p2.X) * Math.Abs(p.X - p2.X) + Math.Abs(p.Y - p2.Y) * Math.Abs(p.Y - p2.Y));
                                if (a <= 5)
                                {
                                    addRemovePt = false;
                                    continue;
                                }
                            }
                            if (status[i] == 0x00)
                                continue;

                            points2[k++] = points2[i];
                            Cv2.Circle(image, (Point)points2[i], 3, new Scalar(0, 255, 0), 8);
                        }
                        // C++ 的resize功能
                        points2 = points2.ToList().Take(k).ToList().ToArray();
                    }

                    if (addRemovePt && (points2 == null || points2.Length < MAX_COUNT))
                    {
                        Point2f[] tmp = new Point2f[] { point };
                        Point2f[] tmp2 = Cv2.CornerSubPix(gray, tmp, new Size(10, 10), new Size(-1, -1), criteria);

                        // C++ 的push_back功能
                        List<Point2f> a = points2.ToList();
                        a.Add(tmp2[0]);
                        points2 = a.ToArray();

                        addRemovePt = false;
                    }
                    needToInit = false;

                    // 在Window窗口中播放视频(方法1)
                    window.ShowImage(image);
                    // 在Window窗口中播放视频(方法2)
                    //Cv2.ImShow("CamShift Demo", image);
                    // 在pictureBox1中显示效果图
                    //pictureBox1.Image = BitmapConverter.ToBitmap(image);

                    char c = (char)Cv2.WaitKey(10);
                    if (c == 27) break;
                    switch (c)
                    {
                        case 'r':
                            needToInit = true;
                            break;
                        case 'c':
                            points1 = new Point2f[] { };
                            points2 = new Point2f[] { };
                            break;
                        case 'n':
                            nightMode = !nightMode;
                            break;
                    }

                    Swap(ref points2, ref points1);
                    Swap(ref prevGray, ref gray);
                }
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (@event == MouseEventTypes.LButtonDown)
            {
                point = new Point2f((float)x, (float)y);
                addRemovePt = true;
            }
        }
    }
}
