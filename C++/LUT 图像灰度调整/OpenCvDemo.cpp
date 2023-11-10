/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：LUT 图像灰度调整
博客：http://www.bilibili996.com/Course?id=5825870000239
作者：高仁宝
时间：2023.11
*/

#include <iostream>    
#include <opencv2\opencv.hpp>    
#include <opencv2\highgui\highgui.hpp>    
#include <windows.h>

using namespace cv;
using namespace std;

int main()
{
	//查找表，数组的下标对应图片里面的灰度值
	//例如lutData[20]=0;表示灰度为20的像素其对应的值0.
	//可能这样说的不清楚仔细看下代码就清楚了。
	uchar lutData[256];
	for (int i = 0; i < 256; i++)
	{
		if (i <= 120)lutData[i] = 0;
		if (i > 120 && i <= 200)lutData[i] = 120;
		if (i > 200)lutData[i] = 255;
	}

	Mat lut(1, 256, CV_8UC1, lutData);
	Mat a = imread("../images/1050.jpg");
	Mat b;

	imshow("TEST1", a);
	LUT(a, lut, b);
	imshow("TEST2", b);
	waitKey(0);
}