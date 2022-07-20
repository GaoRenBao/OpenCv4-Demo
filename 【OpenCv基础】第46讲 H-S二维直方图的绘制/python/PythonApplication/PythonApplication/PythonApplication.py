import cv2
import numpy as np
import random

# 载入源图，转化为HSV颜色模型
srcImage=cv2.imread('1.jpg')
hsvImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2HSV)

# 参数准备
hueBinNum = 30 # 色调的直方图直条数量
saturationBinNum = 32 # 饱和度的直方图直条数量
histSize = [hueBinNum, saturationBinNum]
dstHist = cv2.calcHist([hsvImage], [0,1], None, histSize, [0, 180, 0, 256])

# 查找数组和子数组的全局最小值和最大值存入maxValue中
maxValue=cv2.minMaxLoc(dstHist)

# 网上的一种方法
# dstHist2 = (255*dstHist/maxValue[1]).astype(np.uint8)

scale = 10
histImg = np.zeros((saturationBinNum * scale,  hueBinNum * 10, 3), np.uint8)

# 双层循环，进行直方图绘制
for hue in range(hueBinNum):
    for saturation in range(saturationBinNum):
        binValue = dstHist[hue, saturation] # 直方图组距的值
        intensity = round(binValue * 255 / maxValue[1])

        # 绘制矩形
        cv2.rectangle(histImg, (hue * scale, saturation * scale),
                     ((hue + 1) * scale - 1, (saturation + 1) * scale - 1), 
                     intensity, -1)

# 显示效果图
cv2.imshow('srcImage', srcImage)
cv2.imshow('histImg', histImg)
cv2.waitKey(0)


