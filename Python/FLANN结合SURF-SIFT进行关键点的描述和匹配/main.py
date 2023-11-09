# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：FLANN结合SURF/SIFT进行关键点的描述和匹配
# 博客：http://www.bilibili996.com/Course?id=3106495000236
# 作者：高仁宝
# 时间：2023.11

import cv2

Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if not Cap.isOpened():
    print('Open Camera Error.')
    exit()

Cap.read()
Cap.read()
Cap.read()
Cap.read()

# 载入图像、显示并转化为灰度图
grabbed, trainImage = Cap.read()
# trainImage = cv2.imread("1.jpg")
trainImage_gray = cv2.cvtColor(trainImage, cv2.COLOR_BGR2GRAY)

# 检测SIFT关键点、提取训练图像描述符
sift = cv2.SIFT_create(80)

# 方法1：计算描述符（特征向量），将Detect和Compute操作分开
# train_keyPoint = sift.detect(trainImage_gray)
# (train_keyPoint, trainDescriptor) = sift.compute(trainImage_gray, train_keyPoint)

# 方法2：计算描述符（特征向量），将Detect和Compute操作合并
(train_keyPoint, trainDescriptor) = sift.detectAndCompute(trainImage_gray, None)

# 创建基于FLANN的描述符匹配对象
matcher = cv2.FlannBasedMatcher()  # cv2.BFMatcher() #建立匹配关系
matcher.add(trainDescriptor)
matcher.train()

# 读取图像
while True:
    grabbed, testImage = Cap.read()
    if testImage is None:
        continue

    testImage_gray = cv2.cvtColor(testImage, cv2.COLOR_BGR2GRAY)

    # 方法1
    # test_keyPoint = sift.detect(testImage_gray)
    # (test_keyPoint, testDescriptor) = sift.compute(testImage_gray, test_keyPoint)

    # 方法2
    (test_keyPoint, testDescriptor) = sift.detectAndCompute(testImage_gray, None)

    # 匹配训练和测试描述符
    matches = matcher.knnMatch(testDescriptor, trainDescriptor, 2)
    goodMatches = []
    # 舍弃大于0.7的匹配
    for m, n in matches:
        if m.distance < 0.7 * n.distance:
            goodMatches.append(m)

    # 画出匹配关系
    dstImage = cv2.drawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, None)
    cv2.imshow("dstImage", dstImage)
    # 按ESC退出
    if cv2.waitKey(10) == 27:
        break

cv2.destroyAllWindows()
