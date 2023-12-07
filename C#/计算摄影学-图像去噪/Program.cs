/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：计算摄影学-图像去噪
博客：http://www.bilibili996.com/Course?id=4faa3bccc8084d3498b91a610fec4ecb
作者：高仁宝
时间：2023.11

参考来源：https://github.com/shimat/opencvsharp/blob/main/test/OpenCvSharp.Tests/photo/PhotoTest.cs
*/

using OpenCvSharp;
using System.Collections.Generic;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //demo1();
            demo2();
        }

        /// <summary>
        /// FastNlMeansDenoisingColored
        /// </summary>
        static void demo1()
        {
            Mat img = Cv2.ImRead("../../../images/die.png");
            Cv2.CvtColor(img, img, ColorConversionCodes.BGR2RGB);

            Mat dst = new Mat();
            Cv2.FastNlMeansDenoisingColored(img, dst, 10, 10, 7, 21);

            Cv2.ImShow("img", img);
            Cv2.ImShow("dst", dst);
            Cv2.WaitKey(0);
        }

        /// <summary>
        /// fastNlMeansDenoisingMulti
        /// </summary>
        static void demo2()
        {
            VideoCapture cap = new VideoCapture("../../../images/vtest.avi");

            // 读取三帧图像
            double pos = cap.Get(VideoCaptureProperties.FrameCount);

            List<Mat> srcImgs = new List<Mat>();
            Mat frame = new Mat();
            for (int i = 0; i < 5; i++)
            {
                cap.Set(VideoCaptureProperties.PosFrames, (int)(pos / 6 * i));
                cap.Read(frame);
                //Cv2.CvtColor(frame, frame, ColorConversionCodes.BGR2GRAY);
                srcImgs.Add(frame);
            }

            // 一共5帧图像，我们取第二帧
            Mat dst = new Mat();

            // FastNlMeansDenoisingMulti存在内存异常，这个版本的OpenCv不能用....

            //Cv2.FastNlMeansDenoisingMulti(srcImgs, dst,
            //    imgToDenoiseIndex: 2,
            //    temporalWindowSize: 5,
            //    h: 4,
            //    templateWindowSize: 7,
            //    searchWindowSize: 35);

            // 这个可以用，但是运行有点慢
            Cv2.FastNlMeansDenoisingColoredMulti(srcImgs, dst,
               imgToDenoiseIndex: 2,
               temporalWindowSize: 5,
               h: 4,
               templateWindowSize: 7,
               searchWindowSize: 35);

            Cv2.ImShow("srcImgs", srcImgs[2]);
            Cv2.ImShow("dst", dst);
            Cv2.WaitKey(0);
        }
    }
}
