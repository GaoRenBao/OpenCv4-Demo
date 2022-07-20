using OpenCvSharp;
using System;
using System.Windows.Forms;

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
            // 【1】载入原图并显示
            Mat srcImage = Cv2.ImRead("1.jpg", 0);
            if (srcImage.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }
            Cv2.ImShow("原图", srcImage);

            //【2】定义变量
            Mat dstHist = new Mat();
            Rangef hueRanges = new Rangef(0, 255);
            Rangef[] ranges = { hueRanges };
            int[] size = { 256 };
            int[] channels = { 0, 1 };
            int dims = 1;

            //【3】计算图像的直方图
            Cv2.CalcHist(new Mat[] { srcImage },//输入的数组
                channels, // 通道索引
                null,     // 不使用掩膜
                dstHist,  // 输出的目标直方图
                dims,     // 需要计算的直方图的维度为2
                size,     // 存放每个维度的直方图尺寸的数组
                ranges,   // 每一维数值的取值范围数组
                true,     // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);   // 累计标识符，false表示直方图在配置阶段会被清零
            
            int scale = 1;
            Mat dstImage = new Mat(size[0] * scale, size[0] * 1, MatType.CV_8U);
            dstImage.SetIdentity(new Scalar(0));
    
            //【4】获取最大值和最小值
            double minValue = 0;//最小值
            double maxValue = 0;//最大值
            Cv2.MinMaxLoc(dstHist, out minValue, out maxValue); //查找数组和子数组的全局最小值和最大值存入maxValue中

            //【5】绘制出直方图
            int hpt = (int)(0.9 * size[0]);
            for (int i = 0; i < 256; i++)
            {
                float binValue = dstHist.At<float>(i);
                int realValue = (int)Math.Round(binValue * hpt / maxValue);

                //绘制矩形
                int a = size[0] - realValue;
                int b = size[0] - 1;
                int min = a > b ? b : a;
                int max = a > b ? a : b;
                Cv2.Rectangle(dstImage, new Point(i * scale, min),
                       new Point((i + 1) * scale - 1, max),
                       new Scalar(255, 255, 255));
            }

            Cv2.ImShow("一维直方图", dstImage);
            Cv2.WaitKey(0);
        }
    }
}

