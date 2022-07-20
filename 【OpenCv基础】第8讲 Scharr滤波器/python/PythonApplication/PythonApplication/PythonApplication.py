import cv2
import numpy as np

def zh_ch(string):    
	return string.encode("gbk").decode(errors="ignore")

# 【1】载入原始图  
src=cv2.imread('1.jpg')

# 【2】显示原始图 
cv2.imshow(zh_ch('原始图'), src)

# 【3】求 X方向梯度
grad_x = cv2.Scharr(src, cv2.CV_16S, 1, 0, None, 1, 0, cv2.BORDER_DEFAULT)
abs_grad_x = cv2.convertScaleAbs(grad_x)
cv2.imshow(zh_ch('X方向Scharr'), abs_grad_x)

# 【4】求 Y方向梯度
grad_y = cv2.Scharr(src, cv2.CV_16S, 0, 1 ,None, 1, 0, cv2.BORDER_DEFAULT)
abs_grad_y = cv2.convertScaleAbs(grad_y)
cv2.imshow(zh_ch('Y方向Scharr'), abs_grad_y)

# 【5】合并梯度(近似)
dst = cv2.addWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0)

# 【6】显示效果图 
cv2.imshow(zh_ch('【效果图】合并梯度后Scharr'), dst)
cv2.waitKey(0)
cv2.destroyAllWindows()


