using System;

namespace GeneticAlgorithm.Controller
{
    public class RandomValueGenerator
    {
        private static RandomValueGenerator randomValueGenerator;
        private Random random;
        public static RandomValueGenerator Instance => randomValueGenerator ?? (randomValueGenerator = new RandomValueGenerator());

        public RandomValueGenerator()
        {
            random = new Random();
        }

        public int GetRange(int min, int max)
        {
            return random.Next(min, max);
        }

        public float GetRange(float min, float max)
        {
            var value = random.NextDouble();
            value *= (max - min);
            value += min;
            return (float)value;
        }
    }
}