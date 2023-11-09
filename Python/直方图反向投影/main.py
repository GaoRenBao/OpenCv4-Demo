# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：直方图反向投影
# 博客：http://www.bilibili996.com/Course?id=3519953000217
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def on_BinChange(x):
    global g_hueImage

    # 获取滑动条的值
    g_bins = cv2.getTrackbarPos('g_bins', 'backproj')

    # 计算直方图并归一化
    histSize = [max(g_bins, 2)]
    hist = cv2.calcHist([g_hueImage], [0], None, histSize, [0, 180])
    cv2.normalize(hist, hist, 0, 255, cv2.NORM_MINMAX)

    # 计算反向投影
    backproj = cv2.calcBackProject([g_hueImage], [0], hist, [0, 180], 1)

    # 显示反向投影
    cv2.imshow("backproj", backproj)

    # 绘制直方图的参数准备
    w = 400
    h = 400
    bin_w = int(w / histSize[0])
    histImg = np.zeros((w, h, 3), np.uint8)

    # 绘制直方图
    for i in range(g_bins):
        cv2.rectangle(histImg, (i * bin_w, h), (int((i + 1) * bin_w), int(h - hist[i][0] * h / 255.0)),
                      (100, 123, 255), -1)

    # 显示直方图窗口
    cv2.imshow("histImg", histImg)


# 读取源图像，并转换到 HSV 空间
g_srcImage = cv2.imread("../images/hand.jpg")
g_hsvImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2HSV)

# 【2】分离 Hue 色调通道
g_hueImage = np.zeros((g_hsvImage.shape[0], g_hsvImage.shape[1], 3), np.uint8)
ch = [0, 0]
cv2.mixChannels([g_hsvImage], [g_hueImage], ch)

# 创建 Trackbar 来输入bin的数目
cv2.namedWindow('backproj')
cv2.createTrackbar('g_bins', 'backproj', 0, 180, on_BinChange)
cv2.setTrackbarPos('g_bins', 'backproj', 30)

# 等待用户按键
cv2.waitKey(0)
cv2.destroyAllWindows()
