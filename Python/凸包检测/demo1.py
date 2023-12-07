# OpenCv版本：opencv-python 4.6.0.66
# 内容：凸包检测
# 博客：http://www.bilibili996.com/Course?id=3741165000202
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def zh_ch(string):
    return string.encode("gbk").decode(errors="ignore")


srcImage = cv2.imread("../images/kele.jpg")
# 图像灰度图转化并平滑滤波
grayImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
# 均值滤波
grayImage = cv2.blur(grayImage, (3, 3))
cv2.imshow(zh_ch("原始图"), grayImage)

# 二值化
ret, threshold_output = cv2.threshold(grayImage, 200, 255, cv2.THRESH_BINARY)

# 寻找图像轮廓
contours, hierarchy = cv2.findContours(threshold_output, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

hull = []
# 寻找图像凸包
for i in range(len(contours)):
    hull.append(cv2.convexHull(contours[i], False))

# 初始化一个空白图像
drawing = np.zeros((threshold_output.shape[0], threshold_output.shape[1], 3), np.uint8)

# 绘制轮廓和凸包
for i in range(len(contours)):
    # 绘制轮廓
    cv2.drawContours(drawing, contours, i, (0, 255, 0), 1, 8, hierarchy)
    # 绘制凸包
    cv2.drawContours(drawing, hull, i, (255, 0, 0), 1, 8)

# 显示图片
cv2.imshow("drawing", drawing)
cv2.waitKey(0)
