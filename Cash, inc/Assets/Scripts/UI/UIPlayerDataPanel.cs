using UnityEngine;
using TMPro;

public class UIPlayerDataPanel : MonoBehaviour {

    public TextMeshProUGUI MyCash;

	void Update () {
        MyCash.text = FloorDataManager.CorrectCashName(MdlPlayer.Cash);
	}
}
