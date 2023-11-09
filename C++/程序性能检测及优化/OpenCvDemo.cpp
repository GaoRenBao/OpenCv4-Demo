/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：程序性能检测及优化
博客：http://www.bilibili996.com/Course?id=2284309000315
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 使用getTickCount方法来计算程序运行时间。
void demo1()
{
    Mat img1 = imread("../images/ml.png");
    int64 e1 = getTickCount();
    for (int i = 5; i < 49; i += 2)
        medianBlur(img1, img1, i);

    int64 e2 = getTickCount();
    double t = (e2 - e1) / getTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
    printf("%f", t);
}

// 使用函数cv2.useOptimized() 来查看优化是否开启了，使用函数cv2.setUseOptimized()来开启优化
void demo2()
{
    printf("%d\n", useOptimized());
    setUseOptimized(false);
    printf("%d\n", useOptimized());
}

void main()
{
    demo1();
    demo2();
}