using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralNetwork {
    public class Evolution {
        List<NeuralNetwork> population;
        int qunatity = 1000;
        int generations = 10;
        int mutationRate = 1;

        public Evolution()
        {
            this.population = new List<NeuralNetwork>();
            var rand = new Random();
            for (int i = 0; i < this.qunatity; i++)
            {
                var layers = GenerateLayers();
                this.population.Add(new NeuralNetwork(layers));
            }
        }

        public void Run()
        {
            var examples = Program.GeneratExamples(1000);
            for (int i = 0; i < generations; i++)
            {
                Console.WriteLine(String.Concat("Population no: ", i, "\n"));
                this.Exist(examples);
                var selected =  this.Selection();
                this.Reproduce(selected);
                Console.WriteLine();
            }

            this.SortPopulation();
            this.population[0].SaveNeuralNetwork("winner");
        }

        private void Reproduce(NeuralNetwork[] organisms)
        {
            this.population.Clear();

            foreach (var organism in organisms)
            {
                var network = organism.GetNetwork();
                foreach (var layer in network)
                {
                    for (int i = 0; i < layer.rows; i++)
                    {
                        for (int j = 0; j < layer.cols; j++)
                        {
                            layer[i,j] = Mutate(layer[i,j]);
                        }
                    }
                }

                this.population.Add(new NeuralNetwork(network));
            }
        }

        private double Mutate(double property)
        {
            Random rand = new Random();
            var range = 1000;

            if(rand.Next(0,range) < this.mutationRate)
            {
                property = (float)rand.Next(-range, range) / range;
            }

            return property;
        }

        private NeuralNetwork[] Selection()
        {
            var selected = new NeuralNetwork[this.qunatity];
            var chancesList = new List<NeuralNetwork>();
            var rand = new Random();

            this.SortPopulation();
            Console.WriteLine();
            Console.WriteLine("Best nn fitness: " + this.population[0].fitness + " errors: " + this.population[0].errors);
            Console.WriteLine("Worst nn fitness: " + this.population[this.qunatity - 1].fitness + " errors: " + this.population[this.qunatity - 1].errors);

            foreach (var nn in this.population)
            {
                for (int i = 0; i < nn.fitness + 1; i++)
                {
                    chancesList.Add(nn);
                }
            }

            for (int i = 0; i < this.qunatity; i++)
            {
                selected[i] = chancesList[rand.Next(0, chancesList.Count)];
            }

            return selected;
        }

        private void SortPopulation()
        {
            this.population.Sort((x, y) => y.fitness.CompareTo(x.fitness));
        }

        private void Exist(Matrix[] examples)
        {
            var i = 0;
            var tasks = new Task[this.qunatity];

            foreach (var nn in this.population)
            {
                tasks[i++] = Task.Run(() => nn.Learn(examples[0], examples[1], examples[2], examples[3]));
            }
            Task.WaitAll(tasks);
        }

        private int[] GenerateLayers() {
            var rand = new Random();
            var lsl = rand.Next(2,4);

            int[] layers = new int[lsl+1];

            layers[0] = 7;
            for (int i = 1; i < lsl; i++)
            {
                layers[i] = rand.Next(4,10);
            }
            layers[lsl] = 2;

            return layers;
        }
    }
}