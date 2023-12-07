# OpenCv版本：opencv-python 4.6.0.66
# 内容：计算摄影学-图像去噪
# 博客：http://www.bilibili996.com/Course?id=4faa3bccc8084d3498b91a610fec4ecb
# 作者：高仁宝
# 时间：2023.11

"""
fastNlMeansDenoisingColored
"""

import cv2
from matplotlib import pyplot as plt

img = cv2.imread('../images/die.png')
img = cv2.cvtColor(img, code=cv2.COLOR_BGR2RGB)

dst = cv2.fastNlMeansDenoisingColored(img, None, 10, 10, 7, 21)
# dst2=cv2.cvtColor(dst,code=cv2.COLOR_BGR2RGB)

plt.subplot(121), plt.imshow(img)
plt.subplot(122), plt.imshow(dst)
# plt.subplot(122), plt.imshow(dst2)
plt.show()



