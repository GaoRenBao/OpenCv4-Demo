#include <vector>
#include <stdio.h>
#include <opencv2/opencv.hpp>

using namespace cv;
using namespace std;

int main()
{
	// 读入一张图片
	Mat srcImage = imread("girl.jpg");
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
