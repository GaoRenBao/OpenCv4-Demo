/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：背景减除
博客：http://www.bilibili996.com/Course?id=56cb7ea6a7b84e2297af1334aaeb7609
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>

using namespace cv;
using namespace std;

/// <summary>
/// BackgroundSubtractorMOG
/// </summary>
void demo1()
{
    /* opencv-4.5.5-vc14_vc15 没有BackgroundSubtractorMOG */
    /* opencv-4.6.0-vc14_vc15 没有BackgroundSubtractorMOG */
}

/// <summary>
/// BackgroundSubtractorMOG2
/// </summary>
void demo2()
{
    VideoCapture cap("../images/vtest.avi");
    // 笔记本摄像头
    // VideoCapture cap(0); 

    Ptr<cv::BackgroundSubtractorMOG2> fgbg = createBackgroundSubtractorMOG2();

    // 可选参数 比如 进行建模场景的时间长度 高斯混合成分的数量-阈值等
    Mat frame, fgmask;
    while (true)
    {
        cap.read(frame);

        flip(frame, frame, 1);  // 左右翻转
        fgbg->apply(frame, fgmask);

        imshow("frame", frame);
        imshow("fgmask", fgmask);
        if (waitKey(1) == 27)
        {
            break;
        }
    }
    cap.release();
    destroyAllWindows();
}

/// <summary>
/// createBackgroundSubtractorGMG
/// </summary>
void demo3()
{
    /* opencv-4.5.5-vc14_vc15 没有createBackgroundSubtractorGMG */
    /* opencv-4.6.0-vc14_vc15 没有createBackgroundSubtractorGMG */
}

int main()
{
    demo2();
    return 0;
}