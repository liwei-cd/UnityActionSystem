using System;

namespace LActionSystem{

    public class LQueNd<T>{
        public T d_;
        public LQueNd<T> l_;
        public LQueNd<T> r_;
        public LQueNd(T d){d_ = d;}
        public static LQueNd<T> operator++(LQueNd<T> nd){
            return nd = nd.r_;
        }
    }

    public class LQueue<T>{
        protected LQueNd<T> root_ = null;
        protected LQueNd<T> last_ = null;
        
        public void push(T d){
            pushNd(new LQueNd<T>(d));
        }
        public T pop(){
            LQueNd<T> nd = popNd();
            if(nd != null) return nd.d_;
            return default(T);
        }
        virtual public void pushNd(LQueNd<T> nd){
            if(root_ == null) root_ = nd;
            else last_.r_ = nd;
            nd.r_ = null;  last_ = nd;
        }
        virtual public LQueNd<T> popNd(){
            if(root_ == null) return null;
            LQueNd<T> nd = root_;
            root_ = root_.r_;
            if(root_ == null) last_ = null;
            return nd;
        }
        public void clear(){
            root_ = last_ = null;
        }
    }

    public class LReuseQue<T>{
        LQueue<T> que_ = new LQueue<T>();
        LQueue<T> reuse_ = new LQueue<T>();
        
        public void push(T d){
            LQueNd<T> nd = reuse_.popNd();
            if(nd == null) nd = new LQueNd<T>(d);
            else nd.d_ = d;
            que_.pushNd(nd);
        }
        public T pop(){
            LQueNd<T> nd = que_.popNd();
            if(nd == null) return default(T);
            T d = nd.d_; nd.d_ = default(T);
            reuse_.pushNd(nd);
            return d;
        }
        public void clear(){
            que_.clear();
            reuse_.clear();
        }
    }

    public class LDeque<T> : LQueue<T>{
        public LQueNd<T> begin{get{return root_;}}
        override public void pushNd(LQueNd<T> nd){
            if(root_ == null){
                root_ = nd; 
                nd.l_ = null;
            }else{
                last_.r_ = nd;
                nd.l_ = last_;
            }
            last_ = nd;
            nd.r_ = null;
        }
        override public LQueNd<T> popNd(){
            if(root_ == null) return null;
            LQueNd<T> nd = root_;
            root_ = root_.r_;
            if(root_ == null) last_ = null;
            else root_.l_ = null;
            return nd;
        }
        
        public void delete(LQueNd<T> nd){
            if(nd.l_ == null){
                root_ = nd.r_;
                if(root_ == null) last_ = null;
                else root_.l_ = null;
            }else if(nd.r_ == null){
                last_ = nd.l_;
                if(last_ == null) root_ = null;
                else last_.r_ = null;
            }else{
                nd.l_.r_ = nd.r_;
                nd.r_.l_ = nd.l_;
            }
        }
        public void round(Func<T,bool> fc){
            for(LQueNd<T> iter = root_; iter != null; ){
                if(fc(iter.d_)){
                    LQueNd<T> next = iter.r_;
                    delete(iter);
                    iter = next;
                }else{
                    iter ++ ;
                }
            }
        }
    }

    public class LReuseDeque<T>{
        LDeque<T> que_ = new LDeque<T>();
        LDeque<T> reuse_ = new LDeque<T>();

        public LQueNd<T> push(T d){
            LQueNd<T> nd = reuse_.popNd();
            if(nd == null) nd = new LQueNd<T>(d);
            else nd.d_ = d;
            que_.pushNd(nd);
            return nd;
        }
        public T pop(){
            LQueNd<T> nd = que_.popNd();
            if(nd == null) return default(T);
            T d = nd.d_; nd.d_ = default(T);
            reuse_.pushNd(nd);
            return d;
        }
        public void delete(LQueNd<T> nd){
            que_.delete(nd);
            nd.l_ = nd.r_  = null;
            nd.d_ = default(T);
            reuse_.pushNd(nd);
        }
        public void round(Func<T,bool> fc){
            for(LQueNd<T> iter = que_.begin; iter != null; ){
                if(fc(iter.d_)){
                    LQueNd<T> next = iter.r_;
                    delete(iter);
                    iter = next;
                }else{
                    iter ++ ;
                }
            }
        }
        public void clear(){
            que_.clear();
            reuse_.clear();
        }
    }
}