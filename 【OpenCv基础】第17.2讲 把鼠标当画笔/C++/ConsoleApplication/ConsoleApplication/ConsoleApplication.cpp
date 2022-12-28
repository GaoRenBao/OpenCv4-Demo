#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 当鼠标按下时变为 True
bool drawing = false;
// 矩形和圆形模式切换
bool mode = false;
// 如果 mode 为 true 绘制矩形。按下'm' 变成绘制曲线。 mode=True
int ix = -1, iy = -1;

void on_MouseHandle(int event, int x, int y, int flags, void* param)
{
    Mat& image = *(cv::Mat*)param;
    if (event == EVENT_LBUTTONDOWN)
    {
        cout << to_string(x) << "," << to_string(y) << endl;
    }
    if (event == EVENT_RBUTTONDOWN)
    {
        Vec3b vec = image.at<Vec3b>(y, x);
        uchar blue = vec[0];
        uchar green = vec[1];
        uchar red = vec[2];
        string strRGB = to_string(red) + "," + to_string(green) + "," + to_string(blue);
        cout << strRGB << endl;
        putText(image, strRGB, Point(x, y), FONT_HERSHEY_SIMPLEX, 1, Scalar(255, 255, 255), 2, LINE_AA);
        imshow("original", image);
    }
}

void main()
{
    Mat img = imread("messi5.jpg");
    imshow("original", img);

    namedWindow("original");
    setMouseCallback("original", on_MouseHandle, (void*)&img);
    waitKey(0);
}
