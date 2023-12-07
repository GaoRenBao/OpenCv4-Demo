/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：二维码、条形码识别
博客：http://www.bilibili996.com/Course?id=fd336dddfd3a4ae49603f53bd74550f2
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using ZXing;

namespace demo
{
    internal class Program
    {
        /// <summary>
        /// ZBar方法
        /// 通过NuGet安装的ZBar包运行会提示找不到dll文件。
        /// 只能手动将dll文件扔到执行文件目录下。
        /// ZBar只支持条形码识别。
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        static string ZBarQrCode(Mat srcImage)
        {
            Bitmap pImg = BitmapConverter.ToBitmap(srcImage);
            using (ZBar.ImageScanner scanner = new ZBar.ImageScanner())
            {
                scanner.SetConfiguration(ZBar.SymbolType.None, ZBar.Config.Enable, 0);
                scanner.SetConfiguration(ZBar.SymbolType.CODE39, ZBar.Config.Enable, 1);
                scanner.SetConfiguration(ZBar.SymbolType.CODE128, ZBar.Config.Enable, 1);
                scanner.SetConfiguration(ZBar.SymbolType.QRCODE, ZBar.Config.Enable, 1);

                List<ZBar.Symbol> symbols = new List<ZBar.Symbol>();
                symbols = scanner.Scan((System.Drawing.Image)pImg);

                if (symbols != null && symbols.Count > 0)
                {
                    string result = string.Empty;
                    symbols.ForEach(s => result += "条码内容:" + s.Data + " 条码质量:" + s.Quality + Environment.NewLine);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Zxing方法：ZXing.Net 0.16.9
        /// 支持条形码和二维码的识别
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        static string ZxingQrCode(Mat srcImage)
        {
            Bitmap pImg = BitmapConverter.ToBitmap(srcImage);
            Result result = new BarcodeReader().Decode(pImg);
            return result?.Text;
        }


        static void Main(string[] args)
        {
            Mat img1 = Cv2.ImRead("../../../images/macbookPro.jpg");
            Mat img2 = Cv2.ImRead("../../../images/macbookPro2.jpg");
            //Console.WriteLine($"ZBarQrCode1: {ZBarQrCode(img1)}");
            Console.WriteLine($"ZBarQrCode2: {ZBarQrCode(img2)}");

            Console.WriteLine($"ZxingQrCode1: {ZxingQrCode(img1)}");
            Console.WriteLine($"ZxingQrCode2: {ZxingQrCode(img2)}");
            Console.Read();
        }
    }
}



