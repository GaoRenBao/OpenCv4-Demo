# OpenCv版本：opencv-python 4.6.0.66
# 内容：摄像机标定
# 博客：http://www.bilibili996.com/Course?id=3b7dadd95f6a4508867768c07a59db3e
# 作者：高仁宝
# 时间：2023.11

# OpenCV-Python-Tutorial-中文版.pdf P247

import numpy as np
import cv2
import glob

# termination criteria
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 30, 0.001)
# prepare object points, like (0,0,0), (1,0,0), (2,0,0) ....,(6,5,0)
objp = np.zeros((6 * 7, 3), np.float32)
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
        cv2.waitKey(10)
cv2.destroyAllWindows()

##################### 标定  #####################
ret, mtx, dist, rvecs, tvecs = cv2.calibrateCamera(objpoints, imgpoints, gray.shape[::-1], None, None)

# 畸变校正
img = cv2.imread('../images/lefts/left12.jpg')
h, w = img.shape[:2]
newcameramtx, roi = cv2.getOptimalNewCameraMatrix(mtx, dist, (w, h), 1, (w, h))

# 使用 cv2.undistort()  是最简单的方法。只 使用这个函数和上面得到 的 ROI 对结果进行裁剪。

# undistort
dst = cv2.undistort(img, mtx, dist, None, newcameramtx)
# crop the image
x, y, w, h = roi
dst = dst[y:y + h, x:x + w]
cv2.imshow('calibresult1.png', dst)

# 使用 remapping  应 属于 曲线救国 了。 先我们 找到从畸变图像到畸变图像的映射方程。再使用 重映射方程
# undistort
mapx, mapy = cv2.initUndistortRectifyMap(mtx, dist, None, newcameramtx, (w, h), 5)
dst = cv2.remap(img, mapx, mapy, cv2.INTER_LINEAR)
# crop the image
x, y, w, h = roi
dst = dst[y:y + h, x:x + w]
cv2.imshow('calibresult2.png', dst)
# 你会发现结果图像中所有的边界变直了

# 反向投影误差
# 我们可以利用反向投影误差对我们找到的参数的准确性进行估计。得到的结果越接近 0越好。
# 有了内部参数，畸变参数和旋转变换矩阵，我们就可以使用cv2.projectPoints()将对象点转换到图像点。
# 然后就可以计算变换得到图像与角点检测算法的绝对差了。
# 然后我们计算所有标定图像的误差平均值。
mean_error = 0
for i in range(len(objpoints)):
    imgpoints2, _ = cv2.projectPoints(objpoints[i], rvecs[i], tvecs[i], mtx, dist)
    error = cv2.norm(imgpoints[i], imgpoints2, cv2.NORM_L2) / len(imgpoints2)
    mean_error += error
print("total error: ", mean_error / len(objpoints))
cv2.waitKey(0)