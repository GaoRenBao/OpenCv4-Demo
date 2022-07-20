import cv2

if __name__ == "__main__":
    #读取一张图片
    srcImage=cv2.imread("a.jpg")
    # 创建一个窗口
    cv2.namedWindow("image")
    # 显示图片
    cv2.imshow("image", srcImage)
    # 等待任意输入
    cv2.waitKey(0)

