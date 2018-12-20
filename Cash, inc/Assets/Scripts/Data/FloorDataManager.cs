using System;
using System.Collections.Generic;

public static class FloorDataManager {

    public const int FLOOR_MAX_EXP = 50;

    private static List<int> _floors;
    private static List<int> _towers;

    //private static List<int> _timeToEarn;
    //private static List<int> _baseCash;
    private static List<int> _lvlMultiplier;
    private static List<int> _expMultiplier;

    private static Dictionary<int, FloorSettings> _floorSettings;

    public static void Init()
    {
        _floors = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        _towers = new List<int> { 1, 2, 3 };


        //_timeToEarn = new List<int> { 1, 2, 4, 8, 15, 30, 60, 120, 240, 480 };//time for each floor to earn base cash
        //_baseCash = new List<int> { 10, 22, 44, 88, 160, 400, 700, 1400, 2600, 5000 }; //base cash for each floor

        _lvlMultiplier = new List<int> { 1, 10, 20, 30, 40, 50, 60, 70, 80, 100 };//Multiply index for each lvl of floor

        _floorSettings = new Dictionary<int, FloorSettings>();
        _floorSettings.Add(1, new FloorSettings(1, 4, 0.07f, 1, 2));
        _floorSettings.Add(2, new FloorSettings(3, 65, 0.15f, 21, 2));
        _floorSettings.Add(3, new FloorSettings(6, 725, 0.14f, 376, 2));
        _floorSettings.Add(4, new FloorSettings(12, 8650, 0.13f, 2000, 2));
        _floorSettings.Add(5, new FloorSettings(24, 103680, 0.12f, 99000, 2));
        _floorSettings.Add(6, new FloorSettings(100, 1244160, 0.11f, 1980000, 2));
        _floorSettings.Add(7, new FloorSettings(360, 14929920, 0.1f, 33000000, 2));
        _floorSettings.Add(8, new FloorSettings(1500, 179159040, 0.09f, 1152000000, 2));
        _floorSettings.Add(9, new FloorSettings(6000, 2150000000, 0.08f, 33201000000, 2));
        _floorSettings.Add(10, new FloorSettings(36000, 25799000000, 0.07f, 997920000000, 2));
    }

    public static List<int> GetAllTowers()
    {
        return _towers;
    }

    public static List<int> GetAllFloors()
    {
        return _floors;
    }

    public static float TimeToEarnOfFloor(int floor)
    {
        return _floorSettings[floor].TimeToEarn;
    }

    public static double BaseCashOfFloor(int floor)
    {
        return _floorSettings[floor].BaseCash;
    }

    public static double BaseFloorCost(int floor)
    {
        return _floorSettings[floor].BaseCost;
    }

    public static FloorSettings FlorSetting(int floor)
    {
        return _floorSettings[floor];
    }

    public static int MultiplierOfLevel(int lvl)
    {
        return _lvlMultiplier[lvl - 1];
    }

    public static bool IsMaxLevel(int currentlvl)
    {
        return currentlvl >= _lvlMultiplier.Count;
    }

    public static double GetFloorNextExpUpgradeCost(MdlFloor floor)
    {
        if (floor.Id == 1 && floor.Level == 1 && floor.Exp == 0) return 0;
        var currentExp = floor.Exp;
        if(floor.Level != 1)
        {
            currentExp = floor.Level * FLOOR_MAX_EXP + floor.Exp;
        }
        var floorSettings = FlorSetting(floor.Id);

        double baseCost = floorSettings.BaseCost;
        var coef = floorSettings.ExpUpgradeCoefficient;
        for(int i = 0; i < currentExp + 1; i++)
        {
            baseCost = baseCost + (baseCost * coef);
        }
        return baseCost;
    }

    public static double GetFloorNextExpUpgradeCost(int toExp, double cost, float coef)
    {
        var baseCost = cost;
        for (int i = 0; i < toExp + 1; i++)
        {
            baseCost = baseCost + (baseCost * coef);
        }
        return baseCost;
    }

    public static string CorrectCashName(double cash)
    {
        var digitCount = (int)Math.Log10(cash) + 1;
        if (digitCount < 3)
        {
            return  "$ " + Math.Round(cash, 2).ToString();
        }
        else
        {
            return "$ " + Math.Round(cash).ToString();
        }
    }
}

public struct FloorSettings
{
    public float TimeToEarn;//how much time do player need to wait for floor to bring incme
    public double BaseCost;//base floor cost(lvl 1 exp 1)
    public float ExpUpgradeCoefficient;//NextExpUpgradeCost = PreviousExpUpgradeCost + PreviousExpUpgradeCost * ExpUpgradeCoefficient
    public double BaseCash;//cash, that floor will bring at lvl 1 exp 1
    public int CashGrowthMultiplier;//how many times will income grow per 1 lvl exp

    public FloorSettings(float timeToEarn, double baseCost, float expUpgradeCoef, double baseCash, int cashGrowthMultiplier)
    {
        TimeToEarn = timeToEarn;
        BaseCost = baseCost;
        ExpUpgradeCoefficient = expUpgradeCoef;
        BaseCash = baseCash;
        CashGrowthMultiplier = cashGrowthMultiplier;
    }
}
