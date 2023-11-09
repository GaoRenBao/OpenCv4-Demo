/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：直方图均衡化
博客：http://www.bilibili996.com/Course?id=0735601000188
作者：高仁宝
时间：2023.11
*/

#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
using namespace cv;

int main()
{
	// 【1】加载源图像
	Mat srcImage, dstImage;
	srcImage = imread("../images/Astral.jpg");
	if (!srcImage.data) { 
		printf("读取图片错误，请确定目录下是否有imread函数指定图片存在~！ \n"); 
		return false; 
	}

	// 【2】转为灰度图并显示出来
	cvtColor(srcImage, srcImage, COLOR_BGR2GRAY);
	imshow("原始图", srcImage);

	// 【3】进行直方图均衡化
	equalizeHist(srcImage, dstImage);

	// 【4】显示结果
	imshow("经过直方图均衡化后的图", dstImage);

	// 等待用户按键退出程序
	waitKey(0);
	return 0;
}