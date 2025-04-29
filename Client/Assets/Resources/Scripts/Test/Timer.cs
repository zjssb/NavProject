
using System.Collections;
using UnityEngine;

public class Timer{

    IEnumerator Run(float seconds,float jiange){
        while (true){
            yield return new WaitForSeconds(jiange);
            seconds-=jiange;
        }
    }
}
