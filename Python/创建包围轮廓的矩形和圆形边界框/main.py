# OpenCv版本：opencv-python 4.6.0.66
# 内容：创建包围轮廓的矩形和圆形边界框
# 博客：http://www.bilibili996.com/Course?id=5756630000206
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random

g_nThresh = 100


def on_ThreshChange(x):
    global g_nThresh, g_grayImage, drawing

    # 获取滑动条的值
    g_nThresh = cv2.getTrackbarPos('g_nThresh', 'drawing')

    # 二值化
    ret, threshold_output = cv2.threshold(g_grayImage, g_nThresh, 255, cv2.THRESH_BINARY)

    # 寻找图像轮廓
    contours, hierarchy = cv2.findContours(threshold_output, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

    # 初始化一个空白图像
    drawing = np.zeros((g_grayImage.shape[0], g_grayImage.shape[1], 3), np.uint8)

    # 一个循环，遍历所有部分，进行本程序最核心的操作
    for i in range(len(contours)):
        # 随机设置颜色
        color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        # 用指定精度逼近多边形曲线
        contours_poly = cv2.approxPolyDP(contours[i], 3, True)
        # 绘制轮廓
        cv2.polylines(drawing, [contours_poly], True, color, 1, 8)

        # 计算点集的最外面（up-right）矩形边界
        x, y, w, h = cv2.boundingRect(contours_poly)
        # 绘制矩形
        cv2.rectangle(drawing, (x, y), (x + w, y + h), color, 1, 8, 0)

        # 对给定的 2D 点集，寻找最小面积的包围圆形
        (x, y), radius = cv2.minEnclosingCircle(contours_poly)
        center = (int(x), int(y))
        # 绘制圆
        cv2.circle(drawing, center, int(radius), color, 1, 8, 0)


# 载入3通道的原图像
g_srcImage = cv2.imread("../images/home2.jpg")
# 得到原图的灰度图像并进行平滑
g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)
# 均值滤波
g_grayImage = cv2.blur(g_grayImage, (3, 3))
cv2.imshow("g_srcImage", g_srcImage)

# 初始化一个空白图像
drawing = np.zeros((g_grayImage.shape[0], g_grayImage.shape[1], 3), np.uint8)

# 创建窗口
cv2.namedWindow('drawing')
cv2.createTrackbar('g_nThresh', 'drawing', 0, 255, on_ThreshChange)
cv2.setTrackbarPos('g_nThresh', 'drawing', 100)

while True:
    cv2.imshow("drawing", drawing)
    if cv2.waitKey(10) == ord('q'):
        break
cv2.destroyAllWindows()
