using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor{
    [CustomEditor(typeof(NavMeshCostManager))]
    public class NavMeshManagerComponentEditor : UnityEditor.Editor{
        private NavMeshCostManager _manager;

        private string _elevatorCost;

        private Dictionary<string, string> _areaCostDict;

        NavMeshManagerComponentEditor(){
            _areaCostDict = new Dictionary<string, string>();
            _areaCostDict.Add("F1", "1");
            _areaCostDict.Add("F2", "1");
            _areaCostDict.Add("F3", "1");
            _areaCostDict.Add("F4", "1");
            _areaCostDict.Add("F5", "1");
        }

        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            if (!_manager){
                _manager = target as NavMeshCostManager;
                _elevatorCost = _manager.GetElevatorCost().ToString();
                _areaCostDict["F1"] = _manager.GetStreetPlateCost("F1").ToString();
                _areaCostDict["F2"] = _manager.GetStreetPlateCost("F2").ToString();
                _areaCostDict["F3"] = _manager.GetStreetPlateCost("F3").ToString();
                _areaCostDict["F4"] = _manager.GetStreetPlateCost("F4").ToString();
                _areaCostDict["F5"] = _manager.GetStreetPlateCost("F5").ToString();
            }

            GUILayout.Space(20);
            //电梯权值
            GUILayout.BeginHorizontal();
            GUILayout.Label("电梯权值");
            _elevatorCost = GUILayout.TextField(_elevatorCost, 10);
            GUILayout.EndHorizontal();
            List<string> keys = new List<string>(_areaCostDict.Keys);
            for (int i = 0; i < keys.Count; i++){
                var key = keys[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(key);
                _areaCostDict[key] = GUILayout.TextField(_areaCostDict[key], 10);
                

                GUILayout.EndHorizontal();
            }


            if (GUILayout.Button("设置权值")){
                _manager.SetElevatorCost(float.TryParse(_elevatorCost, out float elevatorCost) ? elevatorCost : 0);
                float areaCost;
                foreach (var key in _areaCostDict.Keys){
                    _manager.SetStreetPlateCost(key, float.TryParse(_areaCostDict[key], out areaCost) ? areaCost : 0);
                }
            }
        }
    }
}