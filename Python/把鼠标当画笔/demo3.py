# OpenCv版本：opencv-python 4.6.0.66
# 内容：把鼠标当画笔
# 博客：http://www.bilibili996.com/Course?id=3993445000251
# 作者：高仁宝
# 时间：2023.11

import cv2


def click_event(event, x, y, flags, param):
    if event == cv2.EVENT_LBUTTONDOWN:
        print(x, y)

    if event == cv2.EVENT_RBUTTONDOWN:
        red = img[y, x, 2]
        blue = img[y, x, 0]
        green = img[y, x, 1]
        print(red, green, blue)

        strRGB = str(red) + "," + str(green) + "," + str(blue)
        font = cv2.FONT_HERSHEY_SIMPLEX
        cv2.putText(img, strRGB, (x, y), font, 1, (255, 255, 255), 2)
        cv2.imshow('original', img)


img = cv2.imread('../images/messi5.jpg')
cv2.imshow('original', img)

cv2.setMouseCallback("original", click_event)
cv2.waitKey(0)
cv2.imshow('putText.jpg', img)

cv2.destroyAllWindows()
