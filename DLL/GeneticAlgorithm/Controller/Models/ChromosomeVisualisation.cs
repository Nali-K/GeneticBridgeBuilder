using GeneticAlgorithm.Controller;

namespace GeneticAlgorithm.Controller.Models
{
    public class ChromosomeVisualisation
    {
        public string ImageLocation;
        public string name;
        public string visualisationFunctionName;
        //public IVisualisationFunction visualisationFunction;
        public ChromosomeVisualisation(string ImageLocation,string visualisationFunctionName,string name="")
        {
            this.name = name;
            this.visualisationFunctionName = visualisationFunctionName;
            this.ImageLocation = ImageLocation;
        }
    }
}