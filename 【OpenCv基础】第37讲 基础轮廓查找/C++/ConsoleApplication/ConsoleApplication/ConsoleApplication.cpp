#include <opencv2/opencv.hpp>
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"

using namespace cv;
using namespace std;

int main()
{
	// 【1】载入原始图，且必须以二值图模式载入
	Mat srcImage = imread("1.jpg", 0);
	imshow("原始图", srcImage);

	//【2】初始化结果图
	Mat dstImage = Mat::zeros(srcImage.rows, srcImage.cols, CV_8UC3);

	//【3】srcImage取大于阈值119的那部分
	srcImage = srcImage > 119;
	imshow("取阈值后的原始图", srcImage);

	//【4】定义轮廓和层次结构
	vector<vector<Point>> contours;
	vector<Vec4i> hierarchy;

	//【5】查找轮廓
	findContours(srcImage, contours, hierarchy,RETR_CCOMP, CHAIN_APPROX_SIMPLE);

	//【6】遍历所有顶层的轮廓， 以随机颜色绘制出每个连接组件颜色
	int index = 0;
	for (; index >= 0; index = hierarchy[index][0])
	{
		Scalar color(rand() & 255, rand() & 255, rand() & 255);
		drawContours(dstImage, contours, index, color, FILLED, 8, hierarchy);
	}

	//【7】显示最后的轮廓图
	imshow("轮廓图", dstImage);

	waitKey(0);
}
