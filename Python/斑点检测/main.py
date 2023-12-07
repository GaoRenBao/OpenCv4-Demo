# OpenCv版本：opencv-python 4.6.0.66
# 内容：斑点检测
# 博客：http://www.bilibili996.com/Course?id=5ecd1f6fca2641a2a228c60ae09ce4c1
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# Read image
im = cv2.imread("../images/blob.jpg", cv2.IMREAD_GRAYSCALE)

# Setup SimpleBlobDetector parameters.
params = cv2.SimpleBlobDetector_Params()
params.filterByColor = False

# Change thresholds
# params.minThreshold = 10
# params.maxThreshold = 200

# 斑点面积
params.filterByArea = True
params.minArea = 100
params.maxArea = 100000

# 斑点圆度
params.filterByCircularity = False
# params.minCircularity = 0.1
# params.maxCircularity = 0.1

# 斑点凸度
params.filterByConvexity = False
# params.minConvexity = 0.87
# params.maxConvexity = 0.87

# 斑点惯性率
params.filterByInertia = False
# params.minInertiaRatio = 0.01
# params.maxInertiaRatio = 0.01

# Create a detector with the parameters
ver = (cv2.__version__).split('.')
if int(ver[0]) < 3:
    detector = cv2.SimpleBlobDetector(params)
else:
    detector = cv2.SimpleBlobDetector_create(params)

# Detect blobs.
keypoints = detector.detect(im)
# Draw detected blobs as red circles.
# cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS ensures the size of the circle corresponds to the size of blob
im_with_keypoints = cv2.drawKeypoints(im, keypoints, np.array([]), (0, 0, 255),
                                      cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS)
# Show keypoints
cv2.imshow("Keypoints.jpg", im_with_keypoints)
cv2.imwrite("Keypoints.jpg", im_with_keypoints)
cv2.waitKey(0)

