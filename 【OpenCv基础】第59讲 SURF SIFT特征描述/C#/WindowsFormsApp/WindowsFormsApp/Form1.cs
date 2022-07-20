using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SURF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("1.jpg");
            Mat srcImage2 = Cv2.ImRead("2.jpg");

            // 定义SURF中的hessian阈值特征点检测算子
            int minHessian = 700;
            // 定义一个特征检测类对象
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(minHessian, 4, 3, true, true);

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            KeyPoint[] keyPoint1 = MySurf.Detect(srcImage1);
            KeyPoint[] keyPoint2 = MySurf.Detect(srcImage2);

            // 方法1：计算描述符（特征向量）
            //Mat descriptors1 = new Mat();
            //Mat descriptors2 = new Mat();
            //MySurf.Compute(srcImage1, ref keyPoint1, descriptors1);
            //MySurf.Compute(srcImage2, ref keyPoint2, descriptors2);

            // 方法2：计算描述符（特征向量）
            KeyPoint[] keypoints1, keypoints2;
            Mat descriptors1 = new Mat();
            Mat descriptors2 = new Mat();
            MySurf.DetectAndCompute(srcImage1, null, out keypoints1, descriptors1);
            MySurf.DetectAndCompute(srcImage2, null, out keypoints2, descriptors2);

            // 使用BruteForce进行匹配
            // 创建特征点匹配器
            BFMatcher matcher = new BFMatcher();
            // 匹配两幅图中的描述子（descriptors）
            DMatch[] matches = matcher.Match(descriptors1, descriptors2);

            //【6】绘制从两个图像中匹配出的关键点
            Mat imgMatches = new Mat();
            Cv2.DrawMatches(srcImage1, keyPoint1, srcImage2, keyPoint2, matches, imgMatches);//进行绘制

            // 显示效果图
            Cv2.ImShow("匹配图", imgMatches);
        }

        /// <summary>
        /// SIFT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("1.jpg");
            Mat srcImage2 = Cv2.ImRead("2.jpg");

            // 定义SURF中的hessian阈值特征点检测算子
            int minHessian = 700;
            // 定义一个特征检测类对象
            var MySift = OpenCvSharp.Features2D.SIFT.Create(minHessian);

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            KeyPoint[] keyPoint1 = MySift.Detect(srcImage1);
            KeyPoint[] keyPoint2 = MySift.Detect(srcImage2);

            // 方法1：计算描述符（特征向量）
            //Mat descriptors1 = new Mat();
            //Mat descriptors2 = new Mat();
            //MySift.Compute(srcImage1, ref keyPoint1, descriptors1);
            //MySift.Compute(srcImage2, ref keyPoint2, descriptors2);

            // 方法2：计算描述符（特征向量）
            KeyPoint[] keypoints1, keypoints2;
            Mat descriptors1 = new Mat();
            Mat descriptors2 = new Mat();
            MySift.DetectAndCompute(srcImage1, null, out keypoints1, descriptors1);
            MySift.DetectAndCompute(srcImage2, null, out keypoints2, descriptors2);

            // 使用BruteForce进行匹配
            // 创建特征点匹配器
            BFMatcher matcher = new BFMatcher();
            // 匹配两幅图中的描述子（descriptors）
            DMatch[] matches = matcher.Match(descriptors1, descriptors2);

            //【6】绘制从两个图像中匹配出的关键点
            Mat imgMatches = new Mat();
            Cv2.DrawMatches(srcImage1, keyPoint1, srcImage2, keyPoint2, matches, imgMatches);//进行绘制

            // 显示效果图
            Cv2.ImShow("匹配图", imgMatches);
        }
    }
}