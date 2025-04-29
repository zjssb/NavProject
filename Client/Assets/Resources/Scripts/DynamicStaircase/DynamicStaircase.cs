using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Serialization;

public class DynamicStaircase : MonoBehaviour{
    
    [FormerlySerializedAs("isToggle")] public bool IsToggle = false;

    public int Cost;

    public string AreaName = "water";

    // Update is called once per frame
    void Update()
    {
        if (IsToggle){
            IsToggle = false;
            ChangeAreaCost();
        }
            
    }

    void ChangeAreaCost(){
        var areaIndex = NavMesh.GetAreaFromName(AreaName);
        if (areaIndex != -1){
            NavMesh.SetAreaCost(areaIndex,Cost);
        }
    }
    
}
