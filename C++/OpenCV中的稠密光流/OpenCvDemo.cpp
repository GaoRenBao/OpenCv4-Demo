/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：OpenCV中的稠密光流
博客：http://www.bilibili996.com/Course?id=6af89dda880846cca6da479b654d68d0
作者：高仁宝
时间：2023.11
*/

#include <iostream>
#include <opencv2/core.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/video.hpp>
#include <opencv2/highgui/highgui_c.h>

using namespace cv;
using namespace std;

/// <summary>
/// 《OpenCV-Python-Tutorial-中文版.pdf》 P235
/// </summary>
void demo1()
{
	VideoCapture cap(samples::findFile("../images/vtest.avi"));

	Mat frame1, prvs;
	cap >> frame1;
	cvtColor(frame1, prvs, COLOR_BGR2GRAY);

	Mat hsv = Mat::ones(frame1.size(), frame1.type());
	Mat t[3];
	t[0] = Mat(frame1.size(), CV_32F, Scalar(0));
	t[1] = Mat(frame1.size(), CV_32F, Scalar(255));
	t[2] = Mat(frame1.size(), CV_32F, Scalar(0));
	merge(t, 3, hsv);

	while (true) 
	{
		Mat frame2, next;
		cap >> frame2;
		if (frame2.empty())
			break;
		cvtColor(frame2, next, COLOR_BGR2GRAY);

		//计算光流
		Mat flow(prvs.size(), CV_32FC2);
		calcOpticalFlowFarneback(prvs, next, flow, 0.5, 3, 15, 3, 5, 1.2, 0);

		// 分离通道
		Mat flow_parts[2];
		split(flow, flow_parts);
		Mat mag, ang, magn_norm;
		cartToPolar(flow_parts[0], flow_parts[1], mag, ang, true);
		normalize(mag, magn_norm, 0.0f, 1.0f, NORM_MINMAX);

		Mat hsvCh[3];
		split(hsv, hsvCh);
		Mat _hsv[3], bgr;
		_hsv[0] = ang * 180 / CV_PI / 2;;
		_hsv[1] = hsvCh[1];
		_hsv[2] = magn_norm,
		merge(_hsv, 3, hsv);
		cvtColor(hsv, bgr, COLOR_HSV2BGR);

		imshow("frame2", frame2);
		imshow("bgr", bgr);

		if (waitKey(1) == 27)
			break;
		prvs = next;
	}
}

/// <summary>
/// 来源：https://blog.csdn.net/ResumeProject/article/details/128507520
/// </summary>
void demo2()
{
	VideoCapture cap(samples::findFile("../images/vtest.avi"));
	if (!cap.isOpened()) {
		cerr << "Unable to open file!" << endl;
		return;
	}

	Mat frame1, prvs;
	cap >> frame1;
	cvtColor(frame1, prvs, COLOR_BGR2GRAY);

	while (true) 
	{
		Mat frame2, next;
		cap >> frame2;
		if (frame2.empty())
			break;

		cvtColor(frame2, next, COLOR_BGR2GRAY);
		//计算光流
		Mat flow(prvs.size(), CV_32FC2);
		calcOpticalFlowFarneback(prvs, next, flow, 0.5, 3, 15, 3, 5, 1.2, 0);

		// 分离通道
		Mat flow_parts[2];
		split(flow, flow_parts);
		Mat mag, ang, magn_norm;
		cartToPolar(flow_parts[0], flow_parts[1], mag, ang);
		normalize(mag, magn_norm, 0.0f, 1.0f, NORM_MINMAX);

		//build hsv image
		Mat _hsv[3], hsv, hsv8, bgr;
		_hsv[0] = ang * 180 / CV_PI / 2;
		_hsv[1] = Mat::ones(ang.size(), CV_32F);
		_hsv[2] = magn_norm;
		merge(_hsv, 3, hsv);
		hsv.convertTo(hsv8, CV_8U, 255.0);
		cvtColor(hsv8, bgr, COLOR_HSV2BGR);

		imshow("frame2", frame2);
		imshow("bgr", bgr);

		if (waitKey(1) == 27)
			break;
		prvs = next;
	}
}

int main()
{
	demo1();
	//demo2();
}