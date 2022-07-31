import cv2
import numpy as np

# 载入源图，显示并转化为灰度图
srcImage1 = cv2.imread("1.jpg")
cv2.imshow("srcImage1", srcImage1)
srcImage1_gray = cv2.cvtColor(srcImage1,cv2.COLOR_BGR2GRAY)

# ------------------检测ORB特征点并在图像中提取物体的描述符----------------------
# 参数定义
orb = cv2.ORB_create();
# 计算描述符（特征向量）
(kp1, des1) = orb.detectAndCompute(srcImage1_gray, None)
des1 = np.array(des1, np.float32)
flannIndex = cv2.flann_Index(des1, dict(algorithm=0, trees=5))

Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    while True:
        grabbed, srcImage2 = Cap.read()
        if srcImage2 is None:
            continue

        # 转化图像到灰度
        srcImage2_gray = cv2.cvtColor(srcImage2, cv2.COLOR_BGR2GRAY)
        # 计算描述符（特征向量）
        (kp2, des2) = orb.detectAndCompute(srcImage2_gray, None)
        des2 = np.array(des2, np.float32)
        # 调用K邻近算法
        idx2, matchDistance = flannIndex.knnSearch(des2, 2, params = {})

        #舍弃大于0.6的匹配
        goodMatches = []
        for m,n in matchDistance:
            #if m.distance < 0.7 * n.distance:
            goodMatches.append(m)

        #画出匹配关系
        dstImage = cv2.drawMatches(srcImage2,kp2,srcImage1, kp1,goodMatches,None) 
        cv2.imshow("dstImage", dstImage)
        cv2.waitKey(1)  # 设置延迟时间


cv2.waitKey(0)
cv2.destroyAllWindows()
