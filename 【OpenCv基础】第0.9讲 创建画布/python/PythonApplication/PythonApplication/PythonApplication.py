import numpy as np
import cv2

size = (200, 200)
# 全黑.可以用在屏保
black=np.zeros(size)
print(black[34][56])
cv2.imshow('black',black)

#white 全白
black[:]=255
print(black[34][56])
cv2.imshow('white',black)
cv2.waitKey(0);
​