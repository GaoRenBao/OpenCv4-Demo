# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：几何变换
# 博客：http://www.bilibili996.com/Course?id=1154656000256
# 作者：高仁宝
# 时间：2023.11

'''
仿射变换
在仿射变换中 原图中所有的平行线在结果图像中同样平行。
为了创建 这个矩阵，我们需要从原图像中找到三个点以及他们在 出图像中的位置。
然后 cv2.getAffineTransform 会创建一个 2x3 的矩  最后 个矩 会 传给 函数 cv2.warpAffine。
'''

import cv2
import numpy as np
from matplotlib import pyplot as plt

img = cv2.imread('../images/drawing.png')
rows, cols, ch = img.shape
print(img.shape)

pts1 = np.float32([[50, 50], [200, 50], [50, 200]])
pts2 = np.float32([[10, 100], [200, 50], [100, 250]])

M = cv2.getAffineTransform(pts1, pts2)
dst = cv2.warpAffine(img, M, (cols, rows))

# plt.subplot(121, plt.imshow(img), plt.title('Input'))
# plt.subplot(122, plt.imshow(dst), plt.title('Output'))

plt.figure(figsize=(8, 7), dpi=98)
p1 = plt.subplot(211)
# p1.show(img) # 官方代码采用的是这个操作，运行会报错
plt.imshow(img)
p1.set_title('Input')

p2 = plt.subplot(212)
# p2.show(dst)# 官方代码采用的是这个操作，运行会报错
plt.imshow(dst)
p2.set_title('Output')

plt.show()
