# OpenCv版本：opencv-python 4.6.0.66
# 内容：7种图像处理形态学（1）
# 博客：http://www.bilibili996.com/Course?id=5402740000127
# 作者：高仁宝
# 时间：2023.11

import cv2

# 载入原图
srcImage = cv2.imread('../images/dog.jpg')

# 显示原图
cv2.imshow('image', srcImage)

# 定义核大小
element = cv2.getStructuringElement(cv2.MORPH_RECT, (15, 15))

# 进行腐蚀操作
out1 = cv2.erode(srcImage, element)

# 进行膨胀操作
out2 = cv2.dilate(srcImage, element)

# 显示效果图
cv2.imshow('Erode', out1)
cv2.imshow('Dilate', out2)

cv2.waitKey(0)
cv2.destroyAllWindows()
