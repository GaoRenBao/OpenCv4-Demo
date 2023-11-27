/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：OpenCV中的稠密光流
博客：http://www.bilibili996.com/Course?id=6af89dda880846cca6da479b654d68d0
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VideoCapture cap = new VideoCapture("../../../images/vtest.avi");

            Mat frame1 = new Mat();
            cap.Read(frame1);

            Mat prvs = new Mat();
            Cv2.CvtColor(frame1, prvs, ColorConversionCodes.BGR2GRAY);

            Mat hsv = new Mat();
            // 设置hsv矩阵的第二个通道（索引为1）为255  
            Mat[] SetChannel = {
                new Mat(frame1.Size(), MatType.CV_8UC1, new Scalar(0)),
                new Mat(frame1.Size(), MatType.CV_8UC1, new Scalar(255)),
                new Mat(frame1.Size(), MatType.CV_8UC1, new Scalar(0))
            };
            Cv2.Merge(SetChannel, hsv);

            Mat frame2 = new Mat();
            Mat next = new Mat();
            Mat flow = new Mat();
            Mat mag = new Mat();
            Mat ang = new Mat();
            Mat magn_norm = new Mat();
            Mat bgr = new Mat();

            while (true)
            {
                cap.Read(frame2);
                Cv2.CvtColor(frame2, next, ColorConversionCodes.BGR2GRAY);
                Cv2.CalcOpticalFlowFarneback(prvs, next, flow, 0.5, 3, 15, 3, 5, 1.2, OpticalFlowFlags.None);

                // 分离通道
                Mat[] channels = new Mat[2];
                Cv2.Split(flow, out channels);
                Cv2.CartToPolar(channels[0], channels[1], mag, ang);
                Cv2.Normalize(mag, magn_norm, 0, 255, NormTypes.MinMax);

                Mat[] hsvCh = new Mat[3];
                Cv2.Split(hsv, out hsvCh);

                // CV_32FC1转CV_8UC1
                ang = ang * 180 / Cv2.PI / 2;
                ang.ConvertTo(ang, MatType.CV_8UC1);
                magn_norm.ConvertTo(magn_norm, MatType.CV_8UC1);
                hsvCh[1].ConvertTo(hsvCh[1], MatType.CV_8UC1);

                Mat[] _hsv = { ang, hsvCh[1], magn_norm };
                Cv2.Merge(_hsv, hsv);
                Cv2.CvtColor(hsv, bgr, ColorConversionCodes.HSV2BGR);

                Cv2.ImShow("frame2", frame2);
                Cv2.ImShow("flow", bgr);

                if (Cv2.WaitKey(1) == 27)
                    break;
                next.CopyTo(prvs);
            }
            cap.Release();
            Cv2.DestroyAllWindows();
        }
    }
}
