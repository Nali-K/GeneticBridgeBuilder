using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using GenenticAlgorithmBlazor.Client.Controllers;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;

namespace GenenticAlgorithmBlazor.Client.Models
{
    public class GenerationUIData
    {
        public int numGeneration;
        //public List<ChromosomeScores> generationChromosomes;
        public Generation generation;
        public List<string> fitnessFunctions;
        public bool opened;
        public bool openedAllChromosomes;
        public GenerationUIData(int numGeneration,Generation generation)
        {
            this.numGeneration = numGeneration;
            this.generation = generation;
            fitnessFunctions = new List<string>();
            opened = false;
            openedAllChromosomes = false;
        }

        public List<ChromosomeScores> GetSelectedChromosomes(bool not=false)
        {
            var selectedChromsomes = new List<ChromosomeScores>();
            foreach (var chromosome in generation.population)
            {
                if (chromosome.scores == null) continue;
                if ((chromosome.scores.Selected && !not) || (!chromosome.scores.Selected && not))
                {
                    selectedChromsomes.Add(chromosome.scores);
                }
            }
            
            return selectedChromsomes;
        }

        public string AsCSV(List<IFitnessFunction> fitnessFunctions, string fitnessFunction = "",
            bool winnersOnly = true, bool header = true, int generationNum = 0, bool averg = false)

        {
            var CSV = "";
            if (fitnessFunction == "")
            {
                if (header)
                {
                    CSV += "Result;";
                    CSV += "Sum";
                    foreach (var ff in fitnessFunctions)
                    {
                        CSV += ";";
                        CSV += ff.Name;

                    }

                    CSV += "\n";
                }

                var i = 0;
                foreach (var chromosome in generation.population)
                {

                    if (chromosome.scores == null) continue;
                    if ((chromosome.scores.Selected || !winnersOnly))
                    {

                        var sum = 0f;
                        var str = "";
                        foreach (var ff in fitnessFunctions)
                        {
                            str += ";";
                            str += chromosome.scores.Scores[ff.Name];
                            sum += chromosome.scores.Scores[ff.Name];

                        }

                        CSV += "r" + i + ";";
                        CSV += sum;
                        CSV += str;
                        CSV += "\n";
                        i += 1;
                    }
                }

            }
            else
            {
                var headerStr = "generation";
                var values = "Generation " + generationNum;


                var i = 0;
                var total = 0f;
                var high = float.NegativeInfinity;
                var low = float.PositiveInfinity;
                foreach (var chromosome in generation.population)
                {

                    if (chromosome.scores == null) continue;
                    if ((!chromosome.scores.Selected && winnersOnly)) continue;
                    if (GetValueFromFitnessFunction(fitnessFunction,fitnessFunctions,chromosome,out var v))
                    {
                        if (averg)
                        {

                            total += v;
                            if (v > high) high = v;
                            if (v < low) low = v;
                        }
                        else
                        {
                            headerStr += ";" + i;
                            values += ";" + v;
                        }


                    }


                    i += 1;
                }

                if (header)
                {
                    if (averg)
                    {
                        headerStr += ";lowest;average;highest";
                    }


                    CSV += headerStr;
                    CSV += "\n";

                }

                if (averg)
                {
                    values += ";" + low + ";" + total/i + ";" + high;
                }

                CSV += values;
                
            }

            return CSV;
        }


        static bool GetValueFromFitnessFunction(string fitnessFunctionName,List<IFitnessFunction> fitnessFunctions,Chromosome chromosome,out float value)
        {
            if (chromosome.scores.Scores.ContainsKey(fitnessFunctionName))
            {
                value = chromosome.scores.Scores[fitnessFunctionName];
                return true;
            }

            if (fitnessFunctionName == "Sum")
            {                                
                var sum = 0f;

                foreach (var ff in fitnessFunctions)
                {

                    sum += chromosome.scores.Scores[ff.Name];

                }

                value = sum;
                return true;
                
            }

            value = 0;
            return false;

        }

    }
}