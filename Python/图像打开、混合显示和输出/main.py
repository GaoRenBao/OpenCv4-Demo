# OpenCv版本：opencv-python 4.6.0.66
# 内容：图像打开、混合显示和输出
# 博客：http://www.bilibili996.com/Course?id=4335202000004
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 载入图片
image1 = cv2.imread('../images/a.jpg')
image2 = cv2.imread('../images/dota_logo.jpg')

# 载入后先显示
cv2.imshow('image1', image1)
cv2.imshow('image2', image2)

# 设置显示区域和位置
rows, cols, channels = image2.shape
roi = image1[350:350 + rows, 800:800 + cols]
imageROI = cv2.cvtColor(image2, cv2.COLOR_BGR2GRAY)
imageROI = cv2.bitwise_and(roi, roi, mask=imageROI)

# 组合图像
dst = cv2.addWeighted(imageROI, 0.7, image2, 0.3, 0, imageROI)
image1[350:350 + rows, 800:800 + cols] = dst

# 显示组合结果
cv2.imshow('合并', image1)
cv2.waitKey(0)
cv2.destroyAllWindows()
