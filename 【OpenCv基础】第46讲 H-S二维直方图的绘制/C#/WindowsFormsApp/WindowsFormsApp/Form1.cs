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
            // 载入源图，转化为HSV颜色模型
            Mat srcImage = Cv2.ImRead("1.jpg");
            if (srcImage.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }
            Mat hsvImage = new Mat();
            Cv2.CvtColor(srcImage, hsvImage, ColorConversionCodes.BGR2HSV);

            //【2】参数准备
            //将色调量化为30个等级，将饱和度量化为32个等级
            int hueBinNum = 30;//色调的直方图直条数量
            int saturationBinNum = 32;//饱和度的直方图直条数量
            int[] histSize = { hueBinNum, saturationBinNum };
            // 定义色调的变化范围为0到179
            Rangef hueRanges = new Rangef(0, 180);
            //定义饱和度的变化范围为0（黑、白、灰）到255（纯光谱颜色）
            Rangef saturationRanges = new Rangef(0, 256);
            Rangef[] ranges = { hueRanges, saturationRanges };
            Mat dstHist = new Mat();
            //参数准备，calcHist函数中将计算第0通道和第1通道的直方图
            int[] channels = { 0, 1 };

            //【3】正式调用calcHist，进行直方图计算
            Cv2.CalcHist(new Mat[] { hsvImage } ,//输入的数组
                channels,//通道索引
                null, //不使用掩膜
                dstHist, //输出的目标直方图
                2, //需要计算的直方图的维度为2
                histSize, //存放每个维度的直方图尺寸的数组
                ranges,//每一维数值的取值范围数组
                true, // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);//累计标识符，false表示直方图在配置阶段会被清零

            //【4】为绘制直方图准备参数
            double minValue = 0;//最小值
            double maxValue = 0;//最大值
            Cv2.MinMaxLoc(dstHist, out minValue, out maxValue); //查找数组和子数组的全局最小值和最大值存入maxValue中
            int scale = 10;
            Mat histImg = new Mat(saturationBinNum * scale, hueBinNum * 10, MatType.CV_8UC3);

            //【5】双层循环，进行直方图绘制
            for (int hue = 0; hue < hueBinNum; hue++)
                for (int saturation = 0; saturation < saturationBinNum; saturation++)
                {
                    float binValue = dstHist.At<float>(hue, saturation);//直方图组距的值
                    int intensity = (int)Math.Round(binValue * 255 / maxValue);//强度

                    //绘制矩形
                    Cv2.Rectangle(histImg,new Point(hue * scale, saturation * scale), 
                        new Point((hue + 1) * scale - 1, (saturation + 1) * scale - 1),
                        intensity, -1);
                }

            //【6】显示效果图
            Cv2.ImShow("素材图", srcImage);
            Cv2.ImShow("H-S 直方图", histImg);
            Cv2.WaitKey(0);
        }
    }
}

