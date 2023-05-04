using System.Collections.Generic;

namespace GenenticAlgorithmBlazor.Server.Models
{
   public class JsonChromosomeUnityAdapter
    {
        public int[] values;

        public string[] simulationResultsKeys;
        public float[] simulationResultsValues; 

        public string[] imageResultsKeys;
        public string[] imageResultsValues;
        public  int[] size= new int[3];
        public int id;

        public JsonChromosomeUnityAdapter()
        {
            
        }
        public JsonChromosomeUnityAdapter(ChromosomeUnityAdapter chromosome)
        {
            id = chromosome.id;
            size = chromosome.size;
            values = new int[size[0] * size[1] * size[2]];
            for (var i = 0; i < size[0]; i++)
            {
                for (var j = 0; j < size[1]; j++)
                {
                    for (var k = 0; k < size[2]; k++)
                    {
                        values[i * size[1] * size[2] + j * size[2] + k] = chromosome.values[i, j, k];
                    }
                }
            }
            
            imageResultsKeys = new string[chromosome.imageResults.Count];
            imageResultsValues = new string[chromosome.imageResults.Count];
            simulationResultsKeys = new string[chromosome.simulationResults.Count];
            simulationResultsValues = new float[chromosome.simulationResults.Count];
            var n = 0;
            foreach (var image in chromosome.imageResults)
            {
                imageResultsKeys[n] = image.Key;
                imageResultsValues[n] = image.Value;
                n++;
            }
            n = 0;
            foreach (var result in chromosome.simulationResults)
            {
                simulationResultsKeys[n] = result.Key;
                simulationResultsValues[n] = result.Value;
                n++;
            }
        }

        public ChromosomeUnityAdapter ToCromosome()
        {

            var vals = new int[size[0],size[1],size[2]];
            for (var i = 0; i < size[0]; i++)
            {
                for (var j = 0; j < size[1]; j++)
                {
                    for (var k = 0; k < size[2]; k++)
                    {
                        vals[i, j, k]=values[i * size[1] * size[2] + j * size[2] + k] ;
                    }
                }
            }
            var chromosome = new ChromosomeUnityAdapter(vals);
            chromosome.id = id;
            chromosome.size = size;
            chromosome.simulationResults = new Dictionary<string, float>();
            chromosome.imageResults = new Dictionary<string, string>();
            
            for(var i=0;i<simulationResultsValues.Length;i++)
            {
                chromosome.simulationResults.Add(simulationResultsKeys[i],simulationResultsValues[i]);
            }
            for(var i=0;i<imageResultsKeys.Length;i++)
            {
                chromosome.imageResults.Add(imageResultsKeys[i],imageResultsValues[i]);
            }

            return chromosome;
        }

    }
}