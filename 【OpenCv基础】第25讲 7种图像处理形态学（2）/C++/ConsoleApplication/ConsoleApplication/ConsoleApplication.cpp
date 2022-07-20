#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include "opencv2/imgproc/imgproc.hpp"
#include <iostream>

using namespace std;
using namespace cv;

void MyMorphology(int, void*);

int g_element = 15;  // 核
Mat srcImage;        // 原图

int main()
{
	// 载入原图
	srcImage = imread("1.jpg");

	//// 创建滑动条窗体
	//namedWindow("【原图】", 1);
	//createTrackbar("核", "【原图】", &g_element, 100, MyMorphology);
	//MyMorphology(g_element, 0);
	//waitKey(0);

	/****** 形态学击中击不中 独立测试部分（网络资源） ******/ 

	//创建输入图像和核
	Mat input_image = (Mat_<uchar>(8, 8) <<
		0, 0, 0, 0, 0, 0, 0, 0,
		0, 255, 255, 255, 0, 0, 0, 255,
		0, 255, 255, 255, 0, 0, 0, 0,
		0, 255, 255, 255, 0, 255, 0, 0,
		0, 0, 255, 0, 0, 0, 0, 0,
		0, 0, 255, 0, 0, 255, 255, 0,
		0, 255, 0, 255, 0, 0, 255, 0,
		0, 255, 255, 255, 0, 0, 0, 0);

	Mat kernel = (Mat_<int>(3, 3) <<
		0, 1, 0,
		1, -1, 1,
		0, 1, 0);
	//创建输出图像，并进行变换
	Mat output_image;
	morphologyEx(input_image, output_image, MORPH_HITMISS, kernel);

	// 为便于观察，将输入图像、输出图像、核放大五十倍，显示
	// 一个小方块表示一个像素
	const int rate = 50;
	kernel = (kernel + 1) * 127;
	kernel.convertTo(kernel, CV_8U);
	resize(kernel, kernel, Size(), rate, rate, INTER_NEAREST);
	imshow("kernel", kernel);
	resize(input_image, input_image, Size(), rate, rate, INTER_NEAREST);
	imshow("Original", input_image);
	resize(output_image, output_image, Size(), rate, rate, INTER_NEAREST);
	imshow("Hit or Miss", output_image);

	waitKey(0);
	return 0;
}

void MyMorphology(int, void*)
{
	// 创建窗口
	namedWindow("腐蚀【效果图】");
	namedWindow("膨胀【效果图】");
	namedWindow("开运算【效果图】");
	namedWindow("闭运算【效果图】");
	namedWindow("梯度【效果图】");
	namedWindow("顶帽【效果图】");
	namedWindow("黑帽【效果图】");
	namedWindow("击中击不中【效果图】");

	// 显示原图
	imshow("【原图】", srcImage);

	//定义核
	if (g_element <= 0) g_element = 1;
	Mat element = getStructuringElement(MORPH_RECT, Size(g_element, g_element));

	//进行形态学腐蚀操作
	Mat out1;
	morphologyEx(srcImage, out1, MORPH_ERODE, element);

	//进行形态学膨胀操作
	Mat out2;
	morphologyEx(srcImage, out2, MORPH_DILATE, element);

	//进行形态学开运算操作
	Mat out3;
	morphologyEx(srcImage, out3, MORPH_OPEN, element);

	//进行形态学闭运算操作
	Mat out4;
	morphologyEx(srcImage, out4, MORPH_CLOSE, element);

	//进行形态学梯度操作
	Mat out5;
	morphologyEx(srcImage, out5, MORPH_GRADIENT, element);

	//进行形态学顶帽操作
	Mat out6;
	morphologyEx(srcImage, out6, MORPH_TOPHAT, element);

	//进行形态学黑帽操作
	Mat out7;
	morphologyEx(srcImage, out7, MORPH_BLACKHAT, element);

	// 进行形态学击中击不中操作，需要将图像转灰度
	Mat des, out8;
	cvtColor(srcImage, des, COLOR_BGR2GRAY);
	morphologyEx(des, out8, MORPH_HITMISS, element);

	// 显示效果图
	putText(out1, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out2, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out3, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out4, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out5, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out6, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out7, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);
	putText(out8, "C++", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, CV_AA);

	imshow("腐蚀【效果图】", out1);
	imshow("膨胀【效果图】", out2);
	imshow("开运算【效果图】", out3);
	imshow("闭运算【效果图】", out4);
	imshow("梯度【效果图】", out5);
	imshow("顶帽【效果图】", out6);
	imshow("黑帽【效果图】", out7);
	imshow("击中击不中【效果图】", out8);
	waitKey(30);
}

