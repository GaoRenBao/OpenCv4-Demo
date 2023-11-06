# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：图像的亮度、对比度调整
# 博客：http://www.bilibili996.com/Course?id=5140059000099
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

alpha = 0.3  # 对比度
beta = 80  # 亮度

img_path = "../images/flowers.jpg"
img = cv2.imread(img_path)
img2 = cv2.imread(img_path)


# 修改对比度
def updateAlpha(x):
    global alpha, img, img2
    alpha = cv2.getTrackbarPos('Alpha', 'image')
    alpha = alpha * 0.01
    img = np.uint8(np.clip((alpha * img2 + beta), 0, 255))


# 修改亮度
def updateBeta(x):
    global beta, img, img2
    beta = cv2.getTrackbarPos('Beta', 'image')
    img = np.uint8(np.clip((alpha * img2 + beta), 0, 255))


# 创建窗口
cv2.namedWindow('image')
cv2.createTrackbar('Alpha', 'image', 0, 300, updateAlpha)
cv2.createTrackbar('Beta', 'image', 0, 200, updateBeta)
cv2.setTrackbarPos('Alpha', 'image', 80)
cv2.setTrackbarPos('Beta', 'image', 80)

while (True):
    cv2.imshow('image', img)
    if cv2.waitKey(1) == ord('q'):
        break

cv2.destroyAllWindows()
