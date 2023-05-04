using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.MutationFunctions.Interfaces;
namespace GeneticAlgorithm.MutationFunctions
{
    public class SwapRowMutation:MutationFunction
    {

        
            private float minValue;
            private float maxValue;
            private float mutationRatio;
            private Random rand;

            private IConsoleController consoleController;
//          public int[] values= new int [30];
            public SwapRowMutation(float ratio,IConsoleController consoleController)
            {
                
                mutationRatio = ratio;
                rand = new Random();
                this.consoleController = consoleController;
            }

            public override async Task<List<IChromosome>> MutateAsync(List<IChromosome> c,CancellationToken token)
            {
                var returnList = new List<IChromosome>();
                
                foreach (var chromosome in c)
                {

                    returnList.Add(await GetMutatedAsync(chromosome,token));

                }
                consoleController.LogMessage("start7");
                return returnList;
            }
            private async Task<IChromosome> GetMutatedAsync(IChromosome chromosome,CancellationToken token)
            {

                //chromosome.GetValuesAndPositions()
                    
                var pos = new int[chromosome.GetNumDimensions()];
                for (var acrossDimension = 0; acrossDimension < chromosome.GetNumDimensions(); acrossDimension++)
                {
                    for (var j = 0; j < chromosome.GetNumDimensions(); j++)
                    {
                        pos[j] = -1;
                    }


                    for (var j = 0; j < chromosome.GetDimensionSize(acrossDimension); j++)
                    {
                        pos[acrossDimension] = j;
                        if (rand.Next(0, 100) < mutationRatio)
                        {

                            var rowToSwapWith = rand.Next(0, chromosome.GetDimensionSize(acrossDimension) - 1);
                            if (rowToSwapWith >= j) rowToSwapWith++;

                            var grabbed = chromosome.GetValuesAndPositions(pos);
                            pos[acrossDimension] = rowToSwapWith;
                            
                            var grabbed2 = chromosome.GetValuesAndPositions(pos);
                            foreach (var grab in grabbed)
                            {
                                grab.Key[acrossDimension] = rowToSwapWith;
                            }
                            foreach (var grab in grabbed2)
                            {
                                grab.Key[acrossDimension] = j;
                            }
                            chromosome.InsertValues(grabbed);
                            chromosome.InsertValues(grabbed2);
                        }

                    }

                }
                
/*
                var i = 0;
                var geneArray = chromosome.GetGeneArray();

                foreach (var gene in geneArray)
                {

                    var r = (float)rand.NextDouble();
                    if (r < mutationRatio)
                    {
                        var v = GetRandomValue();
                        newGenes[i]=(v);
                    
//                        values[(int) (v + 15)]+=1;

                    }
                    else
                    {
                        newGenes[i] = gene;
                    }

                    i++;
                    
                }

                chromosome.Fill(newGenes);
                */
                return chromosome;
            }

            private float GetRandomValue()
            {
                var r =rand.NextDouble();
                r *= maxValue - minValue+1;
                r += minValue;

                return (float)Math.Floor(r);
            }
        }
    
}