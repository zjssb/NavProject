using System.Collections.Generic;
using UnityEngine;

public class StreetPlateManager{

    public static StreetPlateManager Instance => mInstance ??= new StreetPlateManager();

    private static StreetPlateManager mInstance;

    public Dictionary<int, StreetPlateModel> StreetPlateDict = new(10);

    public void AddStreetPlate(int sid, StreetPlateModel streetPlate){
        if (!StreetPlateDict.TryAdd(sid, streetPlate)){
            Debug.LogError("路牌sid重复; sid:" + sid);
        }
    }

    public void RemoveStreetPlate(int sid){
        if (StreetPlateDict.ContainsKey(sid)){
            StreetPlateDict.Remove(sid);
        }
        else{
            Debug.LogError("路牌sid不存在; sid:" + sid);
        }
    }

    public StreetPlateModel GetStreetPlate(int sid){
        if (StreetPlateDict.ContainsKey(sid)){
            return StreetPlateDict[sid];
        }
        else{
            Debug.LogError("路牌sid不存在; sid:" + sid);
            return null;
        }
    }

    public int GetStreePlateCount(){
        return StreetPlateDict.Count;
    }

    public void SetStreetPlate(int sid, StreetPlateModel streetPlate){
        StreetPlateDict[sid] = streetPlate;
    }
}