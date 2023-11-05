# OpenCv版本 OpenCvSharp4.6.0.66
# 博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

size = (200, 200)
# 全黑.可以用在屏保
black=np.zeros(size)
print(black[34][56])
cv2.imshow('black',black)

#white 全白
black[:]=255
print(black[34][56])
cv2.imshow('white',black)
cv2.waitKey(0);