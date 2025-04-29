using System.Xml;
using UnityEngine;

public class StreetPlateModel : MonoBehaviour{
    
    public int sid = 101;

    public string plateName;

    public XmlNode cfgNode;

    private void Awake(){
        StreetPlateManager.Instance.AddStreetPlate(sid, this);
        cfgNode = ConfigManager.Instance.GetConfig("target_info_cfg", sid.ToString());
        plateName = ConfigManager.Instance.GetConfig("target_info_cfg",sid.ToString(),"Name").InnerText;
    }

    private void OnTriggerEnter(Collider other){
        EventManager.Instance.TriggerEvent("AddMainViewButton",sid, plateName);
    }
    
    private void OnTriggerExit(Collider other){
        EventManager.Instance.TriggerEvent("RemoveMainViewButton", sid);
    }
}
