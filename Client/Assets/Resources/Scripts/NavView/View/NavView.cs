using System;
using TMPro;
using UnityEngine;

/*
 * 导航界面
 */
public class NavView : UIBaseView{
    public int targetSid;

    private TextMeshProUGUI targetText;

    public override void Init(params object[] args){
        FirstPersonController.Instance.cameraCanMove = false;
        targetText = goTable.GetListComponent("@_text_target", EnumManager.TypeEnum.Text) as TextMeshProUGUI;

        var text = goTable.GetListComponent("@_text_floor", EnumManager.TypeEnum.Text) as TextMeshProUGUI;
        var floor = ConfigManager.Instance.GetConfig("target_info_cfg", args[0].ToString(), "Name").InnerText;
        if (text) text.text = "您现在位于" + floor;
        targetText.text = floor;
    }


    public override void OnBtnClick(GameObject go, bool isOn){
        var goName = go.name;
        if (goName == "@_btn_close"){
            Close();
        }

        if (goName == "@_btn_SelectTarget1"){
            var call = new Action<int>(ChangeTarget);
            UIManager.Instance.OpenWindow("SelectTargetView", call);
        }

        if (goName == "@_btn_SelectTarget2"){
            
        }

        if (goName == "@_btn_go"){
            Nav();            
            Close();
        }
    }

    /// <summary>
    /// 目的地切换
    /// </summary>
    /// <param name="sid">路牌sid</param>
    private void ChangeTarget(int sid){
        targetSid = sid;
        targetText.text = ConfigManager.Instance.GetConfig("target_info_cfg", sid.ToString(), "Name").InnerText;
    }

    private void Nav(){
        NavAIMoveModel.Instance.NavMove(StreetPlateManager.Instance.GetStreetPlate(targetSid).transform);
    }
    
    
    private void Awake(){
        goTable.BtnOnClick += OnBtnClick;
    }

    private void OnDestroy(){
        FirstPersonController.Instance.ChangeCursor(CursorLockMode.Locked);
        FirstPersonController.Instance.cameraCanMove = true;
    }
}