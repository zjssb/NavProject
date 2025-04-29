using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour{
    public GameObject Button;
    public GameObject Content;

    public bool cameraCanMove = true;

    private Dictionary<int, GameObject> buttonDict = new(3);

    private void AddButtonEvent(object[] objects){
        AddButton((int)objects[0], (string)objects[1]);
    }

    private void RemoveButtonEvent(object[] objects){
        RemoveButton((int)objects[0]);
    }

    private void AddButton(int sid, string text){
        var btn = Instantiate(Button, Content.transform);

        btn.GetComponent<Button>().onClick.AddListener(() => { ButtonOnClick(sid); });
        btn.gameObject.name = sid.ToString();
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        buttonDict.Add(sid, btn);
    }

    private void RemoveButton(int sid){
        var btn = buttonDict[sid];
        if (btn == null){
            Debug.Log(sid + " 按钮不存在");
            return;
        }

        buttonDict.Remove(sid);
        Destroy(btn.gameObject);
    }

    /// <summary>
    /// 左侧按钮执行方法
    /// </summary>
    /// <param name="sid"></param>
    private void ButtonOnClick(int sid){
        UIManager.Instance.OpenWindow("NavView", sid);
    }

    private void Awake(){
        EventManager.Instance.Subscribe("AddMainViewButton", AddButtonEvent);
        EventManager.Instance.Subscribe("RemoveMainViewButton", RemoveButtonEvent);
    }

    private void Update(){
        if (Input.GetKeyUp(KeyCode.Z)){
            if (cameraCanMove){
                FirstPersonController.Instance.ChangeCursor(CursorLockMode.None);
                FirstPersonController.Instance.cameraCanMove = false;
                cameraCanMove = false;
            }
            else{
                FirstPersonController.Instance.ChangeCursor(CursorLockMode.Locked);
                FirstPersonController.Instance.cameraCanMove = true;
                cameraCanMove = true;
            }
        }
    }

    private void OnDisable(){
        EventManager.Instance.Unsubscribe("AddMainViewButton", AddButtonEvent);
        EventManager.Instance.Unsubscribe("RemoveMainViewButton", RemoveButtonEvent);
    }
}