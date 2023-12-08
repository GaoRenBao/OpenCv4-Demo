/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：SVM手写数字识别
博客：http://www.bilibili996.com/Course?id=17494cb1802e403ea25d1096a5659478
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/ml/ml.hpp>
#include <iostream>

using namespace cv;
using namespace cv::ml;
using namespace std;

// 切图
vector<vector<Mat>> RoiImages(string path)
{
    Mat img = cv::imread(path);
    Mat gray;
    cv::cvtColor(img, gray, COLOR_BGR2GRAY);

    vector<vector<Mat>> cells;
    int w = gray.cols / 100; // 每行100个字符
    int h = gray.rows / 50; // 共50行
    for (int i = 0; i < 10; i++) // 数字0~9
    {
        vector<Mat> imgs;
        for (int j = 0; j < 5; j++) // 每组数字有5行
        {
            for (int n = 0; n < 100; n++) // 每行100个字符
            {
                Rect rect(n * w, h * ((i * 5) + j), w, h);
                Mat roi = gray(rect);
                imgs.push_back(roi);
                //cv::imshow("roi", roi);
                //cv::waitKey(1);
            }
        }
        cells.push_back(imgs);
    }
    return cells;
}

int main()
{
    // 切图
    vector<vector<Mat>> cells = RoiImages("../images/digits.png");

    // 训练数据数量，前400张图片用来训练
    int train_sample_count = 400 * 10;
    // 声明训练数据集合 mat，图像尺寸：20*20
    Mat trainData(train_sample_count, 20 * 20, CV_32FC1);
    // 声明训练数据标签 mat
    Mat trainLabel(train_sample_count, 1, CV_32SC1);

    // 组织训练数据，循环训练文件夹内所有图片。
    int trainNum = 0;
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 400; j++)
        {
            Mat temp = cells[i][j];
            // 转换CV_32FC1，因为下面训练函数需要这个格式
            temp.convertTo(temp, CV_32FC1);
            // 写入到训练数据集合的mat内，注意reshape的用法。
            /*
            reshape有两个参数：
            其中，参数：cn为新的通道数，如果cn = 0，表示通道数不会改变。
            参数rows为新的行数，如果rows = 0，表示行数不会改变。
            注意：新的行* 列必须与原来的行*列相等。就是说，如果原来是5行3列，新的行和列可以是1行15列，3行5列，5行3列，15行1列。
                  设置行数后，列数会自动调整。比如此处 调整为 1行784列。
            */
            temp.reshape(0, 1).copyTo(trainData.row(trainNum));
            // 写入到训练标签集合的mat内
            trainLabel.at<int>(trainNum) = i;
            trainNum++;
        }
    }


    // 实例化对象
    Ptr<SVM> svm = SVM::create();
    // 设置核函数
    svm->setKernel(SVM::LINEAR);  //核函数
    // 设置训练类型
    svm->setType(SVM::C_SVC);
    svm->setC(2.67);
    svm->setGamma(5.383);
    // 输入样本，进行训练
    svm->train(TrainData::create(trainData, ROW_SAMPLE, trainLabel));
    // 存储训练结果
    svm->save("svm_data.dat");

    /**************  Now testing ***************/
    // 测试数据数量，后100张图片用来测试
    int test_sample_count = 100 * 10;
    // 声明测试数据集合 mat
    Mat testData(test_sample_count, 20 * 20, CV_32FC1);
    // 声明测试数据标签 mat
    Mat testLabel(test_sample_count, 1, CV_32FC1);
    // 组织测试数据
    int testNum = 0;
    for (int i = 0; i < 10; i++)
    {
        for (int j = 400; j < cells[i].size(); j++)
        {
            Mat temp = cells[i][j];
            temp.convertTo(temp, CV_32FC1);
            temp.reshape(0, 1).copyTo(testData.row(testNum));
            testLabel.at<float>(testNum) = i;
            testNum++;
        }
    }

    Mat result;
    svm->predict(testData, result);
    int t = 0;
    int f = 0;
    for (int i = 0; i < test_sample_count; i++)
    {
        int predict = (int)result.at<float>(i);
        int actual = (int)testLabel.at<float>(i);
        if (predict == actual)
        {
            cout << "正确：" << predict << "-" << actual << endl;
            t++;
        }
        else
        {
            cout << "错误------：" << predict << "-" << actual << endl;
            f++;
        }
    }

    double accuracy = (t * 1.0) / (t + f);
    cout << "准确率：" << accuracy << endl; // 准确率：0.908
}