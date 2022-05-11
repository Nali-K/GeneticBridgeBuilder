using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace GeneticAlgorithm.Controller
{
    public class GeneticController
    {
        //private IUserInterface userInterface;
        public IConsoleController consoleController;
        public List<IFitnessFunction> FitnessFunctions= new List<IFitnessFunction>();
        public List<ISelectionFunction> SelectionFunctions= new List<ISelectionFunction>();
        public List<ICrossOverFunction> CrossOverFunctions=new List<ICrossOverFunction>();
        public List<IMutationFunction> MutationsFunctions=new List<IMutationFunction>();
        public List<ISimulation> Simulations=new List<ISimulation>();
        private CancellationTokenSource cancellationTokenSource;
        private int generationsToGo;

        private List<Chromosome> breedingPopulation;
        private List<Chromosome> population;
        //private List<Chromosome> population;

        public GeneticController(/*IUserInterface userInterface,*/ IConsoleController consoleController)
        {
            
            this.consoleController = consoleController;
           
            cancellationTokenSource = new CancellationTokenSource();
        }        
        /*public GeneticController( IConsoleController consoleController)
        {
            this.consoleController = consoleController;
            this.consoleController.LogMessage("contact");
            cancellationTokenSource = new CancellationTokenSource();
        }*/

        public void InitNewPopulation(int Amount,int[] chromosomeShape,float fillValue)
        {
            breedingPopulation = new List<Chromosome>();

            for (var i = 0; i < Amount; i++)
            {
                breedingPopulation.Add(new Chromosome(chromosomeShape));
                breedingPopulation[i].Fill(fillValue);
            }
        }
        public void InitNewPopulation(int Amount,int[] chromosomeShape,int fillValueMin,int intValueMax)
        {
            breedingPopulation = new List<Chromosome>();
            population = new List<Chromosome>();
            for (var i = 0; i < Amount; i++)
            {
                breedingPopulation.Add(new Chromosome(chromosomeShape));
                breedingPopulation[i].FillRandom(fillValueMin,intValueMax);
            }
        }
        public void InitNewPopulation(int Amount,int[] chromosomeShape,float fillValueMin,float intValueMax)
        {
            breedingPopulation = new List<Chromosome>();
            population = new List<Chromosome>();
            for (var i = 0; i < Amount; i++)
            {
                breedingPopulation.Add(new Chromosome(chromosomeShape));
                breedingPopulation[i].FillRandom(fillValueMin,intValueMax);
            }
        }
        public void Start(int numGenerations)
        {
            generationsToGo = numGenerations;
            Task.Run(() => EvolveAsync(cancellationTokenSource.Token));
            //EvolveAsync(cancellationTokenSource.Token);
        }
        
        public void Stop(bool finishGeneration = true)
        {
            if (finishGeneration)
            {
                generationsToGo = 1;
            }
            else
            {
                cancellationTokenSource.Cancel();
            }
        }

      
        public async Task EvolveAsync(CancellationToken token)
        {
            do
            {
                
                //temporary test code
                consoleController.LogMessage(generationsToGo.ToString());
                foreach (var chromosone in breedingPopulation)
                {
                    consoleController.LogMessage(chromosone.ToString());
                }
                //end test code
                consoleController.LogMessage("breed");
                consoleController.LogMessage(population.Count.ToString());

                population = await BreedAsync(breedingPopulation, token);

                consoleController.LogMessage("mutate");
                consoleController.LogMessage(population.Count.ToString());
                population = await MutateAsync(population, token);

                consoleController.LogMessage("simulate");
                consoleController.LogMessage(population.Count.ToString());
                await RunSimulationsAsync(population, token);
                consoleController.LogMessage("score");
                consoleController.LogMessage(population.Count.ToString());
                var chromosomeScores = await RunFitnessFunctionsAsync(population,token);
                consoleController.LogMessage("select");
                consoleController.LogMessage(population.Count.ToString());
                breedingPopulation = await RunSelectionFunctionsAsync(chromosomeScores, token);
                
                generationsToGo--;
                consoleController.LogMessage("done: "+generationsToGo);
            } while (generationsToGo != 0);
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
                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));
            
            }

            
        }
        private async Task<Dictionary<Chromosome,ChromosomeScores>> RunFitnessFunctionsAsync(List<Chromosome> population,CancellationToken token)
        {
            var scores = new Dictionary<Chromosome,ChromosomeScores>() ;
            var progressStep = 0;
            var totalProgressSteps = (population.Count * FitnessFunctions.Count);


            foreach (var fitnessFunction in FitnessFunctions)
            {
                var newScores =await fitnessFunction.GetFitnessAsync(population,token);
                foreach (var score in newScores)
                {
                    if (!scores.ContainsKey(score.Key))
                    {
                        scores.Add(score.Key, new ChromosomeScores());
                    }

                    scores[score.Key].AddScore(fitnessFunction,score.Value);

                }
                progressStep++;
            }
                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));



            return scores;
        }
        private async Task<List<Chromosome>> RunSelectionFunctionsAsync(Dictionary<Chromosome,ChromosomeScores> chromosomeScores, CancellationToken token)
        {
            var breedingPopulation = new List<Chromosome>();
            var progressStep = 0;
            var totalProgressSteps = (chromosomeScores.Count * FitnessFunctions.Count);

            foreach (var selectionFunction in SelectionFunctions)
            {
                breedingPopulation.AddRange(await selectionFunction.SelectChromosomeAsync(chromosomeScores,token));
                progressStep++;
            }
            //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));

            

            return breedingPopulation;
        }        
        private async Task<List<Chromosome>> BreedAsync(List<Chromosome> breedingPopulation,CancellationToken token)
        {
            var population = new List<Chromosome>();
            foreach (var chromosome in breedingPopulation)
            {
                population.Add(chromosome);
            }

   
            foreach (var crossOverFunction in CrossOverFunctions)
            {
                //var progress = (float) atMerger /  mergers.Count;

                var newChromosomes = await crossOverFunction.CrossOverAsync(breedingPopulation,token);
                population.AddRange(newChromosomes);
                               
                
            }

            return population;
        }

        private async Task<List<Chromosome>> MutateAsync(List<Chromosome> population,CancellationToken token, int startAt=0)
        {
            consoleController.LogMessage("start mutating");

                //UpdateEvolveScreen("mutating", (float)i/(float)(Chromosomes.Count-numBest),0,simulatainousChecks,"");
                foreach (var mutationFunction in MutationsFunctions)
                {
                    
                    population= await mutationFunction.MutateAsync(population,token);
          
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