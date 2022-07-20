import cv2
import numpy as np
import yaml
import time
import random

# 载入原图
image=cv2.imread('1.jpg')

# 显示原图
cv2.imshow('image',image)

# 进行方框滤波操作 BoxFilter
out1 = cv2.boxFilter(image, -1, (5,5))

# 进行均值滤波操作
out2 = cv2.blur(image, (7,7))

# 进行高斯滤波操作
out3 = cv2.GaussianBlur(image,(5,5), 0)

# 显示组合结果
cv2.imshow('boxFilter',out1)
cv2.imshow('blur',out2)
cv2.imshow('GaussianBlur',out3)

cv2.waitKey(0)
cv2.destroyAllWindows()