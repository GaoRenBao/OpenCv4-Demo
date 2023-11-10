# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：LUT 图像灰度调整
# 博客：http://www.bilibili996.com/Course?id=5825870000239
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 查找表，数组的下标对应图片里面的灰度值
# 例如lutData[20]=0;表示灰度为20的像素其对应的值0.
# 可能这样说的不清楚仔细看下代码就清楚了。
a = cv2.imread("../images/1050.jpg")

lut = []
for i in range(256):
    if i <= 120:
        lut.append(0)
    if 120 < i <= 200:
        lut.append(120)
    if i > 200:
        lut.append(255)

lut = np.array(lut).clip(0, 255).astype('uint8')

cv2.imshow("a", a)
b = cv2.LUT(a, lut)
cv2.imshow("b", b)

cv2.waitKey(0)
cv2.destroyAllWindows()
