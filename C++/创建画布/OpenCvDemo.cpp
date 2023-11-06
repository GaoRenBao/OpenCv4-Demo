/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：创建画布
博客：http://www.bilibili996.com/Course?id=4082690000247
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
	// 全黑.可以用在屏保
	Mat mat(200, 200, CV_8UC3);
	mat = Scalar::all(0);
	imshow("black", mat);

	// 全白
	mat = Scalar::all(255);
	imshow("white", mat);
	waitKey(0);
	return 0;
}