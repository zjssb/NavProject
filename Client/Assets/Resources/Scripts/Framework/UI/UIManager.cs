/*
 * 用于管理ui的单例
 */

using System;
using UnityEngine;

public class UIManager 
{

    public static UIManager Instance => mInstance ??= new UIManager();

    private static UIManager mInstance;

    public void OpenWindow(string name, params object[] args){
        var cfg = ConfigManager.Instance.GetConfig("ui_cfg",name);
        
        ResouresManager.Instance.LoadAsync(cfg.SelectSingleNode("Path").InnerText, (GameObject go) => {
            var layer = cfg.SelectSingleNode("Layer").InnerText;
            go.transform.SetParent(GUIModel.Instance.Layers[layer].transform);
            go.gameObject.name = cfg.SelectSingleNode("Name").InnerText;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
            Type sharp = Type.GetType(cfg.SelectSingleNode("CSharp").InnerText);
            var goTable = go.GetComponent<GOTable>();
            goTable.SerializeList();
            var script = go.GetComponent(sharp) as UIBaseView;
            script.Init(args);
        });
    }

    
}
