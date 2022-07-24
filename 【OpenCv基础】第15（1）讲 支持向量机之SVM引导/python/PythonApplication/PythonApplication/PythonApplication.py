import cv2
import numpy as np

# 创建窗口可视化
image = np.ones((512, 512, 4), np.uint8) 

# 设置训练数据
labels = (1, -1, 1, 1)
LabelsMat = np.array(labels)
# LabelsMat转换成4行1列的矩阵
LabelsMat = LabelsMat.reshape(4,1)

#数据集一定要设置成浮点型
trainingDataMat= np.array([[501,10],[255,10],[501,255],[10,501]],dtype='float32'); 
trainingDataMat = trainingDataMat.reshape(4,2)

#创建分类器
svm = cv2.ml.SVM_create()
#设置svm类型
svm.setType(cv2.ml.SVM_C_SVC)
#核函数
svm.setKernel(cv2.ml.SVM_LINEAR)
#训练
svm.train(trainingDataMat, cv2.ml.ROW_SAMPLE, LabelsMat)
 
imgH = image.shape[0]
imgW = image.shape[1]
for i in range(imgH):
    for j in range(imgW):
        arrayTest = np.empty(shape=[0, 2], dtype='float32')
        arrayTest = np.append(arrayTest, [[i,j]], axis = 0)
        pt = np.array(arrayTest , dtype='float32')   
        response = svm.predict(pt)[1] # 进行预测，返回1或-1
        if response[0][0] == 1:
           image[i, j] = [0, 255, 0, 0]
        else:
           image[i, j] = [255, 0, 0, 0]

# 显示训练数据
image = cv2.circle(image, (501, 10), 10, (0, 0, 255), 2)
image = cv2.circle(image, (255, 10), 10, (0, 0, 255), 2)
image = cv2.circle(image, (501, 255), 10, (0, 0, 255), 2)
image = cv2.circle(image, (10, 501), 10, (0, 0, 255), 2)

#绘图时，先宽后高，对应先列后行
imgH = LabelsMat.shape[0]
for i in range(imgH):
    v = trainingDataMat[i]
    x=v[0]
    y=v[1]
    if labels[i] == 1:
        image = cv2.circle(image, (int(x), int(y)), 5, (0, 0, 0), -1)
    else:
        image = cv2.circle(image, (int(x), int(y)), 5, (255, 255, 255), -1)
        
cv2.imshow("image", image)
cv2.waitKey(0)