using GeneticDesignerWebAssembly.Pages;
using GeneticAlgorithm.Controller;

namespace GeneticDesignerWebAssembly.Controllers;

public class UserInterfaceController:IUserInterface
{
    public Dictionary<Activity, string> activities= new Dictionary<Activity,string>();
    public List<string> output= new List<string>();
    public string status= "waiting";
    public event EventHandler UpdatedUI;
    public UserInterfaceController()
    {
        
        activities.Add(Activity.Breeding, "running");
        activities.Add(Activity.WaitingForInput, "done");

    }
    public void UpdateProgress(float progress)
    {
        throw new NotImplementedException();
        OnUpdatedUI();
    }

    public void DisplayMessage(string message)
    {
        output.Add(message);
        OnUpdatedUI();
    }
    public void UpdateActivity(Activity stage, bool isDone = false)
    {

        status = activities[stage];
        OnUpdatedUI();
        //fronthand.Status=activities[stage];
    }

    protected  virtual void OnUpdatedUI()
    {
        EventHandler handler = UpdatedUI;
        handler?.Invoke(this,EventArgs.Empty);
    }
    
}