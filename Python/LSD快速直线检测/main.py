# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：LSD快速直线检测
# 博客：http://www.bilibili996.com/Course?id=600776a4089d4f99b46a0fd854e8c74d
# 作者：高仁宝
# 时间：2023.11

import cv2

# Read gray image
img0 = cv2.imread("../images/home.jpg")
img = cv2.cvtColor(img0, cv2.COLOR_BGR2GRAY)
cv2.imshow('pokerQ', img0)

# Create default parametrization LSD
lsd = cv2.createLineSegmentDetector(0)

# Detect lines in the image
dlines = lsd.detect(img)  # TODO 返回什么？
lines = lsd.detect(img)[0]  # Position 0 of the returned tuple are the detected lines

# Draw detected lines in the image
drawn_img = lsd.drawSegments(img, lines)

for dline in dlines[0]:
    x0 = int(round(dline[0][0]))
    y0 = int(round(dline[0][1]))
    x1 = int(round(dline[0][2]))
    y1 = int(round(dline[0][3]))
    cv2.line(img0, (x0, y0), (x1, y1), (0, 255, 0), 2, cv2.LINE_AA)
    cv2.imshow("LSD", img0)
    cv2.waitKey(10)

# Show image
cv2.imshow("LSD", drawn_img)
# cv2.imwrite("LSD.jpg", img0)
cv2.waitKey(0)
cv2.destroyAllWindows()
