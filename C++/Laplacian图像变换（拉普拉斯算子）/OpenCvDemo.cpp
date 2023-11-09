/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：Laplacian图像变换（拉普拉斯算子）
博客：http://www.bilibili996.com/Course?id=4232193000169
作者：高仁宝
时间：2023.11
*/

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;

int main()
{
    //【0】变量的定义
    Mat src, src_gray, dst, abs_dst;

    //【1】载入原始图  
    src = imread("../images/girl5.jpg");

    //【2】显示原始图 
    imshow("【原始图】图像Laplace变换", src);

    //【3】使用高斯滤波消除噪声
    GaussianBlur(src, src, Size(3, 3), 0, 0, BORDER_DEFAULT);

    //【4】转换为灰度图
    // opencv3：CV_RGB2GRAY
    // opencv4：COLOR_RGB2GRAY
    cvtColor(src, src_gray, COLOR_RGB2GRAY);

    //【5】使用Laplace函数
    Laplacian(src_gray, dst, CV_16S, 3, 1, 0, BORDER_DEFAULT);

    //【6】计算绝对值，并将结果转换成8位
    convertScaleAbs(dst, abs_dst);

    //【7】显示效果图
    imshow("【效果图】图像Laplace变换", abs_dst);

    waitKey(0);
    return 0;
}