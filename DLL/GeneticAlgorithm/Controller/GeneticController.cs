using System;
using System.Collections.Generic;
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
        public List<IVisualisationFunction> VisualisationFunction= new List<IVisualisationFunction>();
        public List<ISelectionFunction> SelectionFunctions= new List<ISelectionFunction>();
        public List<ICrossOverFunction> CrossOverFunctions=new List<ICrossOverFunction>();
        public List<IMutationFunction> MutationsFunctions=new List<IMutationFunction>();
        public List<ISimulation> Simulations=new List<ISimulation>();
        private CancellationTokenSource cancellationTokenSource;

        //private List<Chromosome> population;

        public GeneticController(IUserInterface userInterface, IConsoleController consoleController)
        {
            
            this.consoleController = consoleController;
            this.userInterface = userInterface;
            cancellationTokenSource = new CancellationTokenSource();
        }        
        /*public GeneticController( IConsoleController consoleController)
        {
            this.consoleController = consoleController;
            this.consoleController.LogMessage("contact");
            cancellationTokenSource = new CancellationTokenSource();
        }*/
        /// <summary>
        /// create a new list of chromosomes
        /// </summary>
        /// <param name="Amount">the amount of chromsomes</param>
        /// <param name="chromosomeShape"> the "shape of the choromosome, meaning the number of dimensions the length of each dimension"</param>
        /// <param name="fillValue">each gene will have this value</param>
        /// <returns>a list of new chromosomes</returns>
        public List<Chromosome> InitNewPopulation(int Amount,int[] chromosomeShape,float fillValue)
        {
            var newPopulation = new List<Chromosome>();

            for (var i = 0; i < Amount; i++)
            {
                newPopulation.Add(new Chromosome(chromosomeShape));
                newPopulation[i].Fill(fillValue);
            }

            return newPopulation;
        }
/// <summary>
/// create a new list of chromosomes the chromosomes are filled with random genens each gene being a whole number
/// </summary>
/// <param name="Amount">the amount of chromsomes</param>
/// <param name="chromosomeShape"> the "shape of the choromosome, meaning the number of dimensions the length of each dimension"</param>
/// <param name="fillValueMin">the minimum value a gene can have</param>
/// <param name="fillValueMax">the maximum values a gene can have</param>
/// <returns>a list of new chromosomes</returns>
        public List<Chromosome> InitNewPopulation(int Amount,int[] chromosomeShape,int fillValueMin,int fillValueMax)
        {
            var newPopulation = new List<Chromosome>();

            for (var i = 0; i < Amount; i++)
            {
                newPopulation.Add(new Chromosome(chromosomeShape));
                newPopulation[i].FillRandom(fillValueMin,fillValueMax);
            }

            return newPopulation;
        }
/// <summary>
/// create a new list of chromosomes the chromosomes are filled with random genens each gene being a floating point number
/// </summary>
/// <param name="Amount">the amount of chromsomes</param>
/// <param name="chromosomeShape"> the "shape of the choromosome, meaning the number of dimensions the length of each dimension"</param>
/// <param name="fillValueMin">the minimum value a gene can have</param>
/// <param name="fillValueMax">the maximum values a gene can have</param>
/// <returns>a list of new chromosomes</returns>
        public List<Chromosome> InitNewPopulation(int Amount,int[] chromosomeShape,float fillValueMin,float fillValueMax)
        {
            var newPopulation = new List<Chromosome>();

            for (var i = 0; i < Amount; i++)
            {
                newPopulation.Add(new Chromosome(chromosomeShape));
                newPopulation[i].FillRandom(fillValueMin,fillValueMax);
            }

            return newPopulation;
        }

/*
        public async Task StartAsync(List<Chromosome>initialChromosomes,int numGenerations)
        {
            generationsToGo = numGenerations;
            consoleController.LogMessage("start method");
            await EvolveAsync(initialChromosomes,cancellationTokenSource.Token);
            
            //EvolveAsync(cancellationTokenSource.Token);
        }*/
        /*
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
        }*/

      
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

                generation.population = await BreedAsync(generation.breedingPopulation, token);

                consoleController.LogMessage("mutate");
                //consoleController.LogMessage(population.Count.ToString());
                generation.population = await MutateAsync(generation.population, token);
                foreach (var chromosome in generation.breedingPopulation)
                {
                    generation.population.Add(chromosome);
                }
                consoleController.LogMessage("simulate");
               // consoleController.LogMessage(population.Count.ToString());
                await RunSimulationsAsync(generation.population, token);
                consoleController.LogMessage("score");
                //consoleController.LogMessage(generation.population.Count.ToString());
                generation.scores = await RunFitnessFunctionsAsync(generation.population,token);
                consoleController.LogMessage("added scores to generation, select");
                generation.scores = await RunVisualisationsFunctionsAsync(generation.population,generation.scores,token);
                //consoleController.LogMessage(population.Count.ToString());
                var NewBreedingPopulation = await RunSelectionFunctionsAsync(evolutionWorld,generation.scores, token);
                
                //userInterface.DisplayGeneration(generation,generationsToGo,chromosomeScores);

                //consoleController.LogMessage("done: "+generationsToGo);
                //userInterface.DisplayMessage("done: "+generationsToGo);
                return NewBreedingPopulation;

           
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
        private async Task<List<ChromosomeScores>> RunFitnessFunctionsAsync(List<Chromosome> population,CancellationToken token)
        {
            var scores = new List<ChromosomeScores>() ;
            var progressStep = 0;
            var totalProgressSteps = (population.Count * FitnessFunctions.Count);
            var tasks = new List<Task<Dictionary<Chromosome,float>>>();

            foreach (var fitnessFunction in FitnessFunctions)
            {

                tasks.Add(fitnessFunction.GetFitnessAsync(population, token));
            }

            await Task.WhenAll(tasks);
            consoleController.LogMessage("ditwerkt");
            consoleController.LogMessage("tasks: "+tasks.Count);
            for (var i=0;i< tasks.Count;i++)
            {
                var newScores = tasks[i].Result;
                consoleController.LogMessage("newscoresLengt= "+ newScores.Count);
                foreach (var score in newScores)
                {
                    var chromosomescore= ChromosomeScores.FindByChromosome(score.Key, scores,consoleController,true);
                    if (chromosomescore == null)
                    {
                        chromosomescore = new ChromosomeScores(consoleController)
                        {
                            chromosome = score.Key
                        };
                        scores.Add(chromosomescore);
                    }
                    chromosomescore.AddScore(FitnessFunctions[i],score.Value);/*
                    
                    if (!scores (score.Key))
                    {
                        scores.Add(score.Key, new ChromosomeScores());
                    }
                    
                    scores[score.Key].AddScore(FitnessFunctions[i],score.Value);*/

                }

                await Task.Delay(10,token);
                progressStep++;
            }
            

                //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));



            return scores;
        }
        private async Task<List<ChromosomeScores>> RunVisualisationsFunctionsAsync(List<Chromosome> population,List<ChromosomeScores> scores,CancellationToken token)
        {

            var progressStep = 0;
            var totalProgressSteps = (population.Count * VisualisationFunction.Count);
            var tasks = new List<Task<Dictionary<Chromosome,List<ChromosomeVisualisation>>>>();
            consoleController.LogMessage("test0");
            foreach (var visualisationFunction in VisualisationFunction)
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

                    
                    var chromosomescore= ChromosomeScores.FindByChromosome(visualiasations.Key, scores,consoleController);

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



            return scores;
        }
        
        private async Task<List<Chromosome>> RunSelectionFunctionsAsync(EvolutionWorld evolutionWorld,List<ChromosomeScores> chromosomeScores, CancellationToken token)
        {
            var breedingPopulation = new List<Chromosome>();
            var progressStep = 0;
            var totalProgressSteps = (chromosomeScores.Count * FitnessFunctions.Count);
            var totalDifference = 0;
            foreach (var selectionFunction in SelectionFunctions)
            {
                var selectedChromosomes = await selectionFunction.SelectChromosomeAsync(chromosomeScores, token);
                var difference =  selectionFunction.GetNumberExpectedWinners() - selectedChromosomes.Count;
                if (difference > 0) totalDifference += difference;
                breedingPopulation.AddRange(selectedChromosomes);
                //breedingPopulation.AddRange(selectedChromosomes);
                progressStep++;
                await Task.Delay(10,token);
            }

            foreach (var selectedChromosome in breedingPopulation)
            {
                ChromosomeScores.FindByChromosome(selectedChromosome, chromosomeScores,consoleController).selected = true;
            }
            //userInterface.UpdateProgress(((float)progressStep/(float)totalProgressSteps));

            if (totalDifference > 0)
            {
                if (evolutionWorld.chromosomesUseWholeNumbers)
                {
                    breedingPopulation.AddRange(InitNewPopulation(totalDifference,evolutionWorld.chromosomeShape, (int)evolutionWorld.fillValueMin,(int)evolutionWorld.fillValueMax));
                }
                else
                {
                    breedingPopulation.AddRange(InitNewPopulation(totalDifference,evolutionWorld.chromosomeShape,evolutionWorld.fillValueMin,evolutionWorld.fillValueMax));
                }

            }        

            return breedingPopulation;
        }        
        private async Task<List<Chromosome>> BreedAsync(List<Chromosome> breedingPopulation,CancellationToken token)
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