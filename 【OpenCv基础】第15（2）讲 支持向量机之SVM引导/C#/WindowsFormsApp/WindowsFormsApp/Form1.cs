using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private int    NTRAINING_SAMPLES = 100; // Number of training samples per class
        private double FRAC_LINEAR_SEP = 0.9;   // Fraction of samples which compose the linear separable part

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int WIDTH = 512, HEIGHT = 512;
            Mat I = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3); //创建窗口可视化

            //--------------------- 1. Set up training data randomly ---------------------------------------
            Mat trainData = new Mat(2 * NTRAINING_SAMPLES, 2, MatType.CV_32FC1);
            Mat labels = new Mat(2 * NTRAINING_SAMPLES, 1, MatType.CV_32SC1);
            RNG rng = new RNG(100);// Random value generation class

            // Set up the linearly separable part of the training data
            int nLinearSamples = (int)(FRAC_LINEAR_SEP * NTRAINING_SAMPLES);

            //! [setup1]
            // Generate random points for the class 1
            Mat trainClass = trainData.RowRange(0, nLinearSamples);
            // The x coordinate of the points is in [0, 0.4)
            Mat c = trainClass.ColRange(0, 1);
            rng.Fill(c, DistributionType.Uniform, new Scalar(1),new Scalar(0.4 * WIDTH));
            // The y coordinate of the points is in [0, 1)
            c = trainClass.ColRange(1, 2);
            rng.Fill(c, DistributionType.Uniform, new Scalar(1), new Scalar(HEIGHT));

            // Generate random points for the class 2
            trainClass = trainData.RowRange(2 * NTRAINING_SAMPLES - nLinearSamples, 2 * NTRAINING_SAMPLES);
            // The x coordinate of the points is in [0.6, 1]
            c = trainClass.ColRange(0, 1);
            rng.Fill(c, DistributionType.Uniform, new Scalar(0.6 * WIDTH), new Scalar(WIDTH));
            // The y coordinate of the points is in [0, 1)
            c = trainClass.ColRange(1, 2);
            rng.Fill(c, DistributionType.Uniform, new Scalar(1), new Scalar(HEIGHT));
            //! [setup1]

            //------------------ Set up the non-linearly separable part of the training data ---------------
            //! [setup2]
            // Generate random points for the classes 1 and 2
            trainClass = trainData.RowRange(nLinearSamples, 2 * NTRAINING_SAMPLES - nLinearSamples);
            // The x coordinate of the points is in [0.4, 0.6)
            c = trainClass.ColRange(0, 1);
            rng.Fill(c, DistributionType.Uniform, new Scalar(0.4 * WIDTH), new Scalar(0.6 * WIDTH));
            // The y coordinate of the points is in [0, 1)
            c = trainClass.ColRange(1, 2);
            rng.Fill(c, DistributionType.Uniform, new Scalar(1), new Scalar(HEIGHT));
            //! [setup2]
            //------------------------- Set up the labels for the classes ---------------------------------
            labels.RowRange(0, NTRAINING_SAMPLES).SetTo(1);  // Class 1
            labels.RowRange(NTRAINING_SAMPLES, 2 * NTRAINING_SAMPLES).SetTo(2);  // Class 2

            //------------------------ 2. Set up the support vector machines parameters --------------------
            //------------------------ 3. Train the svm ----------------------------------------------------
            System.Diagnostics.Debug.WriteLine("Starting training process");
            //! [init]
            SVM svm = SVM.Create(); // 创建分类器并设置参数
            svm.Type = SVM.Types.CSvc;
            svm.C = 0.1;
            svm.KernelType = SVM.KernelTypes.Linear;

            // opencv3：CriteriaType.MaxIter
            // opencv4：CriteriaTypes.MaxIter
            svm.TermCriteria = new TermCriteria(CriteriaTypes.MaxIter, (int)1e7, 1e-6);

            //! [init]
            //! [train]
            svm.Train(trainData, SampleTypes.RowSample, labels); // 训练分类器
            //! [train]
            System.Diagnostics.Debug.WriteLine("Finished training process");

            //------------------------ 4. Show the decision regions ----------------------------------------
            //! [show]
            Vec3b green = new Vec3b(0, 100, 0);
            Vec3b blue = new Vec3b(100, 0, 0);
            for (int i = 0; i < I.Rows; ++i)
            {
                for (int j = 0; j < I.Cols; ++j)
                {
                    float[] testFeatureData = { j, i }; //生成测试数据
                    Mat sampleMat = new Mat(1, 2, MatType.CV_32F, testFeatureData);
                    float response = (int)svm.Predict(sampleMat);//进行预测，返回1或-1
                    if (response == 1)
                        I.Set(i, j, green);
                    else
                        I.Set(i, j, blue);
                }
            }
            //! [show]

            //----------------------- 5. Show the training data --------------------------------------------
            //! [show_data]
            float px, py;
            // Class 1
            for (int i = 0; i < NTRAINING_SAMPLES; ++i)
            {
                px = trainData.At<float>(i, 0);
                py = trainData.At<float>(i, 1);
                Cv2.Circle(I, (int)px, (int)py, 3, new Scalar(0, 255, 0), 1, LineTypes.Link8);
            }
            // Class 2
            for (int i = NTRAINING_SAMPLES; i < 2 * NTRAINING_SAMPLES; ++i)
            {
                px = trainData.At<float>(i, 0);
                py = trainData.At<float>(i, 1);
                Cv2.Circle(I,(int)px, (int)py, 3, new Scalar(255, 0, 0), 1, LineTypes.Link8);
            }
            //! [show_data]

            //------------------------- 6. Show support vectors --------------------------------------------
            //! [show_vectors]
            Mat sv = svm.GetSupportVectors();
            for (int i = 0; i < sv.Rows; ++i)
            {
                unsafe // 允许使用不安全代码，可能导致内存泄露
                {
                    float* v = (float*)sv.Ptr(i).ToPointer(); //取出每行的头指针
                    Point pt = new Point((int)v[0], (int)v[1]);
                    Cv2.Circle(I, pt.X, pt.Y, 6, new Scalar(128, 128, 128), 2, LineTypes.Link8);
                }
            }
            //! [show_vectors]

            Cv2.ImWrite("result.png", I);                      // save the Image
            Cv2.ImShow("SVM for Non-Linear Training Data", I); // show it to the user
            pictureBox1.Image = BitmapConverter.ToBitmap(I);
        }
    }
}