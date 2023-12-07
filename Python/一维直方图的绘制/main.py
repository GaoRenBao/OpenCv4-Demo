# OpenCv版本：opencv-python 4.6.0.66
# 内容：一维直方图的绘制
# 博客：http://www.bilibili996.com/Course?id=2121468000214
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原图并显示
srcImage = cv2.imread('../images/1283.jpg', 0)
cv2.imshow('srcImage', srcImage)

# 【2】定义变量
size = [256]

# 【3】计算图像的直方图
dstHist = cv2.calcHist([srcImage], [0], None, size, [0, 255])
scale = 1
dstImage = np.zeros((size[0] * scale, size[0], 3), np.uint8)

# 【4】获取最大值和最小值
maxValue = cv2.minMaxLoc(dstHist)

# 【5】绘制出直方图
hpt = 0.9 * size[0];
for i in range(256):
    binValue = dstHist[i]  # 直方图组距的值
    realValue = round(binValue[0] * hpt / maxValue[1])

    # 绘制矩形
    cv2.rectangle(dstImage, (i * scale, size[0] - 1),
                  ((i + 1) * scale - 1, size[0] - realValue), (255, 255, 255))

# 显示效果图
cv2.imshow('dstImage', dstImage)
cv2.waitKey(0)
