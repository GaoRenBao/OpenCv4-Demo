/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：漫水填充
博客：http://www.bilibili996.com/Course?id=5744370000163
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        //定义原始图、目标图、灰度图、掩模图
        static Mat g_srcImage = new Mat();
        static Mat g_dstImage = new Mat();
        static Mat g_grayImage = new Mat();
        static Mat g_maskImage = new Mat();

        //漫水填充的模式
        static int g_nFillMode = 1;
        //负差最大值、正差最大值
        static int g_nLowDifference = 20, g_nUpDifference = 20;
        //表示floodFill函数标识符低八位的连通值
        static int g_nConnectivity = 4;
        //是否为彩色图的标识符布尔值
        static bool g_bIsColor = true;
        //是否显示掩膜窗口的布尔值
        static bool g_bUseMask = false;
        //新的重新绘制的像素值
        static int g_nNewMaskVal = 255;
        static Random g_rng = new Random();

        static void ShowHelpText()
        {
            //输出一些帮助信息  
            Console.WriteLine("\n\n\t欢迎来到漫水填充示例程序~");
            Console.WriteLine("\n\n\t本示例根据鼠标选取的点搜索图像中与之颜色相近的点，并用不同颜色标注。");
            Console.WriteLine("\n\n\t按键操作说明:");
            Console.WriteLine("\t\t鼠标点击图中区域- 进行漫水填充操作");
            Console.WriteLine("\t\t键盘按键【ESC】- 退出程序");
            Console.WriteLine("\t\t键盘按键【1】-  切换彩色图/灰度图模式");
            Console.WriteLine("\t\t键盘按键【2】- 显示/隐藏掩膜窗口");
            Console.WriteLine("\t\t键盘按键【3】- 恢复原始图像");
            Console.WriteLine("\t\t键盘按键【4】- 使用空范围的漫水填充");
            Console.WriteLine("\t\t键盘按键【5】- 使用渐变、固定范围的漫水填充");
            Console.WriteLine("\t\t键盘按键【6】- 使用渐变、浮动范围的漫水填充");
            Console.WriteLine("\t\t键盘按键【7】- 操作标志符的低八位使用4位的连接模式");
            Console.WriteLine("\t\t键盘按键【8】- 操作标志符的低八位使用8位的连接模式\n");
        }

        //-------------------【onMouse( )函数】---------------------
        //      描述：鼠标消息onMouse回调函数
        //----------------------------------------------------------
        static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            // 若鼠标左键没有按下，便返回
            if (@event != MouseEventTypes.LButtonDown)
                return;

            //-------------------【<1>调用floodFill函数之前的参数准备部分】---------------
            Point seed = new Point(x, y);
            //空范围的漫水填充，此值设为0，否则设为全局的g_nLowDifference
            int LowDifference = g_nFillMode == 0 ? 0 : g_nLowDifference;

            //空范围的漫水填充，此值设为0，否则设为全局的g_nUpDifference
            int UpDifference = g_nFillMode == 0 ? 0 : g_nUpDifference;

            //标识符的0~7位为g_nConnectivity，8~15位为g_nNewMaskVal左移8位的值，16~23位为CV_FLOODFILL_FIXED_RANGE或者0。
            int mflags = (int)(g_nConnectivity + (g_nNewMaskVal << 8) + (g_nFillMode == 1 ? FloodFillFlags.FixedRange : 0));

            //随机生成bgr值
            int b = g_rng.Next(255);//随机返回一个0~255之间的值
            int g = g_rng.Next(255);//随机返回一个0~255之间的值
            int r = g_rng.Next(255);//随机返回一个0~255之间的值

            //定义重绘区域的最小边界矩形区域
            Rect ccomp = new Rect();

            // 在重绘区域像素的新值，若是彩色图模式，取Scalar(b, g, r)；
            // 若是灰度图模式，取Scalar(r*0.299 + g*0.587 + b*0.114)
            Scalar newVal = g_bIsColor ? new Scalar(b, g, r) : new Scalar(r * 0.299 + g * 0.587 + b * 0.114);
            //目标图的赋值
            Mat dst = g_bIsColor ? g_dstImage : g_grayImage;
            int area;

            //--------------------【<2>正式调用floodFill函数】-----------------------------
            if (g_bUseMask)
            {
                Cv2.Threshold(g_maskImage, g_maskImage, 1, 128, ThresholdTypes.Binary);
                area = Cv2.FloodFill(dst, g_maskImage, seed, newVal, out ccomp,
                    new Scalar(LowDifference, LowDifference, LowDifference),
                    new Scalar(UpDifference, UpDifference, UpDifference), (FloodFillFlags)mflags);
                Cv2.ImShow("mask", g_maskImage);
            }
            else
            {
                area = Cv2.FloodFill(dst, seed, newVal, out ccomp,
                    new Scalar(LowDifference, LowDifference, LowDifference),
                    new Scalar(UpDifference, UpDifference, UpDifference), (FloodFillFlags)mflags);
            }

            Cv2.ImShow("效果图", dst);
            Console.WriteLine($"{area} 个像素被重绘");
            Cv2.WaitKey(10);
        }


        //-----------------【main( )函数】--------------------------
        //   描述：控制台应用程序的入口函数，我们的程序从这里开始  
        //----------------------------------------------------------
        static void Main(string[] args)
        {
            //载入原图
            g_srcImage = Cv2.ImRead("../../../images/girl3.jpg");

            // 缩放原图
            //resize(g_srcImage, g_srcImage, Size(g_srcImage.cols * 0.5, g_srcImage.rows * 0.5));

            //显示帮助文字
            ShowHelpText();

            //拷贝源图到目标图
            g_srcImage.CopyTo(g_dstImage);

            //转换三通道的image0到灰度图
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            //利用image0的尺寸来初始化掩膜mask
            g_maskImage.Create(g_srcImage.Rows + 2, g_srcImage.Cols + 2, MatType.CV_8UC1);

            Cv2.NamedWindow("效果图", WindowFlags.AutoSize);

            //创建Trackbar
            Cv2.CreateTrackbar("负差最大值", "效果图", ref g_nLowDifference, 255);
            Cv2.CreateTrackbar("正差最大值", "效果图", ref g_nUpDifference, 255);

            //鼠标回调函数
            Cv2.SetMouseCallback("效果图", new MouseCallback(onMouse));

            //循环轮询按键
            while (true)
            {
                //先显示效果图
                Cv2.ImShow("效果图", g_bIsColor ? g_dstImage : g_grayImage);

                //获取键盘按键
                int c = Cv2.WaitKey(0);
                //判断ESC是否按下，若按下便退出
                if ((c & 255) == 27)
                {
                    Console.WriteLine("程序退出...........");
                    break;
                }

                //根据按键的不同，进行各种操作
                switch ((char)c)
                {
                    //如果键盘“1”被按下，效果图在在灰度图，彩色图之间互换
                    case '1':
                        if (g_bIsColor) //若原来为彩色，转为灰度图，并且将掩膜mask所有元素设置为0
                        {
                            Console.WriteLine("键盘“1”被按下，切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】");
                            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);
                            g_maskImage = g_maskImage.SetTo(new Scalar(0, 0, 0));//将mask所有元素设置为0
                            g_bIsColor = false; //将标识符置为false，表示当前图像不为彩色，而是灰度
                        }
                        //若原来为灰度图，便将原来的彩图image0再次拷贝给image，并且将掩膜mask所有元素设置为0
                        else
                        {
                            Console.WriteLine("键盘“1”被按下，切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】\n");
                            g_srcImage.CopyTo(g_dstImage);
                            g_maskImage = g_maskImage.SetTo(new Scalar(0, 0, 0));//将mask所有元素设置为0
                            g_bIsColor = true;//将标识符置为true，表示当前图像模式为彩色
                        }
                        break;

                    //如果键盘按键“2”被按下，显示/隐藏掩膜窗口
                    case '2':
                        if (g_bUseMask)
                        {
                            Cv2.DestroyWindow("mask");
                            g_bUseMask = false;
                        }
                        else
                        {
                            Cv2.NamedWindow("mask", WindowFlags.AutoSize);
                            g_maskImage = g_maskImage.SetTo(new Scalar(0, 0, 0));//将mask所有元素设置为0
                            Cv2.ImShow("mask", g_maskImage);
                            g_bUseMask = true;
                        }
                        break;

                    //如果键盘按键“3”被按下，恢复原始图像
                    case '3':
                        Console.WriteLine("按键“3”被按下，恢复原始图像");
                        g_srcImage.CopyTo(g_dstImage);
                        Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);
                        g_maskImage = g_maskImage.SetTo(new Scalar(0, 0, 0));
                        break;

                    //如果键盘按键“4”被按下，使用空范围的漫水填充
                    case '4':
                        Console.WriteLine("按键“4”被按下，使用空范围的漫水填充");
                        g_nFillMode = 0;
                        break;

                    //如果键盘按键“5”被按下，使用渐变、固定范围的漫水填充
                    case '5':
                        Console.WriteLine("按键“5”被按下，使用渐变、固定范围的漫水填充");
                        g_nFillMode = 1;
                        break;

                    //如果键盘按键“6”被按下，使用渐变、浮动范围的漫水填充
                    case '6':
                        Console.WriteLine("按键“6”被按下，使用渐变、浮动范围的漫水填充");
                        g_nFillMode = 2;
                        break;

                    //如果键盘按键“7”被按下，操作标志符的低八位使用4位的连接模式
                    case '7':
                        Console.WriteLine("按键“7”被按下，操作标志符的低八位使用4位的连接模式");
                        g_nConnectivity = 4;
                        break;

                    //如果键盘按键“8”被按下，操作标志符的低八位使用8位的连接模式
                    case '8':
                        Console.WriteLine("按键“8”被按下，操作标志符的低八位使用8位的连接模式");
                        g_nConnectivity = 8;
                        break;

                    default: break;
                }
            }
        }
    }
}
