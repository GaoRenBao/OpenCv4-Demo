# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：BRIEF描述符
# 博客：http://www.bilibili996.com/Course?id=c0c16b9f18ad4b5da127eb7792333229
# 作者：高仁宝

import cv2

img = cv2.imread('../images/blox.jpg', 0)

# 创建BRIEF描述符提取器
brief = cv2.xfeatures2d.BriefDescriptorExtractor_create()

# Initiate FAST detector
star = cv2.xfeatures2d.StarDetector_create()

# Initiate BRIEF extractor
brief = cv2.xfeatures2d.BriefDescriptorExtractor_create()

# find the keypoints with STAR
kp = star.detect(img, None)
# compute the descriptors with BRIEF
kp, des = brief.compute(img, kp)

print(brief.descriptorSize())
print(des.shape)

