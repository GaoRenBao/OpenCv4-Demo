/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：环境亮度检测
博客：http://www.bilibili996.com/Course?id=4400458000246
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/Stitching.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 环境亮度值计算
double BrightnessDetection(Mat srcImage)
{
    Mat dstImage;
    cvtColor(srcImage, dstImage, COLOR_BGR2GRAY);
    // bgr保存了所有0~255个像素值的数量
    int bgr[256] = { 0 };
    // a为所有亮度值的和
    double a = 0;
    // num 为像素点个数
    int num = 0;
    for (int i = 0; i < dstImage.rows; i++)
    {
        for (int j = 0; j < dstImage.cols; j++)
        {
            // 剔除最大和最小值
            if (dstImage.at<uchar>(i, j) == 0) continue;
            if (dstImage.at<uchar>(i, j) == 255) continue;
            num++;
            //在计算过程中，考虑128为亮度均值点
            a += double(dstImage.at<uchar>(i, j) - 128);
            int x = (int)dstImage.at<uchar>(i, j);
            bgr[x]++;
        }
    }
    // 计算像素点平均值
    double da = a / num;
    double D = abs(da);

    // 将平均值乘以每个亮度点的个数，得到整个图像的亮度值，起到均衡亮度值
    // 避免由于某个区域特别亮，导致最终计算结果偏差过大
    double Ma = 0;
    for (int i = 0; i < 256; i++) {
        Ma += fabs(i - 128 - da) * bgr[i];
    }
    // 将整个图像的亮度值除以个数，最终得到每个像素点的亮度值
    Ma /= double((dstImage.rows * dstImage.cols));
    // 输出亮度值
    double M = fabs(Ma) + 0.0001;
    double K = D / M;
    double cast = (da > 0) ? K : -K;
    return cast;
}

int main()
{
    VideoCapture Cap;
    Cap.open(1);
    if (!Cap.isOpened())
        return 0;

    Cap.set(CAP_PROP_FRAME_WIDTH, 1280);
    Cap.set(CAP_PROP_FRAME_HEIGHT, 720);
    Cap.set(CAP_PROP_EXPOSURE, 0);  //曝光

    Mat srcImage;
    while (1)
    {
        Cap >> srcImage;
        if (!srcImage.empty())
        {
            double value = BrightnessDetection(srcImage);

            putText(srcImage, "value:" + to_string(value), Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, LINE_AA);
            imshow("srcImage", srcImage);
            waitKey(1);
        }
    }
    return 0;
}