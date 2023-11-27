# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：K值聚类（二）
# 博客：http://www.bilibili996.com/Course?id=fcd7745b72f4493f81683b2e3e80f911
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

img = cv2.imread('../images/home.jpg')
# img = cv2.imread('../data/opencv_logo.png')
Z = img.reshape((-1, 3))
# convert to np.float32
Z = np.float32(Z)

# define criteria, number of clusters(K) and apply kmeans()
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 10, 1.0)
K = 8
# K = 3
# K = 14
ret, label, center = cv2.kmeans(Z, K, None, criteria, 10, cv2.KMEANS_RANDOM_CENTERS)

# 分离颜色
for y in range(len(center)):
    a1 = []
    for i, x in enumerate(label.ravel()):
        if x == y:
            a1.append(list(Z[i]))
        else:
            a1.append([0, 0, 0])
    a2 = np.array(a1)
    a3 = a2.reshape((img.shape))
    cv2.imshow('res2' + str(y), a3)

# 最大的色块
# # Now convert back into uint8, and make original image
# center = np.uint8(center)
# res = center[label.flatten()]
# res2 = res.reshape((img.shape))

# cv2.imshow('res2', res2)
# cv2.imshow('res2', a3)
cv2.waitKey(0)
cv2.destroyAllWindows()
