/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：利用imwrite生成透明png图像
博客：http://www.bilibili996.com/Course?id=4324175000003
作者：高仁宝
时间：2023.11
*/

#include <vector>
#include <stdio.h>
#include <opencv2/opencv.hpp>

using namespace cv;
using namespace std;

void createAlphaMat(Mat& mat)
{
    for (int i = 0; i < mat.rows; ++i) {
        for (int j = 0; j < mat.cols; ++j) {
            Vec4b& rgba = mat.at<Vec4b>(i, j);
            rgba[0] = UCHAR_MAX;
            rgba[1] = saturate_cast<uchar>((float(mat.cols - j)) / ((float)mat.cols) * UCHAR_MAX);
            rgba[2] = saturate_cast<uchar>((float(mat.rows - i)) / ((float)mat.rows) * UCHAR_MAX);
            rgba[3] = saturate_cast<uchar>(0.5 * (rgba[1] + rgba[2]));
        }
    }
}

int main()
{
    //创建带alpha通道的Mat
    Mat mat(480, 640, CV_8UC4);
    createAlphaMat(mat);

    imshow("透明Alpha值图.png", mat);
    waitKey(0);

    vector<int>compression_params;
    compression_params.push_back(IMWRITE_PNG_COMPRESSION);
    compression_params.push_back(9);

    try {
        imwrite("透明Alpha值图.png", mat, compression_params);
    }
    catch (runtime_error& ex) {
        fprintf(stderr, "图像转换成PNG格式发生错误：%s\n", ex.what());
        return 1;
    }

    fprintf(stdout, "PNG图片文件的alpha数据保存完毕~\n");
    return 0;
}