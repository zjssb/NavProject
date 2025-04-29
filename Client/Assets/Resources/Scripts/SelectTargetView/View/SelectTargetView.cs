using System;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectTargetView : UIBaseView{
    private Action<int> callback;

    private XmlNode cfgs;

    public override void Init(params object[] args){
        callback = (Action<int>)args[0];
        // cfgs = ConfigManager.Instance.GetConfig("target_info_cfg");
        RefreshItem();
    }


    public override void OnBtnClick(GameObject go, bool isOn){
        var name = go.name;
        switch (name){
            case "@_btn_close":
            case "@_btn_closeView":
                Close();
                break;
                
        }
    }

    private void RefreshItem(){
        // for (int i = 0; i < count; i++){
        var modelDict = StreetPlateManager.Instance.StreetPlateDict;
        foreach (var model in modelDict){
            var node = model.Value.cfgNode;
            
            var item = Instantiate(
                goTable.GetListComponent("$_obj_Item", EnumManager.TypeEnum.GameObject) as GameObject);
            item.transform.SetParent(
                (goTable.GetListComponent("@_obj_content", EnumManager.TypeEnum.GameObject) as GameObject).transform);
            item.transform.localScale = Vector3.one;
            
            var go = item.GetComponent<GOTable>();
            var btn = go.GetListComponent("@_btn_item", EnumManager.TypeEnum.Btn) as Button;
            btn.onClick.AddListener(() => {
                callback(Convert.ToInt32(node.SelectSingleNode("Sid").InnerText));
                Close();
            });

            var text = go.GetListComponent("@_text_text", EnumManager.TypeEnum.Text) as TextMeshProUGUI;
            text.text = node.SelectSingleNode("Name").InnerText;
        }
    }

    void Awake(){
        goTable.BtnOnClick += OnBtnClick;
    }
}