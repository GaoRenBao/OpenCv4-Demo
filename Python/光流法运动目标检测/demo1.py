# OpenCv版本：opencv-python 4.6.0.66
# 内容：光流法运动目标检测
# 博客：http://www.bilibili996.com/Course?id=4918981000067
# 作者：高仁宝
# 时间：2023.11

import cv2

gray_prev = None
points1 = None
points2 = None
st = None


def acceptTrackedPoint(a, b, c):
    return (c == 1) and ((abs(a[0][0] - b[0][0]) - abs(a[0][1] - b[0][1])) > 2)


def swap(a, b):
    return b, a


def tracking(frame):
    global gray_prev, points1, points2, st

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    output = frame.copy()

    # 添加特征点
    points1 = cv2.goodFeaturesToTrack(gray, 500, 0.01, 2)
    initial = points1;

    if gray_prev is None:
        gray_prev = gray.copy()

    # 光流金字塔，输出图二的特征点
    # lk_params = dict(winSize=(15, 15),maxLevel=2,criteria=(cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 10, 0.03))
    # points2, st, err = cv2.calcOpticalFlowPyrLK(gray_prev, gray, points1, None, **lk_params)

    # 单独用这个也可以，感觉没多大差异
    points2, st, err = cv2.calcOpticalFlowPyrLK(gray_prev, gray, points1, None)

    # 去掉一些不好的特征点
    k = 0
    for i in range(0, points2.size):
        if i >= st.size:
            break;
        if acceptTrackedPoint(initial[i], points2[i], st[i]) == True:
            initial[k] = initial[i]
            points2[k] = points2[i]
            k = k + 1

    # 显示特征点和运动轨迹
    # 选择good points
    good_new = initial[st == 1]
    good_old = points2[st == 1]
    # 绘制跟踪框
    for i, (new, old) in enumerate(zip(good_new, good_old)):
        if i >= k:
            break
        a, b = new.ravel()
        c, d = old.ravel()
        output = cv2.line(output, (int(a), int(b)), (int(c), int(d)), (0, 0, 255), 1)
        output = cv2.circle(output, (int(c), int(d)), 3, (0, 255, 0), -1)

    # 把当前跟踪结果作为下一此参考
    points2, points1 = swap(points2, points1)
    gray_prev, gray = swap(gray_prev, gray)
    return output;


if __name__ == "__main__":
    video = cv2.VideoCapture('../images/lol.avi')
    fps = video.get(cv2.CAP_PROP_FPS)
    success = True
    while success:
        # 读帧
        success, frame = video.read()
        if success == False:
            break
        result = tracking(frame)
        cv2.imshow('result', result)  # 显示
        cv2.waitKey(int(1000 / int(fps)))  # 设置延迟时间

    video.release()
