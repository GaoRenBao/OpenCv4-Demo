/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：背景减除
博客：http://www.bilibili996.com/Course?id=56cb7ea6a7b84e2297af1334aaeb7609
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
            //demo1();
            //demo2();
            demo3();
        }

        /// <summary>
        /// BackgroundSubtractorMOG
        /// </summary>
        static void demo1()
        {
            VideoCapture cap = new VideoCapture("../../../images/vtest.avi");
            // 笔记本摄像头
            // VideoCapture cap = new VideoCapture(0); 

            BackgroundSubtractorMOG fgbg = BackgroundSubtractorMOG.Create();
            // 可选参数 比如 进行建模场景的时间长度 高斯混合成分的数量-阈值等
            Mat frame = new Mat();
            Mat fgmask = new Mat();
            while (true)
            {
                cap.Read(frame);
                //Cv2.Flip(frame, frame, FlipMode.Y);  // 左右翻转
                fgbg.Apply(frame, fgmask);

                Cv2.ImShow("frame", frame);
                Cv2.ImShow("fgmask", fgmask);
                if (Cv2.WaitKey(1) == 27)
                {
                    break;
                }
            }
            cap.Release();
            Cv2.DestroyAllWindows();
        }

        /// <summary>
        /// BackgroundSubtractorMOG2
        /// </summary>
        static void demo2()
        {
            VideoCapture cap = new VideoCapture("../../../images/vtest.avi");
            // 笔记本摄像头
            // VideoCapture cap = new VideoCapture(0); 

            BackgroundSubtractorMOG2 fgbg = BackgroundSubtractorMOG2.Create();
            Mat frame = new Mat();
            Mat fgmask = new Mat();
            while (true)
            {
                cap.Read(frame);
                //Cv2.Flip(frame, frame, FlipMode.Y);  // 左右翻转
                fgbg.Apply(frame, fgmask);

                Cv2.ImShow("frame", frame);
                Cv2.ImShow("fgmask", fgmask);
                if (Cv2.WaitKey(1) == 27)
                {
                    break;
                }
            }
            cap.Release();
            Cv2.DestroyAllWindows();
        }

        /// <summary>
        /// createBackgroundSubtractorGMG
        /// </summary>
        static void demo3()
        {
            VideoCapture cap = new VideoCapture("../../../images/vtest.avi");
            // 笔记本摄像头
            // VideoCapture cap = new VideoCapture(0); 

            BackgroundSubtractorGMG fgbg = BackgroundSubtractorGMG.Create();
            Mat frame = new Mat();
            Mat fgmask = new Mat();
            while (true)
            {
                cap.Read(frame);
                //Cv2.Flip(frame, frame, FlipMode.Y);  // 左右翻转
                fgbg.Apply(frame, fgmask);

                Cv2.ImShow("frame", frame);
                Cv2.ImShow("fgmask", fgmask);
                if (Cv2.WaitKey(1) == 27)
                {
                    break;
                }
            }
            cap.Release();
            Cv2.DestroyAllWindows();
        }
    }
}
