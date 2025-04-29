using UnityEngine;

/*
 * UI界面父类
 */
public class UIBaseView : MonoBehaviour{
    
    public GOTable goTable;
    
    private void OnValidate(){
        if (!gameObject.GetComponent<GOTable>()){
            this.gameObject.AddComponent<GOTable>();
        }
    }

    public virtual void Init(params object[] args){
    }

    public virtual void OnBtnClick(GameObject go, bool isOn){
    }

    public virtual void Close(){
        Destroy(gameObject);
    }

    private void Awake(){
        goTable = gameObject.GetComponent<GOTable>();
    }
}