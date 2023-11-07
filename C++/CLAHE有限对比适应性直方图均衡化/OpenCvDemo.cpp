/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：CLAHE有限对比适应性直方图均衡化
博客：http://www.bilibili996.com/Course?id=1854373000267
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <io.h>
#include <string>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img = cv::imread("../images/tsukuba_l.png", 0);
    //create a CLAHE object (Arguments are optional)
    Ptr<CLAHE> clahe = cv::createCLAHE(2.0, Size(8, 8));
    Mat cl1;
    clahe->apply(img, cl1);

    cv::imshow("clahe_2", cl1);
    cv::waitKey(0);
}