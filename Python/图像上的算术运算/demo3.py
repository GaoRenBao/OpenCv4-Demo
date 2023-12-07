# OpenCv版本：opencv-python 4.6.0.66
# 内容：图像上的算术运算
# 博客：http://www.bilibili996.com/Course?id=4683988000254
# 作者：高仁宝
# 时间：2023.11

import cv2


# returns just the difference of the two images
def diff(img, img1):
    return cv2.absdiff(img, img1)


# removes the background but requires three images
def diff_remove_bg(img0, img, img1):
    d1 = diff(img0, img)
    d2 = diff(img, img1)
    return cv2.bitwise_and(d1, d2)


# img1=cv2.imread('../images/subtract1.jpg')
img1 = cv2.imread('../images/subtract1.jpg', 0)  # 灰度图
# img2=cv2.imread('../images/subtract2.jpg')
img2 = cv2.imread('../images/subtract2.jpg', 0)

cv2.imshow('subtract1', img1)
cv2.imshow('subtract2', img2)

st = diff_remove_bg(img2, img1, img2)
cv2.imshow('after subtract', st)

cv2.waitKey(0)
