using System.Collections.Generic;

public class MdlTower {

    public string TowerName { get; private set; }
    public int TowerLvl { get; private set; }
    public int TowerMultiplier { get; private set; }

    public MdlTower(string towerName, int towerLvl, int towerMultiplier)
    {
        TowerName = towerName;
        TowerLvl = towerLvl;
        TowerMultiplier = towerMultiplier;
    }
}
