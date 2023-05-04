using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Client.Models;
using GenenticAlgorithmBlazor.Client.Pages;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using Newtonsoft.Json;

namespace GenenticAlgorithmBlazor.Client.Controllers
{

    /// <summary>
    /// allows the DLL to write to UI
    /// </summary>
    public class UserInterfaceController:IUserInterface
    {
        public Dictionary<Activity, string> activities= new Dictionary<Activity,string>();
        public List<string> output= new List<string>();
        public string status= "waiting";
        public event EventHandler UpdatedUI;
        public List<string> saves=new List<string>();
        public List<GenerationUIData> generations=new List<GenerationUIData>();
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

        public void CreateCSV(DLLTestController dllTestController,ref string csv)
        {
            csv = "";
            var i = 0;
            csv += "Sum";
            csv += "\n\n";
            foreach (var generation in generations)
            {
                i++;
                csv+=generation.AsCSV(dllTestController.GeneticAlgorithme.FitnessFunctions,"Sum",header:i==1,generationNum:i,averg:true);
                csv+="\n";
            }

            foreach (var VARIABLE in generations[0].GetSelectedChromosomes()[0].Scores.Keys)
            {
                csv += "\n\n\n\n";
                i = 0;
                csv += VARIABLE;
                csv += "\n\n";
                foreach (var generation in generations)
                {
                    i++;
                    csv+=generation.AsCSV(dllTestController.GeneticAlgorithme.FitnessFunctions,VARIABLE,header:i==1,generationNum:i,averg:true);
                    csv+="\n";
                }
            }


            OnUpdatedUI();
        }

       public void DisplayGeneration(int generation, int generationsToGo, List<ChromosomeScores> scores)
        {
            throw new NotImplementedException();
        }

        public void DisplayGeneration(int generationNumber, int generationsToGo, Generation generation)
        {
            var generationStruct = new GenerationUIData(generationNumber, generation);


            //generationStruct.generationChromosomes = scores;
            
            
            generations.Add(generationStruct);
            OnUpdatedUI();
        }

        public void ClearGenerations()
        {
            generations.Clear();
            OnUpdatedUI();
        }

        public void UpdateActivity(Activity stage, bool isDone = false)
        {

            status = activities[stage];
            OnUpdatedUI();
            //fronthand.Status=activities[stage];
        }
        public async Task GetAllSavesAsync()
        {
            using (var client = new HttpClient())
            {
                //var zresult = await );

                /*var saveJson =JsonConvert.SerializeObject(wrapped);
                var payload = new StringContent(saveJson, Encoding.UTF8, "application/json");*/


                var pResult = await client.GetAsync(new Uri("https://localhost:7141/AssignmentTest/get_all_saves"));
                var savesRaw = await  pResult.Content.ReadAsStringAsync();
                //var saves = JsonConvert.DeserializeObject < Dictionary<long, string>>(savesRaw);
                var returnSaves = new Dictionary<long, int>();
                saves= JsonConvert.DeserializeObject<List<string>>(savesRaw);

                OnUpdatedUI();



            }
            
        }
        protected  virtual void OnUpdatedUI()
        {
            EventHandler handler = UpdatedUI;
            handler?.Invoke(this,EventArgs.Empty);
        }

        
        
    }




}