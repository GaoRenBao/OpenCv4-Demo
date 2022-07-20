using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 视频操作
        /// </summary>
        public VideoCapture Cap = new VideoCapture();

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
            Mat trainImage = Cv2.ImRead("1.jpg");
            Mat trainImage_gray = new Mat();
            Cv2.CvtColor(trainImage, trainImage_gray, ColorConversionCodes.BGR2GRAY);

            // 定义一个特征检测类对象
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(80);

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            Mat trainDescriptor = new Mat();
            KeyPoint[] train_keyPoint = MySurf.Detect(trainImage_gray);

            // 方法1：计算描述符（特征向量）
            MySurf.Compute(trainImage_gray, ref train_keyPoint, trainDescriptor);

            // 方法2：计算描述符（特征向量）
            // KeyPoint[] keypoints1;
            // MySurf.DetectAndCompute(trainImage_gray, null, out keypoints1, trainDescriptor);

            // 创建基于FLANN的描述符匹配对象
            List<Mat> descriptors = new List<Mat>() { trainDescriptor };
            //BFMatcher matcher = new BFMatcher();
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            matcher.Add(descriptors);
            matcher.Train();

            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }

            Mat testImage = new Mat();
            Mat testImage_gray = new Mat();

            while (true)
            {
                if (Cap.Read(testImage))
                {
                    // 转化图像到灰度
                    Cv2.CvtColor(testImage, testImage_gray, ColorConversionCodes.BGR2GRAY);

                    // 检测S关键点、提取测试图像描述符
                    Mat testDescriptor = new Mat();
                    KeyPoint[] test_keyPoint = MySurf.Detect(testImage_gray);

                    // 方法1：计算描述符（特征向量）
                    MySurf.Compute(testImage_gray, ref test_keyPoint, testDescriptor);

                    // 方法2：计算描述符（特征向量）
                    // MySurf.DetectAndCompute(testImage_gray, null, out keypoints1, testDescriptor);

                    // 匹配训练和测试描述符
                    DMatch[][] matches = matcher.KnnMatch(testDescriptor, 2);

                    // 根据劳氏算法（Lowe's algorithm），得到优秀的匹配点
                    List<DMatch> goodMatches = new List<DMatch>();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        if (matches[i][0].Distance < 0.6 * matches[i][1].Distance)
                            goodMatches.Add(matches[i][0]);
                    }

                    //绘制匹配点并显示窗口
                    Mat dstImage = new Mat();
                    Cv2.DrawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, dstImage);

                    // 显示效果图
                    Cv2.ImShow("匹配窗口", dstImage);
                    Cv2.WaitKey(1);

                }
            }
        }

        /// <summary>
        /// SIFT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 载入源图片并显示
            Mat trainImage = Cv2.ImRead("1.jpg");
            Mat trainImage_gray = new Mat();
            Cv2.CvtColor(trainImage, trainImage_gray, ColorConversionCodes.BGR2GRAY);

            // 定义一个特征检测类对象
            var MySift = OpenCvSharp.Features2D.SIFT.Create(80);

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            Mat trainDescriptor = new Mat();
            KeyPoint[] train_keyPoint = MySift.Detect(trainImage_gray);

            // 方法1：计算描述符（特征向量）
            MySift.Compute(trainImage_gray, ref train_keyPoint, trainDescriptor);

            // 方法2：计算描述符（特征向量）
            // KeyPoint[] keypoints1;
            // MySift.DetectAndCompute(trainImage_gray, null, out keypoints1, trainDescriptor);

            // 创建基于FLANN的描述符匹配对象
            List<Mat> descriptors = new List<Mat>() { trainDescriptor };
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            matcher.Add(descriptors);
            matcher.Train();

            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }

            Mat testImage = new Mat();
            Mat testImage_gray = new Mat();

            while (true)
            {
                if (Cap.Read(testImage))
                {
                    // 转化图像到灰度
                    Cv2.CvtColor(testImage, testImage_gray, ColorConversionCodes.BGR2GRAY);

                    // 检测S关键点、提取测试图像描述符
                    Mat testDescriptor = new Mat();
                    KeyPoint[] test_keyPoint = MySift.Detect(testImage_gray);

                    // 方法1：计算描述符（特征向量）
                    MySift.Compute(testImage_gray, ref test_keyPoint, testDescriptor);

                    // 方法2：计算描述符（特征向量）
                    // MySurf.DetectAndCompute(testImage_gray, null, out keypoints1, testDescriptor);

                    // 匹配训练和测试描述符
                    DMatch[][] matches = matcher.KnnMatch(testDescriptor, 2);

                    // 根据劳氏算法（Lowe's algorithm），得到优秀的匹配点
                    List<DMatch> goodMatches = new List<DMatch>();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        if (matches[i][0].Distance < 0.6 * matches[i][1].Distance)
                            goodMatches.Add(matches[i][0]);
                    }

                    //绘制匹配点并显示窗口
                    Mat dstImage = new Mat();
                    Cv2.DrawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, dstImage);

                    // 显示效果图
                    Cv2.ImShow("匹配窗口", dstImage);
                    Cv2.WaitKey(1);

                }
            }
        }
    }
}