/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：人脸识别
博客：http://www.bilibili996.com/Course?id=4962812000072
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        public static CascadeClassifier face_cascade = new CascadeClassifier();
        public static CascadeClassifier eyes_cascade = new CascadeClassifier();

        // 打开摄像头
        static void Main(string[] args)
        {
            // 加载xml文件
            if (!face_cascade.Load(@"Files\haarcascade_frontalface_alt.xml"))
            {
                Console.WriteLine("load haarcascade_frontalface_alt.xml Error.");
                Console.Read();
                return;
            }
            if (!eyes_cascade.Load(@"Files\haarcascade_eye_tree_eyeglasses.xml"))
            {
                Console.WriteLine("load haarcascade_eye_tree_eyeglasses.xml Error.");
                Console.Read();
                return;
            }

            VideoCapture Cap = new VideoCapture();
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                Console.Read();
                return;
            }

            // opencv3
            //Cap.Set(CaptureProperty.FrameWidth, 640); // 设置采集的图像尺寸为：640*480
            //Cap.Set(CaptureProperty.FrameHeight, 480); // 设置采集的图像尺寸为：640*480
            //Cap.Set(CaptureProperty.Exposure, -3); // 曝光

            // opencv4
            Cap.Set(VideoCaptureProperties.FrameWidth, 640); // 设置采集的图像尺寸为：640*480
            Cap.Set(VideoCaptureProperties.FrameHeight, 480); // 设置采集的图像尺寸为：640*480
            Cap.Set(VideoCaptureProperties.Exposure, -3); // 曝光

            Mat frame = new Mat();
            var window = new Window("frame");
            while (true)
            {
                if (Cap.Read(frame))
                {
                    // 人脸识别
                    frame = detectAndDisplay(frame);

                    // 在Window窗口中播放视频(方法1)
                    window.ShowImage(frame);

                    // 在Window窗口中播放视频(方法2)
                    //Cv2.ImShow("frame", frame);

                    // 在pictureBox1中显示效果图
                    //pictureBox1.Image = BitmapConverter.ToBitmap(frame);

                    if (Cv2.WaitKey(10) == 27)
                        break;
                }
            }
        }

        private static Mat detectAndDisplay(Mat frame)
        {
            // 将原图像转换为灰度图像
            Mat frame_gray = new Mat();
            Cv2.CvtColor(frame, frame_gray, ColorConversionCodes.BGR2GRAY);
            // 直方图均衡化, 用于提高图像的质量
            Cv2.EqualizeHist(frame_gray, frame_gray);

            // 人脸检测
            // opencv3：HaarDetectionType.ScaleImage
            // opencv4：HaarDetectionTypes.ScaleImage
            Rect[] faces = face_cascade.DetectMultiScale(frame_gray, 1.1, 2, 0 | HaarDetectionTypes.ScaleImage, new Size(30, 30));
            for (int i = 0; i < faces.Length; i++)
            {
                // 绘制脸部区域
                Point center = new Point() { X = (faces[i].X + faces[i].Width / 2), Y = (faces[i].Y + faces[i].Width / 2) };
                Cv2.Ellipse(frame, center, new Size(faces[i].Width / 2, faces[i].Height / 2), 0, 0, 360, new Scalar(255, 0, 255), 2, LineTypes.Link8, 0);

                // 绘制眼睛区域
                Mat faceROI = new Mat(frame_gray, faces[i]);
                Cv2.ImShow("faceROI", faceROI);

                Rect[] eyes = eyes_cascade.DetectMultiScale(faceROI, 1.1, 2, 0 | HaarDetectionTypes.ScaleImage, new Size(30, 30));
                for (int j = 0; j < eyes.Length; j++)
                {
                    Point eye_center = new Point() { X = (faces[i].X + eyes[j].X + eyes[j].Width / 2), Y = (faces[i].Y + eyes[j].Y + eyes[j].Height / 2) };
                    int radius = (int)Math.Round((decimal)((eyes[j].Width + eyes[j].Height) * 0.25), 0, MidpointRounding.AwayFromZero);
                    Cv2.Circle(frame, eye_center.X, eye_center.Y, radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                }
            }
            return frame;
        }
    }
}
