# OpenCv版本：opencv-python 4.6.0.66
# 博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 载入图片
srcImage = cv2.imread('../images/girl.jpg')
# 显示原图
cv2.imshow('srcImage', srcImage)

# 均值滤波
blur2 = cv2.blur(srcImage, (7, 7))
# 显示效果图
cv2.imshow('out', blur2)

# 左右显示原图和效果图
htich = np.hstack((srcImage, blur2))
cv2.imshow('out2', htich)

cv2.waitKey(0)
cv2.destroyAllWindows()
