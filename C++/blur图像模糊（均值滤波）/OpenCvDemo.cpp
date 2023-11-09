/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：blur图像模糊（均值滤波）
博客：http://www.bilibili996.com/Course?id=4355990000006
作者：高仁宝
时间：2023.11
*/

#include <vector>
#include <stdio.h>
#include <opencv2/opencv.hpp>

using namespace cv;
using namespace std;

int main()
{
	// 读入一张图片
	Mat srcImage = imread("../images/girl.jpg");
	// 显示原图
	imshow("原图", srcImage);
	// 均值滤波
	Mat dstImage;
	blur(srcImage, dstImage, Size(7, 7));
	// 显示效果图
	imshow("均值滤波效果", dstImage);
	// 等待任意键按下
	waitKey(0);
	return 0;
}