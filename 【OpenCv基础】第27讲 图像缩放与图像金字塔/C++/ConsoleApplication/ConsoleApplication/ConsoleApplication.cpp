#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;

int main()
{
	Mat dstImage1, dstImage2, dstImage3, dstImage4;

	// 载入原图
	Mat srcImage = imread("1.jpg");

	//显示原始图  
	imshow("【原始图】", srcImage);

	// 使用resize进行尺寸调整操作
	resize(srcImage, dstImage1, Size(srcImage.cols * 0.8, srcImage.rows * 0.8), (0, 0), (0, 0), 3);
	resize(srcImage, dstImage2, Size(srcImage.cols * 1.2, srcImage.rows * 1.2), (0, 0), (0, 0), 3);

	// 进行向上取样操作
	pyrUp(srcImage, dstImage3, Size(srcImage.cols * 2, srcImage.rows * 2));

	//进行向下取样操作
	pyrDown(srcImage, dstImage4, Size(srcImage.cols * 0.5, srcImage.rows * 0.5));

	//显示效果图  
	imshow("【效果图1】", dstImage1);
	imshow("【效果图2】", dstImage2);
	imshow("【效果图3】", dstImage3);
	imshow("【效果图4】", dstImage4);

	waitKey(0);
	return 0;
}


