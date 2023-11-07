# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：CLAHE有限对比适应性直方图均衡化
# 博客：http://www.bilibili996.com/Course?id=1854373000267
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

img = cv2.imread('../images/tsukuba_l.png', 0)
# create a CLAHE object (Arguments are optional).
clahe = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(8, 8))
cl1 = clahe.apply(img)
cv2.imwrite('clahe_2.jpg', cl1)
