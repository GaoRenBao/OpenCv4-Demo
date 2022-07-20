/* 摄像头操作的所有自定义类 */

#ifndef _CAMERA_H_
#define _CAMERA_H_

#include <iostream>    
#include <opencv2\opencv.hpp>    
#include <opencv2\highgui\highgui.hpp>    
#include <fstream> 

using namespace cv;
using namespace std;

// 当前支持的摄像头数目
#define MAXDEV  10

class Camera
{
	public:
		bool CameraInit(int capid, int rows, int cols);// 初始化指定ID的摄像头
		Mat CameraImg(int capid);                      // 获取指定设备的一帧图像
		int RefreshCameraNum();                        // 获取当前可用的摄像头数目

	private:
		VideoCapture Cap[MAXDEV]; // 根据实际项目，设置最多可达到多少摄像头数目
};

#endif

