/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：使用Haar分类器之面部检测
博客：http://www.bilibili996.com/Course?id=be9bc00c296a4fe59e0c86474bbf9f43
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
            //运行之前，检查cascade文件路径是否在你的电脑上
            CascadeClassifier face_cascade = new CascadeClassifier("../../../images/haarcascade/haarcascade_frontalface_default.xml");
            CascadeClassifier eye_cascade = new CascadeClassifier("../../../images/haarcascade/haarcascade_eye.xml");

            //Mat img = Cv2.ImRead("../../../images/kongjie_hezhao.jpg");
            Mat img = Cv2.ImRead("../../../images/airline-stewardess-bikini.jpg");
            Mat gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("gray", gray);

            //Detects objects of different sizes in the input image.
            //The detected objects are returned as a list of rectangles.
            //cv2.CascadeClassifier.detectMultiScale(image, scaleFactor, minNeighbors, flags, minSize, maxSize)
            //scaleFactor – Parameter specifying how much the image size is reduced at each image
            //scale.
            //minNeighbors – Parameter specifying how many neighbors each candidate rectangle should
            //have to retain it.
            //minSize – Minimum possible object size. Objects smaller than that are ignored.
            //maxSize – Maximum possible object size. Objects larger than that are ignored.
            //faces = face_cascade.detectMultiScale(gray, 1.3, 5)
            Rect[] faces = face_cascade.DetectMultiScale(gray, scaleFactor: 1.1, minNeighbors: 5,
                minSize: new Size(30, 30), flags: HaarDetectionTypes.ScaleImage); //改进
            Console.WriteLine($"Detected：{faces.Length}");

            foreach (var face in faces)
            {
                Cv2.Rectangle(img,
                    new Point(face.X, face.Y),
                    new Point(face.X + face.Width, face.Y + face.Height),
                    new Scalar(255, 0, 0), 2);

                Mat roi_gray = new Mat(gray, face);
                Mat roi_color = new Mat(img, face);
                Rect[] eyes = eye_cascade.DetectMultiScale(roi_gray);
                foreach (var ey in eyes)
                {
                    Cv2.Rectangle(roi_color,
                        new Point(ey.X, ey.Y),
                        new Point(ey.X + ey.Width, ey.Y + ey.Height),
                        new Scalar(0, 255, 0), 2);
                }
            }

            Cv2.ImShow("img", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
