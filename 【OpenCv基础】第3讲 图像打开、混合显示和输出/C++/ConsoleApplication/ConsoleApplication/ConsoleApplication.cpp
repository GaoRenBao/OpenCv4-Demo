#include<opencv2/core/core.hpp>
#include<opencv2/highgui/highgui.hpp>
using namespace cv;

// 描述：初级图像混合

int main()
{
	//载入图片
	Mat image1 = imread("a.jpg");
	Mat image2 = imread("b.jpg");

	//载入后先显示
	namedWindow("image1");
	imshow("image1", image1);
	namedWindow("image2");
	imshow("image2", image2);

	//定义一个Mat类型，用于存放，图像的ROI
	Mat imageROI;
	//方法一
	imageROI = image1(Rect(800, 350, image2.cols, image2.rows));
	//方法二
	//imageROI=image1(Range(350,350+image2.rows), Range(800,800+image2.cols));

	//将image2加到image1上
	addWeighted(imageROI, 0.5, image2, 0.3, 0, imageROI);

	//显示结果
	namedWindow("合并");
	imshow("合并", image1);

	//输出一张jpg图片到工程目录下
	imwrite("out.jpg", image1);
	waitKey(0);
	return 0;
}


