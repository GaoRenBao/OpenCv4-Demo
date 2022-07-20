import cv2
import numpy as np

# 【1】载入图像
srcImage1 = cv2.imread("1.jpg")
srcImage2 = cv2.imread("2.jpg")

# 定义一个特征检测类对象
sift = cv2.SIFT_create(700)
# 调用detect函数检测出SIFT特征关键点，保存在vector容器中
keyPoint1 = sift.detect(srcImage1)
keyPoint2 = sift.detect(srcImage2)

# 方法1：这个操作不行。。。
#(keypoints1, descriptors1) = sift.compute(srcImage1, None)
#(keypoints2, descriptors2) = sift.compute(srcImage2, None)

# 方法2：计算描述符（特征向量）
(keypoints1, descriptors1) = sift.detectAndCompute(srcImage1, None)
(keypoints2, descriptors2) = sift.detectAndCompute(srcImage2, None)

matcher = cv2.BFMatcher() #建立匹配关系
matches=matcher.match(descriptors1,descriptors2) #匹配描述子
matches=sorted(matches,key=lambda x:x.distance)  #据距离来排序
 
#画出匹配关系
outImg = None
imgMatches = cv2.drawMatches(srcImage1, keyPoint1,srcImage2,keyPoint2,matches, outImg = None) 
#imgMatches = cv2.drawMatches(srcImage1, keyPoint1,srcImage2,keyPoint2,mathces[:40], outImg = None) 

cv2.imshow("imgMatches", imgMatches)
cv2.waitKey(0)
cv2.destroyAllWindows()
