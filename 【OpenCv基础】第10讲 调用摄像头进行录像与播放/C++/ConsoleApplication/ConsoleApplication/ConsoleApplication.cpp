#include <iostream>    
#include <opencv2\opencv.hpp>    
#include <opencv2\highgui\highgui.hpp>    
#include <fstream> 
#include <string>
#include <vector>
#include "Camera.h"

using namespace cv;
using namespace std;

int main()
{
	Camera cap;
	int video = 0;
	// 读取ID为video的摄像头，尺寸为640*480
	if (cap.CameraInit(video, 480, 640) == false) {
		cout << "Error0" << endl;
		while (true);
		return 1;
	}

#if false
	// 摄像头实时显示
	while (1)
	{
		Mat srcImage = cap.CameraImg(video);
		if (!srcImage.empty())
		{
			Mat show;
			resize(srcImage, show, Size(srcImage.cols * 1, srcImage.rows * 1));
			imshow("原始图像", show);
			waitKey(1);
		}
	}
#endif

#if false
	// 录像，本地存储
	VideoWriter writer("a.avi", CV_FOURCC('M', 'J', 'P', 'G'), 25.0, Size(640, 480));
	while (1)
	{
		Mat srcImage = cap.CameraImg(0);
		if (!srcImage.empty())
		{
			writer << srcImage;
			imshow("录像", srcImage);
			if (waitKey(10) == 27) // ESC按钮
			{
				writer.release();
				waitKey(0);
				return 0;
			}
		}
	}
#endif

#if true
	// 读取录像信息
	VideoCapture capture("a.avi");
	while (1)
	{
		Mat frame;//定义一个Mat变量，用于存储每一帧的图像
		capture >> frame;  //读取当前帧
		 //若视频播放完成，退出循环
		if (frame.empty()) break;
		imshow("读取视频", frame);  //显示当前帧
		waitKey(30);  //延时30ms
	}
#endif

	waitKey(0);
}