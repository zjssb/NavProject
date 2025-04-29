using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElevatorNode : MonoBehaviour{
    public Dictionary<string, OffMeshLink> link;

    /// <summary>
    /// 传入其他节点，进行offmeshlink初始化
    /// </summary>
    /// <param name="nodes"></param>
    public void Init(){
        var nodes = gameObject.transform.GetComponentsInChildren<OffMeshLink>();
        link = new Dictionary<string, OffMeshLink>(nodes.Length);
        for (int i = 0; i < nodes.Length; i++){
            var node = nodes[i];
            link.Add(node.gameObject.name,node);
        }
    }

    public OffMeshLink GetLink(string name){
        return link[name];
    }

    public void SetLinkCost(string name, int cost){
        OffMeshLink offLink;
        if (link.TryGetValue(name, out offLink)){
            offLink.costOverride = cost;
        }
        else{
            Debug.LogError("没有 " + name);
        }
    }
}