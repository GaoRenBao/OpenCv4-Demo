/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：删除图像中的水印
博客：http://www.bilibili996.com/Course?id=b608bdd358e041478931886b8b08058c
作者：高仁宝
时间：2023.11

来源：https://stackoverflow.com/questions/32125281/removing-watermark-out-of-an-image-using-opencv
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat im = imread("../images/YZeOg.jpg");

    // 原图有点大。。。
    resize(im, im, Size(im.cols * 0.5, im.rows * 0.5), (0, 0), (0, 0), 3);
    imshow("im", im);

    Mat gr, bg, bw, dark;
    cvtColor(im, gr, COLOR_BGR2GRAY);

    // approximate the background
    bg = gr.clone();
    for (int r = 1; r < 5; r++)
    {
        Mat kernel2 = getStructuringElement(MORPH_ELLIPSE, Size(2 * r + 1, 2 * r + 1));
        morphologyEx(bg, bg, MORPH_CLOSE, kernel2);
        morphologyEx(bg, bg, MORPH_OPEN, kernel2);
    }

    // difference = background - initial
    Mat dif = bg - gr;
    // threshold the difference image so we get dark letters
    threshold(dif, bw, 0, 255, THRESH_BINARY_INV | THRESH_OTSU);
    // threshold the background image so we get dark region
    threshold(bg, dark, 0, 255, THRESH_BINARY_INV | THRESH_OTSU);

    // extract pixels in the dark region
    vector<unsigned char> darkpix(countNonZero(dark));
    int index = 0;
    for (int r = 0; r < dark.rows; r++)
    {
        for (int c = 0; c < dark.cols; c++)
        {
            if (dark.at<unsigned char>(r, c))
            {
                darkpix[index++] = gr.at<unsigned char>(r, c);
            }
        }
    }
    // threshold the dark region so we get the darker pixels inside it
    threshold(darkpix, darkpix, 0, 255, THRESH_BINARY | THRESH_OTSU);

    // paste the extracted darker pixels
    index = 0;
    for (int r = 0; r < dark.rows; r++)
    {
        for (int c = 0; c < dark.cols; c++)
        {
            if (dark.at<unsigned char>(r, c))
            {
                bw.at<unsigned char>(r, c) = darkpix[index++];
            }
        }
    }

    imshow("BW", bw);
    waitKey(0);
}