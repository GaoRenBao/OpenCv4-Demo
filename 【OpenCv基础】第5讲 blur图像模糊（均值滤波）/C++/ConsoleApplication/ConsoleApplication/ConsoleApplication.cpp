#include <vector>
#include <stdio.h>
#include <opencv2/opencv.hpp>

using namespace cv;
using namespace std;

int main()
{
	// 读入一张图片
	Mat srcImage = imread("girl.jpg");
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
