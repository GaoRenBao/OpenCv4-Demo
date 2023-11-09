/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：FLANN结合SURF/SIFT进行关键点的描述和匹配
博客：http://www.bilibili996.com/Course?id=3106495000236
作者：高仁宝
时间：2023.11
*/

// 程序描述：FLANN结合SURF进行关键点的描述和匹配
#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	Mat trainImage, trainImage_gray;

	// 创建视频对象、定义帧率
	VideoCapture cap(0);

	// 载入图像、显示并转化为灰度图
	for (int i = 0; i < 10; i++)
		cap >> trainImage;

	imshow("原始图", trainImage);
	cvtColor(trainImage, trainImage_gray, COLOR_BGR2GRAY);

	// 检测SIFT关键点、提取训练图像描述符
	Ptr<SIFT> siftDetector = SIFT::create(80);
	vector<KeyPoint> train_keyPoint;
	Mat trainDescriptor;

	// 方法1：计算描述符（特征向量），将Detect和Compute操作分开
	//siftDetector->detect(trainImage_gray, train_keyPoint);
	//siftDetector->compute(trainImage_gray, train_keyPoint, trainDescriptor);

	// 方法2：计算描述符（特征向量），将Detect和Compute操作合并
	siftDetector->detectAndCompute(trainImage_gray, cv::Mat(), train_keyPoint, trainDescriptor);

	// 创建基于FLANN的描述符匹配对象
	FlannBasedMatcher matcher;
	vector<Mat> train_desc_collection(1, trainDescriptor);
	matcher.add(train_desc_collection);
	matcher.train();

	// 不断循环，直到q键被按下
	while (true)
	{
		//<1>参数设置
		int64 time0 = getTickCount();
		Mat testImage, testImage_gray;
		cap >> testImage;//采集视频到testImage中
		if (testImage.empty())
			continue;

		//imwrite("1.jpg", testImage);
		//imshow("testImage", testImage);
		//waitKey(0);

		//<2>转化图像到灰度
		cvtColor(testImage, testImage_gray, COLOR_BGR2GRAY);

		//<3>检测S关键点、提取测试图像描述符
		vector<KeyPoint> test_keyPoint;
		Mat testDescriptor;

		// 方法1：计算描述符（特征向量），将Detect和Compute操作分开
		//siftDetector->detect(testImage_gray, test_keyPoint);
		//siftDetector->compute(testImage_gray, test_keyPoint, testDescriptor);

		// 方法2：计算描述符（特征向量），将Detect和Compute操作合并
		siftDetector->detectAndCompute(testImage_gray, cv::Mat(), test_keyPoint, testDescriptor);

		//<4>匹配训练和测试描述符
		vector<vector<DMatch> > matches;
		matcher.knnMatch(testDescriptor, matches, 2);

		// <5>根据劳氏算法（Lowe's algorithm），得到优秀的匹配点
		vector<DMatch> goodMatches;
		for (unsigned int i = 0; i < matches.size(); i++)
		{
			if (matches[i][0].distance < 0.6 * matches[i][1].distance)
				goodMatches.push_back(matches[i][0]);
		}

		//<6>绘制匹配点并显示窗口
		Mat dstImage;
		drawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, dstImage);
		imshow("匹配窗口", dstImage);

		//<7>输出帧率信息
		cout << "当前帧率为：" << getTickFrequency() / (getTickCount() - time0) << endl;

		// 按ESC退出
		if (waitKey(10) == 27)
			break;
	}

	return 0;
}
