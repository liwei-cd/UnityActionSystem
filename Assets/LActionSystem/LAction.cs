using UnityEngine;

namespace LActionSystem{

    public class LAction
    {
        float _elapsed;
        float _duration;
        public int tag;
        public bool pause;
        public GameObject _target;
        public float duration{get{return _duration;}}

        protected LAction(float time){
            _duration = time<0.01f ? 0.01f : time;
        }
        
        public virtual bool isDone(){
            if(_target == null) return true;
            return _elapsed >= _duration;
        }

        public virtual void start(GameObject go){
            _target = go;
            _elapsed = 0;
            pause = false;
        }
        
        public virtual void step(float dt){
            if(!_target) return;
            _elapsed += dt;
            float perc = _elapsed/_duration;
            update(perc > 1.0f ? 1.0f : perc);
        }

        public virtual void update(float perc){}
    }

    public class LDelay : LAction
    {
        public LDelay(float time):base(time){}
    }

    public class LMoveBy : LAction
    {
        protected Vector3 _delta;
        protected Vector3 _begin;

        public LMoveBy(float time, float xx=0,float yy=0, float zz=0):base(time){
            _delta = new Vector3(xx,yy,zz);
        }

        public override void start(GameObject go){
            base.start(go);
            _begin = go.getPosition();
        }

        public override void update(float perc){
            _target.setPosition(_begin + _delta * perc);
        }
    }

    public class LMoveTo : LMoveBy
    {
        Vector3 _end;

        public LMoveTo(float time, float xx=0,float yy=0,float zz=0):base(time,xx,yy,zz){
            _end = new Vector3(xx,yy,zz);
        }

        public override void start(GameObject go){
            base.start(go);
            _delta = _end - _begin;
        }
    }

    public class LRotateBy : LAction
    {
        protected Vector3 _axis;
        protected float _begin;
        protected float _delta;
        
        public LRotateBy(float time, float angle,float xx=0,float yy=0,float zz=1):base(time){
            _axis = new Vector3(xx,yy,zz);
            _delta = angle;
        }

        public override void start(GameObject go){
            base.start(go);
            _begin = go.getRotate(_axis);
        }

        public override void update(float perc){
            _target.setRotate(_begin+_delta*perc,_axis);
        }
    }

    public class LRotateTo : LRotateBy
    {
        float _end;

        public LRotateTo(float time, float angle,float xx=0,float yy=0, float zz=1):base(time,angle,xx,yy,zz){
            _end = angle;
        }

        public override void start(GameObject go){
            base.start(go);
            _delta = _end - _begin;
        }
    }

    public class LScaleBy : LAction
    {
        protected Vector3 _delta;
        protected Vector3 _begin;
        
        public LScaleBy(float time, float xx=0,float yy=0, float zz=0):base(time){
            _delta = new Vector3(xx,yy,zz);
        }

        public override void start(GameObject go){
            base.start(go);
            _begin = go.getScale();
        }

        public override void update(float perc){
            _target.setScale(_begin + _delta * perc);
        }
    }

    public class LScaleTo : LScaleBy
    {
        Vector3 _end;

        public LScaleTo(float time, float xx=0,float yy=0, float zz=0):base(time,xx,yy,zz){
            _end = new Vector3(xx,yy,zz);
        }

        public override void start(GameObject go){
            base.start(go);
            _delta = _end - _begin;
        }
    }

    public class LRepeat : LAction
    {
        int _times;
        int _total;
        LAction _act;

        public LRepeat(LAction inner, int times = 1):base(1){
            _act = inner;
            _total = times;
        }

        public override void start(GameObject go){
            _times = 0;
            base.start(go);
            _act.start(go);
        }

        public override void step(float dt){
            if(!_target) return;
            _act.step(dt);
            if(_act.isDone()){
                if(_total > -1){
                    if(++_times >= _total){
                        _times = _total; return;
                    }
                }
                _act.start(_target);
            }
        }

        public override bool isDone(){
            if(_target == null) return true;
            return _times == _total;
        }
    }

    public class LSequence : LAction
    {
        int _idx;
        LAction[] _acts;

        public LSequence(params LAction[] acts):base(1){
            _acts = acts;
        }

        public override void start(GameObject go){
            _idx = 0;
            base.start(go);
            _acts[_idx].start(go);
        }

        public override void step(float dt){
            if(!_target) return;
            _acts[_idx].step(dt);
            if(_acts[_idx].isDone()){
                if(++_idx == _acts.Length){
                    return;
                }else{
                    _acts[_idx].start(_target);
                }
            }
        }

        public override bool isDone(){
            if(_target == null) return true;
            return _idx == _acts.Length;
        }
    }

    public class LCallFunc : LAction
    {
        System.Action<GameObject> _dele;

        public LCallFunc(System.Action<GameObject> dele):base(1){
            _dele = dele;
        }

        public override void update(float perc){
            _dele(_target);
        }

        public override bool isDone(){
            return true;
        }
    }

    public class LFollow : LAction 
    {
        Vector3 _delta;
        GameObject _followGo;

        public LFollow(GameObject go):base(1){
            _followGo = go;
        }

        public override void start(GameObject go){
            if(!_followGo) return;
            base.start(go);
            _delta = _followGo.getPosition() - _target.getPosition();
        }

        public override void step(float dt){
            if(!_target || !_followGo) return;
            _target.setPosition(_followGo.getPosition()-_delta);
        }

        public override bool isDone(){
            if(!_target || !_followGo) return true;
            return false;
        }
    }

    public class LBezier2To : LAction
    {
        float _totalLen;
        bool _uniformSpeed;
        protected Vector3 _start,_end,_p1;

        public LBezier2To(float time,Vector3 e,Vector3 p1):base(time){
            _p1 = p1;
            _end = e;
        }

        public override void start(GameObject go){
            _start = go.getPosition();
            _totalLen = getLen(1.0f);
            base.start(go);
        }

        public LBezier2To Uniform(){
            _uniformSpeed = true;
            return this;
        }

        public override void update(float perc){
            if(!_target) return;
            if(_uniformSpeed) perc = invertLen(perc);
            _target.setPosition(calcpt(perc));
        }

        protected float getLen(float t){
            float len = 0;
            int step = Mathf.CeilToInt(500*t);
            for(int i=0;i<step;i++){
                len += getSpeed(i*0.001f)*0.001f;
            }
            return len;
        }

        float invertLen(float t){
            float t1=t,t2=t;
            float l = _totalLen*t;
            for(int i=0;i<150;i++){
                t2=t1-(getLen(t1)-l)/getSpeed(t1);
                if(Mathf.Abs(t1-t2)<0.001f) break;
                t1 = t2;
            }
            return t2;
        }

        protected virtual float getSpeed(float t){
            float x = 2*(t-1)*_start.x+(2-4*t)*_p1.x+2*t*_end.x;
            float y = 2*(t-1)*_start.y+(2-4*t)*_p1.y+2*t*_end.y;
            float z = 2*(t-1)*_start.z+(2-4*t)*_p1.z+2*t*_end.z;
            return Mathf.Sqrt(x*x+y*y+z*z);
        }

        protected virtual Vector3 calcpt(float t){
            float v1 = Mathf.Pow(1-t,2);
            float v2 = 2*t*(1-t);
            float v3 = Mathf.Pow(t,2);
            float x = v1 * _start.x + v2 * _p1.x + v3 * _end.x;
            float y = v1 * _start.y + v2 * _p1.y + v3 * _end.y;
            float z = v1 * _start.z + v2 * _p1.z + v3 * _end.z;
            return new Vector3(x,y,z);
        }
    }

    public class LBezier3To : LBezier2To
    {
        protected Vector3 _p2;

        public LBezier3To(float time,Vector3 e,Vector3 p1,Vector3 p2):base(time,e,p1){
            _p2 = p2;
        }

        protected override float getSpeed(float t){
            float v1 = -3*Mathf.Pow(1-t,2);
            float v2 = (9*t-3)*(t-1);
            float v3 = 6*t-9*Mathf.Pow(t,2);
            float v4 = 3*Mathf.Pow(t,2);
            float x = v1*_start.x+v2*_p1.x+v3*_p2.x+v4*_end.x;
            float y = v1*_start.y+v2*_p1.y+v3*_p2.y+v4*_end.y;
            float z = v1*_start.z+v2*_p1.z+v3*_p2.z+v4*_end.z;
            return Mathf.Sqrt(x*x+y*y+z*z);
        }

        protected override Vector3 calcpt(float t){
            float v1 = Mathf.Pow(1-t,3);
            float v2 = 3*t*Mathf.Pow(1-t,2);
            float v3 = 3*(1-t)*Mathf.Pow(t,2);
            float v4 = Mathf.Pow(t,3);
            float x = v1*_start.x+v2*_p1.x+v3*_p2.x+v4*_end.x;
            float y = v1*_start.y+v2*_p1.y+v3*_p2.y+v4*_end.y;
            float z = v1*_start.z+v2*_p1.z+v3*_p2.z+v4*_end.z;
            return new Vector3(x,y,z);
        }
    }

    public class LBezier4To : LBezier3To
    {
        protected Vector3 _p3;

        public LBezier4To(float time,Vector3 e,Vector3 p1,Vector3 p2,Vector3 p3):base(time,e,p1,p2){
            _p3 = p3;
        }

        protected override float getSpeed(float t){
            float v1 = -4*Mathf.Pow(1-t,3);
            float v2 = 16*Mathf.Pow(1-t,3) - 12*Mathf.Pow(1-t,2);
            float v3 = 12*t*(1-t)*(1-2*t);
            float v4 = 4*Mathf.Pow(t,2)*(3-4*t);
            float v5 = 4*Mathf.Pow(t,3);
            float x = v1*_start.x+v2*_p1.x+v3*_p2.x+v4*_p3.x+v5*_end.x;
            float y = v1*_start.y+v2*_p1.y+v3*_p2.y+v4*_p3.y+v5*_end.y;
            float z = v1*_start.z+v2*_p1.z+v3*_p2.z+v4*_p3.z+v5*_end.z;
            return Mathf.Sqrt(x*x+y*y+z*z);
        }

        protected override Vector3 calcpt(float t){
            float v1 = Mathf.Pow(1-t,4);
            float v2 = 4*t*Mathf.Pow(1-t,3);
            float v3 = 6*Mathf.Pow(t,2)*Mathf.Pow(1-t,2);
            float v4 = 4*(1-t)*Mathf.Pow(t,3);
            float v5 = Mathf.Pow(t,4);
            float x = v1*_start.x+v2*_p1.x+v3*_p2.x+v4*_p3.x+v5*_end.x;
            float y = v1*_start.y+v2*_p1.y+v3*_p2.y+v4*_p3.y+v5*_end.y;
            float z = v1*_start.z+v2*_p1.z+v3*_p2.z+v4*_p3.z+v5*_end.z;
            return new Vector3(x,y,z);
        }
    }

    public class LEaseAction : LAction
    {
        protected float _rate;
        protected LAction _act;

        protected LEaseAction(LAction inner, float rate):base(inner.duration){
            _act = inner;
            _rate = rate;
        } 

        public override void start(GameObject go){
            base.start(go);
            _act.start(go);
        }

        public override void update(float perc){
            _act.update(between(perc));
        }

        protected virtual float between(float perc){
            return perc;
        }
    }

    public class LEaseIn : LEaseAction
    {
        public LEaseIn(LAction inner, float rate):base(inner,rate){}

        protected override float between(float perc){
            return Mathf.Pow(perc, _rate);
        }
    }

    public class LEaseOut : LEaseAction
    {
        public LEaseOut(LAction inner, float rate):base(inner,rate){}

        protected override float between(float perc){
            return Mathf.Pow(perc, 1/_rate);
        }
    }

    public class LEaseInOut : LEaseAction
    {
        public LEaseInOut(LAction inner, float rate):base(inner,rate){}

        protected override float between(float perc){
            perc *= 2;
            if(perc < 1) return 0.5f * Mathf.Pow(perc, _rate);
            return (1.0f - 0.5f * Mathf.Pow(2-perc, _rate));
        }
    }

    public class LEaseBackIn : LEaseAction
    {
        public LEaseBackIn(LAction inner):base(inner,1){}

        protected override float between(float perc){
            float overshoot = 1.70158f;
            return perc * perc * ((overshoot + 1) * perc - overshoot);
        }
    }

    public class LEaseBackOut : LEaseAction
    {
        public LEaseBackOut(LAction inner):base(inner,1){}

        protected override float between(float perc){
            perc -= 1;
            float overshoot = 1.70158f;
            return perc * perc * ((overshoot + 1) * perc + overshoot) + 1;
        }
    }

    public class LEaseBackInOut : LEaseAction
    {
        public LEaseBackInOut(LAction inner):base(inner,1){}

        protected override float between(float perc){
            float overshoot = 1.70158f * 1.525f;
            perc = perc * 2;
            if (perc < 1){
                return (perc * perc * ((overshoot + 1) * perc - overshoot)) / 2;
            }else{
                perc = perc - 2;
                return (perc * perc * ((overshoot + 1) * perc + overshoot)) / 2 + 1;
            }
        }
    }

    [System.Serializable]
    public class LAnimate
    {
        [Tooltip("动画名字")]
        public string name = "";
        [Tooltip("循环次数，-1 一直循环")]
        public int totalTimes = -1;
        [Tooltip("每一帧间隔")]
        public float _delay = 0.02f;
        [Tooltip("序列帧精灵")]
        public Sprite[] _sprites;
        public System.Action<GameObject> _dele = null;
        private SpriteRenderer _render = null;
        private int _index = 0;
        private float _elapsed = 0;
        private int _repeat = 1;

        public void reset(int idx=0){
            _index = idx;
            _elapsed = 0;
            _repeat = totalTimes;
            if(idx>0 && idx>_sprites.Length-1) _index %= _sprites.Length;
        }

        public bool update(GameObject obj,float dt){
            _elapsed += dt;
            if(_elapsed < _delay) return false;
            if(++_index >= _sprites.Length){
                if(_repeat > 0) _repeat--;
                if(_repeat == 0){
                    if(_dele != null) _dele(obj);
                    return true;
                }else{
                    _index = 0;
                }
            }
            _elapsed = 0;
            if(_render == null) _render = obj.GetComponent<SpriteRenderer>();
            _render.sprite = _sprites[_index];
            return false;
        }
    }

}