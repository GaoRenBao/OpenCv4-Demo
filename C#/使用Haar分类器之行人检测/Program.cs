/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：使用Haar分类器之行人检测
博客：http://www.bilibili996.com/Course?id=c6abbd334a724e8696c1165b7b6b558c
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.IO;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //demo1();
            demo2();
        }

        static void demo1()
        {
            List<string> list_images = new List<string>();
            DirectoryInfo di = new DirectoryInfo("../../../images/haar/");
            FileInfo[] fis = di.GetFiles("*.jpg");
            Mat srcImage = new Mat();
            foreach (FileInfo fi in fis)
            {
                list_images.Add(fi.FullName);
            }

            //initialize the HOG descriptor/person detector
            HOGDescriptor hog = new HOGDescriptor();
            hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            Mat orig = new Mat();

            foreach (var imagePath in list_images)
            {
                //load the image and resize it to (1) reduce detection time
                // and (2) improve detection accuracy
                Mat image = Cv2.ImRead(imagePath);

                // detect people in the image
                Rect[] rects = hog.DetectMultiScale(image, out double[] foundWeights,
                        winStride: new Size(4, 4),
                        padding: new Size(8, 8),
                        scale: 1.05);

                image.CopyTo(orig);
                for (int i = 0; i < rects.Length; i++)
                {
                    // draw the original bounding boxes
                    Cv2.Rectangle(orig,
                        new Point(rects[i].X, rects[i].Y),
                        new Point(rects[i].X + rects[i].Width, rects[i].Y + rects[i].Height),
                        new Scalar(0, 0, 255), 2);
                }
                Cv2.ImShow("orig", orig);

                // OpenCvSharp中没有提供non_max_suppression方法。。。。
                List<float> scores = new List<float>();
                for (int i = 0; i < foundWeights.Length; i++)
                    scores.Add((float)foundWeights[i]);

                int[] indices;
                CvDnn.NMSBoxes(rects, scores, 0.65f, 1, out indices);

                List<Rect> pick = new List<Rect>();
                for (int i = 0; i < indices.Length; i++)
                {
                    pick.Add(rects[indices[i]]);
                }
                for (int i = 0; i < pick.Count; i++)
                {
                    Cv2.Rectangle(image,
                        new Point(pick[i].X, pick[i].Y),
                        new Point(pick[i].X + pick[i].Width, pick[i].Y + pick[i].Height),
                        new Scalar(0, 255, 0), 2);
                }
                Cv2.ImShow("image", image);

                if (Cv2.WaitKey(0) == 'q')
                    break;
            }
        }

        static void demo2()
        {
            HOGDescriptor hog = new HOGDescriptor();
            hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
            VideoCapture cap = new VideoCapture("../../../images/礼让斑马线！齐齐哈尔城市文明的伤！.mp4");
            double fps = cap.Get(VideoCaptureProperties.Fps);  // 25.0
            Console.WriteLine($"Frames per second using video.get(cv2.CAP_PROP_FPS) : {fps}");
            double num_frames = cap.Get(VideoCaptureProperties.FrameCount);
            Console.WriteLine($"共有 {num_frames} 帧");

            double frame_height = cap.Get(VideoCaptureProperties.FrameHeight);
            double frame_width = cap.Get(VideoCaptureProperties.FrameWidth);
            Console.WriteLine($"高：{frame_height},宽：{frame_width}");

            // 跳过多少帧
            int skips = 20;

            Mat image = new Mat();
            Mat orig = new Mat();

            while (cap.IsOpened())
            {
                cap.Read(image);
                //判断是否还有没有视频图像 
                if (image.Empty())
                    break;

                double current = cap.Get(VideoCaptureProperties.PosFrames);
                if (current % skips != 0)
                    continue;

                // detect people in the image
                Rect[] rects = hog.DetectMultiScale(image, out double[] foundWeights,
                        winStride: new Size(4, 4),
                        padding: new Size(8, 8),
                        scale: 1.05);

                image.CopyTo(orig);
                for (int i = 0; i < rects.Length; i++)
                {
                    // draw the original bounding boxes
                    Cv2.Rectangle(orig,
                        new Point(rects[i].X, rects[i].Y),
                        new Point(rects[i].X + rects[i].Width, rects[i].Y + rects[i].Height),
                        new Scalar(0, 0, 255), 2);
                }
                Cv2.ImShow("orig", orig);

                // OpenCvSharp中没有提供 non_max_suppression 方法。。。。
                List<float> scores = new List<float>();
                for (int i = 0; i < foundWeights.Length; i++)
                    scores.Add((float)foundWeights[i]);

                int[] indices;
                CvDnn.NMSBoxes(rects, scores, 0.65f, 1, out indices);

                List<Rect> pick = new List<Rect>();
                for (int i = 0; i < indices.Length; i++)
                {
                    pick.Add(rects[indices[i]]);
                }
                for (int i = 0; i < pick.Count; i++)
                {
                    Cv2.Rectangle(image,
                        new Point(pick[i].X, pick[i].Y),
                        new Point(pick[i].X + pick[i].Width, pick[i].Y + pick[i].Height),
                        new Scalar(0, 255, 0), 2);
                }
                Cv2.ImShow("image", image);

                if (Cv2.WaitKey(10) == 'q')
                    break;
            }
        }
    }
}
