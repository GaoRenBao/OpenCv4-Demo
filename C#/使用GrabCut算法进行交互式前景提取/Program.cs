/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：使用GrabCut算法进行交互式前景提取
博客：http://www.bilibili996.com/Course?id=ebe8b6e9ef6843f486142989d05d438a
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            demo1();
            demo2();
            Cv2.WaitKey(0);
        }

        public static void demo1()
        {
            Mat img = Cv2.ImRead("../../../images/messi5.jpg");

            Mat mask = new Mat();     // 存储掩码图像
            Mat bgdModel = new Mat(); // 存储背景模型
            Mat fgdModel = new Mat(); // 存储前景模型

            // 使用GrabCut算法进行图像分割，指定感兴趣区域的矩形框作为初始化
            Cv2.GrabCut(img, mask, new Rect(50, 50, 450, 290), bgdModel, fgdModel, 5, GrabCutModes.InitWithRect);

            // 创建结果图像
            Mat mask2 = new Mat(mask.Rows, mask.Cols, MatType.CV_8UC1);

            /*
             python实现如下：
             mask2 = np.where((mask == 2) | (mask == 0), 0, 1).astype('uint8')

             该代码可以理解为：
             将数组mask中等于2或等于0的元素替换为0，其他元素替换为1。
             */

            for (int i = 0; i < mask.Width; i++)
            {
                for (int j = 0; j < mask.Height; j++)
                {
                    // 获取掩码图像的像素值
                    byte v = mask.Get<byte>(j, i);
                    // 根据掩码像素值设置结果图像的像素值
                    switch (v)
                    {
                        case 0: // 0 表示背景
                        case 2: // 2 表示可能的背景
                            mask2.Set<byte>(j, i, 0); // 设置为黑色
                            break;

                        case 1: // 1 表示前景
                        case 3: // 3 表示可能的前景
                            // 设置为白
                            // 按照python的逻辑，如果设置成1。
                            // 掩码的使用就必须采用 Cv2.Multiply 方法才行。
                            // 如果设置成255，就的采用 Cv2.BitwiseAnd 方法。

                            mask2.Set<byte>(j, i, 1);
                            // 或者
                            //mask2.Set<byte>(j, i, 255);
                            break;
                    }
                }
            }
            // 按Python逻辑，使用相乘算法
            Cv2.CvtColor(mask2, mask2, ColorConversionCodes.GRAY2BGR);
            Cv2.Multiply(img, mask2, img);
            Cv2.ImShow("out", img);

            // 如果前景设置的是255，则用这个
            //Mat ret = new Mat();
            //Cv2.BitwiseAnd(img, img, ret, mask2);
            //Cv2.ImShow("out", ret); 
        }

        public static void demo2()
        {
            Mat img = Cv2.ImRead("../../../images/messi5.jpg");

            Mat mask = new Mat();     // 存储掩码图像
            Mat bgdModel = new Mat(); // 存储背景模型
            Mat fgdModel = new Mat(); // 存储前景模型
            Cv2.GrabCut(img, mask, new Rect(50, 50, 450, 290), bgdModel, fgdModel, 5, GrabCutModes.InitWithRect);

            Mat mask2 = new Mat(mask.Rows, mask.Cols, MatType.CV_8UC1);
            for (int i = 0; i < mask.Width; i++)
            {
                for (int j = 0; j < mask.Height; j++)
                {
                    // 获取掩码图像的像素值
                    byte v = mask.Get<byte>(j, i);
                    // 根据掩码像素值设置结果图像的像素值
                    switch (v)
                    {
                        case 0: // 0 表示背景
                        case 2: // 2 表示可能的背景
                            mask2.Set<byte>(j, i, 0); // 设置为黑色
                            break;

                        case 1: // 1 表示前景
                        case 3: // 3 表示可能的前景
                            mask2.Set<byte>(j, i, 1);
                            break;
                    }
                }
            }
            Cv2.CvtColor(mask2, mask2, ColorConversionCodes.GRAY2BGR);
            Cv2.Multiply(img, mask2, img);

            Mat newmask = Cv2.ImRead("../../../images/newmask2.jpg", 0);

            /*
            python中下面的算法在C#中的实现没找到。。
            mask[newmask == 0] = 0
            mask[newmask == 255] = 1
             */
            Mat rMask = new Mat();
            Cv2.BitwiseAnd(mask, mask, rMask, newmask);
            Cv2.GrabCut(img, rMask, new Rect(), bgdModel, fgdModel, 5, GrabCutModes.InitWithMask);

            mask = new Mat(rMask.Rows, rMask.Cols, MatType.CV_8UC1);
            for (int i = 0; i < rMask.Width; i++)
            {
                for (int j = 0; j < rMask.Height; j++)
                {
                    // 获取掩码图像的像素值
                    byte v = rMask.Get<byte>(j, i);
                    // 根据掩码像素值设置结果图像的像素值
                    switch (v)
                    {
                        case 0: // 0 表示背景
                        case 2: // 2 表示可能的背景
                            mask.Set<byte>(j, i, 0); // 设置为黑色
                            break;

                        case 1: // 1 表示前景
                        case 3: // 3 表示可能的前景
                            mask.Set<byte>(j, i, 1);
                            break;
                    }
                }
            }

            Cv2.CvtColor(mask, mask, ColorConversionCodes.GRAY2BGR);
            Cv2.Multiply(img, mask, img);
            Cv2.ImShow("result", img);
        }
    }
}