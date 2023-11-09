# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：RGB三色直方图的绘制
# 博客：http://www.bilibili996.com/Course?id=2488638000215
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原图并显示
srcImage = cv2.imread('../images/6919.jpg')
cv2.imshow('srcImage', srcImage)

# 【2】定义变量
size = [256]

# 【3】进行直方图的计算（红色分量部分）
redHist = cv2.calcHist([srcImage], [0], None, size, [0, 256])

# 【4】进行直方图的计算（绿色分量部分）
grayHist = cv2.calcHist([srcImage], [1], None, size, [0, 256])

# 【5】进行直方图的计算（蓝色分量部分）
blueHist = cv2.calcHist([srcImage], [2], None, size, [0, 256])

# -----------------------绘制出三色直方图------------------------
# 参数准备
scale = 1
bins = 256
histHeight = 256
histImage = np.zeros((histHeight, bins * 3, 3), np.uint8)
maxValue_red = cv2.minMaxLoc(redHist)
maxValue_green = cv2.minMaxLoc(grayHist)
maxValue_blue = cv2.minMaxLoc(blueHist)

# 正式开始绘制
for i in range(256):
    binValue_red = redHist[i]  # 直方图组距的值
    binValue_green = grayHist[i]  # 直方图组距的值
    binValue_blue = blueHist[i]  # 直方图组距的值

    intensity_red = round(binValue_red[0] * histHeight / maxValue_red[1])
    intensity_green = round(binValue_green[0] * histHeight / maxValue_green[1])
    intensity_blue = round(binValue_blue[0] * histHeight / maxValue_blue[1])

    # 绘制矩形
    cv2.rectangle(histImage, (i * scale, histHeight - 1),
                  ((i + 1) * scale - 1, histHeight - intensity_red), (255, 0, 0))

    cv2.rectangle(histImage, ((i + bins) * scale, histHeight - 1),
                  ((i + bins + 1) * scale - 1, histHeight - intensity_green), (0, 255, 0))

    cv2.rectangle(histImage, ((i + bins * 2) * scale, histHeight - 1),
                  ((i + bins * 2 + 1) * scale - 1, histHeight - intensity_blue), (0, 0, 255))

# 显示效果图
cv2.imshow('histImage', histImage)
cv2.waitKey(0)
