using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using System;
using System.Windows.Forms;

// 参考网站
// https://www.csharpcodi.com/csharp-examples/OpenCvSharp.ML.SVM.Create()/
// https://www.csharpcodi.com/csharp-examples/OpenCvSharp.Mat.Ptr(int,%20int)/

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width = 512, height = 512;
            Mat image = new Mat(height, width, MatType.CV_8UC3); //创建窗口可视化

            //// 设置训练数据
            int[] labels = { +1, -1, +1, +1 };
            Mat LabelsMat = new Mat(4, 1, MatType.CV_32SC1, labels);

            float[,] trainingData = { { 501, 10 }, { 255, 10 }, { 501, 255 }, { 10, 501 } };
            Mat trainingDataMat = new Mat(4, 2, MatType.CV_32FC1, trainingData);

            // 创建分类器并设置参数
            SVM model = SVM.Create();
            model.Type = SVM.Types.CSvc;
            model.KernelType = SVM.KernelTypes.Linear;
            model.TermCriteria = new TermCriteria(CriteriaType.MaxIter, 100, 1e-6);
            model.Train(trainingDataMat, SampleTypes.RowSample, LabelsMat); // 训练分类器

            Vec3b green = new Vec3b(0, 255, 0);
            Vec3b blue = new Vec3b(255, 0, 0);
            // Show the decision regions given by the SVM
            for (int i = 0; i < image.Rows; ++i)
            {
                for (int j = 0; j < image.Cols; ++j)
                {
                    float[] testFeatureData = { j, i }; //生成测试数据
                    Mat sampleMat = new Mat(1, 2, MatType.CV_32F, testFeatureData);
                    float response = (int)model.Predict(sampleMat);//进行预测，返回1或-1
                    if (response == 1)
                        image.Set(i, j, green);
                    else
                        image.Set(i, j, blue);
                }
            }

            // 显示训练数据
            Cv2.Circle(image, 501, 10, 10,new  Scalar(0, 0, 255), 2, LineTypes.Link8);
            Cv2.Circle(image, 255, 10, 10, new Scalar(0, 0, 255), 2, LineTypes.Link8);
            Cv2.Circle(image, 501, 255, 10, new Scalar(0, 0, 255), 2, LineTypes.Link8);
            Cv2.Circle(image, 10, 501, 10, new Scalar(0, 0, 255), 2, LineTypes.Link8);

            //绘图时，先宽后高，对应先列后行
            for (int i = 0; i < LabelsMat.Rows; i++)
            {
                unsafe // 允许使用不安全代码，可能导致内存泄露
                {
                    float* v = (float*)trainingDataMat.Ptr(i).ToPointer(); //取出每行的头指针
                    Point pt = new Point((int)v[0], (int)v[1]);
                    if (labels[i] == 1)
                        Cv2.Circle(image, pt.X, pt.Y, 5, new Scalar(0, 0, 0), -1, LineTypes.Link8);
                    else
                        Cv2.Circle(image, pt.X, pt.Y, 5, new Scalar(255, 255, 255), -1, LineTypes.Link8);
                }
            }
            pictureBox1.Image = BitmapConverter.ToBitmap(image);
        }
    }
}