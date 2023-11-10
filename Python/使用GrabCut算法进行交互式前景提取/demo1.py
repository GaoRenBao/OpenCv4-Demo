# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：使用GrabCut算法进行交互式前景提取
# 博客：http://www.bilibili996.com/Course?id=ebe8b6e9ef6843f486142989d05d438a
# 作者：高仁宝
# 时间：2023.11

# demo1，简单提取

import numpy as np
import cv2
from matplotlib import pyplot as plt

img = cv2.imread('../images/messi5.jpg')

mask = np.zeros(img.shape[:2], np.uint8)
bgdModel = np.zeros((1, 65), np.float64)
fgdModel = np.zeros((1, 65), np.float64)
rect = (50, 50, 450, 290)

# 函数的返回值是更新的 mask, bgdModel, fgdModel
cv2.grabCut(img, mask, rect, bgdModel, fgdModel, iterCount=5, mode=cv2.GC_INIT_WITH_RECT)
# 迭代 5 次

mask2 = np.where((mask == 2) | (mask == 0), 0, 1).astype('uint8')
img = img * mask2[:, :, np.newaxis]

plt.imshow(img), plt.colorbar(), plt.show()


