/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：使用Haar分类器之面部检测
博客：http://www.bilibili996.com/Course?id=be9bc00c296a4fe59e0c86474bbf9f43
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <string>

using namespace cv;
using namespace std;

int main()
{
    //运行之前，检查cascade文件路径是否在你的电脑上
    CascadeClassifier face_cascade = CascadeClassifier("../images/haarcascade/haarcascade_frontalface_default.xml");
    CascadeClassifier eye_cascade = CascadeClassifier("../images/haarcascade/haarcascade_eye.xml");

    //Mat img = imread("../images/kongjie_hezhao.jpg");
    Mat img = imread("../images/airline-stewardess-bikini.jpg");
    Mat gray;
    cvtColor(img, gray, COLOR_BGR2GRAY);
    imshow("gray", gray);

    //Detects objects of different sizes in the input image.
    //The detected objects are returned as a list of rectangles.
    //cv2.CascadeClassifier.detectMultiScale(image, scaleFactor, minNeighbors, flags, minSize, maxSize)
    //scaleFactor – Parameter specifying how much the image size is reduced at each image
    //scale.
    //minNeighbors – Parameter specifying how many neighbors each candidate rectangle should
    //have to retain it.
    //minSize – Minimum possible object size. Objects smaller than that are ignored.
    //maxSize – Maximum possible object size. Objects larger than that are ignored.
    //faces = face_cascade.detectMultiScale(gray, 1.3, 5)

    vector<Rect> faces;
    face_cascade.detectMultiScale(gray, faces, 1.1, 5, 2, Size(30, 30)); //改进
    cout << "Detected：" << faces.size() << endl;
    for (int i = 0; i < faces.size(); i++)
    {
        rectangle(img,
            Point(faces[i].x, faces[i].y),
            Point(faces[i].x + faces[i].width, faces[i].y + faces[i].height),
            Scalar(255, 0, 0), 2);

        Mat roi_gray = gray(faces[i]);
        Mat roi_color = img(faces[i]);

        vector<Rect> eyes;
        eye_cascade.detectMultiScale(roi_gray, eyes);
        for (int j = 0; j < eyes.size(); j++)
        {
            rectangle(roi_color,
                Point(eyes[j].x, eyes[j].y),
                Point(eyes[j].x + eyes[j].width, eyes[j].y + eyes[j].height),
                Scalar(0, 255, 0), 2);
        }
    }

    cv::imshow("img", img);
    cv::waitKey(0);
    cv::destroyAllWindows();
}