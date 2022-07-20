import cv2
import numpy as np

# 【1】载入图像
srcImage1 = cv2.imread("1.jpg")
srcImage2 = cv2.imread("2.jpg")

# 定义一个特征检测类对象
sift = cv2.SIFT_create(400)
# 调用detect函数检测出SIFT特征关键点，保存在vector容器中
keypoints_1 = sift.detect(srcImage1)
keypoints_2 = sift.detect(srcImage2)

# 绘制特征关键点
img_keypoints_1 = srcImage1.copy()
img_keypoints_2 = srcImage2.copy()
img_keypoints_1 = cv2.drawKeypoints(srcImage1, keypoints_1, img_keypoints_1, color=(0,0,0))
img_keypoints_2 = cv2.drawKeypoints(srcImage2, keypoints_2, img_keypoints_2, color=(0,0,0))

# 显示效果图
cv2.imshow("img_keypoints_1", img_keypoints_1)
cv2.imshow("img_keypoints_2", img_keypoints_2)
cv2.waitKey(0)
cv2.destroyAllWindows()



