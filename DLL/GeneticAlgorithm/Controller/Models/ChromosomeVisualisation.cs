using GeneticAlgorithm.Controller;

namespace GeneticAlgorithm.Controller.Models
{
    public class ChromosomeVisualisation
    {
        public string ImageLocation;
        public string name;
        public IVisualisationFunction visualisationFunction;
        public ChromosomeVisualisation(string ImageLocation,IVisualisationFunction visualisationFunction,string name="")
        {
            this.name = name;
            this.visualisationFunction = visualisationFunction;
            this.ImageLocation = ImageLocation;
        }
    }
}