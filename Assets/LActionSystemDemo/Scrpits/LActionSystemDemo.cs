using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LActionSystem;

public class LActionSystemDemo : MonoBehaviour
{
    public GameObject sprite;
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        var act1 = new LMoveBy(2, 100);
        var act2 = new LScaleBy(2, 0.5f,0.5f);
        var act3 = new LDelay(2);
        var act4 = new LBezier2To(3,new Vector3(450,-190),new Vector3(172,420));
        var act5 = new LCallFunc((GameObject go)=>{
            doSomething(go);
        });
        sprite.runAnim(0,0);
        sprite.runAction(new LSequence(act1,act2,act3,act4.Uniform(),act5));
        cube.runAction(new LFollow(sprite));
    }

    public void doSomething(GameObject go){
        
    }
    
}
