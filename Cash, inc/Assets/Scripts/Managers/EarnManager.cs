using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnManager : MonoBehaviour {

    //private List<MdlFloor> _floors;

    //private void Start()
    //{
    //    _floors = MdlPlayer.GetAllFloorsByCurrentTower();
    //    foreach (var floor in _floors)
    //    {
    //        StartCoroutine(CalculateTime(floor));
    //        Debug.Log("Floor # " + floor.Id);
    //    }
    //}

    //private IEnumerator CalculateTime(MdlFloor floor)
    //{
    //    float timeToEarn = FloorDataManager.TimeToEarnOfFloor(floor.Id);
    //    while (timeToEarn > 0)
    //    {
    //        timeToEarn -= Time.deltaTime;
    //        yield return null;
    //    }
    //    CalculateMultipliedCash(floor);
    //    StartCoroutine(CalculateTime(floor));
    //}

    //private void CalculateMultipliedCash(MdlFloor floor)
    //{
    //    var baseCash = FloorDataManager.BaseCashOfFloor(floor.Id);
    //    var multiplier = FloorDataManager.MultiplierOfLevel(floor.Level);

    //    var cash = baseCash * multiplier;

    //    MdlPlayer.AddCash(cash);
    //}
}
