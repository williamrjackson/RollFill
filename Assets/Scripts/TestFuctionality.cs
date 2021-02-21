using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFuctionality : MonoBehaviour
{
    public GameObject editPanel;
    public GameObject gameHierarchy;
    private bool testState = false;
    // Start is called before the first frame update
    public void OnClick()
    {
        testState = !testState;
        if (testState)
        {
            SerializeJson.SerializeEditToFile();
            editPanel.SetActive(false);
            gameHierarchy.SetActive(true);
        }
        else
        {
            gameHierarchy.SetActive(false);
            editPanel.SetActive(true);
        }
    }
}
