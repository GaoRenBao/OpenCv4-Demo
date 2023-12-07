# OpenCv版本：opencv-python 4.6.0.66
# 内容：OpenCv中的KNN
# 博客：http://www.bilibili996.com/Course?id=bf3a812d3521492cac7789f32a488c0a
# 作者：高仁宝
# 时间：2023.11

"""
k 的取值最好为奇数
根据 k 个 最近邻居进行分类的方法 称为 kNN

权重
距离近的具有更高的权重， 距离远的权重更低

knn-find_nearest.py:
http://www.bogotobogo.com/python/OpenCV_Python/python_opencv3_Machine_Learning_Classification_K-nearest_neighbors_k-NN.php
"""

import cv2
import numpy as np
import matplotlib.pyplot as plt

# Feature set containing (x,y) values of 25 known/training data
trainData = np.random.randint(0, 100, (25, 2)).astype(np.float32)
# Labels each one either Red or Blue with numbers 0 and 1
responses = np.random.randint(0, 2, (25, 1)).astype(np.float32)
# Take Red families and plot them
red = trainData[responses.ravel() == 0]

plt.scatter(red[:, 0], red[:, 1], 80, 'r', '^')
# Take Blue families and plot them
blue = trainData[responses.ravel() == 1]

plt.scatter(blue[:, 0], blue[:, 1], 80, 'b', 's')
plt.show()

# 测试数据被标记为绿色
# # 回值包括
# 1. 由 kNN算法计算得到的测 数据的类别标志0或1 。
# 如果你想使用最近邻算法 只需 将 k  置为 1 k 就是最近邻的数目。
# 2. k 个最近邻居的类别标志。
# 3. 每个最近邻居到测 数据的 离。
newcomer = np.random.randint(0, 100, (1, 2)).astype(np.float32)
plt.scatter(newcomer[:, 0], newcomer[:, 1], 80, 'g', 'o')
knn = cv2.ml.KNearest_create()
knn.train(trainData, cv2.ml.ROW_SAMPLE, responses)
ret, results, neighbours, dist = knn.findNearest(newcomer, 3)

print("result: ", results, "\n")
print("neighbours: ", neighbours, "\n")
print("distance: ", dist)
plt.show()

# 如果我们有大 的数据   测  可以直接传入一个数组。对应的结果 同样也是数组

# 10 new comers
newcomers = np.random.randint(0, 100, (10, 2)).astype(np.float32)
ret, results, neighbours, dist = knn.findNearest(newcomer, 3)
# The results also will contain 10 labels.

