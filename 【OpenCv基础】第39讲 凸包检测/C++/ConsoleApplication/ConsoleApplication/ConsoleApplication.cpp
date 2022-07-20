#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>
using namespace cv;
using namespace std;

#define OBJ 3

#if(OBJ == 1)
// 演示1

Mat srcImage, grayImage;
RNG rng(12345);
int main()
{
	srcImage = imread("1.jpg");

	//图像灰度图转化并平滑滤波
	cvtColor(srcImage, grayImage, COLOR_BGR2GRAY);
	blur(grayImage, grayImage, Size(3, 3));
	namedWindow("原图像", WINDOW_AUTOSIZE);
	imshow("原图像", grayImage);

	Mat threshold_output;
	Mat src_copy = srcImage.clone();
	vector<vector<Point>>contours;
	vector<Vec4i>hierarchy;
	int thresh = 100;

	//使用Threshold检测图像边缘
	threshold(grayImage, threshold_output, 200, 255, THRESH_BINARY);

	//寻找图像轮廓
	findContours(threshold_output, contours, hierarchy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));

	//寻找图像凸包
	vector<vector<Point>>hull(contours.size());
	for (int i = 0; i < contours.size(); i++) {
		convexHull(Mat(contours[i]), hull[i], false);
	}

	//绘制轮廓和凸包
	Mat drawing = Mat::zeros(threshold_output.size(), CV_8UC3);
	for (int i = 0; i < contours.size(); i++)
	{
		Scalar color = Scalar(rng.uniform(0, 255), rng.uniform(0, 255), rng.uniform(0, 255));
		// 绘制轮廓
		drawContours(drawing, contours, i, color, 1, 8, vector<Vec4i>(), 0, Point());
		// 绘制凸包
		drawContours(drawing, hull, i, color, 1, 8, vector<Vec4i>(), 0, Point());
	}

	namedWindow("凸包", WINDOW_AUTOSIZE);
	imshow("凸包", drawing);

	waitKey(0);
	return 0;
}
#elif OBJ == 2

// 演示2
Mat srcImage, grayImage;
int thresh = 100;
const int threshMaxValue = 255;
RNG rng(12345);

//定义回调函数
void thresh_callback(int, void*);

int main()
{
	srcImage = imread("1.jpg");

	//图像灰度图转化并平滑滤波
	cvtColor(srcImage, grayImage, COLOR_BGR2GRAY);
	blur(grayImage, grayImage, Size(3, 3));
	namedWindow("原图像", WINDOW_AUTOSIZE);
	imshow("原图像", grayImage);

	//创建轨迹条
	createTrackbar("Threshold:", "原图像", &thresh, threshMaxValue, thresh_callback);
	thresh_callback(thresh, 0);
	waitKey(0);

	return 0;
}

void thresh_callback(int, void*)
{
	Mat src_copy = srcImage.clone();
	Mat threshold_output;
	vector<vector<Point>>contours;
	vector<Vec4i>hierarchy;

	//使用Threshold检测图像边缘
	threshold(grayImage, threshold_output, thresh, 255, THRESH_BINARY);

	//寻找图像轮廓
	findContours(threshold_output, contours, hierarchy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));

	//寻找图像凸包
	vector<vector<Point>>hull(contours.size());
	for (int i = 0; i < contours.size(); i++)
	{
		convexHull(Mat(contours[i]), hull[i], false);
	}

	//绘制轮廓和凸包
	Mat drawing = Mat::zeros(threshold_output.size(), CV_8UC3);
	for (int i = 0; i < contours.size(); i++)
	{
		Scalar color = Scalar(rng.uniform(0, 255), rng.uniform(0, 255), rng.uniform(0, 255));
		drawContours(drawing, contours, i, color, 1, 8, vector<Vec4i>(), 0, Point());
		drawContours(drawing, hull, i, color, 1, 8, vector<Vec4i>(), 0, Point());
	}

	namedWindow("凸包", WINDOW_AUTOSIZE);
	imshow("凸包", drawing);
}

#elif OBJ == 3

// 演示3
//键盘按键【ESC】、【Q】、【q】- 退出程序
//键盘按键任意键 - 重新生成随机点，并进行凸包检测

int main()
{
	//改变console字体颜色
	system("color 1F");

	//初始化变量和随机值
	Mat image(600, 600, CV_8UC3);
	RNG& rng = theRNG();

	//循环，按下ESC,Q,q键程序退出，否则有键按下便一直更新
	while (1)
	{
		//参数初始化
		char key;//键值
		int count = (unsigned)rng % 100 + 3;//随机生成点的数量
		vector<Point> points; //点值

		//随机生成点坐标
		for (int i = 0; i < count; i++)
		{
			Point point;
			point.x = rng.uniform(image.cols / 4, image.cols * 3 / 4);
			point.y = rng.uniform(image.rows / 4, image.rows * 3 / 4);

			points.push_back(point);
		}

		//检测凸包
		vector<int> hull;
		convexHull(Mat(points), hull, true);

		//绘制出随机颜色的点
		image = Scalar::all(0);
		for (int i = 0; i < count; i++)
			circle(image, points[i], 3, Scalar(rng.uniform(0, 255), rng.uniform(0, 255), rng.uniform(0, 255)), FILLED, LINE_AA);

		//准备参数
		int hullcount = (int)hull.size();//凸包的边数
		Point point0 = points[hull[hullcount - 1]];//连接凸包边的坐标点

		//绘制凸包的边
		for (int i = 0; i < hullcount; i++)
		{
			Point point = points[hull[i]];
			line(image, point0, point, Scalar(255, 255, 255), 2, LINE_AA); // 低版本是：CV_AA
			point0 = point;
		}

		//显示效果图
		imshow("凸包检测示例", image);

		//按下ESC,Q,或者q，程序退出
		key = (char)waitKey();
		if (key == 27 || key == 'q' || key == 'Q')
			break;
	}
	return 0;
}
#endif







