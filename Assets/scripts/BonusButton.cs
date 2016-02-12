using UnityEngine;
using System.Collections;

public class BonusButton : MonoBehaviour {

    pullLever script;
    public pullLever.modifierTypes bonus;//sets bonus to activate
    public pullLever.modifierTypes exclude1;//ideally this would be done with an array
    public pullLever.modifierTypes exclude2;//but arrays can't be set directly via the editor, defeating the point
    public string hoverText;

	// Use this for initialization
	void Start () {
        script = GameObject.Find("lever").GetComponent<pullLever>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseEnter()
    {
        script.toolTip.text = hoverText;
    }

    public void OnMouseExit()
    {
        script.toolTip.text = "";
    }

    public void BuyBonus()
    {
        if(script.bonusPrices[(int)bonus] <= script.coins || script.cheatsOn)
        {
            script.SetFlagState((int)bonus, true);
            if (!script.cheatsOn)
            {
                script.coins -= script.bonusPrices[(int)bonus];
                script.SetFlagState((int)exclude1, false);
                script.SetFlagState((int)exclude2, false);
                gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
                script.buttons[(int)exclude1].GetComponent<UnityEngine.UI.Button>().interactable = false;
                script.buttons[(int)exclude2].GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }
    }
}
