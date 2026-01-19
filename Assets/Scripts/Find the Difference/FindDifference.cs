using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDifference : MonoBehaviour
{
    private int totalDifferences;
    private int foundDifferences = 0;
    public int chances = 3;
    public GameObject[] correctDifferencesObjects;
    public GameObject correctDifferenceParent;

    public Button[] wrongClickButton;

    void Start()
    {
        totalDifferences = correctDifferenceParent.transform.childCount;
        correctDifferencesObjects = new GameObject[correctDifferenceParent.transform.childCount];
        for (int i = 0; i < correctDifferenceParent.transform.childCount; i++)
        {
            correctDifferencesObjects[i] = correctDifferenceParent.transform.GetChild(i).gameObject;
            Button btn = correctDifferencesObjects[i].AddComponent<Button>();
            if (btn != null)
            {
                // Capture the button reference in the lambda
                Button currentBtn = btn;
                btn.onClick.AddListener(() => OnDifferenceClicked(currentBtn));
            }
        }
    }

    private void OnDifferenceClicked(Button clickedButton)
    {
        if (chances > 0) // Changed != to >
        {
            Debug.Log("Difference Found!");
        
            // Make the button non-interactable
            clickedButton.interactable = false;
            
            // Increment found differences counter
            foundDifferences++;
            
            // Check if all differences are found
            if (foundDifferences >= totalDifferences)
            {
                Debug.Log("All differences found!");
                // Add your game completion logic here
            }
        }
        else
        {
            Debug.Log("No more chances left! Game Over.");
            DisableAllButtons();
        }
    }

    public void OnWrongClick()
    {
        chances--;
        Debug.Log("Wrong click! Chances left: " + chances);
        if (chances <= 0)
        {
            Debug.Log("No more chances left! Game Over.");
            DisableAllButtons();
        }
    }

    private void DisableAllButtons()
    {
        // Disable wrong click buttons
        foreach (Button btn in wrongClickButton)
        {
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
        
        // Disable all difference buttons
        foreach (GameObject obj in correctDifferencesObjects)
        {
            Button btn = obj.GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
    }
}