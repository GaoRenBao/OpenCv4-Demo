import os
import cv2
import numpy as np

#读取图片和缩放图片
srcImage1=cv2.imread('11.jpg')
srcImage2=cv2.imread('2.jpg')

#创建SIFT
sift = cv2.SIFT_create(400)
#计算特征点和描述点
(keypoints_object, descriptors_object) = sift.detectAndCompute(srcImage1, None)
(keypoints_scene, descriptors_scene) = sift.detectAndCompute(srcImage2, None)

# 使用FLANN匹配算子进行匹配
#matcher = cv2.BFMatcher() #建立匹配关系
#matches=matcher.match(descriptors_object,descriptors_scene) #匹配描述子

matcher=cv2.FlannBasedMatcher()
matches=matcher.match(descriptors_object,descriptors_scene)

# 最小距离和最大距离
max_dist = 0
min_dist = 100
for i in range(descriptors_object.shape[1]):
    dist = matches[i].distance
    if dist < min_dist:
       min_dist = dist;
    if dist > max_dist:
       max_dist = dist;

print(">Max dist 最大距离 :", max_dist);
print(">Min dist 最小距离 :", min_dist);

good_matches = []
for i in range(descriptors_object.shape[1]):
    if matches[i].distance < 3 * min_dist:
        good_matches.append(matches[i])

#绘制出匹配到的关键点
img_matches = cv2.drawMatches(srcImage1, keypoints_object, srcImage2, keypoints_scene, good_matches, None) 

#当匹配项大于4时
if len(good_matches)>=4:
    #查找单应性矩阵
    #转换为n行的元素，每一行一个元素，并且这个元素由两个值组成
    obj=np.float32([keypoints_object[m.queryIdx].pt for m in good_matches]).reshape(-1,1,2)
    scene=np.float32([keypoints_scene[m.trainIdx].pt for m in good_matches]).reshape(-1,1,2)

    #获取单应性矩阵
    H,_=cv2.findHomography(obj,scene,cv2.RANSAC)

    #要搜索的图的四个角点
    h, w = srcImage1.shape[0:2]
    obj_corners=np.float32([[0,0],[w,0],[w,h],[0,h]]).reshape(-1,1,2)
    scene_corners=cv2.perspectiveTransform(obj_corners,H)

    scene_corners[0][0][0] += w;
    scene_corners[1][0][0] += w;
    scene_corners[2][0][0] += w;
    scene_corners[3][0][0] += w;

    #绘制多边形
    cv2.polylines(img_matches,pts=[np.int32(scene_corners)],isClosed=True,color=(255, 0, 123), thickness = 4)
 
cv2.imshow("img_matches", img_matches)
cv2.waitKey(0)

