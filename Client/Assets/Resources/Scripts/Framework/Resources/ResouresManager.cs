using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResouresManager : MonoBehaviour{

    public static ResouresManager Instance{get; private set;}

    public void LoadAsync(string path, Action<GameObject> callback = null){
        StartCoroutine(Load(path, callback));
    }

    IEnumerator Load(string path, Action<GameObject> callback){
        var res = Resources.LoadAsync<GameObject>(path);
        yield return res;
        var asset = Instantiate(res.asset) as GameObject;
        callback?.Invoke(asset);
    }

    private void Awake(){
        Instance = this;
    }
}