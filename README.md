# Note: the plug-in can use code to quickly develop common actions, such as move, rotate, zoom, callback, uniform Bezier curve, slow motion action, frame animation. The details are as follows. 
# [中文说明](./README_cn.md)

## 一.Basic action
    * LDelay(float time) 
    ** Time delay, used with other actions

    * LMoveBy(float time, float xx=0,float yy=0, float zz=0)
    ** Relative to current movement（time is duration）（Vector3(xx,yy,zz)）

    * LMoveTo(float time, float xx=0,float yy=0,float zz=0)
    ** Move to a location (time is duration)（Vector3(xx,yy,zz)）
    
    * LRotateBy(float time, float angle,float xx=0,float yy=0,float zz=1)
    ** Relative to the current rotation (time is duration) (angle is rotate angle)（Vector3(xx,yy,zz)Rotate on this axis）

    * LRotateTo(float time, float angle,float xx=0,float yy=0,float zz=1)
    ** Rotate to an angle (time is duration) (angle is rotate angle)（Vector3(xx,yy,zz)Rotate on this axis）
    
    * LScaleBy(float time, float xx=0,float yy=0, float zz=0)
    ** Relative to current scaling (time is duration)（Vector3(xx,yy,zz)）

    * LScaleTo(float time, float xx=0,float yy=0, float zz=0)
    ** Zoom to a scaling (time is duration)（Vector3(xx,yy,zz)）

    * LCallFunc(System.Action<GameObject> dele)
    ** Action callback to return the calling object

    * LFollow(GameObject go)
    ** Follow the object all the time

    * LBezier2To(float time,Vector3 e,Vector3 p1)
    ** Bezier curve of a control point, e is the end point, p1 is the control point
        (default non-uniform speed, call .Uniform() for uniform speed)

    * LBezier3To(float time,Vector3 e,Vector3 p1, Vector3 p2)
    ** Bezier curve of two control points, e is the end point, P1, P2 are control points
        (default non-uniform speed, call .Uniform() for uniform speed)

    * LBezier4To(float time,Vector3 e,Vector3 p1, Vector3 p2, Vector3 p3)
    ** Bezier curve of three control points, e is the end point, P1, P2, P3 are control points
        (default non-uniform speed, call .Uniform() for uniform speed)


## 二.Compound action
    * LRepeat(LAction inner, int times = 1)
    ** Repeat an action, times = = - 1, repeat the action all the time

    * LSequence(params LAction[] acts)
    ** Continuous action, when one action is finished, the next action is executed

## 三.Slow motion
    * LEaseIn(LAction inner, float rate)
    ** Slow start (rate ratio)

    * LEaseOut(LAction inner, float rate)
    ** End jog (rate ratio)

    * LEaseInOut(LAction inner, float rate)
    ** Slow motion at start and end (rate ratio)

    * LEaseBackIn(LAction inner)
    ** Rebound at the beginning

    * LEaseBackOut(LAction inner, float rate)
    ** Rebound at end

    * LEaseBackInOut(LAction inner, float rate)
    ** Rebound at start and end

## 四. Frame animation
    * Mount LAnimator.cs to the object, and configure the frame animation resources and properties
    ** name (Name animation name)
    ** totalTimes (number of cycles, - 1 is a continuous cycle)
    ** _delay (every frame interval)
    ** _sprites (Resources of sprites frame animation)

## 五.Control description
    * go.runAction(LAction)  
    ** Perform the action

    * go.stopAllActions()    
    ** Stop one gameObject's all actions

    * go.stopActByTag(int tag) 
    ** Stop an action (can be set LAction.tag = 1)

    * go.pauseActByTag(int tag, bool pause) 
    ** Do you want to pause an action

    * go.pauseAllActions(bool pause) 
    ** Do you want to pause all actions of go

    * go.runAnim(int tag, int idx)
    ** Execute the frame animation, tag is the index of the configuration frame animation, 
        and idx is the frame of the animation to start playing

    * go.pauseAnim(bool pause)
    ** Pause the current frame animation

    * LActionMng.pauseAnim{get,set}
    ** Do you want to pause all frame animation

    * LActionMng.pauseAction{get,set}
    ** Do you want to pause all actions

    * LActionMng.stopAction()
    ** Stop all actions

## 六.Precautions
    * LActionMng.cs needs to be pre mounted on any object, which will not be deleted when switching scenes

## Copyright © 2020 liwei   email:289423619@qq.com