# OpenCv版本：opencv-python 4.6.0.66
# 内容：双目摄像头与图像拼接
# 博客：http://www.bilibili996.com/Course?id=1048651000244
# 作者：高仁宝
# 时间：2023.11

import cv2

img_1 = cv2.imread('../images/orb (1).jpg')
img_2 = cv2.imread('../images/orb (2).jpg')
cv2.imshow("img_1", img_1)
cv2.imshow("img_2", img_2)

stitcher = cv2.Stitcher_create(cv2.Stitcher_SCANS)
(retval, pano) = stitcher.stitch([img_1, img_2])
if retval != cv2.Stitcher_OK:
    print("Error.")
    cv2.waitKey(0)
    exit(-1)

cv2.imshow("pano", pano)
cv2.waitKey(0)
