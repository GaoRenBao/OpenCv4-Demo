#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace cv;
using namespace std;

int main()
{
	//【1】载入原始图片
	Mat srcImage1 = imread("11.jpg");
	Mat srcImage2 = imread("2.jpg");
	if (!srcImage1.data || !srcImage2.data)
	{
		printf("读取图片错误，请确定目录下是否有imread函数指定的图片存在~！ \n"); return false;
	}

	// 使用SIFT算子检测关键点
	Ptr<SiftFeatureDetector> detector = SiftFeatureDetector::create(400);

	// vector模板类，存放任意类型的动态数组
	vector<KeyPoint> keypoints_object, keypoints_scene;
	Mat descriptors_object, descriptors_scene;

	// 调用detect函数检测出SURF特征关键点，保存在vector容器中
	detector->detect(srcImage1, keypoints_object);
	detector->detect(srcImage2, keypoints_scene);
	// 计算描述符（特征向量）
	detector->compute(srcImage1, keypoints_object, descriptors_object);
	detector->compute(srcImage2, keypoints_scene, descriptors_scene);

	// 使用FLANN匹配算子进行匹配
	FlannBasedMatcher matcher;
	vector<DMatch> matches;
	matcher.match(descriptors_object, descriptors_scene, matches);
	double max_dist = 0; double min_dist = 100;//最小距离和最大距离

	// 计算出关键点之间距离的最大值和最小值
	for (int i = 0; i < descriptors_object.rows; i++)
	{
		double dist = matches[i].distance;
		if (dist < min_dist) min_dist = dist;
		if (dist > max_dist) max_dist = dist;
	}

	printf(">Max dist 最大距离 : %f \n", max_dist);
	printf(">Min dist 最小距离 : %f \n", min_dist);

	// 存下匹配距离小于3*min_dist的点对
	std::vector< DMatch > good_matches;
	for (int i = 0; i < descriptors_object.rows; i++)
	{
		if (matches[i].distance < 3 * min_dist)
		{
			good_matches.push_back(matches[i]);
		}
	}

	// 绘制出匹配到的关键点
	Mat img_matches;
	drawMatches(srcImage1, keypoints_object, srcImage2, keypoints_scene,
		good_matches, img_matches, Scalar::all(-1), Scalar::all(-1),
		vector<char>(), DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS);

	// 定义两个局部变量
	vector<Point2f> obj;
	vector<Point2f> scene;

	//从匹配成功的匹配对中获取关键点
	for (unsigned int i = 0; i < good_matches.size(); i++)
	{
		obj.push_back(keypoints_object[good_matches[i].queryIdx].pt);
		scene.push_back(keypoints_scene[good_matches[i].trainIdx].pt);
	}

	Mat H = findHomography(obj, scene, RANSAC);//计算透视变换 

	//从待测图片中获取角点
	vector<Point2f> obj_corners(4);
	obj_corners[0] = Point(0, 0); 
	obj_corners[1] = Point(srcImage1.cols, 0);
	obj_corners[2] = Point(srcImage1.cols, srcImage1.rows); 
	obj_corners[3] = Point(0, srcImage1.rows);
	vector<Point2f> scene_corners(4);

	//进行透视变换
	perspectiveTransform(obj_corners, scene_corners, H);

	//绘制出角点之间的直线
	line(img_matches, scene_corners[0] + Point2f(static_cast<float>(srcImage1.cols), 0), scene_corners[1] + Point2f(static_cast<float>(srcImage1.cols), 0), Scalar(255, 0, 123), 4);
	line(img_matches, scene_corners[1] + Point2f(static_cast<float>(srcImage1.cols), 0), scene_corners[2] + Point2f(static_cast<float>(srcImage1.cols), 0), Scalar(255, 0, 123), 4);
	line(img_matches, scene_corners[2] + Point2f(static_cast<float>(srcImage1.cols), 0), scene_corners[3] + Point2f(static_cast<float>(srcImage1.cols), 0), Scalar(255, 0, 123), 4);
	line(img_matches, scene_corners[3] + Point2f(static_cast<float>(srcImage1.cols), 0), scene_corners[0] + Point2f(static_cast<float>(srcImage1.cols), 0), Scalar(255, 0, 123), 4);

	//显示最终结果
	imshow("效果图", img_matches);

	waitKey(0);
	return 0;
}




