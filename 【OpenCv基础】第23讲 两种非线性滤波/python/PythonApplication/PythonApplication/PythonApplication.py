import cv2
import numpy as np

# 载入原图
image=cv2.imread('1.jpg')

# 显示原图
cv2.imshow('image',image)

# 进行中值滤波操作
out1 = cv2.medianBlur(image, 7)

# 进行双边滤波操作
out2 = cv2.bilateralFilter(image, 25, 25 * 2, 25 / 2);

# 显示结果
cv2.imshow('medianBlur',out1)
cv2.imshow('bilateralFilter',out2)

cv2.waitKey(0)
cv2.destroyAllWindows()