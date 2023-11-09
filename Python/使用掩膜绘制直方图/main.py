# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：使用掩膜绘制直方图
# 博客：http://www.bilibili996.com/Course?id=2605222000268
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
from matplotlib import pyplot as plt

img = cv2.imread('../images/home3.jpg', 0)

# create a mask
mask = np.zeros(img.shape[:2], np.uint8)
mask[100:300, 100:400] = 255
masked_img = cv2.bitwise_and(img, img, mask=mask)

# Calculate histogram with mask and without mask
# Check third argument for mask
hist_full = cv2.calcHist([img], [0], None, [256], [0, 256])
hist_mask = cv2.calcHist([img], [0], mask, [256], [0, 256])

plt.subplot(221), plt.imshow(img, 'gray')
plt.subplot(222), plt.imshow(mask, 'gray')
plt.subplot(223), plt.imshow(masked_img, 'gray')
plt.subplot(224), plt.plot(hist_full), plt.plot(hist_mask)
plt.xlim([0, 256])
plt.show()