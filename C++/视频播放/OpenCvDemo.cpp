/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：视频播放
博客：http://www.bilibili996.com/Course?id=4380386000009
作者：高仁宝
时间：2023.11
*/

#include <opencv2\opencv.hpp>  
using namespace cv;

int main()
{
	//【1】读入视频
	VideoCapture capture("../images/lol.avi");

	//【2】循环显示每一帧
	while (1)
	{
		Mat frame;//定义一个Mat变量，用于存储每一帧的图像
		capture >> frame;  //读取当前帧

		//若视频播放完成，退出循环
		if (frame.empty())
		{
			break;
		}

		imshow("读取视频", frame);  //显示当前帧
		waitKey(30);  //延时30ms
	}
	return 0;
}