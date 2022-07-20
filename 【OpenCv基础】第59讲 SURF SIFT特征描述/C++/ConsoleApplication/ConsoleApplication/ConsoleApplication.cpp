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
	Mat srcImage1 = imread("1.jpg");
	Mat srcImage2 = imread("2.jpg");
	if (!srcImage1.data || !srcImage2.data)
	{
		printf("读取图片错误，请确定目录下是否有imread函数指定的图片存在~！ \n"); return false;
	}

	//【2】定义SIFT中的hessian阈值特征点检测算子
	int minHessian = 700;
	Ptr<SIFT> siftDetector = SIFT::create(minHessian);
	//vector模板类，存放任意类型的动态数组
	vector<KeyPoint> keyPoint1, keyPoint2;
	
	//【3】调用detect函数检测出SIFT特征关键点，保存在vector容器中
	siftDetector->detect(srcImage1, keyPoint1);
	siftDetector->detect(srcImage2, keyPoint2);

	//【4】方法1：计算描述符（特征向量）
	//Mat descriptors1, descriptors2;
	//siftDetector->compute(srcImage1, keyPoint1, descriptors1);
	//siftDetector->compute(srcImage2, keyPoint2, descriptors2);
	
	//【4】方法2：计算描述符（特征向量）
	vector<KeyPoint> keypoints1, keypoints2;
	Mat descriptors1, descriptors2;
	siftDetector->detectAndCompute(srcImage1, cv::Mat(), keypoints1, descriptors1);
	siftDetector->detectAndCompute(srcImage2, cv::Mat(), keypoints2, descriptors2);

	//【5】使用BruteForce进行匹配
	// 实例化一个匹配器

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

