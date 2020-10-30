using UnityEngine;
using System.Collections.Generic;

namespace LActionSystem{

    public class LAnimator : MonoBehaviour 
    {
        [Tooltip("暂停帧动画")] public bool pause = false;
        [Tooltip("所有帧动画")] public List<LAnimate> anims = new List<LAnimate>();
        private LAnimate _curAnim = null;
        /** 播放动画 */
        public void runAnim(int tag,int idx){
            if(tag>=anims.Count) return;
            _curAnim = anims[tag];
            _curAnim.reset(idx);
            pause = false;
        }

        protected virtual void FixedUpdate () {
            if(LActionMng.pauseAnim) return;
            if(pause || _curAnim == null) return;
            if(_curAnim.update(gameObject, Time.fixedDeltaTime)) _curAnim = null;
        }
    }
}