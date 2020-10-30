using UnityEngine;
using System.Collections.Generic;

namespace LActionSystem{
    
    public class LActionMng : MonoBehaviour
    {
        static bool _pauseAnim = false;
        static bool _pauseAction = false;
        static LActionMng _shared = null;
        static LReuseDeque<LDeque<LAction>> _acts = new LReuseDeque<LDeque<LAction>>();
        static Dictionary<GameObject, LQueNd<LDeque<LAction>>> _actMap = new Dictionary<GameObject, LQueNd<LDeque<LAction>>>();
        
        /** 暂停所有帧动画 */
        public static bool pauseAnim{
            get{return _pauseAnim;} 
            set{_pauseAnim = value;}
        }
        /** 暂停所有动作 */
        public static bool pauseAction{
            get{return _pauseAction;} 
            set{_pauseAction = value;}
        } 

        private void Awake(){
            if(_shared){
                Destroy(gameObject); return;
            }
            _shared = this;
            DontDestroyOnLoad(gameObject);
        }

        /** 播放动作 */
        public static void runAction(GameObject go,LAction act){
            if(act == null || go == null) return;
            LQueNd<LDeque<LAction>> queNds = null;
            if(!_actMap.TryGetValue(go, out queNds)){
                queNds = _acts.push(new LDeque<LAction>());
                _actMap[go] = queNds;
            }
            queNds.d_.push(act);
            act.start(go);
        }

        /** 停止所有动作 */
        public static void stopAction(){
            _acts.clear();
            _actMap.Clear();
        }

        public static void stopAllActions(GameObject go){
            if(go == null) return;
            LQueNd<LDeque<LAction>> queNds = null;
            if(!_actMap.TryGetValue(go, out queNds)) return;
            _acts.delete(queNds);
            _actMap.Remove(go);
        }
        /** 停止动作 */
        public static void stopActByTag(GameObject go, int tag){
            if(go == null) return;
            LQueNd<LDeque<LAction>> queNds = null;
            if(!_actMap.TryGetValue(go, out queNds)) return;
            queNds.d_.round((LAction act)=>{
                return (act.tag == tag);
            });
            if(queNds.d_.begin == null){
                _acts.delete(queNds);
                _actMap.Remove(go);
            }
        }
        /** 暂停恢复动画 */
        public static void pauseActByTag(GameObject go, int tag,bool pause){
            if(go == null) return;
            LQueNd<LDeque<LAction>> queNds = null;
            if(!_actMap.TryGetValue(go, out queNds)) return;
            queNds.d_.round((LAction act)=>{
                if(act.tag == tag) act.pause = pause;
                return false;
            });
        }
        /** 暂停恢复所有动画 */
        public static void pauseAllActions(GameObject go, bool pause){
            if(go == null) return;
            LQueNd<LDeque<LAction>> queNds = null;
            if(!_actMap.TryGetValue(go, out queNds)) return;
            queNds.d_.round((LAction act)=>{
                act.pause = pause;
                return false;
            });
        }

        void FixedUpdate() {
            if(_pauseAction) return;

            GameObject go = null;
            _acts.round((LDeque<LAction> que)=>{
                go = que.begin.d_._target;
                que.round((LAction act)=>{
                    if(act.pause) return false;
                    act.step(Time.fixedDeltaTime);
                    return act.isDone();
                });
                if(que.begin == null){
                    if(go != null) _actMap.Remove(go);
                    return true;
                }
                return false;
            });
        }
    }
}