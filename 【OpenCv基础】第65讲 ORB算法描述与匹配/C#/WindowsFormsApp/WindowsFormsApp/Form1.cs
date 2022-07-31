using OpenCvSharp;
using OpenCvSharp.XFeatures2D;
using System;
using System.Windows.Forms;
using OpenCvSharp.Flann;
using System.Collections.Generic;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// FREAKSample
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var gray = new Mat("1.jpg", ImreadModes.Grayscale);
            var dst = new Mat("2.jpg", ImreadModes.Color);

            // ORB
            var orb = ORB.Create(1000);
            KeyPoint[] keypoints = orb.Detect(gray);

            // FREAK
            FREAK freak = FREAK.Create();
            Mat freakDescriptors = new Mat();
            freak.Compute(gray, ref keypoints, freakDescriptors);

            if (keypoints != null)
            {
                var color = new Scalar(0, 255, 0);
                foreach (KeyPoint kpt in keypoints)
                {
                    float r = kpt.Size / 2;
                    Cv2.Circle(dst, (Point)kpt.Pt, (int)r, color);
                    Cv2.Line(dst,
                        (Point)new Point2f(kpt.Pt.X + r, kpt.Pt.Y + r),
                        (Point)new Point2f(kpt.Pt.X - r, kpt.Pt.Y - r),
                        color);
                    Cv2.Line(dst,
                        (Point)new Point2f(kpt.Pt.X - r, kpt.Pt.Y + r),
                        (Point)new Point2f(kpt.Pt.X + r, kpt.Pt.Y - r),
                        color);
                }
            }

            // Cv2.ImWrite("FREAKSample.jpg", dst);

            using (new Window("FREAK", dst))
            {
                Cv2.WaitKey();
            }
        }

        /// <summary>
        /// FlannSample
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("===== FlannTest =====");

            // creates data set
            using (var features = new Mat(10000, 2, MatType.CV_32FC1))
            {
                var rand = new Random();
                for (int i = 0; i < features.Rows; i++)
                {
                    features.Set<float>(i, 0, rand.Next(10000));
                    features.Set<float>(i, 1, rand.Next(10000));
                }

                // query
                var queryPoint = new Point2f(7777, 7777);
                var queries = new Mat(1, 2, MatType.CV_32FC1);
                queries.Set<float>(0, 0, queryPoint.X);
                queries.Set<float>(0, 1, queryPoint.Y);
                Console.WriteLine("query:({0}, {1})", queryPoint.X, queryPoint.Y);
                Console.WriteLine("-----");

                // knnSearch
                using (var nnIndex = new Index(features, new KDTreeIndexParams(4)))
                {
                    const int Knn = 1;
                    int[] indices;
                    float[] dists;
                    nnIndex.KnnSearch(queries, out indices, out dists, Knn, new SearchParams(32));

                    for (int i = 0; i < Knn; i++)
                    {
                        int index = indices[i];
                        float dist = dists[i];
                        var pt = new Point2f(features.Get<float>(index, 0), features.Get<float>(index, 1));
                        Console.Write("No.{0}\t", i);
                        Console.Write("index:{0}", index);
                        Console.Write(" distance:{0}", dist);
                        Console.Write(" data:({0}, {1})", pt.X, pt.Y);
                        Console.WriteLine();
                    }
                }
            }
            Console.Read();
        }

        /// <summary>
        /// 毛星云demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
			//【0】载入源图，显示并转化为灰度图
			Mat srcImage = Cv2.ImRead("1.jpg");
			Cv2.ImShow("原始图", srcImage);
			Mat grayImage = new Mat();
			Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGR2GRAY);

			//------------------检测ORB特征点并在图像中提取物体的描述符----------------------

			//【1】参数定义
			var featureDetector = ORB.Create();

			//【2】调用detect函数检测出特征关键点，保存在KeyPoint容器中
			KeyPoint[] keyPoints = featureDetector.Detect(grayImage);

			//【3】计算描述符（特征向量）
			Mat descriptors = new Mat();
			featureDetector.Compute(grayImage, ref keyPoints, descriptors);

			//【4】基于FLANN的描述符对象匹配
			var flannIndex = new Index(descriptors, new LshIndexParams(12, 20, 2), FlannDistance.Hamming);

			//【5】初始化视频采集对象
			VideoCapture Cap = new VideoCapture();
			Cap.Open(0);
			// 判断摄像头是否成功打开
			if (!Cap.IsOpened())
			{
				MessageBox.Show("摄像头打开失败.");
				return;
			}

			//【6】轮询，直到按下ESC键退出循环
			Mat captureImage = new Mat();
			Mat captureImage_gray = new Mat();
			Mat captureDescription = new Mat();

			while (true)
			{
				if (!Cap.Read(captureImage))
					continue;

				//转化图像到灰度
				Cv2.CvtColor(captureImage, captureImage_gray, ColorConversionCodes.BGR2GRAY);//采集的视频帧转化为灰度图

				//【7】检测SIFT关键点并提取测试图像中的描述符
				//【8】调用detect函数检测出特征关键点，保存在vector容器中
				KeyPoint[] captureKeyPoints = featureDetector.Detect(captureImage_gray);

				//【9】计算描述符
				featureDetector.Compute(captureImage_gray, ref captureKeyPoints, captureDescription);

				//【10】匹配和测试描述符，获取两个最邻近的描述符
				Mat matchIndex = new Mat(captureDescription.Rows, 2, MatType.CV_32SC1);
				Mat matchDistance = new Mat(captureDescription.Rows, 2, MatType.CV_32FC1);

				//调用K邻近算法
				flannIndex.KnnSearch(captureDescription, matchIndex, matchDistance, 2, new SearchParams());

				//【11】根据劳氏算法（Lowe's algorithm）选出优秀的匹配
				List<DMatch> goodMatches = new List<DMatch>();
				for (int i = 0; i < matchDistance.Rows; i++)
				{
					if (matchDistance.Get<float>(i, 0) < 0.6 * matchDistance.Get<float>(i, 1))
                    {
						DMatch dmatches = new DMatch(i, matchIndex.Get<int>(i, 0), matchDistance.Get<float>(i, 0));
						goodMatches.Add(dmatches);
					}
				}

				//【12】绘制并显示匹配窗口
				Mat resultImage = new Mat();
				Cv2.DrawMatches(captureImage, captureKeyPoints, srcImage, keyPoints, goodMatches, resultImage);

                // 显示效果图
                Cv2.Resize(resultImage, resultImage, new Size(resultImage.Cols * 0.5, resultImage.Rows * 0.5), 0, 0, InterpolationFlags.Area);
                Cv2.ImShow("匹配窗口", resultImage);
				Cv2.WaitKey(1);
			}
		}
    }
}