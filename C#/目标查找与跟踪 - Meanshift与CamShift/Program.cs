/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：目标查找与跟踪 - Meanshift与CamShift
博客：http://www.bilibili996.com/Course?id=0699ef6cd6e1407bbfb39a5e39b81e9a
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
            var capture = new VideoCapture("../../../images/Meanshift_CamShift.mp4");
            if (!capture.IsOpened())
            {
                Console.WriteLine("Open video failed!");
                return;
            }

            // take first frame of the video
            Mat frame = new Mat();
            capture.Read(frame);

            //setup initial location of window
            Rect track_window = new Rect(65, 275, 105, 105); // simply hardcoded the values

            //set up the ROI for tracking
            Mat roi = frame[track_window];
            //Cv2.ImShow("frame", frame);
            //Cv2.ImShow("roi", roi);
            //Cv2.WaitKey(0);

            Mat hsv_roi = new Mat();
            Cv2.CvtColor(roi, hsv_roi, ColorConversionCodes.BGR2HSV);

            //将低亮度的值忽略掉
            Mat mask = new Mat();
            Cv2.InRange(hsv_roi, new Scalar(0, 100, 0), new Scalar(100, 255, 255), mask);

            Mat roi_hist = new Mat();
            Cv2.CalcHist(new Mat[] { hsv_roi }, new int[] { 0 }, mask,
                roi_hist, 1, new int[] { 180 }, new float[][] { new float[] { 0, 180 } });
            //归一化
            Cv2.Normalize(roi_hist, roi_hist, 0, 255, NormTypes.MinMax);

            //Setup the termination criteria, either 10 iteration or move by atleast 1 pt
            TermCriteria term_crit = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.Count, 10, 1);

            Mat hsv = new Mat();
            Mat dst = new Mat();
            Mat img1 = new Mat();
            Mat img2 = new Mat();
            Rect track_window1 = track_window;
            Rect track_window2 = track_window;
            while (true)
            {
                capture.Read(frame);
                if (frame.Empty())
                    break;
                frame.CopyTo(img1);
                frame.CopyTo(img2);

                Cv2.CvtColor(frame, hsv, ColorConversionCodes.BGR2HSV);
                Cv2.CalcBackProject(new Mat[] { hsv }, new int[] { 0 },
                    roi_hist, dst, new Rangef[] { new Rangef(0, 180) }, true);

                // meanShift 效果
                Cv2.MeanShift(dst, ref track_window1, term_crit);
                Cv2.PutText(img1, "MeanShift", new Point(10, 30), HersheyFonts.HersheyComplex, 1, new Scalar(0, 0, 255), 2);
                Cv2.Rectangle(img1, track_window1, 255, 2);
                Cv2.ImShow("img1", img1);

                // CamShift 效果
                RotatedRect ret = Cv2.CamShift(dst, ref track_window2, term_crit);
                Point2f[] line = ret.Points();
                Cv2.PutText(img2, "CamShift", new Point(10, 30), HersheyFonts.HersheyComplex, 1, new Scalar(0, 0, 255), 2);
                Cv2.Line(img2, (Point)line[0], (Point)line[1], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(img2, (Point)line[1], (Point)line[2], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(img2, (Point)line[2], (Point)line[3], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(img2, (Point)line[3], (Point)line[0], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.ImShow("img2", img2);

                // 按ESC退出
                if (Cv2.WaitKey(30) == 27)
                    break;
            }
        }
    }
}
