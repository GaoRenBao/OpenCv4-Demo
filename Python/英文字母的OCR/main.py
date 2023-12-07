# OpenCv版本：opencv-python 4.6.0.66
# 内容：英文字母的OCR
# 博客：http://www.bilibili996.com/Course?id=6f064938b1b94d0d940b4f602b5df75d
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# Load the data, converters convert the letter to a number
data = np.loadtxt('letter-recognition.data', dtype='float32', delimiter=',',
                  converters={0: lambda ch: ord(ch) - ord('A')})#20000个
# split the data to two, 10000 each for train and test
train, test = np.vsplit(data, 2)
# split trainData and testData to features and responses
responses, trainData = np.hsplit(train, [1])
labels, testData = np.hsplit(test, [1])

# Initiate the kNN, classify, measure accuracy.
knn = cv2.ml.KNearest_create()
knn.train(trainData, cv2.ml.ROW_SAMPLE, responses)
ret, result, neighbours, dist = knn.findNearest(testData, k=5)

correct = np.count_nonzero(result == labels)
accuracy = correct * 100.0 / 10000
print('准确率', accuracy)#93.06
#准确率 到了 93.22%。同样你可以  增加训练样本的数量来提 准确率。


# save the data
np.savez('knn_data_alphabet.npz', train_alphabet=train, train_labels_alphabet=responses,test_alphabet=testData,test_labels_alphabet=labels)

#怎样预测字母？跟预测数字的一样
