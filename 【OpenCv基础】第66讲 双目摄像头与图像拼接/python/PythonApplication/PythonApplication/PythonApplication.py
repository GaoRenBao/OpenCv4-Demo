import cv2
import numpy as np
import matplotlib.pyplot as plt

img_1 = cv2.imread('1.jpg')
img_2 = cv2.imread('2.jpg')
cv2.imshow("img_1", img_1)
cv2.imshow("img_2",img_2)

stitcher = cv2.Stitcher_create(cv2.Stitcher_SCANS)
(retval, pano) = stitcher.stitch([img_1,img_2])
if retval != cv2.Stitcher_OK:
    print("Error.")
    cv2.waitKey(0)
    sys.exit(-1)

cv2.imshow("pano",pano)
cv2.waitKey(0)


 

