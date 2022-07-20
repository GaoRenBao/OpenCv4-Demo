#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>

using namespace cv;
using namespace std;

#define WINDOW_NAME "【亚像素级角点检测】"        //为窗口标题定义的宏 

Mat g_srcImage, g_grayImage;
int g_maxCornerNumber = 33;
int g_maxTrackbarNumber = 500;
RNG g_rng(12345);//初始化随机数生成器

void on_GoodFeaturesToTrack(int, void*)
{
	//【1】对变量小于等于1时的处理
	if (g_maxCornerNumber <= 1) { g_maxCornerNumber = 1; }

	//【2】Shi-Tomasi算法（goodFeaturesToTrack函数）的参数准备
	vector<Point2f> corners;
	double qualityLevel = 0.01;//角点检测可接受的最小特征值
	double minDistance = 10;//角点之间的最小距离
	int blockSize = 3;//计算导数自相关矩阵时指定的邻域范围
	double k = 0.04;//权重系数
	Mat copy = g_srcImage.clone();	//复制源图像到一个临时变量中，作为感兴趣区域

									//【3】进行Shi-Tomasi角点检测
	goodFeaturesToTrack(g_grayImage,//输入图像
		corners,//检测到的角点的输出向量
		g_maxCornerNumber,//角点的最大数量
		qualityLevel,//角点检测可接受的最小特征值
		minDistance,//角点之间的最小距离
		Mat(),//感兴趣区域
		blockSize,//计算导数自相关矩阵时指定的邻域范围
		false,//不使用Harris角点检测
		k);//权重系数

		   //【4】输出文字信息
	cout << "\n\t>-------------此次检测到的角点数量为：" << corners.size() << endl;

	//【5】绘制检测到的角点
	int r = 4;
	for (unsigned int i = 0; i < corners.size(); i++)
	{
		//以随机的颜色绘制出角点
		circle(copy, corners[i], r, Scalar(g_rng.uniform(0, 255), g_rng.uniform(0, 255),
			g_rng.uniform(0, 255)), -1, 8, 0);
	}

	//【6】显示（更新）窗口
	imshow(WINDOW_NAME, copy);

	//【7】亚像素角点检测的参数设置
	Size winSize = Size(5, 5);
	Size zeroZone = Size(-1, -1);
	TermCriteria criteria = TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 40, 0.001);

	//【8】计算出亚像素角点位置
	cornerSubPix(g_grayImage, corners, winSize, zeroZone, criteria);

	//【9】输出角点信息
	for (int i = 0; i < corners.size(); i++)
	{
		cout << " \t>>精确角点坐标[" << i << "]  (" << corners[i].x << "," << corners[i].y << ")" << endl;
	}
}

int main()
{

	//【1】载入源图像并将其转换为灰度图
	g_srcImage = imread("1.jpg", 1);
	cvtColor(g_srcImage, g_grayImage, COLOR_BGR2GRAY);

	//【2】创建窗口和滑动条，并进行显示和回调函数初始化
	namedWindow(WINDOW_NAME, WINDOW_AUTOSIZE);
	createTrackbar("最大角点数", WINDOW_NAME, &g_maxCornerNumber, g_maxTrackbarNumber, on_GoodFeaturesToTrack);
	imshow(WINDOW_NAME, g_srcImage);
	on_GoodFeaturesToTrack(0, 0);

	waitKey(0);
	return(0);
}

