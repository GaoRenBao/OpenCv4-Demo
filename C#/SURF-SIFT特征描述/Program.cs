/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：SURF/SIFT特征描述
博客：http://www.bilibili996.com/Course?id=2950814000235
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
            int minHessian = 700;
            // 定义一个特征检测类对象
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(minHessian, 4, 3, true, true);

            Mat descriptors1 = new Mat();
            Mat descriptors2 = new Mat();

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            //KeyPoint[] keyPoint1 = MySurf.Detect(srcImage1);
            //KeyPoint[] keyPoint2 = MySurf.Detect(srcImage2);
            //MySurf.Compute(srcImage1, ref keyPoint1, descriptors1);
            //MySurf.Compute(srcImage2, ref keyPoint2, descriptors2);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            KeyPoint[] keyPoint1, keyPoint2;
            MySurf.DetectAndCompute(srcImage1, null, out keyPoint1, descriptors1);
            MySurf.DetectAndCompute(srcImage2, null, out keyPoint2, descriptors2);

            // 使用BruteForce进行匹配
            // 创建特征点匹配器
            BFMatcher matcher = new BFMatcher();
            // 匹配两幅图中的描述子（descriptors）
            DMatch[] matches = matcher.Match(descriptors1, descriptors2);

            //【6】绘制从两个图像中匹配出的关键点
            Mat imgMatches = new Mat();
            Cv2.DrawMatches(srcImage1, keyPoint1, srcImage2, keyPoint2, matches, imgMatches);//进行绘制

            // 显示效果图
            Cv2.ImShow("SURF匹配图", imgMatches);
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
            int minHessian = 700;
            // 定义一个特征检测类对象
            var MySift = OpenCvSharp.Features2D.SIFT.Create(minHessian);

            Mat descriptors1 = new Mat();
            Mat descriptors2 = new Mat();

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            //KeyPoint[] keyPoint1 = MySift.Detect(srcImage1);
            //KeyPoint[] keyPoint2 = MySift.Detect(srcImage2);
            //MySift.Compute(srcImage1, ref keyPoint1, descriptors1);
            //MySift.Compute(srcImage2, ref keyPoint2, descriptors2);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            KeyPoint[] keyPoint1, keyPoint2;
            MySift.DetectAndCompute(srcImage1, null, out keyPoint1, descriptors1);
            MySift.DetectAndCompute(srcImage2, null, out keyPoint2, descriptors2);

            // 使用BruteForce进行匹配
            // 创建特征点匹配器
            BFMatcher matcher = new BFMatcher();
            // 匹配两幅图中的描述子（descriptors）
            DMatch[] matches = matcher.Match(descriptors1, descriptors2);

            //【6】绘制从两个图像中匹配出的关键点
            Mat imgMatches = new Mat();
            Cv2.DrawMatches(srcImage1, keyPoint1, srcImage2, keyPoint2, matches, imgMatches);//进行绘制

            // 显示效果图
            Cv2.ImShow("SIFT匹配图", imgMatches);
        }
    }
}
