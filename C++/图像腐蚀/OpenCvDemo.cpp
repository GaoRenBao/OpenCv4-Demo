/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：图像腐蚀
博客：http://www.bilibili996.com/Course?id=4347382000005
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
	// 在窗口中显示原画
	imshow("原图", srcImage);
	// 进行腐蚀操作
	Mat element = getStructuringElement(MORPH_RECT, Size(15, 15));
	Mat dstImage;
	erode(srcImage, dstImage, element);
	// 显示效果图
	imshow("效果", dstImage);
	// 等待任意键按下
	waitKey(0);
	return 0;
}