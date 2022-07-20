import cv2
import numpy as np

# 载入原图
srcImage=cv2.imread('1.jpg')

# 显示原图
cv2.imshow('image',srcImage)
imgH = srcImage.shape[0]
imgW = srcImage.shape[1]

# 使用resize进行尺寸调整操作
dstImage1 = cv2.resize(srcImage, ((int)(imgW*0.8), (int)(imgH*0.8)), interpolation = cv2.INTER_NEAREST)
dstImage2 = cv2.resize(srcImage, ((int)(imgW*1.2), (int)(imgH*1.2)), interpolation = cv2.INTER_NEAREST)

# 进行向上取样操作
dstImage3 = cv2.pyrUp(srcImage, (imgW * 2, imgH * 2));

# 进行向下取样操作
dstImage4 =	cv2.pyrDown(srcImage, (imgW * 0.5, imgH * 0.5));

# 显示效果图
cv2.imshow('dstImage1',dstImage1)
cv2.imshow('dstImage2',dstImage2)
cv2.imshow('dstImage3',dstImage3)
cv2.imshow('dstImage4',dstImage4)

cv2.waitKey(0)
cv2.destroyAllWindows()