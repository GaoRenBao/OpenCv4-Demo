/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：SVM手写数字识别
博客：http://www.bilibili996.com/Course?id=17494cb1802e403ea25d1096a5659478
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace demo
{
    internal class Program
    {
        #region GitHub上的一个demo
        private static double Function(double x)
        {
            return x + 50 * Math.Sin(x / 15.0);
        }

        /// <summary>
        /// GitHub上的一个demo
        /// https://github.com/shimat/opencvsharp_samples/tree/master/SamplesLegacy/Samples/SVMSample.cs
        /// </summary>
        public static void RunTest()
        {
            // Training data          
            var points = new Point2f[500];
            var responses = new int[points.Length];
            var rand = new Random();
            for (int i = 0; i < responses.Length; i++)
            {
                float x = rand.Next(0, 300);
                float y = rand.Next(0, 300);
                points[i] = new Point2f(x, y);
                responses[i] = (y > Function(x)) ? 1 : 2;
            }

            // Show training data and f(x)
            using (Mat pointsPlot = Mat.Zeros(300, 300, MatType.CV_8UC3))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    int x = (int)points[i].X;
                    int y = (int)(300 - points[i].Y);
                    int res = responses[i];
                    Scalar color = (res == 1) ? Scalar.Red : Scalar.GreenYellow;
                    pointsPlot.Circle(x, y, 2, color, -1);
                }
                // f(x)
                for (int x = 1; x < 300; x++)
                {
                    int y1 = (int)(300 - Function(x - 1));
                    int y2 = (int)(300 - Function(x));
                    pointsPlot.Line(x - 1, y1, x, y2, Scalar.LightBlue, 1);
                }
                Cv2.ImShow("pointsPlot", pointsPlot);
            }

            // Train
            var dataMat = new Mat(points.Length, 2, MatType.CV_32FC1, points);
            var resMat = new Mat(responses.Length, 1, MatType.CV_32SC1, responses);
            var svm = SVM.Create();
            // normalize data
            dataMat /= 300.0;

            // SVM parameters
            svm.Type = SVM.Types.CSvc;
            svm.KernelType = SVM.KernelTypes.Rbf;
            svm.TermCriteria = TermCriteria.Both(1000, 0.000001);
            svm.Degree = 100.0;
            svm.Gamma = 100.0;
            svm.Coef0 = 1.0;
            svm.C = 1.0;
            svm.Nu = 0.5;
            svm.P = 0.1;

            svm.Train(dataMat, SampleTypes.RowSample, resMat);

            // Predict for each 300x300 pixel
            Mat retPlot = Mat.Zeros(300, 300, MatType.CV_8UC3);
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    float[] sample = { x / 300f, y / 300f };
                    var sampleMat = new Mat(1, 2, MatType.CV_32FC1, sample);
                    int ret = (int)svm.Predict(sampleMat);
                    var plotRect = new Rect(x, 300 - y, 1, 1);
                    if (ret == 1)
                        retPlot.Rectangle(plotRect, Scalar.Red);
                    else if (ret == 2)
                        retPlot.Rectangle(plotRect, Scalar.GreenYellow);
                }
            }
            Cv2.ImShow("retPlot", retPlot);
        }
        #endregion

        #region SVM手写数字识别
        /// <summary>
        /// 切图
        /// </summary>
        /// <returns></returns>
        static List<List<Mat>> RoiImages(string path)
        {
            Mat img = Cv2.ImRead(path);
            Mat gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

            List<List<Mat>> cells = new List<List<Mat>>();
            int w = gray.Width / 100; // 每行100个字符
            int h = gray.Height / 50; // 共50行
            Mat roi = new Mat();
            for (int i = 0; i < 10; i++) // 数字0~9
            {
                cells.Add(new List<Mat>());
                for (int j = 0; j < 5; j++) // 每组数字有5行
                {
                    for (int n = 0; n < 100; n++) // 每行100个字符
                    {
                        Rect rect = new Rect()
                        {
                            X = n * w,
                            Y = h * ((i * 5) + j),
                            Width = w,
                            Height = h
                        };
                        roi = new Mat(gray, rect);
                        cells[i].Add(roi);

                        //Console.WriteLine(rect);
                        //Cv2.ImShow("roi", roi);
                        //Cv2.WaitKey(1);
                    }
                }
            }
            return cells;
        }

        /// <summary>
        /// SVM手写数字识别
        /// </summary>
        static void demo()
        {
            // 切图
            var cells = RoiImages("../../../images/digits.png");

            // 训练数据数量，前400张图片用来训练
            int train_sample_count = 400 * 10;
            // 声明训练数据集合 mat，图像尺寸：20*20
            Mat trainData = new Mat(train_sample_count, 20 * 20, MatType.CV_32FC1);
            // 声明训练数据标签 mat
            Mat trainLabel = new Mat(train_sample_count, 1, MatType.CV_32SC1);

            // 组织训练数据，循环训练文件夹内所有图片。
            int trainNum = 0;
            for (int i = 0; i < 10; i++)
            {
                var cell = cells[i].Take(400);
                foreach (Mat temp in cell)
                {
                    // 转换CV_32FC1，因为下面训练函数需要这个格式
                    temp.ConvertTo(temp, MatType.CV_32FC1);
                    // 写入到训练数据集合的mat内，注意reshape的用法。
                    /*
                    reshape有两个参数：
                    其中，参数：cn为新的通道数，如果cn = 0，表示通道数不会改变。
                    参数rows为新的行数，如果rows = 0，表示行数不会改变。
                    注意：新的行* 列必须与原来的行*列相等。就是说，如果原来是5行3列，新的行和列可以是1行15列，3行5列，5行3列，15行1列。
                          设置行数后，列数会自动调整。比如此处 调整为 1行784列。
                    */
                    temp.Reshape(0, 1).CopyTo(trainData.Row(trainNum));
                    // 写入到训练标签集合的mat内
                    // 这个必须是int类型，不能用float，和knn的操作有点差异
                    trainLabel.Set<int>(trainNum, i);
                    trainNum++;
                }
            }

            // 实例化对象
            SVM svm = SVM.Create();
            // 设置核函数
            svm.KernelType = SVM.KernelTypes.Linear;
            // 设置训练类型
            svm.Type = SVM.Types.CSvc;
            svm.C = 2.67;
            svm.Gamma = 5.383;
            // 输入样本，进行训练
            svm.Train(trainData, SampleTypes.RowSample, trainLabel);
            // 存储训练结果
            //svm.Save("svm_data.dat");

            /**************  Now testing ***************/
            // 测试数据数量，后100张图片用来测试
            int test_sample_count = 100 * 10;
            // 声明测试数据集合 mat
            Mat testData = new Mat(test_sample_count, 20 * 20, MatType.CV_32FC1);
            // 声明测试数据标签 mat
            Mat testLabel = new Mat(test_sample_count, 1, MatType.CV_32FC1);
            // 组织测试数据
            int testNum = 0;
            for (int i = 0; i < 10; i++)
            {
                var cell = cells[i].Skip(400);
                foreach (Mat temp in cell)
                {
                    temp.ConvertTo(temp, MatType.CV_32FC1);
                    temp.Reshape(0, 1).CopyTo(testData.Row(testNum));
                    testLabel.Set<float>(testNum, i);
                    testNum++;
                }
            }

            Mat result = new Mat();
            svm.Predict(testData, result);
            int t = 0;
            int f = 0;
            for (int i = 0; i < test_sample_count; i++)
            {
                int predict = (int)result.At<float>(i);
                int actual = (int)testLabel.At<float>(i);
                if (predict == actual)
                {
                    Console.WriteLine("正确：" + predict + "-" + actual);
                    t++;
                }
                else
                {
                    Console.WriteLine("错误------：" + predict + "-" + actual);
                    f++;
                }
            }

            double accuracy = (t * 1.0) / (t + f);
            Console.WriteLine("准确率：" + accuracy); // 准确率：0.908
            Console.Read();
        }
        #endregion

        static void Main(string[] args)
        {
            //RunTest();
            //Cv2.WaitKey();

            demo();
        }
    }
}
