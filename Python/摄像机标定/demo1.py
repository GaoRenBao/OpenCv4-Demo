# OpenCv版本：opencv-python 4.6.0.66
# 内容：摄像机标定
# 博客：http://www.bilibili996.com/Course?id=3b7dadd95f6a4508867768c07a59db3e
# 作者：高仁宝
# 时间：2023.11

# OpenCV-Python-Tutorial-中文版.pdf P243

# 其他: 这个函数可能不会找出所有图像中应有的图案。所以一个好的方法是编写代码，启动摄像机并在每一帧中检查是否有应有的图案。
# 在我们获得图案之后我们要找到角点并把它们保存成一个列表。在读取下一帧图像之前要设置一定的间隔，这样我们就有足够的时间调整棋盘的方向。
# 继续这个过程直到我们得到足够多好的图案。
# 就算是我们举得这个例子，在所有的 14 幅图像中也不知道有几幅是好的。所以我们要读取每一张图像从其中找到好的能用的。
#
# 其他: 除了使用棋盘之外，我们还可以使用环形格子，但是要使用函数cv2.findCirclesGrid来找图案。据说使用环形格子只需要很少的图像就可以了。

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
        cv2.waitKey(0)
cv2.destroyAllWindows()
