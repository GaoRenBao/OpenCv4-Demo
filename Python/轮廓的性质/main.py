# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：轮廓的性质
# 博客：http://www.bilibili996.com/Course?id=4292381000261
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def test1(cnt):
    # 边界矩形的宽高比
    x, y, w, h = cv2.boundingRect(cnt)
    aspect_ratio = float(w) / h
    print('边界矩形的宽高比:', aspect_ratio)


def test2(cnt):
    # Extent轮廓面积与边界矩形面积的比
    area = cv2.contourArea(cnt)
    x, y, w, h = cv2.boundingRect(cnt)
    rect_area = w * h
    extent = float(area) / rect_area
    print('Extent轮廓面积与边界矩形面积的比:', extent)


def test3(cnt):
    # Solidity轮廓面积与凸包面积的比。
    area = cv2.contourArea(cnt)
    hull = cv2.convexHull(cnt)
    hull_area = cv2.contourArea(hull)
    solidity = float(area) / hull_area
    print('Solidity轮廓面积与凸包面积的比:', solidity)


def test4(cnt):
    # Equivalent Diameter与轮廓面积相等的圆形的直径
    area = cv2.contourArea(cnt)
    equi_diameter = np.sqrt(4 * area / np.pi)
    print('Equivalent Diameter与轮廓面积相等的圆形的直径:', equi_diameter)


def test5(cnt):
    # Orientation对象的方向下的方法会返回长轴和短轴的长度（椭圆拟合）
    (x, y), (MA, ma), angle = cv2.fitEllipse(cnt)
    print('Orientation对象的方向下的方法会返回长轴和短轴的长度:', (x, y), (MA, ma), angle)


def test6(imgray):
    # Mask and Pixel Points掩模和像素点
    mask = np.zeros(imgray.shape, np.uint8)
    cv2.drawContours(mask, [cnt], 0, 255, -1)
    pixelpoints = np.transpose(np.nonzero(mask))

    # 最大值和最小值及它们的位置
    min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(imgray, mask=mask)
    print('最大值和最小值及它们的位置:', min_val, max_val, min_loc, max_loc)

    # 我们也可以使用相同的掩模求一个对象的平均颜色或平均灰度
    mean_val = cv2.mean(imgray, mask=mask)
    print('平均色:', mean_val)


def test7(img, cnt):
    # 极点 一个对象最上面、最下面、最左、最右，的点。
    leftmost = tuple(cnt[cnt[:, :, 0].argmin()][0])
    rightmost = tuple(cnt[cnt[:, :, 0].argmax()][0])
    topmost = tuple(cnt[cnt[:, :, 1].argmin()][0])
    bottommost = tuple(cnt[cnt[:, :, 1].argmax()][0])
    print('最上面:', topmost)
    print('最下面:', bottommost)
    print('最左:', leftmost)
    print('最右:', rightmost)
    cv2.circle(img, topmost, 5, (0, 0, 255), -1)
    cv2.circle(img, bottommost, 5, (0, 0, 255), -1)
    cv2.circle(img, leftmost, 5, (0, 0, 255), -1)
    cv2.circle(img, rightmost, 5, (0, 0, 255), -1)
    cv2.imshow("img", img)


srcImage = cv2.imread('../images/star3.jpg')
img = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
ret, thresh = cv2.threshold(img, 127, 255, 0)
contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
print('contours len:', len(contours))
cnt = contours[0]

test1(cnt)
test2(cnt)
test3(cnt)
test4(cnt)
test5(cnt)

test6(img)
test7(srcImage, cnt)

cv2.waitKey(0)
cv2.destroyAllWindows()
