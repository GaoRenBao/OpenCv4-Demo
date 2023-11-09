# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：凸缺陷/凸包检测
# 博客：http://www.bilibili996.com/Course?id=3128323000263
# 作者：高仁宝
# 时间：2023.11

import cv2

img = cv2.imread('../images/star3.jpg')
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

ret, thresh = cv2.threshold(img_gray, 127, 255, 0)
contours, hierarchy = cv2.findContours(thresh, 2, 1)

cnt = contours[0]

# 函数 cv2.isContourConvex() 可以可以用来检测一个曲线是不是凸的。它只能返回 True 或 False。
k = cv2.isContourConvex(cnt)
print("K=", k)

# 函数 cv2.convexHull() 可以用来检测一个曲线的凸包
hull = cv2.convexHull(cnt, returnPoints=False)
# 将所有凸包提取出来
defects = cv2.convexityDefects(cnt, hull)

for i in range(defects.shape[0]):
    s, e, f, d = defects[i, 0]
    start = tuple(cnt[s][0])
    end = tuple(cnt[e][0])
    far = tuple(cnt[f][0])
    cv2.line(img, start, end, [0, 255, 0], 2)
    cv2.circle(img, far, 5, [0, 0, 255], -1)
cv2.imshow('img', img)
cv2.imwrite('out.jpg', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
