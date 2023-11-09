/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：cornerHarris角点检测
博客：http://www.bilibili996.com/Course?id=0862817000219
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>  
#include <opencv2/imgproc/imgproc.hpp>  
using namespace cv;

int main()
{
	//以灰度模式载入图像并显示
	Mat srcImage = imread("../images/home4.jpg", 0);
	imshow("原始图", srcImage);

	//进行Harris角点检测找出角点
	Mat cornerStrength;
	cornerHarris(srcImage, cornerStrength, 2, 3, 0.01);

	//对灰度图进行阈值操作，得到二值图并显示  
	Mat harrisCorner;
	threshold(cornerStrength, harrisCorner, 0.00001, 255, THRESH_BINARY);
	imshow("角点检测后的二值效果图", harrisCorner);
	waitKey(0);
	return 0;
}