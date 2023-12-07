# OpenCv版本：opencv-python 4.6.0.66
# 内容：OpenCv中的KNN
# 博客：http://www.bilibili996.com/Course?id=bf3a812d3521492cac7789f32a488c0a
# 作者：高仁宝
# 时间：2023.11

# 来源：https://blog.csdn.net/qq_41895747/article/details/87926205

import numpy as np
import cv2
import matplotlib.pyplot as plt

# 设置绘图属性
plt.style.use('ggplot')


# 生成训练数据
# 输入：每个点的个数和每个数据点的特征数
def generate_data(num_samples, num_features):
    data_size = (num_samples, num_features)
    data = np.random.randint(0, 100, size=data_size)

    labels_size = (num_samples, 1)
    labels = np.random.randint(0, 2, size=labels_size)

    return data.astype(np.float32), labels  # 确保将数据转换成np.float32


train_data, labels = generate_data(11, 2)


# 分红和蓝色分别作图
def plot_data(all_blue, all_red):
    plt.scatter(all_blue[:, 0], all_blue[:, 1], c='b', marker='s', s=180)
    plt.scatter(all_red[:, 0], all_red[:, 1], c='r', marker='^', s=180)
    plt.xlabel('x coordinate (feature 1)')
    plt.ylabel('y coordinate (feature 2)')


print("train_data:", train_data)  # 观察数据集

# 平面化数据
bule = train_data[labels.ravel() == 0]
red = train_data[labels.ravel() == 1]
plot_data(bule, red)
plt.show()

# 训练分类器
knn = cv2.ml.KNearest_create()  # 实例化
knn.train(train_data, cv2.ml.ROW_SAMPLE, labels)
# 利用findNearest发现新的数据点
newcomer, _ = generate_data(1, 2)
plot_data(bule, red)
plt.plot(newcomer[0, 0], newcomer[0, 1], 'go', markersize=14)
plt.show()






