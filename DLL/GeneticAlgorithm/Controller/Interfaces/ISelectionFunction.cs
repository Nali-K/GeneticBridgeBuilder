namespace GeneticAlgorithm.Controller
{
    public interface ISelectionFunction
    {
        string ToJson();
        bool FromJson(string json);
    }
}