using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class EvolveScreen:UIScreen
    {
        public ProgressBar ProgressBar;
        public Text fase;
        public TextMeshProUGUI output;
        public Text amountOfSimulations;
        public void SetFase(string name)
        {
            fase.text = "Current fase: " + name;
        }

        public void SetProgress(float progress)
        {
            ProgressBar.progress = progress;
        }

        public void SetOutput(string op)
        {
            output.text += op;
        }

        public void SetAmountOfSimulations(int current, int max)
        {

            amountOfSimulations.text = "Simulation: " + current.ToString() + " / " + max.ToString();
        }
        public void StopNow()
        {

        }

        public void StopNextGeneration()
        {

        }
    }
}