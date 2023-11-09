# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：如何判断加载的文件是否存在
# 博客：http://www.bilibili996.com/Course?id=4316440000002
# 作者：高仁宝
# 时间：2023.11

import cv2
import os
import errno

# path = '../images/messi6.jpg'#不正确的路径，文件不存在
path = '../images/messi5.jpg'
if not os.path.exists(path):
    raise FileNotFoundError(errno.ENOENT, os.strerror(errno.ENOENT), path)

img = cv2.imread(path, cv2.IMREAD_UNCHANGED)

cv2.imshow('src', img)
cv2.waitKey(0)
