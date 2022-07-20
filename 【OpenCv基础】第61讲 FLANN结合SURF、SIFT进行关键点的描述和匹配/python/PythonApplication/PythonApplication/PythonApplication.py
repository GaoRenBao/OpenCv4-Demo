import cv2
import numpy as np

# 载入图像、显示并转化为灰度图
trainImage = cv2.imread("1.jpg")
trainImage_gray=cv2.cvtColor(trainImage, cv2.COLOR_BGR2GRAY)

# 检测SIFT关键点、提取训练图像描述符
sift = cv2.SIFT_create(80)

# 调用detect函数检测出SIFT特征关键点，保存在vector容器中
train_keyPoint = sift.detect(trainImage_gray)

# 方法1：这个操作不行。。。
# (train_keyPoint, trainDescriptor) = sift.compute(trainImage_gray, None)

# 方法2：计算描述符（特征向量）
(train_keyPoint, trainDescriptor) = sift.detectAndCompute(trainImage_gray, None)

# 创建基于FLANN的描述符匹配对象
matcher =  cv2.FlannBasedMatcher() # cv2.BFMatcher() #建立匹配关系
matcher.add(trainDescriptor)
matcher.train()

Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    # 读取图像
    while True:
        grabbed, testImage = Cap.read()
        if testImage is None:
            continue

        testImage_gray=cv2.cvtColor(testImage, cv2.COLOR_BGR2GRAY)

        # 调用detect函数检测出SIFT特征关键点，保存在vector容器中
        test_keyPoint = sift.detect(testImage_gray)

        # 方法1：这个操作不行。。。
        # (test_keyPoint, testDescriptor) = sift.compute(testImage_gray, None)

        # 方法2：计算描述符（特征向量）
        (test_keyPoint, testDescriptor) = sift.detectAndCompute(testImage_gray, None)

        # 匹配训练和测试描述符
        matches=matcher.knnMatch(testDescriptor,trainDescriptor,2)
        goodMatches = []
        #舍弃大于0.7的匹配
        for m,n in matches:
            if m.distance < 0.7 * n.distance:
                goodMatches.append(m)

        #画出匹配关系
        dstImage = cv2.drawMatches(testImage, test_keyPoint,trainImage, train_keyPoint, goodMatches, None) 
        cv2.imshow("dstImage", dstImage)
        cv2.waitKey(1)  # 设置延迟时间

cv2.waitKey(0)
cv2.destroyAllWindows()



