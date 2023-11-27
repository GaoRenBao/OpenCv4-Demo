# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：姿势估计
# 博客：http://www.bilibili996.com/Course?id=f2a6235f09a849a9a93974ce8b00272f
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import glob


# 函数 draw 它的参数有棋盘上的角点
#  使用 cv2.findChessboardCorners() 得到
#  绘制的 3D 坐标轴上的点
def draw(img, corners, imgpts):
    corner = tuple(corners[0].ravel())
    l0 = (int(corner[0]), int(corner[1]))
    l1 = (int(imgpts[0].ravel()[0]), int(imgpts[0].ravel()[1]))
    l2 = (int(imgpts[1].ravel()[0]), int(imgpts[1].ravel()[1]))
    l3 = (int(imgpts[2].ravel()[0]), int(imgpts[2].ravel()[1]))
    print(l0, l1, l2, l3)
    img = cv2.line(img, l0, l1, (255, 0, 0), 5)
    img = cv2.line(img, l0, l2, (0, 255, 0), 5)
    img = cv2.line(img, l0, l3, (0, 0, 255), 5)
    return img


# 渲染一个立方体
def draw_cube(img, corners, imgpts):
    imgpts = np.int32(imgpts).reshape(-1, 2)
    # draw ground floor in green
    img = cv2.drawContours(img, [imgpts[:4]], -1, (0, 255, 0), -3)
    # draw pillars in blue color
    for i, j in zip(range(4), range(4, 8)):
        img = cv2.line(img, tuple(imgpts[i]), tuple(imgpts[j]), (255), 3)
    # draw top layer in red color
    img = cv2.drawContours(img, [imgpts[4:]], -1, (0, 0, 255), 3)
    return img


# termination criteria
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 30, 0.001)
# prepare object points, like (0,0,0), (1,0,0), (2,0,0) ....,(6,5,0)
objp = np.zeros((7 * 6, 3), np.float32)
objp[:, :2] = np.mgrid[0:7, 0:6].T.reshape(-1, 2)
# Arrays to store object points and image points from all the images.
objpoints = []  # 3d point in real world space
imgpoints = []  # 2d points in image plane.
images = glob.glob('../images/lefts/left*.jpg')
images += glob.glob('../images/rights/right*.jpg')

for fname in images:
    img = cv2.imread(fname)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    # Find the chess board corners
    ret, corners = cv2.findChessboardCorners(gray, (7, 6), None)
    # If found, add object points, image points (after refining them)
    if ret == True:
        objpoints.append(objp)
        corners2 = cv2.cornerSubPix(gray, corners, (11, 11), (-1, -1), criteria)
        imgpoints.append(corners)
        # Draw and display the corners
        cv2.drawChessboardCorners(img, (7, 6), corners2, ret)
        cv2.imshow('img', img)
        cv2.waitKey(1)
cv2.destroyAllWindows()

# 标定
ret, mtx, dist, _, _ = cv2.calibrateCamera(objpoints, imgpoints, gray.shape[::-1], None, None)

axis = np.float32([[3, 0, 0], [0, 3, 0], [0, 0, -3]]).reshape(-1, 3)
# 渲染一个立方体
# axis = np.float32([[0, 0, 0], [0, 3, 0], [3, 3, 0], [3, 0, 0],
#                    [0, 0, -3], [0, 3, -3], [3, 3, -3], [3, 0, -3]])

for fname in glob.glob('../images/lefts/left*.jpg'):
    img = cv2.imread(fname)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    ret, corners = cv2.findChessboardCorners(gray, (7, 6), None)
    if ret:
        corners2 = cv2.cornerSubPix(gray, corners, (11, 11), (-1, -1), criteria)
        # Find the rotation and translation vectors.
        ret, rvecs, tvecs = cv2.solvePnP(objp, corners2, mtx, dist)
        # project 3D points to image plane
        imgpts, jac = cv2.projectPoints(axis, rvecs, tvecs, mtx, dist)
        img = draw(img, corners2, imgpts)
        cv2.imshow('img', img)
        cv2.imwrite('img.jpg', img)
        if cv2.waitKey(0) == 27:
            break
cv2.destroyAllWindows()
