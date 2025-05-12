using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor{
    [CustomEditor(typeof(NavMeshCostManager))]
    public class NavMeshManagerComponentEditor : UnityEditor.Editor{
        private NavMeshCostManager manager;
        
        private int elevatorCount;
        
        private string[] elevatorsCost;

        private Dictionary<string, string> areaCostDict;
        
        public override void OnInspectorGUI(){
            base.OnInspectorGUI(); 
            manager = target as NavMeshCostManager;



            elevatorCount = manager.Elevators.Length;
            elevatorsCost = new string[elevatorCount];
            
            for (int i = 0; i < elevatorCount; i++){
                elevatorsCost[i] = manager.GetElevatorCost(i).ToString();
            }            
            if (this.areaCostDict == null){
                this.areaCostDict = new Dictionary<string, string>{
                    { "F11", "1" },
                    { "F12", "1" },
                    { "F21", "1" },
                    { "F22", "1" },
                    { "F31", "1" },
                    { "F32", "1" }
                };
                return;
            }
            areaCostDict["F11"] = manager.GetStreetPlateCostByDict("F11").ToString();
            areaCostDict["F12"] = manager.GetStreetPlateCostByDict("F12").ToString();
            areaCostDict["F21"] = manager.GetStreetPlateCostByDict("F21").ToString();
            areaCostDict["F22"] = manager.GetStreetPlateCostByDict("F22").ToString();
            areaCostDict["F31"] = manager.GetStreetPlateCostByDict("F31").ToString();
            areaCostDict["F32"] = manager.GetStreetPlateCostByDict("F32").ToString();

            GUILayout.Space(20);
            //电梯权值
            for (int i = 0; i < elevatorCount; i++){
                GUILayout.BeginHorizontal();
                GUILayout.Label($"电梯{i}权值");
                elevatorsCost[i] = GUILayout.TextField(elevatorsCost[i], 10);
                GUILayout.EndHorizontal();
            }
            List<string> keys = new List<string>(areaCostDict.Keys);
            for (int i = 0; i < keys.Count; i++){
                var key = keys[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(key);
                areaCostDict[key] = GUILayout.TextField(areaCostDict[key], 10);
                

                GUILayout.EndHorizontal();
            }


            if (GUILayout.Button("设置权值")){
                for (int i = 0; i < elevatorCount; i++){
                    manager.SetElevatorCost(i,
                        float.TryParse(elevatorsCost[i], out float elevatorCost) ? elevatorCost : -1f);
                }

                float areaCost;
                foreach (var key in areaCostDict.Keys){
                    manager.SetStreetPlateCost(key, float.TryParse(areaCostDict[key], out areaCost) ? areaCost : 1f);
                }
            }

            if (GUILayout.Button("复原")){
                for (int i = 0; i < elevatorCount; i++){
                    manager.SetElevatorCost(i, -1f);
                }
                
                foreach (var key in areaCostDict.Keys){
                    manager.SetStreetPlateCost(key, 1f);
                }
            }
            
            EditorUtility.SetDirty(target);
        }
        
    }
}