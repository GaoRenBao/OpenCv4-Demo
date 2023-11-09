# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：多对象模板匹配
# 博客：http://www.bilibili996.com/Course?id=bd204811ae5949549f1a2fbeedaeb470
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

img_rgb = cv2.imread('../images/mario.png')
img_gray = cv2.cvtColor(img_rgb, cv2.COLOR_BGR2GRAY)
template = cv2.imread('../images/mario_coin.png', 0)
w, h = template.shape[::-1]

res = cv2.matchTemplate(img_gray, template, cv2.TM_CCOEFF_NORMED)
threshold = 0.8
loc = np.where(res >= threshold)
print(len(loc))

for pt in zip(*loc[::-1]):
    cv2.rectangle(img_rgb, pt, (pt[0] + w, pt[1] + h), (0, 255, 0), 2)
    print("rectangle 1")

cv2.imwrite('res.jpg',img_rgb)
cv2.imshow("result", img_rgb)
cv2.waitKey(0)

