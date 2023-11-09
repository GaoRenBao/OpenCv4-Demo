/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：使用FLANN进行特征点匹配
博客：http://www.bilibili996.com/Course?id=1443670000237
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	//【1】载入源图片
	Mat img_1 = imread("../images/book2.jpg", 1);
	Mat img_2 = imread("../images/book3.jpg", 1);
	if (!img_1.data || !img_2.data) { printf("读取图片image0错误~！ \n"); return false; }

	//【2】利用SIFT检测器检测的关键点
	Ptr<SIFT> siftDetector = SIFT::create(300);
	Mat descriptors_1, descriptors_2;
	vector<KeyPoint> keypoints_1, keypoints_2;

	// 方法1：计算描述符（特征向量），将Detect和Compute操作分开
	//siftDetector->detect(img_1, keypoints_1);
	//siftDetector->detect(img_2, keypoints_2);
	//siftDetector->compute(img_1, keypoints_1, descriptors_1);
	//siftDetector->compute(img_2, keypoints_2, descriptors_2);

	// 方法2：计算描述符（特征向量），将Detect和Compute操作合并
	siftDetector->detectAndCompute(img_1, cv::Mat(), keypoints_1, descriptors_1);
	siftDetector->detectAndCompute(img_2, cv::Mat(), keypoints_2, descriptors_2);

	//【4】采用FLANN算法匹配描述符向量
	FlannBasedMatcher matcher;
	vector<DMatch> matches;
	matcher.match(descriptors_1, descriptors_2, matches);
	double max_dist = 0; double min_dist = 100;

	//【5】快速计算关键点之间的最大和最小距离
	for (int i = 0; i < descriptors_1.rows; i++)
	{
		double dist = matches[i].distance;
		if (dist < min_dist) min_dist = dist;
		if (dist > max_dist) max_dist = dist;
	}
	//输出距离信息
	printf("> 最大距离（Max dist） : %f \n", max_dist);
	printf("> 最小距离（Min dist） : %f \n", min_dist);

	//【6】存下符合条件的匹配结果（即其距离小于2* min_dist的），使用radiusMatch同样可行
	std::vector< DMatch > good_matches;
	for (int i = 0; i < descriptors_1.rows; i++)
	{
		if (matches[i].distance < 2 * min_dist)
		{
			good_matches.push_back(matches[i]);
		}
	}

	//【7】绘制出符合条件的匹配点
	Mat img_matches;
	drawMatches(img_1, keypoints_1, img_2, keypoints_2,
		good_matches, img_matches, Scalar::all(-1), Scalar::all(-1),
		vector<char>(), DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS);

	//【8】输出相关匹配点信息
	for (int i = 0; i < good_matches.size(); i++)
	{
		printf(">符合条件的匹配点 [%d] 特征点1: %d  -- 特征点2: %d  \n", i, good_matches[i].queryIdx, good_matches[i].trainIdx);
	}

	//【9】显示效果图
	imshow("匹配效果图", img_matches);

	//按任意键退出程序
	waitKey(0);
	return 0;
}