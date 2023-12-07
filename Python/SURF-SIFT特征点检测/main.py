# OpenCv版本：opencv-python 4.6.0.66
# 内容：SURF/SIFT特征点检测
# 博客：http://www.bilibili996.com/Course?id=2848892000230
# 作者：高仁宝
# 时间：2023.11

import cv2

# 【1】载入图像
srcImage1 = cv2.imread("../images/book2.jpg")
srcImage2 = cv2.imread("../images/book3.jpg")

# 定义一个特征检测类对象
sift = cv2.SIFT_create(400)
# 调用detect函数检测出SIFT特征关键点，保存在vector容器中
keypoints_1 = sift.detect(srcImage1)
keypoints_2 = sift.detect(srcImage2)

# 绘制特征关键点
img_keypoints_1 = srcImage1.copy()
img_keypoints_2 = srcImage2.copy()
img_keypoints_1 = cv2.drawKeypoints(srcImage1, keypoints_1, img_keypoints_1, color=(0,255,0))
img_keypoints_2 = cv2.drawKeypoints(srcImage2, keypoints_2, img_keypoints_2, color=(0,255,0))

# 显示效果图
cv2.imshow("img_keypoints_1", img_keypoints_1)
cv2.imshow("img_keypoints_2", img_keypoints_2)
cv2.waitKey(0)
cv2.destroyAllWindows()