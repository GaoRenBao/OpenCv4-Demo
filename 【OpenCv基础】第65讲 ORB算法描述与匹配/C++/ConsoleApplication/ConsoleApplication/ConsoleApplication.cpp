#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	//【0】载入源图，显示并转化为灰度图
	Mat srcImage = imread("1.jpg");
	imshow("原始图", srcImage);
	Mat grayImage;
	cvtColor(srcImage, grayImage, COLOR_BGR2GRAY);

	//------------------检测ORB特征点并在图像中提取物体的描述符----------------------

	//【1】参数定义
	Ptr<ORB> featureDetector = ORB::create();
	vector<KeyPoint> keyPoints;
	Mat descriptors;

	//【2】调用detect函数检测出特征关键点，保存在vector容器中
	featureDetector->detect(grayImage, keyPoints);

	//【3】计算描述符（特征向量）
	featureDetector->compute(grayImage, keyPoints, descriptors);

	//【4】基于FLANN的描述符对象匹配
	flann::Index flannIndex(descriptors, flann::LshIndexParams(12, 20, 2), cvflann::FLANN_DIST_HAMMING);

	//【5】初始化视频采集对象
	VideoCapture cap(0);

	unsigned int frameCount = 0;//帧数

	//【6】轮询，直到按下ESC键退出循环
	while (1)
	{
		double time0 = static_cast<double>(getTickCount());//记录起始时间
		Mat captureImage, captureImage_gray;//定义两个Mat变量，用于视频采集
		cap >> captureImage;//采集视频帧
		if (captureImage.empty())//采集为空的处理
			continue;

		// 保存原图用
		//imshow("1.jpg", captureImage);
		//imwrite("1.jpg", captureImage);
		//waitKey(0);
		//continue;

		//转化图像到灰度
		cvtColor(captureImage, captureImage_gray, COLOR_BGR2GRAY);//采集的视频帧转化为灰度图

		//【7】检测SIFT关键点并提取测试图像中的描述符
		vector<KeyPoint> captureKeyPoints;
		Mat captureDescription;

		//【8】调用detect函数检测出特征关键点，保存在vector容器中
		featureDetector->detect(captureImage_gray, captureKeyPoints);

		//【9】计算描述符
		featureDetector->compute(captureImage_gray, captureKeyPoints, captureDescription);

		//【10】匹配和测试描述符，获取两个最邻近的描述符
		Mat matchIndex(captureDescription.rows, 2, CV_32SC1), matchDistance(captureDescription.rows, 2, CV_32FC1);
		flannIndex.knnSearch(captureDescription, matchIndex, matchDistance, 2, flann::SearchParams());//调用K邻近算法

		//【11】根据劳氏算法（Lowe's algorithm）选出优秀的匹配
		vector<DMatch> goodMatches;
		for (int i = 0; i < matchDistance.rows; i++)
		{
			if (matchDistance.at<float>(i, 0) < 0.6 * matchDistance.at<float>(i, 1))
			{
				DMatch dmatches(i, matchIndex.at<int>(i, 0), matchDistance.at<float>(i, 0));
				goodMatches.push_back(dmatches);
			}
		}

		//【12】绘制并显示匹配窗口
		Mat resultImage;
		drawMatches(captureImage, captureKeyPoints, srcImage, keyPoints, goodMatches, resultImage);
		imshow("匹配窗口", resultImage);

		//【13】显示帧率
		cout << ">帧率= " << getTickFrequency() / (getTickCount() - time0) << endl;

		//按下ESC键，则程序退出
		if (char(waitKey(1)) == 27) break;
	}

	return 0;
}


