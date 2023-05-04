using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public class GeneticController
    {
        private IUserInterface userInterface;
        public IConsoleController consoleController;
        public List<IFitnessFunction> FitnessFunctions= new List<IFitnessFunction>();
        public List<IVisualisationFunction> VisualisationFunctions= new List<IVisualisationFunction>();
        public List<ISelectionFunction> SelectionFunctions= new List<ISelectionFunction>();
        public List<ICrossOverFunction> CrossOverFunctions=new List<ICrossOverFunction>();
        public List<IMutationFunction> MutationsFunctions=new List<IMutationFunction>();
        public List<ISimulation> Simulations=new List<ISimulation>();
        private CancellationTokenSource cancellationTokenSource;
        public bool KeepWinners=true;

        //private List<Chromosome> population;

        public GeneticController(IUserInterface userInterface, IConsoleController consoleController)
        {
            
            this.consoleController = consoleController;
            this.userInterface = userInterface;
            cancellationTokenSource = new CancellationTokenSource();
        }        

        public async Task<List<Chromosome>> EvolveAsync(EvolutionWorld evolutionWorld, CancellationToken token)
        {

            consoleController.LogMessage("evolveAsync Method");

            consoleController.LogMessage("awaited");
            try
            {
                userInterface.UpdateActivity(Activity.Breeding);
            }
            catch (Exception e)
            {
                consoleController.LogError(e.Message);
                throw;
            }
            consoleController.LogMessage("no");
            var generation = evolutionWorld.GetCurrentGeneration();
                await Task.Delay(10,token);

                consoleController.LogMessage("breed");
                consoleController.LogMessage(generation.population.Count.ToString());

                generation.population = await CrossOverAsync(generation.breedingPopulation, token);

                consoleController.LogMessage("mutate");

                generation.population = await MutateAsync(generation.population, token);
                if (KeepWinners)
                foreach (var chromosome in generation.breedingPopulation)
                {
  
                    generation.population.Add(chromosome.Copy(evolutionWorld,true));
                }
                consoleController.LogMessage("simulate");

                await RunSimulationsAsync(generation.population, token);
                consoleController.LogMessage("score");

                await RunFitnessFunctionsAsync(generation.population,token);
                consoleController.LogMessage("added scores to generation, select");
                await RunVisualisationsFunctionsAsync(generation.population,token);
                consoleController.LogMessage("test1");
                var newBreedingPopulation = await RunSelectionFunctionsAsync(evolutionWorld,generation.population, token);
                foreach (var VARIABLE in newBreedingPopulation)
                {
                    consoleController.LogMessage("hi");
                    /*foreach (var cb in VARIABLE.scores.ChozenBy)
                    {
                        //consoleController.LogMessage(cb.ToString());
                    }*/

                }

                return newBreedingPopulation;

           
            userInterface.UpdateActivity(Activity.WaitingForInput);
        }




        private async Task RunSimulationsAsync(List<Chromosome> population, CancellationToken token)
        {
            var progressStep = 0;
            var totalProgressSteps = (population.Count * Simulations.Count);
            foreach (var simulation in Simulations)
            {
                simulation.ResetResults();

                await simulation.RunSimulationAsync(population);
                progressStep++;
                await Task.Delay(10,token);
                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));
            
            }

            
        }
        private async Task RunFitnessFunctionsAsync(List<Chromosome> population,CancellationToken token)
        {
            //var scores = new List<ChromosomeScores>() ;
            var progressStep = 0;
            var totalProgressSteps = (population.Count * FitnessFunctions.Count);
            var tasks = new List<Task<Dictionary<Chromosome,float>>>();

           // foreach (var fitnessFunction in FitnessFunctions)
           // {
            //    var s = await fitnessFunction.GetFitnessAsync(population, token)
                //newScores.Add();
                //tasks.Add(fitnessFunction.GetFitnessAsync(population, token));
         //   }

         //   await Task.WhenAll(tasks);
            consoleController.LogMessage("ditwerkt");
            consoleController.LogMessage("tasks: "+tasks.Count);
            //for (var i=0;i< tasks.Count;i++)
            var test = 0;
            foreach (var fitnessFunction in FitnessFunctions)
            {
                //var newScores = tasks[i].Result;
                var newScores = await fitnessFunction.GetFitnessAsync(population, token);
                consoleController.LogMessage("newscoresLengt= "+ newScores.Count);
                foreach (var score in newScores)
                {

                    if (score.Key.scores == null)
                    {
                        score.Key.scores = new ChromosomeScores(consoleController);
                        //scores.Add(chromosomescore);
                        test++;
                    }
                    score.Key.scores.AddScore(fitnessFunction.Name,score.Value);/*
                    
                    if (!scores (score.Key))
                    {
                        scores.Add(score.Key, new ChromosomeScores());
                    }
                    
                    scores[score.Key].AddScore(FitnessFunctions[i],score.Value);*/

                }

                await Task.Delay(10,token);
                progressStep++;
            }
            consoleController.LogError("blabla"+test);

                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));




        }
        private async Task RunVisualisationsFunctionsAsync(List<Chromosome> population,CancellationToken token)
        {

            var progressStep = 0;
            var totalProgressSteps = (population.Count * VisualisationFunctions.Count);
            var tasks = new List<Task<Dictionary<Chromosome,List<ChromosomeVisualisation>>>>();
            consoleController.LogMessage("test0");
            foreach (var visualisationFunction in VisualisationFunctions)
            {

                tasks.Add(visualisationFunction.GetVisualisationsAsync(population, token));
            }

            await Task.WhenAll(tasks);
            for (var i=0;i< tasks.Count;i++)
            {
                var visualiasationResults = tasks[i].Result;

                var j = 0;
                foreach (var visualiasations in visualiasationResults)
                {
                   
                    var chromosomescore = visualiasations.Key.scores;
                   
                    chromosomescore.visualisations.AddRange(visualiasations.Value);
                   
                    j++;
                     
                     /* if (!scores (score.Key))
                      {
                          scores.Add(score.Key, new ChromosomeScores());
                      }
                      
                      scores[score.Key].AddScore(FitnessFunctions[i],score.Value);*/

                }

                await Task.Delay(10,token);
                progressStep++;
            }

                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));



        }
        
        private async Task<List<Chromosome>> RunSelectionFunctionsAsync(EvolutionWorld evolutionWorld,List<Chromosome> population, CancellationToken token)
        {
            var breedingPopulation = new List<Chromosome>();
            var progressStep = 0;
            var totalProgressSteps = (population.Count * FitnessFunctions.Count);
            var totalDifference = 0;
            foreach (var selectionFunction in SelectionFunctions)
            {
                var selectedChromosomes = await selectionFunction.SelectChromosomeAsync(population, token);
                var difference =  selectionFunction.GetNumberExpectedWinners() - Chromosome.CountSelected(selectedChromosomes);
                if (difference > 0) totalDifference += difference;
                foreach (var selectedChromosome in selectedChromosomes)
                {
                    selectedChromosome.scores.Selected = true;
                    breedingPopulation.Add(selectedChromosome.Copy(evolutionWorld,true,true));
                }

                //breedingPopulation.AddRange(selectedChromosomes);
                progressStep++;
                await Task.Delay(10,token);
            }


            //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));

            if (totalDifference > 0)
            {
                breedingPopulation.AddRange(Chromosome.InitNewPopulation(totalDifference, evolutionWorld,consoleController));
                /*if (evolutionWorld.chromosomesUseWholeNumbers)
                {
                    breedingPopulation.AddRange(InitNewPopulation(totalDifference,evolutionWorld.chromosomeShape, (int)evolutionWorld.fillValueMin,(int)evolutionWorld.fillValueMax));
                }
                else
                {
                    breedingPopulation.AddRange(InitNewPopulation(totalDifference,evolutionWorld.chromosomeShape,evolutionWorld.fillValueMin,evolutionWorld.fillValueMax));
                }*/

            }        

            return breedingPopulation;
        }        
        private async Task<List<Chromosome>> CrossOverAsync(List<Chromosome> breedingPopulation,CancellationToken token)
        {
            var population = new List<Chromosome>();

            await Task.Delay(10,token);
   
            foreach (var crossOverFunction in CrossOverFunctions)
            {
                //var progress = (float) atMerger /  mergers.Count;

                var newChromosomes = await crossOverFunction.CrossOverAsync(breedingPopulation,token);
                population.AddRange(newChromosomes);
                await Task.Delay(10,token);
                
            }

            return population;
        }

        private async Task<List<Chromosome>> MutateAsync(List<Chromosome> population,CancellationToken token, int startAt=0)
        {
            consoleController.LogMessage("start mutating2");


                //UpdateEvolveScreen("mutating", (float)i/(float)(Chromosomes.Count-numBest),0,simulatainousChecks,"");
                foreach (var mutationFunction in MutationsFunctions)
                {
                    
                    consoleController.LogMessage("start mutating");
                    population= await mutationFunction.MutateAsync(population,token);

                    await Task.Delay(10,token);
                }

            

            return population;
        }
        

        public void AddFitnessFunction(IFitnessFunction fitnessFunction)
        {
            FitnessFunctions.Add(fitnessFunction);


        }
        public void AddSelectionFunction(ISelectionFunction selectionFunction)
        {
            SelectionFunctions.Add(selectionFunction);

        }

        public void AddCrossOverFunction(ICrossOverFunction crossOverFunction)
        {
            CrossOverFunctions.Add(crossOverFunction);

        }

        public void AddMutationFunction(IMutationFunction mutationFunction)
        {
            MutationsFunctions.Add(mutationFunction);

        }
        public void AddVisualisationFunction(IVisualisationFunction visualisationFunction)
        {
            VisualisationFunctions.Add(visualisationFunction);

        }
        private void AddSimulation(ISimulation simulation)
        {
            if (GetSimulation(simulation.GetType())!=null)
            {
                consoleController.LogWarning("added the same simulation two times");
            }
            Simulations.Add(simulation);


        }

        public ISimulation GetSimulation(Type simulationType)
        {
            foreach (var simulation in Simulations)
            {
                if (simulation.GetType() == simulationType) return simulation;
            }

            return null;
        }
        
        public void RemoveFitnessFunction(IFitnessFunction fitnessFunction)
        {
            FitnessFunctions.Remove(fitnessFunction);
            

        }
        public void RemoveSelectionFunction(ISelectionFunction selectionFunction)
        {
            SelectionFunctions.Remove(selectionFunction);
        }

        public void RemoveCrossOverFunction(ICrossOverFunction crossOverFunction)
        {
            CrossOverFunctions.Remove(crossOverFunction);
        }

        public void RemoveMutationFunction(IMutationFunction mutationFunction)
        {
            MutationsFunctions.Remove(mutationFunction);
        }
        public void RemoveSimulation(ISimulation simulation)
        {
            Simulations.Remove(simulation);
        }
        
    }
}