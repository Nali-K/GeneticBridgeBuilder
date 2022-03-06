using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]private Screens currentScreen = Screens.main;
    public GeneticController geneticController;
    [SerializeField]private List<UIScreen> screens=new List<UIScreen>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var s in GetComponentsInChildren<UIScreen>(true))
        {
            screens.Add(s);
            s.Init(geneticController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var s in screens)
        {
            if (currentScreen == s.screen)
            {
                s.Show();
            }
            else
            {
                s.Hide();
            }
        }
    }

    public void SwitchScreen(Screens s)
    {
        currentScreen = s;
    }
}
