import cv2
import numpy as np
import random

def zh_ch(string):    
	return string.encode("gbk").decode(errors="ignore")

##############################【演示1】##################################

srcImage = cv2.imread("1.jpg")
# 图像灰度图转化并平滑滤波
grayImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
# 均值滤波
grayImage = cv2.blur(grayImage, (3,3))
cv2.imshow(zh_ch("原始图"), grayImage)

# 二值化
ret, threshold_output = cv2.threshold(grayImage, 200, 255, cv2.THRESH_BINARY)

# 寻找图像轮廓
contours, hierarchy  = cv2.findContours(threshold_output,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)

hull = []
# 寻找图像凸包
for i in range(len(contours)):
    hull.append(cv2.convexHull(contours[i], False))

# 初始化一个空白图像
drawing = np.zeros((threshold_output.shape[0], threshold_output.shape[1], 3), np.uint8)

# 绘制轮廓和凸包
for i in range(len(contours)):
    # 绘制轮廓
    cv2.drawContours(drawing, contours, i, (0, 255, 0), 1, 8, hierarchy)
    # 绘制凸包
    cv2.drawContours(drawing, hull, i, (255, 0, 0), 1, 8)

# 显示图片
cv2.imshow("drawing", drawing)
cv2.waitKey(0)

##############################【演示2】##################################

thresh = 80

def on_ThreshChange(x):
    global thresh, grayImage, drawing

    # 获取滑动条的值
    thresh = cv2.getTrackbarPos('thresh', 'drawing')

    # 二值化
    ret, threshold_output = cv2.threshold(grayImage, thresh, 255, cv2.THRESH_BINARY)

    # 寻找图像轮廓
    contours, hierarchy  = cv2.findContours(threshold_output,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)

    # 寻找图像凸包
    hull = []
    for i in range(len(contours)):
        hull.append(cv2.convexHull(contours[i], False))

    # 初始化一个空白图像
    drawing = np.zeros((threshold_output.shape[0], threshold_output.shape[1], 3), np.uint8)

    # 绘制轮廓和凸包
    for i in range(len(contours)):
        # 绘制轮廓
        cv2.drawContours(drawing, contours, i, (0, 255, 0), 1, 8, hierarchy)
        # 绘制凸包
        cv2.drawContours(drawing, hull, i, (255, 0, 0), 1, 8)

srcImage = cv2.imread("1.jpg")
# 图像灰度图转化并平滑滤波
grayImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)

# 初始化一个空白图像
drawing = np.zeros((srcImage.shape[0], srcImage.shape[1], 3), np.uint8)

# 均值滤波
grayImage = cv2.blur(grayImage, (3,3))
cv2.imshow(zh_ch("原始图"), grayImage)

# 创建窗口
cv2.namedWindow('drawing')
cv2.createTrackbar('thresh', 'drawing', 0, 255, on_ThreshChange)
cv2.setTrackbarPos('thresh', 'drawing', 80)

while (True):
    cv2.imshow("drawing", drawing)
    if cv2.waitKey(10) == ord('q'):
        break
cv2.destroyAllWindows()

##############################【演示3】##################################

while (True):

    # 随机生成点的数量
    count = random.randint(30,50) 

    point = [] 
    for i in range(count): 
        x = random.randint(600 / 4, 600 * 3 / 4) 
        y = random.randint(600 / 4, 600 * 3 / 4) 
        point.append([x,y])

    points = np.array(point, np.int32) # 点值

    # 寻找图像凸包
    hull = cv2.convexHull(points, False)

    # 绘制出随机颜色的点
    image = np.zeros((600, 600, 3), np.uint8)
    for i in range(count): 
        cv2.circle(image,(points[i][0],points[i][1]),3, (random.randint(0,255), random.randint(0,255), random.randint(0,255)),-1)


    # 准备参数
    point0 = hull[len(hull) - 1] # 连接凸包边的坐标点

    # 绘制轮廓和凸包
    for i in range(len(hull)):
        point = hull[i]
        image = cv2.line(image, (point0[0][0], point0[0][1]), 
            (point[0][0],point[0][1]), (random.randint(0,255), random.randint(0,255), random.randint(0,255)), 2)
        point0 = point

    cv2.imshow("image", image)
    cv2.waitKey(0)


