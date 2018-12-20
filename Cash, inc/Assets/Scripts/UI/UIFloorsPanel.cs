using UnityEngine;

public class UIFloorsPanel : MonoBehaviour
{
    public Transform FloorsGridParent;
    public delegate void OnPlayerCashUpdate();
    public event OnPlayerCashUpdate OnCashUpdate;
    public void Init()
    {
        var floors = MdlPlayer.GetAllFloorsByCurrentTower();
        //floors.Reverse();

        var bottomBuildingPref = Resources.Load<GameObject>("Prefabs/Towers/Tower_1/Bottom");
        var floorPref = Resources.Load<GameObject>("Prefabs/Towers/Tower_1/Floor");
        var roofPref = Resources.Load<GameObject>("Prefabs/Towers/Tower_1/Roof");

        Instantiate(bottomBuildingPref, FloorsGridParent);

        foreach (var floor in floors)
        {
            var go = Instantiate(floorPref, FloorsGridParent);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            var goComp = go.GetComponent<FloorWidget>();
            goComp.Init(floor, this);
        }

        Instantiate(roofPref, FloorsGridParent);
    }

    public void RebuildWidgets()
    {
        if (OnCashUpdate != null)
            OnCashUpdate();
    }
}
