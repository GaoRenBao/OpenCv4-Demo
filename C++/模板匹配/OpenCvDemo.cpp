/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：模板匹配
博客：http://www.bilibili996.com/Course?id=5363388000218
作者：高仁宝
时间：2023.11
*/

#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#include <sstream>
#include <string>
#include <iomanip> 

using namespace cv;
using namespace std;

#define WINDOW_NAME1 "【原始图片】"        //为窗口标题定义的宏 
#define WINDOW_NAME2 "【匹配窗口】"        //为窗口标题定义的宏 

Mat g_srcImage; Mat g_templateImage; Mat g_resultImage;
int g_nMatchMethod;
int g_nMaxTrackbarNum = 5;

static void ShowHelpText()
{
	printf("\n\n 当前使用的OpenCV版本为：" CV_VERSION);
	printf("\n\n  ----------------------------------------------------------------------------\n");
	//输出一些帮助信息
	printf("\t欢迎来到【模板匹配】示例程序~\n");
	printf("\n\n\t请调整滑动条观察图像效果\n\n");
	printf("\n\t滑动条对应的方法数值说明: \n\n"
		"\t\t方法【0】- 平方差匹配法(SQDIFF)\n"
		"\t\t方法【1】- 归一化平方差匹配法(SQDIFF NORMED)\n"
		"\t\t方法【2】- 相关匹配法(TM CCORR)\n"
		"\t\t方法【3】- 归一化相关匹配法(TM CCORR NORMED)\n"
		"\t\t方法【4】- 相关系数匹配法(TM COEFF)\n"
		"\t\t方法【5】- 归一化相关系数匹配法(TM COEFF NORMED)\n");
}

void on_Matching(int, void*)
{
	//【1】给局部变量初始化
	Mat srcImage;
	g_srcImage.copyTo(srcImage);

	//【2】初始化用于结果输出的矩阵
	int resultImage_rows = g_srcImage.rows - g_templateImage.rows + 1;
	int resultImage_cols = g_srcImage.cols - g_templateImage.cols + 1;
	g_resultImage.create(resultImage_rows, resultImage_cols, CV_32FC1);

	//【3】进行匹配和标准化
	// 当模板匹配采用CV_TM_SQDIFF（g_nMatchMethod = 0）模式时，minValue值越小，说明匹配度越高
	matchTemplate(g_srcImage, g_templateImage, g_resultImage, g_nMatchMethod);
	normalize(g_resultImage, g_resultImage, 0, 1, NORM_MINMAX, -1, Mat());

	//【4】通过函数 minMaxLoc 定位最匹配的位置
	double minValue;
	double maxValue;
	Point minLocation;
	Point maxLocation;
	Point matchLocation;
	minMaxLoc(g_resultImage, &minValue, &maxValue, &minLocation, &maxLocation, Mat());

	//【5】对于方法 SQDIFF 和 SQDIFF_NORMED, 越小的数值有着更高的匹配结果. 而其余的方法, 数值越大匹配效果越好
	double matched = 0; // 匹配度
	if (g_nMatchMethod == TM_SQDIFF || g_nMatchMethod == TM_SQDIFF_NORMED)
	{
		matchLocation = minLocation;
		matched = minValue;
	}
	else
	{
		matchLocation = maxLocation;
		matched = maxValue;
	}

	std::string txts[] = { "SqDiff","SqDiffNormed","CCorr","CCorrNormed","CCoeff","CCoeffNormed" };

	std::ostringstream oss;
	// digits10 返回 double 类型能表示的有效数字的十进制位数，加 1 是为了安全起见（尽管通常不需要）
	oss << std::fixed << std::setprecision(std::numeric_limits<double>::digits10 + 1);
	oss << matched;
	putText(srcImage, txts[g_nMatchMethod], Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, LINE_AA);
	putText(srcImage, oss.str(), Point(10, 65), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, LINE_AA);

	//【6】绘制出矩形，并显示最终结果
	rectangle(srcImage, matchLocation, Point(matchLocation.x + g_templateImage.cols, matchLocation.y + g_templateImage.rows), Scalar(0, 0, 255), 2, 8, 0);
	rectangle(g_resultImage, matchLocation, Point(matchLocation.x + g_templateImage.cols, matchLocation.y + g_templateImage.rows), Scalar(0, 0, 255), 2, 8, 0);

	imshow(WINDOW_NAME1, srcImage);
	imshow(WINDOW_NAME2, g_resultImage);
}

int main()
{
	//【0】改变console字体颜色
	system("color 1F");

	//【0】显示帮助文字
	ShowHelpText();

	//【1】载入原图像和模板块
	g_srcImage = imread("../images/girl6.jpg", 1);
	g_templateImage = imread("../images/girl6_roi.jpg", 1);

	//【2】创建窗口
	namedWindow(WINDOW_NAME1, WINDOW_AUTOSIZE);
	namedWindow(WINDOW_NAME2, WINDOW_AUTOSIZE);

	//【3】创建滑动条并进行一次初始化
	createTrackbar("方法", WINDOW_NAME1, &g_nMatchMethod, g_nMaxTrackbarNum, on_Matching);
	on_Matching(0, 0);

	waitKey(0);
	return 0;

}