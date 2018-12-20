using System.Collections.Generic;

public static class MdlPlayer {

    public static int PlayerId { get; private set; }
    public static double Cash { get; private set; }

    public static int CurrentTowerId { get; private set; }
    public static Dictionary<int, List<MdlFloor>> FloorsLvlsInTowers;

    public static void Init()
    {
        SwitchCurrentTower(1);
        FloorsLvlsInTowers = new Dictionary<int, List<MdlFloor>>();
        CreateFloors();
    }
    
    public static void Init(PlayerData data)
    {
        CurrentTowerId = data.CurrentTowerId;
        Cash = data.Cash;
        FloorsLvlsInTowers = data.FloorsLvlsInTowers;
        SwitchCurrentTower(CurrentTowerId);
    }

    private static void CreateFloors()
    {
        foreach(var tower in FloorDataManager.GetAllTowers())
        {
            FloorsLvlsInTowers.Add(tower, new List<MdlFloor>());
            foreach (var _floor in FloorDataManager.GetAllFloors())
            {
                FloorsLvlsInTowers[tower].Add(new MdlFloor(_floor, 0, 1));                
            }
        }
    }

    public static void AddCash(double cash)
    {
        if(cash != 0)
        {
            Cash += cash;
        }
    }
    
    public static List<MdlFloor> GetAllFloorsByTowerLvl(int towerLvl)
    {
        return FloorsLvlsInTowers[towerLvl];
    }

    public static List<MdlFloor> GetAllFloorsByCurrentTower()
    {
        return FloorsLvlsInTowers[CurrentTowerId];
    }

    public static MdlFloor GetFloorByIndex(int floor)
    {
        return GetAllFloorsByCurrentTower()[floor-1];
    }

    public static void SwitchCurrentTower(int towerId)
    {
        CurrentTowerId = towerId;
    }
}

[System.Serializable]
public class PlayerData
{
    public double Cash;
    public int CurrentTowerId;
    public Dictionary<int, List<MdlFloor>> FloorsLvlsInTowers;
}
