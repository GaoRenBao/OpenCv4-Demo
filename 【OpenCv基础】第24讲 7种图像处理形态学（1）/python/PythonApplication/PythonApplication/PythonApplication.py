import cv2
import numpy as np

# 载入原图
srcImage=cv2.imread('1.jpg')

# 显示原图
cv2.imshow('image',srcImage)

# 定义核大小
element = cv2.getStructuringElement(cv2.MORPH_RECT,(15, 15))

# 进行腐蚀操作
out1 = cv2.erode(srcImage, element)

# 进行膨胀操作
out2 = cv2.dilate(srcImage, element);

# 显示效果图
cv2.imshow('Erode',out1)
cv2.imshow('Dilate',out2)

cv2.waitKey(0)
cv2.destroyAllWindows()