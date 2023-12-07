# OpenCv版本：opencv-python 3.4.2.16
# 内容：高动态范围成像（HDRI或HDR）
# 博客：http://www.bilibili996.com/Course?id=ccb32eb54dd748f9a4fc137544039d51
# 作者：高仁宝
# 时间：2023.11

"""
opencv-python版本必须大于3.4.2.16（不含），createTonemapDurand被作者生气专利了，高版本无法使用

安装包下载：
https://pypi.tuna.tsinghua.edu.cn/simple/opencv-python/
https://pypi.tuna.tsinghua.edu.cn/simple/opencv-contrib-python/

然后离线安装：
pip install numpy==1.14.5 -i https://mirror.baidu.com/pypi/simple
pip install opencv_python-3.4.2.16-cp37-cp37m-win_amd64.whl opencv_contrib_python-3.4.2.16-cp37-cp37m-win_amd64.whl
"""

import cv2
import numpy as np
from re import match

# 通过这个代码，可以查看opencv中是否包含createTonemapDurand
cv2_filtered = filter(lambda v: match('.*Tonemap', v), dir(cv2))
[print(val) for val in cv2_filtered]

# 第一阶段只是将所有图像加载到列表中。此外，我们将需要常规HDR算法的曝光时间。注意数据类型，因为图像应为1通道或3通道8位（np.uint8），曝光时间需要为float32，以秒为单位。
# Loading exposure images into a list
img_fn = ["../images/tbs/1tl.jpg", "../images/tbs/2tr.jpg", "../images/tbs/3bl.jpg", "../images/tbs/4br.jpg"]
img_list = [cv2.imread(fn) for fn in img_fn]
exposure_times = np.array([15.0, 2.5, 0.25, 0.0333], dtype=np.float32)

# Merge exposures to HDR image
# 在这个阶段，我们将曝光序列合并成一个HDR图像，显示了我们在OpenCV中的两种可能性。第一种方法是Debvec，第二种是Robertson。请注意，HDR图像的类型为float32，而不是uint8，因为它包含所有曝光图像的完整动态范围。

merge_debvec = cv2.createMergeDebevec()
hdr_debvec = merge_debvec.process(img_list, times=exposure_times.copy())
merge_robertson = cv2.createMergeRobertson()
hdr_robertson = merge_robertson.process(img_list, times=exposure_times.copy())

# Tonemap HDR image
# 我们将32位浮点HDR数据映射到范围[0..1]。实际上，在某些情况下，值可能大于1或低于0，所以注意我们以后不得不剪切数据，以避免溢出。
tonemap1 = cv2.createTonemapDurand(gamma=2.2)
res_debvec = tonemap1.process(hdr_debvec.copy())
tonemap2 = cv2.createTonemapDurand(gamma=1.3)
res_robertson = tonemap2.process(hdr_robertson.copy())

# Exposure fusion using Mertens
# 这里我们展示了一种可以合并曝光图像的替代算法，我们不需要曝光时间。我们也不需要使用任何tonemap算法，因为Mertens算法已经给出了[0..1]范围内的结果。
merge_mertens = cv2.createMergeMertens()
res_mertens = merge_mertens.process(img_list)

# Convert datatype to 8-bit and save
# 为了保存或显示结果，我们需要将数据转换为[0..255]范围内的8位整数。
res_debvec_8bit = np.clip(res_debvec * 255, 0, 255).astype('uint8')
res_robertson_8bit = np.clip(res_robertson * 255, 0, 255).astype('uint8')
res_mertens_8bit = np.clip(res_mertens * 255, 0, 255).astype('uint8')

cv2.imshow("ldr_debvec.jpg", res_debvec_8bit)
cv2.imshow("ldr_robertson.jpg", res_robertson_8bit)
cv2.imshow("fusion_mertens.jpg", res_mertens_8bit)
cv2.waitKey()
exit(0)

# Estimate camera response function (CRF)
# 相机响应功能（CRF）给出了场景辐射度与测量强度值之间的连接。如果在一些计算机视觉算法中非常重要，包括HDR算法，CRF。这里我们估计反相机响应函数并将其用于HDR合并。
cal_debvec = cv2.createCalibrateDebevec()
crf_debvec = cal_debvec.process(img_list, times=exposure_times)
hdr_debvec = merge_debvec.process(img_list, times=exposure_times.copy(), response=crf_debvec.copy())
cal_robertson = cv2.createCalibrateRobertson()
crf_robertson = cal_robertson.process(img_list, times=exposure_times)
hdr_robertson = merge_robertson.process(img_list, times=exposure_times.copy(), response=crf_robertson.copy())

