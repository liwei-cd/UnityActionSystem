# 说明:该插件可以使用代码快速开发常用动作,如移动、旋转、缩放、回调、匀速贝塞尔曲线、缓动动作，帧动画。详细说明如下。

## 一.基础动作
    * LDelay(float time) 
    ** 延迟(time)，配合其他动作使用

    * LMoveBy(float time, float xx=0,float yy=0, float zz=0)
    ** 相对当前移动（time时长）（Vector3(xx,yy,zz)）

    * LMoveTo(float time, float xx=0,float yy=0,float zz=0)
    ** 移动到某位置 (time时长)（Vector3(xx,yy,zz)）
    
    * LRotateBy(float time, float angle,float xx=0,float yy=0,float zz=1)
    ** 相对当前旋转 (time时长) (angle旋转角度)（Vector3(xx,yy,zz)以该轴旋转）

    * LRotateTo(float time, float angle,float xx=0,float yy=0,float zz=1)
    ** 旋转到某角度 (time时长) (angle旋转角度)（Vector3(xx,yy,zz)以该轴旋转）
    
    * LScaleBy(float time, float xx=0,float yy=0, float zz=0)
    ** 相对当前缩放 (time时长)（Vector3(xx,yy,zz)）

    * LScaleTo(float time, float xx=0,float yy=0, float zz=0)
    ** 缩放到 (time时长)（Vector3(xx,yy,zz)）

    * LCallFunc(System.Action<GameObject> dele)
    ** 动作回调，返回调用对象

    * LFollow(GameObject go)
    ** 一直跟随对象移动

    * LBezier2To(float time,Vector3 e,Vector3 p1)
    ** 一个控制点的贝塞尔曲线，e为终点，p1为控制点(默认非匀速，调用.Uniform()为匀速)

    * LBezier3To(float time,Vector3 e,Vector3 p1, Vector3 p2)
    ** 两个控制点的贝塞尔曲线，e为终点，p1,p2为控制点(默认非匀速，调用.Uniform()为匀速)

    * LBezier4To(float time,Vector3 e,Vector3 p1, Vector3 p2, Vector3 p3)
    ** 三个控制点的贝塞尔曲线，e为终点，p1,p2,p3为控制点(默认非匀速，调用.Uniform()为匀速)

## 二.复合动作
    * LRepeat(LAction inner, int times = 1)
    ** 重复某动作，times == -1 时，一直重复该动作

    * LSequence(params LAction[] acts)
    ** 连续动作， 当一个动作执行完毕，执行下一个动作

## 三.缓动动作
    * LEaseIn(LAction inner, float rate)
    ** 开始时缓动 (rate 比率)

    * LEaseOut(LAction inner, float rate)
    ** 结束时缓动 (rate 比率)

    * LEaseInOut(LAction inner, float rate)
    ** 开始结束时缓动 (rate 比率)

    * LEaseBackIn(LAction inner)
    ** 开始时回弹

    * LEaseBackOut(LAction inner, float rate)
    ** 结束时回弹

    * LEaseBackInOut(LAction inner, float rate)
    ** 开始结束时回弹

## 四.帧动画
    * 挂载LAnimator到对象上,配置好帧动画资源和属性
    ** name 动画名字
    ** totalTimes 循环次数， -1为 一直循环
    ** _delay 每一帧间隔
    ** _sprites 帧动画的资源

## 五.控制说明
    * go.runAction(LAction)  
    ** 执行动作

    * go.stopAllActions()    
    ** 停止所有动作

    * go.stopActByTag(int tag) 
    ** 停止某个动作 (可设置LAction.tag = 1)

    * go.pauseActByTag(int tag, bool pause) 
    ** 是否暂停某个动作

    * go.pauseAllActions(bool pause) 
    ** 是否暂停go的所有动作

    * go.runAnim(int tag, int idx)
    ** 执行帧动画，tag为配置帧动画的索引，idx为该动画第几帧开始播放

    * go.pauseAnim(bool pause)
    ** 是否暂停当前帧动画

    * LActionMng.pauseAnim{get,set}
    ** 是否暂停所有帧动画

    * LActionMng.pauseAction{get,set}
    ** 是否暂停所有动作

    * LActionMng.stopAction()
    ** 停止所有动作

## 六.注意事项
    * LActionMng 需要预先挂载到任意对象上，该对象切换场景时不会删除

## Copyright © 2020 liwei   email:289423619@qq.com