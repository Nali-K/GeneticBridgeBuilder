using BlazorGeneticAlgorithm.Pages;
using GeneticAlgorithm.Controller;

namespace BlazorGeneticAlgorithm.Controllers;

public class UserInterfaceController:IUserInterface
{
    private GeneticAlgorithmFronthand fronthand;
    public Dictionary<Activity, string> activities= new Dictionary<Activity,string>();

    public UserInterfaceController(GeneticAlgorithmFronthand fronthand)
    {

        activities.Add(Activity.Breeding, "running");
        activities.Add(Activity.WaitingForInput, "done");
        this.fronthand = fronthand;
    }
    public void UpdateProgress(float progress)
    {
        throw new NotImplementedException();
    }

    public void UpdateActivity(Activity stage, bool isDone = false)
    {

        fronthand.Status=activities[stage];
    }
}