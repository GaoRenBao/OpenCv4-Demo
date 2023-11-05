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

        static void Main(string[] args)
        {
            demo1();
            //demo2();
            //demo3();
            //demo4();
        }

        public static void myImshow(string name, Mat img)
        {
            Mat temp = new Mat();
            Cv2.Resize(img, temp, new Size(img.Cols * 1, img.Rows * 1), 0, 0, InterpolationFlags.Nearest);
            Cv2.ImShow(name, temp);
        }

        #region 图像相减1：图像的减法运算
        public static void demo1()
        {
            Mat img1 = Cv2.ImRead("../../../images/subtract1.jpg", 0); // 灰度图
            Mat img2 = Cv2.ImRead("../../../images/subtract2.jpg", 0);

            myImshow("subtract1", img1);
            myImshow("subtract2", img2);

            Mat st = img2 - img1;
            myImshow("after subtract", st);

            Mat threshold = new Mat();
            Cv2.Threshold(st, threshold, 50, 255, ThresholdTypes.Binary);

            myImshow("after threshold", threshold);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 图像相减2：通过图像相减，查找扑克牌位置
        public static void demo2()
        {
            Mat img1 = Cv2.ImRead("../../../images/subtract1.jpg", 0);
            Mat img22 = Cv2.ImRead("../../../images/subtract2.jpg");

            Mat img2 = new Mat();
            Cv2.CvtColor(img22, img2, ColorConversionCodes.BGR2GRAY);

            Mat st = new Mat();
            Cv2.Subtract(img2, img1, st);
            //Cv2.Subtract(img1, img2, st); // 相反

            // 把小于5的像素点设为0
            for (int i = 0; i < st.Rows; ++i)
            {
                for (int j = 0; j < st.Cols; ++j)
                {
                    byte rgb = st.At<byte>(i, j);
                    if (rgb > 0 && rgb <= 5)
                        st.Set(i, j, 0);
                }
            }

            Mat threshold = new Mat();
            Cv2.Threshold(st, threshold, 50, 255, ThresholdTypes.Binary);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(threshold, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

            //List<(int, double)> areas = new List<(int, double)>();
            //for (int i = 0; i < contours.Length; i++)
            //{
            //    areas.Add((i, Cv2.ContourArea(contours[i])));
            //}
            //// 降序排序
            //var a2 = areas.OrderByDescending(x => x.Item2).ToList();
            //foreach(var are in a2)
            //{
            //    if (are.Item2 < 100)
            //        continue;
            //    Cv2.DrawContours(img22, contours, are.Item1, new Scalar(0, 0, 255), 3);
            //    myImshow("approxPolyDP", img22);
            //    Cv2.WaitKey(0);
            //}

            // TODO 截取原图，把长方形纠正
            Point[] cnt = contours[0];
            Point[] hull = Cv2.ConvexHull(cnt);
            double epsilon = 0.001 * Cv2.ArcLength(hull, true);
            Point[] simplified_cnt = Cv2.ApproxPolyDP(hull, epsilon, true);

            epsilon = 0.1 * Cv2.ArcLength(cnt, true);
            Point[] approx = Cv2.ApproxPolyDP(cnt, epsilon, true);
            Point[][] approxs = new Point[][] { approx };

            Cv2.DrawContours(img22, approxs, 0, new Scalar(255, 0, 0), 3);
            myImshow("approxPolyDP", img22);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 图像相减3：通过图像相减，凸显扑克牌位置
        //  returns just the difference of the two images
        public static Mat diff(Mat img, Mat img1)
        {
            Mat diff = new Mat();
            Cv2.Absdiff(img, img1, diff);
            return diff;
        }

        // removes the background but requires three images
        public static Mat diff_remove_bg(Mat img0, Mat img, Mat img1)
        {
            Mat d1 = diff(img0, img);
            Mat d2 = diff(img, img1);
            Mat a = new Mat();
            Cv2.BitwiseAnd(d1, d2, a);
            return a;
        }

        public static void demo3()
        {
            Mat img1 = Cv2.ImRead("../../../images/subtract1.jpg", 0);
            Mat img2 = Cv2.ImRead("../../../images/subtract2.jpg", 0);
            myImshow("subtract1", img1);
            myImshow("subtract2", img2);
            Mat st = diff_remove_bg(img2, img1, img2);
            myImshow("after subtract", st);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 调用摄像头，通过图像相减，标记扑克牌位置
        public static void demo4()
        {
            VideoCapture Cap = new VideoCapture();
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }

            /* opencv3 */
            //Cap.Set(CaptureProperty.FrameWidth, 1280);  // 设置采集的图像宽度
            //Cap.Set(CaptureProperty.FrameHeight, 720); // 设置采集的图像高度

            /* opencv4 */
            Cap.Set(VideoCaptureProperties.FrameWidth, 1280);  // 设置采集的图像宽度
            Cap.Set(VideoCaptureProperties.FrameHeight, 720); // 设置采集的图像高度
            //Cap.Set(VideoCaptureProperties.Exposure, 0); // 设置曝光值

            Mat bgimg0 = new Mat();
            Mat bgimg = new Mat();
            int frame_no = 10; // 第10帧稳定图像
            while (frame_no > 0)
            {
                if (Cap.Read(bgimg0))
                    frame_no--;
            }
            Cv2.CvtColor(bgimg0, bgimg, ColorConversionCodes.BGR2GRAY);

            Mat frame = new Mat();
            Mat gray = new Mat();
            Mat st = new Mat();
            Mat img = new Mat();
            while (true)
            {
                if (Cap.Read(frame))
                {
                    Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.Subtract(gray, bgimg, st);

                    Mat threshold = new Mat();
                    Cv2.Threshold(st, threshold, 50, 255, ThresholdTypes.Binary);

                    Point[][] contours = new Point[][] { };
                    HierarchyIndex[] hierarcy;
                    Cv2.FindContours(threshold, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

                    Cv2.DrawContours(st, contours, -1, new Scalar(255, 255, 255), 3);
                    img = st;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        var area = Cv2.ContourArea(contours[i]);
                        if (area < 200)
                            continue;

                        double peri = Cv2.ArcLength(contours[i], true);
                        Point[] approx = Cv2.ApproxPolyDP(contours[i], 0.04 * peri, true);
                        if (approx.Length == 4)
                        {
                            Rect rect = Cv2.BoundingRect(approx);
                            Cv2.Rectangle(frame, new Point(rect.X, rect.Y),
                                new Point(rect.X + rect.Width, rect.Y + rect.Height),
                                new Scalar(0, 0, 255), 2);
                        }
                    }

                    // TODO 对比前几/十几帧，新放一张扑克，知道是那张
                    // 等待图像稳定，不放牌后，再计算
                    myImshow("frame", frame);
                    myImshow("subtract", img);
                    myImshow("threshold", threshold);

                    Cv2.WaitKey(1);
                }
            }
        }
        #endregion
    }
}
