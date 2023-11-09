/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：SURF/SIFT特征描述
博客：http://www.bilibili996.com/Course?id=2950814000235
作者：高仁宝
时间：2023.11
*/

#include <iostream>
#include <opencv2/opencv.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	//【0】改变console字体颜色
	system("color 1F");

	//【1】载入素材图
	Mat srcImage1 = imread("../images/book2.jpg");
	Mat srcImage2 = imread("../images/book3.jpg");
	if (!srcImage1.data || !srcImage2.data)
	{
		printf("读取图片错误，请确定目录下是否有imread函数指定的图片存在~！ \n"); return false;
	}

	//【2】定义SIFT中的hessian阈值特征点检测算子
	int minHessian = 700;
	Ptr<SIFT> siftDetector = SIFT::create(minHessian);
	//vector模板类，存放任意类型的动态数组
	vector<KeyPoint> keyPoint1, keyPoint2;
	Mat descriptors1, descriptors2;

	//【4】方法1：计算描述符（特征向量），将Detect和Compute操作分开
	siftDetector->detect(srcImage1, keyPoint1);
	siftDetector->detect(srcImage2, keyPoint2);
	siftDetector->compute(srcImage1, keyPoint1, descriptors1);
	siftDetector->compute(srcImage2, keyPoint2, descriptors2);

	//【4】方法2：计算描述符（特征向量），将Detect和Compute操作合并
	//siftDetector->detectAndCompute(srcImage1, cv::Mat(), keyPoint1, descriptors1);
	//siftDetector->detectAndCompute(srcImage2, cv::Mat(), keyPoint2, descriptors2);

	//【5】使用BruteForce进行匹配
		// 创建特征点匹配器
	Ptr<BFMatcher> matcher = BFMatcher::create();
	// 匹配两幅图中的描述子（descriptors）
	vector<DMatch> matches;
	matcher->match(descriptors1, descriptors2, matches);

	//【6】绘制从两个图像中匹配出的关键点
	Mat imgMatches;
	drawMatches(srcImage1, keyPoint1, srcImage2, keyPoint2, matches, imgMatches);//进行绘制

	//【7】显示效果图
	imshow("匹配图", imgMatches);
	waitKey(0);
	return 0;
}