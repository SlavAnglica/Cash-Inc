using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager Inst;
    public UIFloorsPanel FloorPanel;


    private void Awake()
    {
        if (Inst == null) Inst = this;
    }

    public void StartPlay()//add to scene to button start to instantiate floors;
    {
        FloorPanel.gameObject.SetActive(true);
        FloorPanel.Init();
    }


}
