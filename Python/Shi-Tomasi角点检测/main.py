# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：Shi-Tomasi角点检测
# 博客：http://www.bilibili996.com/Course?id=0812352000224
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random


def on_GoodFeaturesToTrack(x):
    global g_maxCornerNumber

    # 获取滑动条的值
    g_maxCornerNumber = cv2.getTrackbarPos('max', 'WINDOW_NAME')

    # 对变量小于等于1时的处理
    if g_maxCornerNumber <= 1:
        g_maxCornerNumber = 1

    copy = np.copy(g_srcImage)  # 复制源图像到一个临时变量中，作为感兴趣区域

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


# 载入原图像
g_srcImage = cv2.imread("../images/home6.jpg")

# 存留一张灰度图
g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)

# 创建窗口
cv2.namedWindow('WINDOW_NAME')
g_maxCornerNumber = 33
cv2.createTrackbar('max', 'WINDOW_NAME', g_maxCornerNumber, 500, on_GoodFeaturesToTrack)
cv2.setTrackbarPos('max', 'WINDOW_NAME', 0)

# 等待用户按键
cv2.waitKey(0)
cv2.destroyAllWindows()
