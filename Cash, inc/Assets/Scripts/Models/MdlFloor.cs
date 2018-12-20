[System.Serializable]
public class MdlFloor {

    public int Id { get; private set; }
    public int Exp { get; private set; }
    public int Level { get; private set; }
    public int HelperLevel { get; private set; }

    public MdlFloor(int id, int exp, int lvl, int helperLevel = 0)
    {
        Id = id;
        Exp = exp;
        Level = lvl;
        HelperLevel = helperLevel;
    }

    public void  AddLvl()
    {
        if(!FloorDataManager.IsMaxLevel(Level))
            Level++;
    }

    public void AddExp()
    {
        MdlPlayer.AddCash(-FloorDataManager.GetFloorNextExpUpgradeCost(this));
        Exp++;
        if (Exp >= FloorDataManager.FLOOR_MAX_EXP)
        {
            AddLvl();
            Exp = 0;
        }
    }

    public void AddExp(int exp)
    {
        Exp += exp;
        if(Exp >= FloorDataManager.FLOOR_MAX_EXP)
        {
            AddLvl();
            var expDif = Exp - FloorDataManager.FLOOR_MAX_EXP;
            Exp = expDif;
        }
    }

    public void AddHelperLevel()
    {
        HelperLevel++;
    }
}
