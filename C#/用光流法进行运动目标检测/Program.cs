/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        // 当前图片
        public static Mat gray = new Mat();
        // 预测图片
        public static Mat gray_prev = new Mat();
        // point1为特征点的原来位置，point2为特征点的新位置
        public static Point2f[] points1;
        public static Point2f[] points2;
        // 初始化跟踪点的位置
        public static Point2f[] initial;
        // 检测的最大特征数
        public static int maxCount = 500;
        // 特征检测的等级
        public static double qLevel = 0.01;
        // 两特征点之间的最小距离
        public static double minDist = 10.0;
        // 跟踪特征的状态，特征的流发现为1，否则为0
        public static byte[] status;
        public static float[] err;

        static void Main(string[] args)
        {
            var capture = new VideoCapture("../../../images/lol.avi");
            // 计算帧率
            int sleepTime = (int)Math.Round(1000 / capture.Fps);

            // 声明实例 Mat类
            Mat image = new Mat();
            // 进入读取视频每镇的循环
            while (true)
            {
                capture.Read(image);
                //判断是否还有没有视频图像 
                if (image.Empty())
                    break;

                Mat result = tracking(image);
                Cv2.ImShow("效果图", result);

                // 在pictureBox1中显示效果图
                // pictureBox1.Image = BitmapConverter.ToBitmap(result);
                Cv2.WaitKey(sleepTime);
            }
        }

        //--------------------------------------
        // function: tracking
        // brief: 跟踪
        // parameter: frame 输入的视频帧
        //            output 有跟踪结果的视频帧
        // return: void
        //--------------------------------------
        public static Mat tracking(Mat frame)
        {
            Mat output = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            frame.CopyTo(output);

            // 添加特征点
            if (addNewPoints())
            {
                // 只用这个好像也没啥区别
                points1 = Cv2.GoodFeaturesToTrack(gray, maxCount, qLevel, minDist, new Mat(), 10, true, 0.04);
                initial = points1;

                //// 像素级检测特征点
                //Point2f[] po = Cv2.GoodFeaturesToTrack(gray, maxCount, qLevel, minDist, new Mat(), 3, true, 0.04);
                //// 亚像素级检测
                //points1 = Cv2.CornerSubPix(gray, po, new Size(5, 5), new Size(-1, -1), new TermCriteria());
            }
            if (gray_prev.Empty())
            {
                gray.CopyTo(gray_prev);
            }

            //光流金字塔，输出图二的特征点
            points2 = new Point2f[points1.Length];
            Cv2.CalcOpticalFlowPyrLK(gray_prev, gray, points1, ref points2, out status, out err);

            // 去掉一些不好的特征点
            int k = 0;
            for (int i = 0; i < points2.Length; i++)
            {
                if (acceptTrackedPoint(i))
                {
                    initial[k] = initial[i];
                    points2[k++] = points2[i];
                }
            }

            // 显示特征点和运动轨迹
            for (int i = 0; i < k; i++)
            {
                Cv2.Line(output, (Point)initial[i], (Point)points2[i], new Scalar(0, 0, 255));
                Cv2.Circle(output, (Point)points2[i], 3, new Scalar(0, 255, 0), -1);
            }

            // 把当前跟踪结果作为下一此参考
            Swap(ref points2, ref points1);
            Swap(ref gray_prev, ref gray);
            return output;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        //-------------------------------------
        // function: addNewPoints
        // brief: 检测新点是否应该被添加
        // parameter:
        // return: 是否被添加标志
        //-------------------------------------
        public static bool addNewPoints()
        {
            if (points1 == null) return true;

            // 这个实际上是限制了点数，最好别开
            //return  points1.Length <= 10;
            //System.Diagnostics.Debug.WriteLine(points1.Length);
            return true;
        }

        //--------------------------------------
        // function: acceptTrackedPoint
        // brief: 决定哪些跟踪点被接受
        // parameter:
        // return:
        //-------------------------------------
        public static bool acceptTrackedPoint(int i)
        {
            return status[i] == 1 && ((Math.Abs(points1[i].X - points2[i].X) + Math.Abs(points1[i].Y - points2[i].Y)) > 5);
        }
    }
}
