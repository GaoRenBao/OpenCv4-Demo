// 程序描述：FLANN结合SURF进行关键点的描述和匹配

#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	// 载入图像、显示并转化为灰度图
	Mat trainImage = imread("1.jpg"), trainImage_gray;
	imshow("原始图", trainImage);
	cvtColor(trainImage, trainImage_gray, COLOR_BGR2GRAY);

	// 检测SIFT关键点、提取训练图像描述符
	Ptr<SIFT> siftDetector = SIFT::create(80);
	vector<KeyPoint> train_keyPoint;
	// 调用detect函数检测出SIFT特征关键点，保存在vector容器中
	siftDetector->detect(trainImage_gray, train_keyPoint);

	Mat trainDescriptor;
	siftDetector->compute(trainImage_gray, train_keyPoint, trainDescriptor);
	//siftDetector->detectAndCompute(trainImage_gray, cv::Mat(), train_keyPoint, trainDescriptor);

	// 创建基于FLANN的描述符匹配对象
	FlannBasedMatcher matcher;
	vector<Mat> train_desc_collection(1, trainDescriptor);
	matcher.add(train_desc_collection);
	matcher.train();

	// 创建视频对象、定义帧率
	VideoCapture cap(0);
	unsigned int frameCount = 0;//帧数

	// 不断循环，直到q键被按下
	while (char(waitKey(1)) != 'q')
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
		siftDetector->detect(testImage_gray, test_keyPoint);
		siftDetector->compute(testImage_gray, test_keyPoint, testDescriptor);
		//siftDetector->detectAndCompute(testImage_gray, cv::Mat(), test_keyPoint, testDescriptor);

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
	}

	return 0;
}

