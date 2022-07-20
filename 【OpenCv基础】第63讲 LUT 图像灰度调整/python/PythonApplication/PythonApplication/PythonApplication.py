import cv2
import numpy as np

#查找表，数组的下标对应图片里面的灰度值
#例如lutData[20]=0;表示灰度为20的像素其对应的值0.
#可能这样说的不清楚仔细看下代码就清楚了。
a = cv2.imread("b.jpg")

lut = []
for i in range(256):
    if i <= 120 :
        lut.append(0)
    if i > 120 and i <= 200 :
        lut.append(120)
    if i > 200 :
        lut.append(255)

lut = np.array(lut).clip(0,255).astype('uint8')

cv2.imshow("a", a)
b = cv2.LUT(a, lut)
cv2.imshow("b", b)

cv2.waitKey(0)
cv2.destroyAllWindows()



