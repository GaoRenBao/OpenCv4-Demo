/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：SURF/SIFT特征点检测
博客：http://www.bilibili996.com/Course?id=2848892000230
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
            demo1();
            demo2();
            Cv2.WaitKey();
        }

        /// <summary>
        /// SURF
        /// </summary>
        static void demo1()
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("../../../images/book2.jpg");
            Mat srcImage2 = Cv2.ImRead("../../../images/book3.jpg");

            // 定义SURF中的hessian阈值特征点检测算子
            int minHessian = 400;
            // 定义一个特征检测类对象
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(minHessian, 4, 3, true, true);

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            KeyPoint[] keypoints_1 = MySurf.Detect(srcImage1);
            KeyPoint[] keypoints_2 = MySurf.Detect(srcImage2);

            // 绘制特征关键点.
            Mat img_keypoints_1 = new Mat();
            Mat img_keypoints_2 = new Mat();
            Cv2.DrawKeypoints(srcImage1, keypoints_1, img_keypoints_1, new Scalar(0, 0, 255), DrawMatchesFlags.Default);
            Cv2.DrawKeypoints(srcImage2, keypoints_2, img_keypoints_2, new Scalar(0, 0, 255), DrawMatchesFlags.Default);

            // 显示效果图
            Cv2.ImShow("SURF特征点检测效果图1", img_keypoints_1);
            Cv2.ImShow("SURF特征点检测效果图2", img_keypoints_2);
        }

        /// <summary>
        /// SIFT
        /// </summary>
        static void demo2()
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("../../../images/book2.jpg");
            Mat srcImage2 = Cv2.ImRead("../../../images/book3.jpg");

            // 定义SURF中的hessian阈值特征点检测算子
            int minHessian = 400;
            // 定义一个特征检测类对象
            var MySift = OpenCvSharp.Features2D.SIFT.Create(minHessian);
            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            KeyPoint[] keypoints_1 = MySift.Detect(srcImage1);
            KeyPoint[] keypoints_2 = MySift.Detect(srcImage2);

            // 绘制特征关键点.
            Mat img_keypoints_1 = new Mat();
            Mat img_keypoints_2 = new Mat();
            Cv2.DrawKeypoints(srcImage1, keypoints_1, img_keypoints_1, new Scalar(0, 255, 0), DrawMatchesFlags.Default);
            Cv2.DrawKeypoints(srcImage2, keypoints_2, img_keypoints_2, new Scalar(0, 255, 0), DrawMatchesFlags.Default);

            // 显示效果图
            Cv2.ImShow("SIFT特征点检测效果图1", img_keypoints_1);
            Cv2.ImShow("SIFT特征点检测效果图2", img_keypoints_2);
        }
    }
}
