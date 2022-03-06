using System;
using UnityEngine.UI;

namespace UI
{
    public class MainScreen:UIScreen
    {
        public InputField generations;
            
        private void Awake()
        {
            screen = Screens.main;
            generations.text = "0";
        }

        private void Update()
        {
            
        }

        public void ButtonStart()
        {
            int gen = 0;
            if (!int.TryParse(generations.text, out gen)) return;
            if (gen<0) return;
            geneticController.StartEvolving(gen);

        }
    }
}