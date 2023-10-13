using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState psInstance = null;
    public enum PSData {walk,dash,stop,onTheRock,exitDashFromRock}

    private PSData psData = PSData.stop;
    
    void Awake() {
        if(psInstance == null) { //생성 전이면
            psInstance = this; //생성
        } else if(psInstance != this) { //이미 생성되어 있으면
            Destroy(this); //새로만든거 삭제
        }
    }

    public PSData getPsData ()
    {
        return psData;
    }

    public void setPsData(PSData data)
    {
        this.psData = data;
        return;
    }
}
