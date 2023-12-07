# OpenCv版本：opencv-python 4.6.0.66
# 内容：角点检测的FAST算法（FAST特征检测器）
# 博客：http://www.bilibili996.com/Course?id=421a041d9cd54125a4d8f035ca7e2b24
# 作者：高仁宝
# 时间：2023.11

import cv2

img = cv2.imread('../images/blox.jpg', 0)

# Initiate FAST object with default values
fast = cv2.FastFeatureDetector_create()
# find and draw the keypoints
kp = fast.detect(img, None)
img2 = cv2.drawKeypoints(img, kp, None, color=(255, 0, 0))

# Print all default params
print("Threshold: ", fast.getThreshold())
print("nonmaxSuppression: ", fast.getNonmaxSuppression())
print("neighborhood: ", fast.getType())
print("Total Keypoints with nonmaxSuppression: ", len(kp))
cv2.imshow('fast_true.png', img2)

# Disable nonmaxSuppression
fast.setNonmaxSuppression(0)
kp = fast.detect(img, None)
print("Total Keypoints without nonmaxSuppression: ", len(kp))

img3 = cv2.drawKeypoints(img, kp, None, color=(255, 0, 0))
cv2.imshow('fast_false.png', img3)
cv2.waitKey(0)

# 第一幅图是使用了非最大值抑制的结果
# 第二幅没有使用非最大值抑制。



