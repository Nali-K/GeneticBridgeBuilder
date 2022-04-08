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

        public void Start(int numGenerations)
        {
            generationsToGo = numGenerations;
            Evolve(cancellationTokenSource.Token);
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

        public async Task Evolve(CancellationToken token)
        {
            var population = new List<Chromosome>();
            var breedingPopulation = new List<Chromosome>();
            //temporary test code
            for (var i = 0; i < 5; i++)
            {
                breedingPopulation.Add(new Chromosome(new [] {3,3}));
                breedingPopulation[i].FillRandom(0,9);
            }
            
            
            //end test code
            do
            {
                //temporary test code
                consoleController.LogMessage(generationsToGo.ToString());
                foreach (var VARIABLE in breedingPopulation)
                {
                    consoleController.LogMessage(VARIABLE.ToString());
                }
                //end test code
                consoleController.LogMessage("breed");
                consoleController.LogMessage(population.Count.ToString());
                
                population = await Breed(breedingPopulation, token);
                foreach (var VARIABLE in population)
                {
                    consoleController.LogMessage(VARIABLE.ToString());
                }
                consoleController.LogMessage("mutate");
                consoleController.LogMessage(population.Count.ToString());
                population = await Mutate(population, token);
                foreach (var VARIABLE in population)
                {
                    consoleController.LogMessage(VARIABLE.ToString());
                }
                consoleController.LogMessage("simulate");
                consoleController.LogMessage(population.Count.ToString());
                await RunSimulations(population, token);
                consoleController.LogMessage("score");
                consoleController.LogMessage(population.Count.ToString());
                var chromosomeScores = await RunFitnessFunctions(population,token);
                consoleController.LogMessage("select");
                consoleController.LogMessage(population.Count.ToString());
                breedingPopulation = await RunSelectionFunctions(chromosomeScores, token);
                
                generationsToGo--;

            } while (generationsToGo != 0);
        }




        private async Task RunSimulations(List<Chromosome> population, CancellationToken token)
        {
            var progressStep = 0;
            var totalProgressSteps = (population.Count * Simulations.Count);
            foreach (var simulation in Simulations)
            {
                simulation.ResetResults();
                foreach (var chromosome in population)
                {
                    await simulation.RunSimulation(chromosome);
                    progressStep++;
                    //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));
                }
            }

            
        }
        private async Task<Dictionary<Chromosome,ChromosomeScores>> RunFitnessFunctions(List<Chromosome> population,CancellationToken token)
        {
            var scores = new Dictionary<Chromosome,ChromosomeScores>() ;
            var progressStep = 0;
            var totalProgressSteps = (population.Count * FitnessFunctions.Count);
            foreach (var chromosome in population)
            {
                var chromosomeScores = new ChromosomeScores();
                foreach (var fitnessFunction in FitnessFunctions)
                {
                    var score = await fitnessFunction.GetFitness(chromosome,token);
                    chromosomeScores.AddScore(fitnessFunction,score);
                    progressStep++;
                }
                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));
                scores.Add(chromosome,chromosomeScores);
            }

            return scores;
        }
        private async Task<List<Chromosome>> RunSelectionFunctions(Dictionary<Chromosome,ChromosomeScores> chromosomeScores, CancellationToken token)
        {
            var breedingPopulation = new List<Chromosome>();
            var progressStep = 0;
            var totalProgressSteps = (chromosomeScores.Count * FitnessFunctions.Count);

            foreach (var selectionFunction in SelectionFunctions)
            {
                breedingPopulation.AddRange(await selectionFunction.SelectChromosome(chromosomeScores,token));
                progressStep++;
            }
            //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));

            

            return breedingPopulation;
        }        
        private async Task<List<Chromosome>> Breed(List<Chromosome> breedingPopulation,CancellationToken token)
        {
            var population = new List<Chromosome>();
            foreach (var chromosome in breedingPopulation)
            {
                population.Add(chromosome);
            }

   
            foreach (var crossOverFunction in CrossOverFunctions)
            {
                //var progress = (float) atMerger /  mergers.Count;
                for (int i = 0; i < breedingPopulation.Count; i++)
                {
                    for (int j = 0; j < breedingPopulation.Count; j++)
                    {
                        //progress+=(float) atMerger /  mergers.Count/(numBest * numBest);  
                        //UpdateEvolveScreen("breeding",progress,0,simulatainousChecks,"");
                        //if (i != j)
                        {
                            var newChromosome = await crossOverFunction.CrossOver(new[]
                                {breedingPopulation[i], breedingPopulation[j]},token);
                            population.Add(newChromosome);
                        }

                    }
                }                
                await Task.Delay(50,token);
            }

            return population;
        }

        private async Task<List<Chromosome>> Mutate(List<Chromosome> population,CancellationToken token, int startAt=0)
        {
            consoleController.LogMessage("start mutating");
            for (int i = startAt; i < population.Count; i++)
            {
                //UpdateEvolveScreen("mutating", (float)i/(float)(Chromosomes.Count-numBest),0,simulatainousChecks,"");
                foreach (var mutationFunction in MutationsFunctions)
                {
                    
                    population[i]= await mutationFunction.Mutate(population[i],token);
          
                }

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