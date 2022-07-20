#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;

int main()
{
	// 载入原图
	Mat image = imread("1.jpg");

	// 创建窗口
	namedWindow("【原图】");
	namedWindow("方框滤波【效果图】");
	namedWindow("均值滤波【效果图】");
	namedWindow("高斯滤波【效果图】");

	// 显示原图
	imshow("【原图】", image);

	// 进行方框滤波操作
	Mat out1;
	boxFilter(image, out1, -1, Size(5, 5));

	// 进行均值滤波操作
	Mat out2;
	blur(image, out2, Size(7, 7));

	//进行高斯滤波操作
	// sigmaX，表示高斯核函数在X方向的的标准偏差。
	// sigmaY，表示高斯核函数在Y方向的的标准偏差。
	// 若sigmaY为零，就将它设为sigmaX。
	// 如果sigmaX和sigmaY都是0，那么就由ksize.width和ksize.height计算出来。
	Mat out3;
	GaussianBlur(image, out3, Size(5, 5), 0, 0);

	//显示效果图
	imshow("方框滤波【效果图】", out1);
	imshow("均值滤波【效果图】", out2);
	imshow("高斯滤波【效果图】", out3);

	waitKey(0);
	return 0;
}


