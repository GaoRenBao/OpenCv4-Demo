#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;

int main()
{
	// 载入原图
	Mat srcImage = imread("1.jpg");

	// 创建窗口
	namedWindow("【原图】");
	namedWindow("腐蚀【效果图】");
	namedWindow("膨胀【效果图】");

	// 显示原图
	imshow("【原图】", srcImage);

	// 定义核大小
	Mat element = getStructuringElement(MORPH_RECT, Size(15, 15));

	// 进行腐蚀操作 
	Mat out1;
	erode(srcImage, out1, element);

	// 进行膨胀操作 
	Mat out2;
	dilate(srcImage, out2, element);

	// 显示效果图
	imshow("腐蚀【效果图】", out1);
	imshow("膨胀【效果图】", out2);

	waitKey(0);
	return 0;
}


