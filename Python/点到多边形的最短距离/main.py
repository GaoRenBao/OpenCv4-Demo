# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：点到多边形的最短距离
# 博客：http://www.bilibili996.com/Course?id=3657332000262
# 作者：高仁宝
# 时间：2023.11

import cv2

srcImage = cv2.imread('../images/lightning.png')
img = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
ret, thresh = cv2.threshold(img, 127, 255, 0)
contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
print('contours len:', len(contours))
cnt = contours[0]

point = (50, 50)
cv2.circle(srcImage, point, 5, (0, 0, 255), -1)
cv2.imshow("srcImage", srcImage)

dist = cv2.pointPolygonTest(cnt, point, True)
print("距离：", dist)

cv2.waitKey(0)
cv2.destroyAllWindows()
