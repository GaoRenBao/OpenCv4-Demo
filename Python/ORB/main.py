# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：ORB
# 博客：http://www.bilibili996.com/Course?id=e64cab8fe66f4380ba8a7e18469f2d21
# 作者：高仁宝
# 时间：2023.11

import cv2
from matplotlib import pyplot as plt

img = cv2.imread('../images/blox.jpg', 0)

# Initiate ORB detector
orb = cv2.ORB_create()
# find the keypoints with ORB
kp = orb.detect(img, None)
# compute the descriptors with ORB
kp, des = orb.compute(img, kp)

# draw only keypoints location,not size and orientation
img2 = cv2.drawKeypoints(img, kp, None, color=(0, 255, 0), flags=0)

cv2.imwrite("out.jpg", img2)
plt.imshow(img2), plt.show()


