# OpenCv版本：opencv-python 4.6.0.66
# 内容：K值聚类
# 博客：http://www.bilibili996.com/Course?id=7d02676837f34dcc84606d906d877c3b
# 作者：高仁宝
# 时间：2023.11

"""
含有多个特征的数据
身高
体重
"""

# 在前面的T恤例子中我们只考虑了身高，现在我们也把体重考虑进去，也就是两个特征。
# 在前一节我们的数据是一个单列向量。每一个特征被排列成一列，每一行对应一个测试样本。
# 在本例中我们的测试数据适应 50x2 的向量，其中包含 50个人的身高和体重。第一列对应与身高，第二列对应与体重。
# 第一行包含两个元素，第一个是第一个人的身高，第二个是第一个人的体重。剩下的行对应与其他人的身高和体重。

import numpy as np
import cv2
from matplotlib import pyplot as plt

X = np.random.randint(25, 50, (25, 2))
Y = np.random.randint(60, 85, (25, 2))
Z = np.vstack((X, Y))

# convert to np.float32
Z = np.float32(Z)
# define criteria and apply kmeans()
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 10, 1.0)
ret, label, center = cv2.kmeans(Z, 2, None, criteria, 10, cv2.KMEANS_RANDOM_CENTERS)

# Now separate the data, Note the flatten()
A = Z[label.ravel() == 0]
B = Z[label.ravel() == 1]

# Plot the data
plt.scatter(A[:, 0], A[:, 1])
plt.scatter(B[:, 0], B[:, 1], c='r')
plt.scatter(center[:, 0], center[:, 1], s=80, c='y', marker='s')
plt.xlabel('Height'), plt.ylabel('Weight')
plt.show()
