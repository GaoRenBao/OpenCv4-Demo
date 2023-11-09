# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：cornerHarris角点检测
# 博客：http://www.bilibili996.com/Course?id=0862817000219
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random

# 以灰度模式载入图像并显示
srcImage = cv2.imread("../images/home4.jpg", 0)
# 显示原图
cv2.imshow("srcImage", srcImage)

# 进行Harris角点检测找出角点
cornerStrength = cv2.cornerHarris(srcImage,2, 3, 0.01)

# 对灰度图进行阈值操作，得到二值图并显示
ret, harrisCorner = cv2.threshold(cornerStrength, 0.00001, 255, cv2.THRESH_BINARY)
cv2.imshow("End", harrisCorner)

cv2.waitKey(0)
cv2.destroyAllWindows()