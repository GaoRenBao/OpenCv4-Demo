# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：查找和绘制图片轮廓矩
# 博客：http://www.bilibili996.com/Course?id=4739103000210
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

    g_cannyMat_output = cv2.Canny(g_grayImage, g_nThresh, g_nThresh * 2, 3)

    # 找出轮廓
    g_vContours, g_vHierarchy = cv2.findContours(g_cannyMat_output, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

    # 计算矩
    mu = []
    for i in range(len(g_vContours)):
        mu.append(cv2.moments(g_vContours[i]))

    # 计算中心矩
    mc = []
    for i in range(len(g_vContours)):
        if mu[i]['m00'] != 0:
            mc.append((int(mu[i]['m10'] / mu[i]['m00']), int(mu[i]['m01'] / mu[i]['m00'])))
        else:
            mc.append((0, 0))

    # 绘制轮廓
    drawing = np.zeros((g_cannyMat_output.shape[0], g_cannyMat_output.shape[1], 3), np.uint8)
    for i in range(len(g_vContours)):
        # 随机设置颜色
        color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        # 绘制外层和内层轮廓
        cv2.drawContours(drawing, g_vContours, i, color, 2, 8, g_vHierarchy, 0)
        # 绘制圆
        cv2.circle(drawing, mc[i], 4, color, 1, 8, 0)

    print("输出内容: 面积和轮廓长度")
    for i in range(len(g_vContours)):
        # 随机设置颜色
        color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        print("通过m00计算出轮廓" + str(i) + "的面积: (M_00) = " + str(mu[i]["m00"]))
        print("OpenCV函数计算出的面积=" + str(cv2.contourArea(g_vContours[i])))
        print("长度=" + str(cv2.arcLength(g_vContours[i], True)) + "\n")

        cv2.drawContours(drawing, g_vContours, i, color, 2, 8, g_vHierarchy, 0)
        cv2.circle(drawing, mc[i], 4, color, 1, 8, 0)


# 载入3通道的原图像
g_srcImage = cv2.imread("../images/lake2.jpg")
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



