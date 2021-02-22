using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFuctionality : MonoBehaviour
{
    public GameObject editPanel;
    public GameObject gameHierarchy;
    public GameObject player;

    private bool testState = false;

    public void OnClick()
    { 
        if (!EditButton.IsCurrentStartSet) return;

        testState = !testState;
        if (testState)
        {
            SerializeJson.SerializeEditToFile();
            editPanel.SetActive(false);
            gameHierarchy.SetActive(true);
            player.SetActive(true);
            GridSystem.Instance.LoadLevel(0);
        }
        else
        {
            player.SetActive(false);
            gameHierarchy.SetActive(false);
            editPanel.SetActive(true);
        }
    }
}
