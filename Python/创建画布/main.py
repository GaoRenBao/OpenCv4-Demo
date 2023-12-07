# OpenCv版本：opencv-python 4.6.0.66
# 内容：创建画布
# 博客：http://www.bilibili996.com/Course?id=4082690000247
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

size = (200, 200)
# 全黑.可以用在屏保
black = np.zeros(size)
print(black[34][56])
cv2.imshow('black', black)

# white 全白
black[:] = 255
print(black[34][56])
cv2.imshow('white', black)
cv2.waitKey(0)
