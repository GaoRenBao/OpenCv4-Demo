# OpenCv版本：opencv-python 4.6.0.66
# 内容：使用matplotlib加载图片
# 博客：http://www.bilibili996.com/Course?id=4316440000002
# 作者：高仁宝
# 时间：2023.11

import cv2
from matplotlib import pyplot as plt

img = cv2.imread('../images/messi5.jpg', 0)
plt.imshow(img, cmap='gray', interpolation='bicubic')
# 彩色图像使用 OpenCV 加载时是 BGR 模式。但是 Matplotlib 是 RGB 模式。所以彩色图像如果已经被OpenCV 读取，  它将不会被 Matplotlib 正 确显示。

plt.xticks([]), plt.yticks([])  # to hide tick values on X and Y axis
plt.show()
