/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：立体图像中的深度地图
博客：http://www.bilibili996.com/Course?id=
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
	// read two input images as grayscale images
	Mat imgL = imread("../images/tsukuba_l.png", 0);
	Mat imgR = imread("../images/tsukuba_r.png", 0);

	// Initiate and StereoBM object
	Ptr<StereoBM> stereo = cv::StereoBM::create(0, 21);

	// compute the disparity map
	Mat disparity;
	stereo->compute(imgL, imgR, disparity);

	cv::imshow("gray", disparity);
	cv::waitKey(0);
	return 0;
}