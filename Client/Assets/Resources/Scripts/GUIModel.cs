using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GUIModel : MonoBehaviour{
    public static GUIModel Instance{ get; private set; }

    /*
     * ui根目录
     */
    public GameObject UIRoot;

    public GameObject TopLayer;

    public GameObject NormalLayer;
    
    public Dictionary<string, GameObject> Layers;

    private void Awake(){
        Instance = this;
        Layers = new(){
            { "TopLayer", TopLayer },
            { "NormalLayer", NormalLayer },
        };
    }


    private void OnGUI(){
        GUI.TextArea(new Rect(10, 10, 100, 50),NavAIMoveModel.Instance.agent.remainingDistance.ToString());
    }
}