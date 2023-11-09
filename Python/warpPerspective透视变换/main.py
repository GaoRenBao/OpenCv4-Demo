# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：warpPerspective透视变换
# 博客：http://www.bilibili996.com/Course?id=1854259000227
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

img = cv2.imread('../images/Astral3.jpg')
h, w = img.shape[0:2]

# 起始坐标
org = np.array([[w * 0.2, h * 0.2],
                [w * 0.8, h * 0.2],
                [w * 0.8, h * 0.8],
                [w * 0.2, h * 0.8]], np.float32)
# 目标坐标
dst = np.array([[0, 0],
                [w, 0],
                [w, h],
                [0, h]], np.float32)

cv2.line(img, (int(org[0][0]), int(org[0][1])), (int(org[1][0]), int(org[1][1])), (0, 0, 255), 4)
cv2.line(img, (int(org[1][0]), int(org[1][1])), (int(org[2][0]), int(org[2][1])), (0, 0, 255), 4)
cv2.line(img, (int(org[2][0]), int(org[2][1])), (int(org[3][0]), int(org[3][1])), (0, 0, 255), 4)
cv2.line(img, (int(org[3][0]), int(org[3][1])), (int(org[0][0]), int(org[0][1])), (0, 0, 255), 4)
cv2.imshow("img", img)

warpR = cv2.getPerspectiveTransform(org, dst)
result = cv2.warpPerspective(img, warpR, (w, h))
cv2.imshow("result", result)
cv2.waitKey(0)
cv2.destroyAllWindows()

