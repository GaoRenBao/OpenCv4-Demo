#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 双摄像头调用
void demo1()
{
    VideoCapture cap0;
    VideoCapture cap1;
    cap0.open(0);
    cap1.open(1);
    if (!cap0.isOpened())
    {
        cout << "Error0" << endl;
        return;
    }
    if (!cap1.isOpened())
    {
        cout << "Error1" << endl;
        return;
    }

    bool ret0 = cap0.set(3, 320);
    bool ret1 = cap0.set(4, 240);
    bool ret2 = cap1.set(3, 320);
    bool ret3 = cap1.set(4, 240);

    while (cap0.isOpened() && cap1.isOpened())
    {
        Mat frame0, frame1;
        cap0 >> frame0;
        cap1 >> frame1;
        if (!frame0.empty())
        {
            imshow("frame0", frame0);
            setWindowTitle("frame0", "On Top");
        }
        if (!frame1.empty())
        {
            imshow("frame1", frame1);
            moveWindow("frame1", frame0.cols, 0);
            //moveWindow("frame1", 320, 40);
        }

        if (waitKey(2) == 27)// ESC按钮
            break;
    }

    // When everything done, release the capture
    cap0.release();
    cap1.release();
    destroyAllWindows();
}

// 均方误差（MSE）
double mse(Mat imageA, Mat imageB)
{
    // 在使用OpenCV时可以通过矩阵操作来避免for循环嵌套计算。
    // 需要注意的是乘除操作一般要注意将图像本身的uint8转换成float后再做，否则精度误差可能会导致较大偏差。
    cv::Mat M1 = imageA.clone();
    cv::Mat M2 = imageB.clone();
    cv::Mat Diff;
    // 提前转换为32F精度
    M1.convertTo(M1, CV_32F);
    M2.convertTo(M2, CV_32F);
    Diff.convertTo(Diff, CV_32F);
    cv::absdiff(M1, M2, Diff); //  Diff = | M1 - M2 |
    Diff = Diff.mul(Diff);     // | M1 - M2 |.^2
    cv::Scalar S = cv::sum(Diff);  //分别计算每个通道的元素之和

    double sse;   // square error
    if (Diff.channels() == 3)
        sse = S.val[0] + S.val[1] + S.val[2];  // sum of all channels
    else
        sse = S.val[0];
    int nTotalElement = M2.channels() * M2.total();
    double mse = (sse / (double)nTotalElement);  //
    return mse;
}

// 结构相似度指数（SSIM），是一种衡量两幅图像相似度的指标，
// 也是一种全参考的图像质量评价指标，它分别从亮度、对比度、结构三方面度量图像相似性
// 参考： https://www.freesion.com/article/50591020225/
// C1，C2和C3为常数，是为了避免分母为0而维持稳定。
// L = 255（ 是像素值的动态范围，一般都取为255）。
// K1 = 0.01, K2 = 0.03。结构相似性的范围为 - 1 到 1 。
// 当两张图像一模一样时，SSIM的值等于1。
double ssim(Mat& i1, Mat& i2) {
    const double C1 = 6.5025, C2 = 58.5225;
    int d = CV_32F;
    Mat I1, I2;
    i1.convertTo(I1, d);
    i2.convertTo(I2, d);
    Mat I1_2 = I1.mul(I1);
    Mat I2_2 = I2.mul(I2);
    Mat I1_I2 = I1.mul(I2);
    Mat mu1, mu2;
    GaussianBlur(I1, mu1, Size(11, 11), 1.5);
    GaussianBlur(I2, mu2, Size(11, 11), 1.5);
    Mat mu1_2 = mu1.mul(mu1);
    Mat mu2_2 = mu2.mul(mu2);
    Mat mu1_mu2 = mu1.mul(mu2);
    Mat sigma1_2, sigam2_2, sigam12;
    GaussianBlur(I1_2, sigma1_2, Size(11, 11), 1.5);
    sigma1_2 -= mu1_2;

    GaussianBlur(I2_2, sigam2_2, Size(11, 11), 1.5);
    sigam2_2 -= mu2_2;

    GaussianBlur(I1_I2, sigam12, Size(11, 11), 1.5);
    sigam12 -= mu1_mu2;
    Mat t1, t2, t3;
    t1 = 2 * mu1_mu2 + C1;
    t2 = 2 * sigam12 + C2;
    t3 = t1.mul(t2);

    t1 = mu1_2 + mu2_2 + C1;
    t2 = sigma1_2 + sigam2_2 + C2;
    t1 = t1.mul(t2);

    Mat ssim_map;
    divide(t3, t1, ssim_map);
    Scalar mssim = mean(ssim_map);

    double ssim = (mssim.val[0] + mssim.val[1] + mssim.val[2]) / 3;
    return ssim;
}

// 峰值信噪比，一种评价图像的客观标准，用来评估图像的保真性。
// 峰值信噪比经常用作图像压缩等领域中信号重建质量的测量方法，
// 它常简单地通过均方差（MSE）进行定义，使用两个m×n单色图像I和K。PSNR的单位为分贝
// PSNR值越大，就代表失真越少，图像压缩中典型的峰值信噪比值在 30 到 40dB 之间，小于30dB时考虑图像无法忍受。
double psnr(Mat& I1, Mat& I2) { //注意，当两幅图像一样时这个函数计算出来的psnr为0 
    Mat s1;
    absdiff(I1, I2, s1);
    s1.convertTo(s1, CV_32F);//转换为32位的float类型，8位不能计算平方  
    s1 = s1.mul(s1);
    Scalar s = sum(s1);  //计算每个通道的和  
    double sse = s.val[0] + s.val[1] + s.val[2];
    if (sse <= 1e-10) // for small values return zero  
        return 0;
    else
    {
        double mse = sse / (double)(I1.channels() * I1.total()); //  sse/(w*h*3)  
        double psnr = 10.0 * log10((255 * 255) / mse);
        return psnr;
    }
}

// 图像相似度对比
void demo2()
{
    VideoCapture cap;
    cap.open(0);

    if (!cap.isOpened())
    {
        cout << "Error0" << endl;
        return;
    }

    Mat frame;
    cap >> frame;

    Mat temp;
    cvtColor(frame, temp, COLOR_BGR2GRAY);
    imshow("temp", temp);

    while (cap.isOpened())
    {
        cap >> frame;
        if (frame.empty())
        {
            continue;
        }

        Mat gray;
        cvtColor(frame, gray, COLOR_BGR2GRAY);
        imshow("gray", gray);
        
        cout << "mse:" << mse(temp, gray) << "\tssim:" << ssim(temp, gray) << "\tpsnr:" << psnr(temp, gray) << endl;
        waitKey(200);
    }

    cap.release();
    destroyAllWindows();
}

void main()
{
    demo2();
}
