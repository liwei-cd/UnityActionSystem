using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LActionSystem{

    public static class LGameObjectExt
    {
        #region Private Methods
        
        public static void setPosition(this GameObject go,Vector3 vec){
            go.transform.localPosition = vec;
        }
        public static Vector3 getPosition(this GameObject go){
            return go.transform.localPosition;
        }
        public static void setScale(this GameObject go, Vector3 vec){
            go.transform.localScale = vec;
        }
        public static Vector3 getScale(this GameObject go){
            return go.transform.localScale;
        }
        public static void setRotate(this GameObject go, float angle, Vector3 vec){
            go.transform.localRotation = Quaternion.AngleAxis(angle,vec);
        }
        public static float getRotate(this GameObject go, Vector3 vec){
            Vector3 euler = go.transform.eulerAngles;
            if(euler.x < 0) euler.x += 360;
            if(euler.y < 0) euler.y += 360;
            if(euler.z < 0) euler.z += 360;
            Quaternion ang1 = Quaternion.Euler(euler);
            Quaternion ang2 = Quaternion.AngleAxis(0,vec);
            Quaternion res = ang1 * ang2;
            return Mathf.Acos(res.w) * 114.59156f;
        }
        public static void runAction(this GameObject go,LAction act){
            LActionMng.runAction(go, act);
        }
        public static void stopAllActions(this GameObject go){
            LActionMng.stopAllActions(go);
        }
        public static void stopActByTag(this GameObject go, int tag){
            LActionMng.stopActByTag(go, tag);
        }
        public static void pauseActByTag(this GameObject go, int tag, bool pause){
            LActionMng.pauseActByTag(go, tag, pause);
        }
        public static void pauseAllActions(this GameObject go, bool pause){
            LActionMng.pauseAllActions(go, pause);
        }
        public static void runAnim(this GameObject go,int tag, int idx){
            LAnimator mn = go.GetComponent<LAnimator>();
            if(mn == null) return;
            mn.runAnim(tag, idx);
        }
        public static void pauseAnim(this GameObject go,bool pause){
            LAnimator mn = go.GetComponent<LAnimator>();
            if(mn == null) return;
            mn.pause = pause;
        }

        #endregion
    }
}