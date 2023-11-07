# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：图像缩放与图像金字塔
# 博客：http://www.bilibili996.com/Course?id=5486413000167
# 作者：高仁宝
# 时间：2023.11

import cv2

# 载入原图
srcImage = cv2.imread('../images/dota_logo.jpg')

# 显示原图
cv2.imshow('image', srcImage)
imgH = srcImage.shape[0]
imgW = srcImage.shape[1]

# 使用resize进行尺寸调整操作
dstImage1 = cv2.resize(srcImage, (int(imgW * 0.8), int(imgH * 0.8)), interpolation=cv2.INTER_NEAREST)
dstImage2 = cv2.resize(srcImage, (int(imgW * 1.2), int(imgH * 1.2)), interpolation=cv2.INTER_NEAREST)

# 进行向上取样操作
dstImage3 = cv2.pyrUp(srcImage, (imgW * 2, imgH * 2));

# 进行向下取样操作
dstImage4 = cv2.pyrDown(srcImage, (imgW * 0.5, imgH * 0.5));

# 显示效果图
cv2.imshow('dstImage1', dstImage1)
cv2.imshow('dstImage2', dstImage2)
cv2.imshow('dstImage3', dstImage3)
cv2.imshow('dstImage4', dstImage4)

cv2.waitKey(0)
cv2.destroyAllWindows()
