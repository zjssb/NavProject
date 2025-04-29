using System;
using UnityEngine;

public class DebugController : MonoBehaviour{
    public GameObject Player;

    public float minY = 100;

    public bool trgger = false;

    private void Update(){
        if (trgger){
            trgger = false;
            minY = Player.transform.position.y;
        }

        if (minY > Player.transform.position.y){
            Debug.Log(1);
        }
    }
    
    
    void Func(Action<int> action,Func<int,int> func){
        action?.Invoke(2);
        var i = func?.Invoke(2);
    }
}
