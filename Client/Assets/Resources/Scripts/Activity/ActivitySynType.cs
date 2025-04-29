using System;
using System.IO;

public class ActivitySynType{
    public static ActivitySynType Instance => mInstance ??= new ActivitySynType();

    private static ActivitySynType mInstance;

    private enum ActivityType{
    }

    public string GetActivityPath(int sid){
        string path;
        if (CheckStreetPlate(sid, out path)){
        }
        return path;
    }

    private bool CheckStreetPlate(int sid, out string path){
        path = "";
        if (sid > 100 && sid < 1000){
            path = Path.Combine("Prefab", "NavView");
            return true;
        }

        return false;
    }
}