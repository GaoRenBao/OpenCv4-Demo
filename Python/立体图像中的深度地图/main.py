# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：立体图像中的深度地图
# 博客：http://www.bilibili996.com/Course?id=
# 作者：高仁宝
# 时间：2023.11

import cv2
from matplotlib import pyplot as plt

# read two input images as grayscale images
imgL = cv2.imread('../images/tsukuba_l.png', 0)
imgR = cv2.imread('../images/tsukuba_r.png', 0)

# Initiate and StereoBM object
stereo = cv2.StereoBM_create(numDisparities=0, blockSize=21)

# compute the disparity map
disparity = stereo.compute(imgL, imgR)
plt.imshow(disparity, 'gray')
plt.show()
disparity.shape
