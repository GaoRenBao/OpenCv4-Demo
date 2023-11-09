# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：寻找已知物体（一）
# 博客：http://www.bilibili996.com/Course?id=058f71e965d74a4284cfe77621ff200f
# 作者：高仁宝
# 时间：2023.11

import cv2
from matplotlib import pyplot as plt

img1 = cv2.imread('../images/book_box.jpg')
# queryImage
img2 = cv2.imread('../images/book2.jpg')
# Initiate SIFT detector
# sift = cv2.SIFT()
sift = cv2.xfeatures2d.SIFT_create()

# find the keypoints and descriptors with SIFT
kp1, des1 = sift.detectAndCompute(img1, None)
kp2, des2 = sift.detectAndCompute(img2, None)

# BFMatcher with default params
bf = cv2.BFMatcher()
matches = bf.knnMatch(des1, des2, k=2)

# Apply ratio test
# 比值测试，首先获取与 A距离最近的点 B （最近）和 C （次近），
# 只有当 B/C 小于阀值时（0.75）才被认为是匹配，
# 因为假设匹配是一一对应的，真正的匹配的理想距离为0
good = []
for m, n in matches:
    if m.distance < 0.75 * n.distance:
        good.append([m])

# cv2.drawMatchesKnn expects list of lists as matches.
# img3 = np.ndarray([2, 2])
# img3 = cv2.drawMatchesKnn(img1, kp1, img2, kp2, good[:10], img3, flags=2)

# cv2.drawMatchesKnn expects list of lists as matches.
img3 = cv2.drawMatchesKnn(img1, kp1, img2, kp2, good, None, flags=2)
cv2.imshow("out", img3)
# cv2.imwrite("out.jpg", img3)
cv2.waitKey(0)