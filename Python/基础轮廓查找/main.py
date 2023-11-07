# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：基础轮廓查找
# 博客：http://www.bilibili996.com/Course?id=3319137000193
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random


def zh_ch(string):
    return string.encode("gbk").decode(errors="ignore")


# 【1】载入原始图
srcImage = cv2.imread("../images/flowers2.jpg", 0)
cv2.imshow("原始图", srcImage)

# 【2】初始化结果图
imgH = srcImage.shape[0]
imgW = srcImage.shape[1]
dstImage = cv2.UMat((imgW, imgH), cv2.CV_8UC3)

# 【3】srcImage取大于阈值119的那部分
ret, srcImage = cv2.threshold(srcImage, 119, 255, cv2.THRESH_BINARY)
cv2.imshow(zh_ch('取阈值后的原始图'), srcImage)

# 【4】查找轮廓
contours, hierarchy = cv2.findContours(srcImage, cv2.RETR_CCOMP, cv2.CHAIN_APPROX_SIMPLE)

for i in range(0, len(contours)):
    cv2.drawContours(dstImage, contours, i, (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)),
                     -1, 8, hierarchy)

cv2.imshow("dstImage", dstImage)
cv2.waitKey(0)
cv2.destroyAllWindows()
