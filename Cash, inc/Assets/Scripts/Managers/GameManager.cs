using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Inst;
    // Use this for initialization
    private void Awake()
    {
        if (Inst == null) Inst = this;

        FloorDataManager.Init();

        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
            MdlPlayer.Init();
        else
        {
            var savedData = LoadData();
            if (savedData != null)
            {
                MdlPlayer.Init(savedData);
                var cash = savedData.Cash;
                var currenttower = savedData.CurrentTowerId;
                var floors = savedData.FloorsLvlsInTowers;
                var tower0 = floors[1];
                Debug.Log(string.Format("cash: {0}, currenttower: {1}, towers count: {2}, floors at 1 tower: {3}, 1st floor exp: {4}, id: {5}, HelperLeve: {6}", cash, currenttower, floors.Count, floors[1].Count, tower0[0].Exp, tower0[0].Id, tower0[0].HelperLevel));
            }

            else
            {
                Debug.Log("SavedGame was damaged, creating new game");
                MdlPlayer.Init();
            }
                
        }

        Application.runInBackground = true;
    }

    private void Start()
    {
        UIManager.Inst.FloorPanel.Init();
    }

    public void SaveData()
    {
        var saveData = CreateDataForSave();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, saveData);
        file.Close();
    }

    private PlayerData LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            PlayerData savedata = (PlayerData) bf.Deserialize(file);
            file.Close();
            return savedata;
        }
        else
        {
            return null;
        }
    }

    private PlayerData CreateDataForSave()
    {
        PlayerData savedata = new PlayerData();
        savedata.Cash = MdlPlayer.Cash;
        savedata.CurrentTowerId = MdlPlayer.CurrentTowerId;
        savedata.FloorsLvlsInTowers = MdlPlayer.FloorsLvlsInTowers;

        return savedata;
    }
}
