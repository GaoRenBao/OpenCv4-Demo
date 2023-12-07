# OpenCv版本：opencv-python 4.6.0.66
# 内容：离散傅里叶变换
# 博客：http://www.bilibili996.com/Course?id=3391564000241
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import matplotlib.pyplot as plt

# 以灰度形式读入
img = cv2.imread('../images/girl.jpg', 0)

# 使用cv2.dft()进行傅里叶变换
dst = cv2.dft(np.float32(img), flags=cv2.DFT_COMPLEX_OUTPUT)

# 将变换后图像的低频部分转移到图像的中心
dst_center = np.fft.fftshift(dst)

# 使用cv2.magnitude将实部和虚部转换为实部，乘以20是为了使得结果更大
result = 20 * np.log(np.abs(cv2.magnitude(dst_center[:, :, 0], dst_center[:, :, 1])))

# 显示图像
plt.subplot(121)
plt.imshow(img, cmap="gray")
plt.axis("off")
plt.subplot(122)
plt.axis("off")
plt.imshow(result, cmap="gray")
plt.show()