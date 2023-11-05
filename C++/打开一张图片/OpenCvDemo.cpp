/*
OpenCv版本 opencv-4.5.5-vc14_vc15
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    // 读取一张图片
    Mat srcImage = imread("../images/a.jpg");

    // 创建一个名字为image的窗口
    namedWindow("image");

    // 显示图片
    imshow("image", srcImage);

    // 等待任意按钮结束
    waitKey(0);
}