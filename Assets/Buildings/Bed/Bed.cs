using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour {

    public float restness;

    private DwarfBehaviour mOwner;
    private bool mIsOccupied;

    public bool isOccupied {
        get {
            return mIsOccupied;
        }
    }

    void Awake () {
        
    }

    public void StartSleep() {
        mIsOccupied = true;
    }

    public void StopSleep() {
        mIsOccupied = false;        
    }

}
