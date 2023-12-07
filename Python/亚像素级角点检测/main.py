# OpenCv版本：opencv-python 4.6.0.66
# 内容：亚像素级角点检测
# 博客：http://www.bilibili996.com/Course?id=0659290000229
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random


def on_GoodFeaturesToTrack(x):
    global g_srcImage, g_grayImage

    # 获取滑动条的值
    g_maxCornerNumber = cv2.getTrackbarPos('value', 'WINDOW_NAME')

    # 复制源图像到一个临时变量中，作为感兴趣区域
    copy = np.copy(g_srcImage)

    if g_maxCornerNumber <= 1:
        g_maxCornerNumber = 1

    # Shi-Tomasi算法（goodFeaturesToTrack函数）的参数准备
    feature_params = dict(maxCorners=g_maxCornerNumber,  # 角点的最大数量
                          qualityLevel=0.01,  # 角点检测可接受的最小特征值
                          minDistance=10,  # 角点之间的最小距离
                          blockSize=3)  # 计算导数自相关矩阵时指定的邻域范围

    corners = cv2.goodFeaturesToTrack(g_grayImage, mask=None, **feature_params)

    # 输出文字信息
    print("此次检测到的角点数量为：", corners.size)

    for i in range(len(corners)):
        # 以随机的颜色绘制出角点
        color = (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        center = (int(corners[i][0][0]), int(corners[i][0][1]))
        cv2.circle(copy, center, 4, color, -1, 8, 0)

    cv2.imshow("WINDOW_NAME", copy)

    # 亚像素角点检测的参数设置
    criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 40, 0.001)
    corners = cv2.cornerSubPix(g_grayImage, np.float32(corners), (5, 5), (-1, -1), criteria)

    # 输出角点信息
    for i in range(len(corners)):
        center = (int(corners[i][0][0]), int(corners[i][0][1]))
        print("精确角点坐标：", center)


# 【1】载入源图像并将其转换为灰度图
g_srcImage = cv2.imread("../images/home7.jpg")
g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)

# 创建窗口
cv2.namedWindow('WINDOW_NAME')

cv2.createTrackbar('value', 'WINDOW_NAME', 0, 500, on_GoodFeaturesToTrack)
cv2.setTrackbarPos('value', 'WINDOW_NAME', 0)

# 等待用户按键
cv2.imshow("WINDOW_NAME", g_srcImage)
cv2.waitKey(0)
cv2.destroyAllWindows()

