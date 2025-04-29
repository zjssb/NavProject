using System.Collections.Generic;
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
    
    
}