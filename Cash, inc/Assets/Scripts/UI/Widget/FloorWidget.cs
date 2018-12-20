using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FloorWidget : MonoBehaviour {

    public GameObject ClosedFloor;
    public GameObject OpenedFloor;

    public TextMeshProUGUI ClosedFloorName;
    public TextMeshProUGUI CostToOpen;
    public TextMeshProUGUI ClosedFloorNameInactive;
    public TextMeshProUGUI CostToOpenInactive;
    public GameObject UnlockFloorBtn;

    public List<GameObject> LevelStars;
    public TextMeshProUGUI FloorName;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI IncomeFromFloor;
    public TextMeshProUGUI TimeToGetIncome;
    public TextMeshProUGUI TimeLeftToGetIncome;
    public TextMeshProUGUI ExpUpgradeCost;
    public Button BtnGetIncome;
    public GameObject BtnGetIncomeText;
    public Button UpgradeFloorExp;
    public Image IncomeProgress;
    public Image ExpProgress;

    private bool _canGetIncome = true;
    private MdlFloor _floor;
    private UIFloorsPanel _owner;

    private bool _canExpUp()
    {
        return MdlPlayer.Cash >= FloorDataManager.GetFloorNextExpUpgradeCost(_floor);
    }

    private bool _hasHelper()
    {
        return _floor.HelperLevel > 0;
    }

    public void Init(MdlFloor floor, UIFloorsPanel owner)
    {
        _owner = owner;
        _floor = floor;
        BtnGetIncome = BtnGetIncome.GetComponent<Button>();
        UpgradeFloorExp = UpgradeFloorExp.GetComponent<Button>();
        _owner.OnCashUpdate += RebuildWidget;
        //CostToOpen.text = FloorDataManager.GetFloorNextExpUpgradeCost(_floor).ToString();
        //CostToOpenInactive.text = FloorDataManager.GetFloorNextExpUpgradeCost(_floor).ToString();
        CostToOpen.text = FloorDataManager.CorrectCashName(FloorDataManager.GetFloorNextExpUpgradeCost(_floor));
        CostToOpenInactive.text = CostToOpen.text;
        OpenFloor();
        RebuildWidget();
        if (_hasHelper())
            GetCash();
    }

    public void BuyHelperLevel() //add cost to helpers
    {        
        _floor.AddHelperLevel();
        if(_floor.HelperLevel == 1)
        {
            StartCoroutine(CalculateTime());
        }
        _owner.RebuildWidgets();
    }

    public void GetCash()//add to button on widget to get money from floor if we don't have helper
    {
        StartCoroutine(CalculateTime());
    }

    private IEnumerator CalculateTime()
    {
        _canGetIncome = false;
        float timeToEarn = FloorDataManager.TimeToEarnOfFloor(_floor.Id);
        float time = timeToEarn;
        while (timeToEarn > 0)
        {
            timeToEarn -= Time.deltaTime;
            IncomeProgress.fillAmount = timeToEarn / time;
            yield return null;
        }
        CalculateMultipliedCash();
        _canGetIncome = true;

        if (_hasHelper()) StartCoroutine(CalculateTime());
        //StartCoroutine(CalculateTime()); recursion to automatisize the process of getting money, NEED HELPER
    }

    private void CalculateMultipliedCash()
    {
        var baseCash = FloorDataManager.BaseCashOfFloor(_floor.Id);
        var lvlMultiplier = FloorDataManager.MultiplierOfLevel(_floor.Level);
        var expMultiplier = _floor.Level != 1 ? _floor.Level * FloorDataManager.FLOOR_MAX_EXP + _floor.Exp : _floor.Exp;

        var cash = baseCash * lvlMultiplier * expMultiplier;

        MdlPlayer.AddCash(cash);
        _owner.RebuildWidgets();
        //RebuildWidget();
    }

    public void UpgradeFloor()//add to button on widget to Upgrade current floor exp(lvl) if we have enough cash
    {
        if (_canExpUp())
        {
            _floor.AddExp();
            OpenFloor();
            _owner.RebuildWidgets();
            //RebuildWidget();
        }
    }


    private void RebuildWidget() //call each time when widget data changes to draw new info
    {
        var baseCash = FloorDataManager.BaseCashOfFloor(_floor.Id);
        var multiplier = FloorDataManager.MultiplierOfLevel(_floor.Level);
        var expMultiplier = _floor.Level != 1 ? _floor.Level * FloorDataManager.FLOOR_MAX_EXP + _floor.Exp : _floor.Exp;
        //IncomeFromFloor.text = (baseCash * multiplier * expMultiplier).ToString();
        IncomeFromFloor.text = FloorDataManager.CorrectCashName(baseCash * multiplier * expMultiplier);

        TimeToGetIncome.text = "/" + FloorDataManager.TimeToEarnOfFloor(_floor.Id).ToString() + "s";
        ExpText.text = _floor.Exp.ToString();
        //ExpUpgradeCost.text = FloorDataManager.GetFloorNextExpUpgradeCost(_floor).ToString();
        ExpUpgradeCost.text = FloorDataManager.CorrectCashName(FloorDataManager.GetFloorNextExpUpgradeCost(_floor));
        if(_floor.Id == 2)
        {
            Debug.Log(string.Format("Can i upgrade floor? {0}, my cash: {1}, need to upgrade: {2}", _canExpUp(), MdlPlayer.Cash, FloorDataManager.GetFloorNextExpUpgradeCost(_floor)));
        }
        UnlockFloorBtn.SetActive(_canExpUp());
        UpgradeFloorExp.interactable = _canExpUp();
        ExpProgress.fillAmount = 1 - (_floor.Exp / (float)FloorDataManager.FLOOR_MAX_EXP);
        UpdateStars();
        GameManager.Inst.SaveData();
    }

    private void UpdateStars()
    {
        for (int i = 1; i <= LevelStars.Count; i++)
        {
            LevelStars[i-1].SetActive(_floor.Level > i);
        }
    }

    private void OpenFloor()
    {
        if((_floor.Exp > 0 && _floor.Level == 1) || _floor.Level > 1)
        {
            ClosedFloor.SetActive(false);
            OpenedFloor.SetActive(true);
        }
    }

    private void Update()
    {
        BtnGetIncome.interactable = _canGetIncome && !_hasHelper();
        BtnGetIncomeText.SetActive(BtnGetIncome.interactable);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_floor.Exp + " _floor.Exp, 1 - (_floor.Exp / FloorDataManager.FLOOR_MAX_EXP) = : " + (1 - (_floor.Exp / FloorDataManager.FLOOR_MAX_EXP)));
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var c = FloorDataManager.GetFloorNextExpUpgradeCost(_floor);
        //    var digitCount = (int)Math.Log10(c) + 1;
        //    if(digitCount > 4)
        //    {
        //        Debug.Log(Math.Round(c));
        //    }
        //    else
        //    {
        //        Debug.Log(string.Format("{0:0.00}", c));
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    var settings = FloorDataManager.FlorSetting(_floor.Id);
        //    var c = FloorDataManager.GetFloorNextExpUpgradeCost(1, settings.BaseCost, settings.ExpUpgradeCoefficient);
        //    var digitCount = (int)Math.Log10(c) + 1;
        //    if (digitCount > 4)
        //    {
        //        Debug.Log(Math.Round(c));
        //    }
        //    else
        //    {
        //        Debug.Log(string.Format("{0:0.00}", c));
        //    }
        //}
    }
}
