/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：摄像头录像与播放
博客：http://www.bilibili996.com/Course?id=4722430000047
作者：高仁宝
时间：2023.11
*/

#include <iostream>    
#include <opencv2\opencv.hpp>    
#include <opencv2\highgui\highgui.hpp>    
#include <fstream> 
#include <string>
#include <vector>

using namespace cv;
using namespace std;

// demo1 摄像头图像实时显示
void demo1()
{
	VideoCapture cap;
	cap.open(0);
	if (!cap.isOpened())
	{
		cout << "Error0" << endl;
		while (true);
		return;
	}

	// 尺寸为640*480
	cap.set(CAP_PROP_FRAME_WIDTH, 640);  // 宽度
	cap.set(CAP_PROP_FRAME_HEIGHT, 480); // 高度
	//cap.set(CAP_PROP_BRIGHTNESS, 1);   // 亮度
	//cap.set(CAP_PROP_CONTRAST, 0);     // 对比度
	//cap.set(CAP_PROP_SATURATION, 100); // 饱和度
	//cap.set(CAP_PROP_HUE, 0);          // 色调
	//cap.set(CAP_PROP_EXPOSURE, 0);     // 曝光

	// 读取设置的参数
	cout << "摄像头设置，宽度:" << cap.get(CAP_PROP_FRAME_WIDTH) << endl;
	cout << "摄像头设置，高度:" << cap.get(CAP_PROP_FRAME_WIDTH) << endl;

	// 摄像头实时显示
	while (1)
	{
		Mat srcImage;
		cap >> srcImage;
		if (!srcImage.empty())
		{
			// 左右翻转, 使用笔记本电脑摄像头才有用。
			flip(srcImage, srcImage, 1);
			imshow("图像", srcImage);
			waitKey(1);
		}
	}
}

// demo2 录像
void demo2()
{
	VideoCapture cap;
	cap.open(0);
	if (!cap.isOpened())
	{
		cout << "Error0" << endl;
		while (true);
		return;
	}

	// 尺寸为640*480
	cap.set(CAP_PROP_FRAME_WIDTH, 640);  // 宽度
	cap.set(CAP_PROP_FRAME_HEIGHT, 480); // 高度

	// 定义编解码器并创建VideoWriter对象，可以是“MJPG”或者“XVID”
	// opencv3：CV_FOURCC('M', 'J', 'P', 'G')
	// opencv4：VideoWriter:: fourcc('M', 'J', 'P', 'G')
	int fourcc = VideoWriter::fourcc('X', 'V', 'I', 'D');
	VideoWriter writer("a.avi", fourcc, 25.0, Size(640, 480));
	while (1)
	{
		Mat srcImage;
		cap >> srcImage;
		if (!srcImage.empty())
		{
			writer << srcImage;
			imshow("录像", srcImage);
			if (waitKey(10) == 27) // ESC按钮
			{
				cap.release();
				writer.release();
				destroyAllWindows();
				return;
			}
		}
	}
}

// demo3 播放录像
void demo3()
{
	VideoCapture cap("a.avi");
	// 帧率
	int	fps = cap.get(CAP_PROP_FPS);
	cout << "Frames per second using video.get(cv2.CAP_PROP_FPS) : " << fps << endl;

	// 总共有多少帧
	int	num_frames = cap.get(CAP_PROP_FRAME_COUNT);
	cout << "共有" << num_frames << "帧" << endl;

	int	frame_height = cap.get(CAP_PROP_FRAME_HEIGHT);
	int	frame_width = cap.get(CAP_PROP_FRAME_WIDTH);
	cout << "高：" << frame_height << ",宽：" << frame_width << endl;

	int	FRAME_NOW = cap.get(CAP_PROP_POS_FRAMES);  // 第0帧
	printf("当前帧数%d", FRAME_NOW);  // 当前帧数 0.0

	//  读取指定帧, 对视频文件才有效，对摄像头无效？？
	int	frame_no = 10;
	cap.set(1, frame_no);  // Where frame_no is the frame you want

	Mat frame;
	cap >> frame;  //读取当前帧
	imshow("frame_no", frame);

	FRAME_NOW = cap.get(CAP_PROP_POS_FRAMES);
	printf("当前帧数%d\r\n", FRAME_NOW);  // 当前帧数

	while (1)
	{
		cap >> frame;  //读取当前帧
		//若视频播放完成，退出循环
		if (frame.empty())
		{
			waitKey(0);
			cap.release();
			destroyAllWindows();
			break;
		}

		FRAME_NOW = cap.get(CAP_PROP_POS_FRAMES); // 当前帧数
		printf("当前帧数%d\r\n", FRAME_NOW);

		imshow("读取视频", frame);  //显示当前帧
		waitKey(fps);
	}
}

// 主函数
void main()
{
	// demo1();
	//demo2();
	demo3();
}