﻿/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：点追踪
博客：http://www.bilibili996.com/Course?id=4954326000071
作者：高仁宝
时间：2023.11
*/

//---------------------------------【头文件、命名空间包含部分】----------------------------
//		描述：包含程序所使用的头文件和命名空间
//-------------------------------------------------------------------------------------------------
#include "opencv2/video/tracking.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"

#include <iostream>
#include <ctype.h>

using namespace cv;
using namespace std;

static void help()
{
	cout << "\n\t操作说明: \n"
		"\t\t通过点击在图像中添加/删除特征点\n"
		"\t\tESC - 退出程序\n"
		"\t\tr -自动进行追踪\n"
		"\t\tc - 删除所有点\n"
		"\t\tn - 开/光-夜晚模式\n" << endl;
}

Point2f point;
bool addRemovePt = false;

//--------------------------------【onMouse( )回调函数】------------------------------------
//		描述：鼠标操作回调
//-------------------------------------------------------------------------------------------------
static void onMouse(int event, int x, int y, int /*flags*/, void* /*param*/)
{
	//此句代码的OpenCV2版为：
	//if (event == CV_EVENT_LBUTTONDOWN)
	//此句代码的OpenCV3版为：
	if (event == EVENT_LBUTTONDOWN)
	{
		point = Point2f((float)x, (float)y);
		addRemovePt = true;
	}
}

//-----------------------------------【main( )函数】--------------------------------------------
//		描述：控制台应用程序的入口函数，我们的程序从这里开始
//-------------------------------------------------------------------------------------------------
int main(int argc, char** argv)
{
	help();

	VideoCapture cap;

	//此句代码的OpenCV2版为：
	//TermCriteria termcrit(CV_TERMCRIT_ITER | CV_TERMCRIT_EPS, 20, 0.03);
	//此句代码的OpenCV3版为：
	TermCriteria termcrit(TermCriteria::MAX_ITER | TermCriteria::EPS, 20, 0.03);
	Size subPixWinSize(10, 10), winSize(31, 31);

	const int MAX_COUNT = 500;
	bool needToInit = false;
	bool nightMode = false;

	cap.open(0);

	if (!cap.isOpened())
	{
		cout << "Could not initialize capturing...\n";
		return 0;
	}

	namedWindow("LK Demo", 1);
	setMouseCallback("LK Demo", onMouse, 0);

	Mat gray, prevGray, image;
	vector<Point2f> points[2];

	for (;;)
	{
		Mat frame;
		cap >> frame;
		if (frame.empty())
			break;

		frame.copyTo(image);
		cvtColor(image, gray, COLOR_BGR2GRAY);

		if (nightMode)
			image = Scalar::all(0);

		if (needToInit)
		{
			// 自动初始化
			goodFeaturesToTrack(gray, points[1], MAX_COUNT, 0.01, 10, Mat(), 3, false, 0.04);
			cornerSubPix(gray, points[1], subPixWinSize, Size(-1, -1), termcrit);
			addRemovePt = false;
		}
		else if (!points[0].empty())
		{
			vector<uchar> status;
			vector<float> err;
			if (prevGray.empty())
				gray.copyTo(prevGray);
			calcOpticalFlowPyrLK(prevGray, gray, points[0], points[1], status, err, winSize,
				3, termcrit, 0, 0.001);
			size_t i, k;
			for (i = k = 0; i < points[1].size(); i++)
			{
				if (addRemovePt)
				{
					if (norm(point - points[1][i]) <= 5)
					{
						addRemovePt = false;
						continue;
					}
				}

				if (!status[i])
					continue;

				points[1][k++] = points[1][i];
				circle(image, points[1][i], 3, Scalar(0, 255, 0), -1, 8);
			}
			points[1].resize(k);
		}

		if (addRemovePt && points[1].size() < (size_t)MAX_COUNT)
		{
			vector<Point2f> tmp;
			tmp.push_back(point);

			//此句代码的OpenCV2版为：
			//cornerSubPix(gray, tmp, winSize, cvSize(-1, -1), termcrit);
			//此句代码的OpenCV3版为：
			cornerSubPix(gray, tmp, winSize, Size(-1, -1), termcrit);
			points[1].push_back(tmp[0]);
			addRemovePt = false;
		}

		needToInit = false;
		imshow("LK Demo", image);

		char c = (char)waitKey(10);
		if (c == 27)
			break;
		switch (c)
		{
		case 'r':
			needToInit = true;
			break;
		case 'c':
			points[0].clear();
			points[1].clear();
			break;
		case 'n':
			nightMode = !nightMode;
			break;
		}

		std::swap(points[1], points[0]);
		cv::swap(prevGray, gray);
	}

	return 0;
}