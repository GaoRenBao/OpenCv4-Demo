/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：多通道图像混合
博客：http://www.bilibili996.com/Course?id=5129212000098
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (MultiChannelBlending())
            {
                Console.WriteLine("运行成功，得出了需要的图像~! ");
            }
            Cv2.WaitKey(0);
        }

        static bool MultiChannelBlending()
        {
            #region 多通道混合-蓝色分量部分
            // 【1】读入图片
            Mat logoImage = Cv2.ImRead("../../../images/dota_logo.jpg", 0);
            Mat srcImage = Cv2.ImRead("../../../images/dota_jugg.jpg");
            if (logoImage.Data == null) { Console.WriteLine("Oh，no，读取logoImage错误~！ \n"); return false; }
            if (srcImage.Data == null) { Console.WriteLine("Oh，no，读取srcImage错误~！ \n"); return false; }

            //【2】把一个3通道图像转换成3个单通道图像
            Mat[] channels;
            Cv2.Split(srcImage, out channels);//分离色彩通道

            //【3】将原图的蓝色通道引用返回给imageBlueChannel，注意是引用，相当于两者等价，修改其中一个另一个跟着变
            Mat imageBlueChannel = channels[0];
            //【4】将原图的蓝色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到imageBlueChannel中

            Cv2.AddWeighted(
                imageBlueChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)],
                1.0, logoImage, 0.5, 0,
                imageBlueChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)]);

            //【5】将三个单通道重新合并成一个三通道
            Cv2.Merge(channels, srcImage);

            //【6】显示效果图
            //Cv2.NamedWindow(" <1>游戏原画+logo蓝色通道");
            Cv2.ImShow(" <1>游戏原画+logo蓝色通道", srcImage);
            #endregion

            #region 多通道混合-绿色分量部分
            // 【1】读入图片
            logoImage = Cv2.ImRead("../../../images/dota_logo.jpg", 0);
            srcImage = Cv2.ImRead("../../../images/dota_jugg.jpg");
            if (logoImage.Data == null) { Console.WriteLine("读取logoImage错误~！ \n"); return false; }
            if (srcImage.Data == null) { Console.WriteLine("读取srcImage错误~！ \n"); return false; }

            //【2】将一个三通道图像转换成三个单通道图像
            Cv2.Split(srcImage, out channels);//分离色彩通道

            //【3】将原图的绿色通道的引用返回给imageBlueChannel，注意是引用，相当于两者等价，修改其中一个另一个跟着变
            Mat imageGreenChannel = channels[1];
            //【4】将原图的绿色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到imageGreenChannel中
            Cv2.AddWeighted(
                imageGreenChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)],
                1.0, logoImage, 0.5, 0,
                imageGreenChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)]);

            //【5】将三个独立的单通道重新合并成一个三通道
            Cv2.Merge(channels, srcImage);

            //【6】显示效果图
            //Cv2.NamedWindow("<2>游戏原画+logo绿色通道");
            Cv2.ImShow("<2>游戏原画+logo绿色通道", srcImage);
            #endregion

            #region 多通道混合-红色分量部分
            // 【1】读入图片
            logoImage = Cv2.ImRead("../../../images/dota_logo.jpg", 0);
            srcImage = Cv2.ImRead("../../../images/dota_jugg.jpg");
            if (logoImage.Data == null) { Console.WriteLine("读取logoImage错误~！ \n"); return false; }
            if (srcImage.Data == null) { Console.WriteLine("读取srcImage错误~！ \n"); return false; }

            //【2】将一个三通道图像转换成三个单通道图像
            Cv2.Split(srcImage, out channels);//分离色彩通道

            //【3】将原图的红色通道引用返回给imageBlueChannel，注意是引用，相当于两者等价，修改其中一个另一个跟着变
            imageGreenChannel = channels[2];
            //【4】将原图的红色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到imageRedChannel中
            Cv2.AddWeighted(
            imageGreenChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)],
            1.0, logoImage, 0.5, 0,
            imageGreenChannel[new Rect(500, 250, logoImage.Cols, logoImage.Rows)]);

            //【5】将三个独立的单通道重新合并成一个三通道
            Cv2.Merge(channels, srcImage);

            //【6】显示效果图
            //Cv2.NamedWindow("<3>游戏原画+logo红色通道 ");
            Cv2.ImShow("<3>游戏原画+logo红色通道 ", srcImage);
            #endregion

            return true;
        }
    }
}
