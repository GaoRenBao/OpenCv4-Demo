# 无法运行

import cv2
import numpy as np

NTRAINING_SAMPLES=100	   # Number of training samples per class
FRAC_LINEAR_SEP=0.9   	   # Fraction of samples which compose the linear separable part


# 创建窗口可视化
WIDTH=512
HEIGHT=512
I=np.ones((HEIGHT, WIDTH, 4), np.uint8) 
trainData=np.ones((2*NTRAINING_SAMPLES, 2, 4), np.uint8) 
labels=np.ones((2*NTRAINING_SAMPLES, 1, 4), np.uint8) 

rng = np.random.RandomState(42) # Random value generation class
rng = rng.randn(100) 

# Set up the linearly separable part of the training data
nLinearSamples=(int)(FRAC_LINEAR_SEP * NTRAINING_SAMPLES)

#! [setup1]
# Generate random points for the class 1
trainClass = trainData.rowRange(0, nLinearSamples);

# The x coordinate of the points is in [0, 0.4)
c = trainClass.colRange(0, 1);
#rng.fill(c, RNG::UNIFORM, Scalar(1), Scalar(0.4 * WIDTH));

# The y coordinate of the points is in [0, 1)
c = trainClass.colRange(1, 2);
#rng.fill(c, RNG::UNIFORM, Scalar(1), Scalar(HEIGHT));

# Generate random points for the class 2
trainClass = trainData.rowRange(2 * NTRAINING_SAMPLES - nLinearSamples, 2 * NTRAINING_SAMPLES);
# The x coordinate of the points is in [0.6, 1]
c = trainClass.colRange(0, 1);
#rng.fill(c, RNG::UNIFORM, Scalar(0.6 * WIDTH), Scalar(WIDTH));
# The y coordinate of the points is in [0, 1)
c = trainClass.colRange(1, 2);
#rng.fill(c, RNG::UNIFORM, Scalar(1), Scalar(HEIGHT));
#! [setup1]

#------------------ Set up the non-linearly separable part of the training data ---------------
#! [setup2]
# Generate random points for the classes 1 and 2
trainClass = trainData.rowRange(nLinearSamples, 2 * NTRAINING_SAMPLES - nLinearSamples);
# The x coordinate of the points is in [0.4, 0.6)
c = trainClass.colRange(0, 1);
#rng.fill(c, RNG::UNIFORM, Scalar(0.4 * WIDTH), Scalar(0.6 * WIDTH));
# The y coordinate of the points is in [0, 1)
c = trainClass.colRange(1, 2);
#rng.fill(c, RNG::UNIFORM, Scalar(1), Scalar(HEIGHT));

#! [setup2]
#------------------------- Set up the labels for the classes ---------------------------------
labels.rowRange(0, NTRAINING_SAMPLES).setTo(1);  # Class 1
labels.rowRange(NTRAINING_SAMPLES, 2 * NTRAINING_SAMPLES).setTo(2);  # Class 2

#------------------------ 2. Set up the support vector machines parameters --------------------
#------------------------ 3. Train the svm ----------------------------------------------------
print("Starting training process")
#创建分类器
svm = cv2.ml.SVM_create()
#设置svm类型
svm.setType(cv2.ml.SVM_C_SVC)
#核函数
svm.setKernel(cv2.ml.SVM_LINEAR)
#训练
svm.train(trainData, cv2.ml.ROW_SAMPLE, labels)
print("Finished training process")

#------------------------ 4. Show the decision regions ----------------------------------------
#! [show]
imgH = I.shape[0]
imgW = I.shape[1]
for i in range(imgH):
    for j in range(imgW):
        arrayTest = np.empty(shape=[1, 2], dtype='float32')
        arrayTest = np.append(arrayTest, [[i,j]], axis = 0)
        pt = np.array(arrayTest , dtype='float32')   
        response = svm.predict(pt)[1] # 进行预测，返回1或-1
        if response[0][0] == 1:
           I[i, j] = [0, 100, 0, 0]
        elif response[0][0] == 2:
           I[i, j] = [100, 0, 0, 0]
#! [show]

#----------------------- 5. Show the training data --------------------------------------------
#! [show_data]
# Class 1
for i in range(NTRAINING_SAMPLES):
    px = trainData.at<float>(i, 0);
    py = trainData.at<float>(i, 1);
    I = cv2.circle(I, (int(px), int(py)), 3, (0, 255, 0), -2, 8 , 0)
# Class 2
for i in range(NTRAINING_SAMPLES, 2 * NTRAINING_SAMPLES):
    px = trainData.at<float>(i, 0);
    py = trainData.at<float>(i, 1);
    I = cv2.circle(I, (int(px), int(py)), 3, (255, 0, 0), -2, 8 , 0)
#! [show_data]

#------------------------- 6. Show support vectors --------------------------------------------
#! [show_vectors]
sv = svm.getUncompressedSupportVectors();
for i in range(sv.rows):
    #const float* v = sv.ptr<float>(i);
    I = cv2.circle(I, (int(sv[0]), int(sv[1])), 6, (128, 128, 128), 2, 8 , 0)
#! [show_vectors]

# imwrite("result.png", I);	                   # save the Image
imshow("SVM for Non-Linear Training Data", I); # show it to the user
waitKey(0);

