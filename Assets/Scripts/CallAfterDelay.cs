using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAfterDelay : MonoBehaviour
{
    float delay;
    System.Action action;
 
    public static CallAfterDelay Create( float delay, System.Action action)
    {
        CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
        cad.delay = delay;
        cad.action = action;
        return cad;
    }
 
    IEnumerator Start()
    {
        yield return new WaitForSeconds( delay);
        action();
        Destroy ( gameObject);
    }
}