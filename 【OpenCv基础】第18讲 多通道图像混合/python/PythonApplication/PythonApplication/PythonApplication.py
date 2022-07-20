import numpy as np
import cv2

def MultiChannelBlending():
	#=================【蓝色通道部分】=================
	#	描述：多通道混合-蓝色分量部分
	#============================================

	# 【1】读入图片
	logoImage=cv2.imread('dota_logo.jpg', 0)
	srcImage=cv2.imread('dota_jugg.jpg')

	if logoImage is None:
		print("Oh，no，读取logoImage错误~！ \n")
		return False

	if srcImage is None:
		print("Oh，no，读取srcImage错误~！ \n")
		return False

	#【2】把一个3通道图像转换成3个单通道图像
	ImgB,ImgG,ImgR = cv2.split(srcImage)

	#【3】将原图的蓝色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到ImgB中
	rows,cols = logoImage.shape
	cv2.addWeighted(
		ImgB[250:250+rows,500:500+cols],
		1,logoImage,0.5,0,
		ImgB[250:250+rows,500:500+cols])

	#【4】将三个单通道重新合并成一个三通道
	srcImage=cv2.merge([ImgB, ImgG, ImgR])

	#【5】显示效果图
	cv2.imshow("Blue", srcImage)

	#=================【绿色分量部分】=================
	#	描述：多通道混合-绿色分量部分
	#============================================

	# 【1】读入图片
	logoImage=cv2.imread('dota_logo.jpg', 0)
	srcImage=cv2.imread('dota_jugg.jpg')

	if logoImage is None:
		print("Oh，no，读取logoImage错误~！ \n")
		return False

	if srcImage is None:
		print("Oh，no，读取srcImage错误~！ \n")
		return False

	#【2】把一个3通道图像转换成3个单通道图像
	ImgB,ImgG,ImgR = cv2.split(srcImage)

	#【3】将原图的绿色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到ImgG中
	rows,cols = logoImage.shape
	cv2.addWeighted(
		ImgG[250:250+rows,500:500+cols],
		1,logoImage,0.5,0,
		ImgG[250:250+rows,500:500+cols])

	#【4】将三个单通道重新合并成一个三通道
	srcImage=cv2.merge([ImgB, ImgG, ImgR])

	#【5】显示效果图
	cv2.imshow("Green", srcImage)

	#=================【红色分量部分】=================
	#	描述：多通道混合-红色分量部分
	#============================================

	# 【1】读入图片
	logoImage=cv2.imread('dota_logo.jpg', 0)
	srcImage=cv2.imread('dota_jugg.jpg')

	if logoImage is None:
		print("Oh，no，读取logoImage错误~！ \n")
		return False

	if srcImage is None:
		print("Oh，no，读取srcImage错误~！ \n")
		return False

	#【2】把一个3通道图像转换成3个单通道图像
	ImgB,ImgG,ImgR = cv2.split(srcImage)

	#【3】将原图的红色通道的（500,250）坐标处右下方的一块区域和logo图进行加权操作，将得到的混合结果存到ImgR中
	rows,cols = logoImage.shape
	cv2.addWeighted(
		ImgR[250:250+rows,500:500+cols],
		1,logoImage,0.5,0,
		ImgR[250:250+rows,500:500+cols])

	#【4】将三个单通道重新合并成一个三通道
	srcImage=cv2.merge([ImgB, ImgG, ImgR])

	#【5】显示效果图
	cv2.imshow("Red", srcImage)

MultiChannelBlending()
cv2.waitKey(0)

