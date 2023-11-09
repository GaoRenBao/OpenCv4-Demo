# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：cornerHarris角点检测综合示例
# 博客：http://www.bilibili996.com/Course?id=3533622000223
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def on_CornerHarris(x):
    global g_srcImage1, g_grayImage

    # 获取滑动条的值
    thresh = cv2.getTrackbarPos('value', 'WINDOW_NAME1')

    # 初始化
    # 置零当前需要显示的两幅图，即清除上一次调用此函数时他们的值
    g_srcImage1 = np.copy(g_srcImage)

    # 进行角点检测
    dstImage = cv2.cornerHarris(g_grayImage, 5, 3, 0.04)

    # 归一化与转换
    # 下面这两种新建画布的方式都可以，新建一个32位浮点型单通道
    normImage = np.empty((g_srcImage.shape[0], g_srcImage.shape[1], 1), dtype=np.float32)
    # normImage = np.ones((g_srcImage.shape[0], g_srcImage.shape[1], 1),np.float32)

    cv2.normalize(dstImage, normImage, 0, 255, cv2.NORM_MINMAX)

    # 将归一化后的图线性变换成8位无符号整型
    scaledImage = cv2.convertScaleAbs(normImage)

    # 进行绘制
    # 将检测到的，且符合阈值条件的角点绘制出来
    imgH = normImage.shape[0]
    imgW = normImage.shape[1]
    for j in range(imgW):
        for i in range(imgH):
            if normImage[i][j].max() > (thresh + 80):
                cv2.circle(g_srcImage1, (j, i), 5, (10, 10, 255), 2, 8, 0)
                cv2.circle(scaledImage, (j, i), 5, (0, 10, 255), 2, 8, 0)

    # 【5】显示最终效果
    cv2.imshow("WINDOW_NAME1", g_srcImage1)
    cv2.imshow("WINDOW_NAME2", scaledImage)


# 【1】载入原始图并进行克隆保存
g_srcImage = cv2.imread("../images/home5.jpg")

cv2.imshow("WINDOW_NAME1", g_srcImage)
g_srcImage1 = np.copy(g_srcImage)

# 【2】存留一张灰度图
g_grayImage = cv2.cvtColor(g_srcImage1, cv2.COLOR_BGR2GRAY)

# 【3】创建窗口和滚动条
cv2.namedWindow('WINDOW_NAME1')
cv2.createTrackbar('value', 'WINDOW_NAME1', 30, 175, on_CornerHarris)
# 【4】调用一次回调函数，进行初始化
cv2.setTrackbarPos('value', 'WINDOW_NAME1', 30)

cv2.waitKey(0)
cv2.destroyAllWindows()
