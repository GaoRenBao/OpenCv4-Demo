/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：高动态范围成像（HDRI或HDR）
博客：http://www.bilibili996.com/Course?id=ccb32eb54dd748f9a4fc137544039d51
作者：高仁宝
时间：2023.11

参考博客：https://blog.csdn.net/jimtien/article/details/119026930
*/

using OpenCvSharp;
using OpenCvSharp.XPhoto;
using System.Collections.Generic;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Mat> images = new List<Mat>()
            {
                Cv2.ImRead("../../../images/tbs/1tl.jpg"),
                Cv2.ImRead("../../../images/tbs/2tr.jpg"),
                Cv2.ImRead("../../../images/tbs/3bl.jpg"),
                Cv2.ImRead("../../../images/tbs/4br.jpg"),
            };
            float[] exposures = new float[] { 15.0f, 2.5f, 0.25f, 0.0333f };

            // 估计相机响应
            Mat response = new Mat();
            CalibrateDebevec calibrate = CalibrateDebevec.Create();
            calibrate.Process(images, response, exposures);

            // 生成HDR图片
            Mat hdr = new Mat();
            MergeDebevec merge_debevec = MergeDebevec.Create();
            merge_debevec.Process(images, hdr, exposures, response);

            /* 找不到 createMergeRobertson 的对应方法 */

            // Tonemap
            Mat ldr = new Mat();
            TonemapDurand tonemap = TonemapDurand.Create(0.5f);
            tonemap.Process(hdr, ldr);

            // Exposure fusion using Mertens
            // 这里我们展示了一种可以合并曝光图像的替代算法
            Mat fusion = new Mat();
            MergeMertens merge_mertens = MergeMertens.Create();
            merge_mertens.Process(images, fusion);

            Cv2.ImShow("fusion.png", fusion); 
            Cv2.ImShow("ldr.png", ldr);
            Cv2.ImShow("hdr.hdr", hdr);
            Cv2.WaitKey(0);
        }
    }
}
