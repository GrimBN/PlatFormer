using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static List<float> timers;
    //float timer;

    /*public Timer()
    {
        timer = 0f;
    }*/
    
    void Update()
    {
       /* for(int i=0; i < timers.ToArray().Length; i++)
        {
            timers[i] += Time.deltaTime;
            
        }*/
        //timer += Time.deltaTime;
    }

    public void CreateTimer(ref float timer)
    {
        timers.Add(timer);        
    }

    /*public float GetTime()
    {
        return timer;
    }*/
}
