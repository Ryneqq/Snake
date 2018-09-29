using System;
using System.Linq;

namespace NeuralNetwork {
    public class NeuralNetwork {
        private const string NewLine = "\r\n";
        private const int BETA = 5;
        private readonly Matrix[] nn;
        private float step, momentum = 1.0f, learningRate = 0.05f;

        public NeuralNetwork(Matrix[] nn)
        {
            this.nn = nn;
        }

        public NeuralNetwork(int[] layers)
        {
            var lsl = layers.Length - 1;
            this.nn = new Matrix[lsl];

            for(int i = 0, n = 0; n < lsl; i++, n++)
            {
                this.nn[n] = Matrix.RandomMatrix(layers[i] + 1, layers[i + 1], 100);
            }
        }

        public void Display()
        {
            for(var i = 0; i < this.nn.Length; i++)
            {
                Console.WriteLine("layer " + i + " rows: " + this.nn[i].rows + " cols: " + this.nn[i].cols);
                Console.WriteLine(this.nn[i].ToString());
            }
        }

        Matrix AddBias(Matrix signal)
        {
            return Matrix.AddRowAndFill(signal, -1.0f);
        }

        Matrix RemoveBias(Matrix signal)
        {
            return Matrix.RemoveRow(signal, 0);
        }

        Matrix GetExample(Matrix examples, int example)
        {
            return examples.GetCol(example);
        }

        Matrix FindError(Matrix signal, Matrix delta)
        {
            var error = delta.Duplicate();

            for(var i = 0; i < error.rows; i++)
            {
                for(var j = 0; j < error.cols; j++)
                {
                    error[i, j] = delta[i, j] * BETA * signal[i, j] * (1 - signal[i, j]);
                }
            }

            return error;
        }

        void BackPropagation(Matrix[] signals, Matrix delta)
        {
            Matrix signal;

            for(var i = this.nn.Length; i > 0; i--)
            {
                signal = signals[i];
                if(signal.rows > delta.rows)
                    signal = RemoveBias(signal);

                var error = FindError(signal, delta);
                var dnn   = this.momentum * this.learningRate * signals[i-1] * Matrix.Transpose(error);

                this.nn[i-1] += dnn;
                delta = RemoveBias(this.nn[i-1]) * error;
            }
        }

        public void Learn(Matrix questions, Matrix anwsers, int repeat)
        {
            var example = 0;
            var rnd = new Random();
            this.step = 1.0f - 1.0f / (float)repeat;
            // this.step = 1.0f - 1.0f / (float)(repeat*repeat);

            for(var i = 0; i < repeat; i++)
            {
                example     = rnd.Next(0, questions.cols);
                var input   = GetExample(questions, example);
                var signals = RunAndReturnSignals(input);
                var delta   = GetExample(anwsers, example) - signals[this.nn.Length];

                this.momentum *= this.step;
                // Console.WriteLine("momentum: " + this.momentum);

                BackPropagation(signals, delta);
            }
        }

        public bool Learn(Matrix learningQuestions, Matrix learningAnwsers, Matrix testQuestions, Matrix testAnwsers)
        {
            // var counter   = 0;
            // var tests     = 100;

            // while(!Test(testQuestions, testAnwsers))
            // {
            //     if (counter > tests)
            //         return false;

            //     // this.momentum = 1;
            //     this.momentum = 1.0f - 1.0f / (float)(tests - counter + 1);
            //     Console.WriteLine("momentum: " + this.momentum);
            //     this.Learn(learningQuestions, learningAnwsers, 10000);
            //     counter++;
            // }

            // return true;

            this.momentum = 1.0f;
            this.Learn(learningQuestions, learningAnwsers, 10000);
            return this.Test(testQuestions, testAnwsers);

        }

        bool Test(Matrix questions, Matrix anwsers)
        {
            var errors   = 0;
            var margin   = 0.4;
            var examples = questions.cols;

            for(var i = 0; i < examples; i++)
            {
                var anwser = this.Run(this.GetExample(questions, i));
                var actual = anwser - this.GetExample(anwsers, i);

                foreach(var a in actual.mat)
                {
                    if(a > margin || a < -margin)
                    {
                        ++errors;
                        break;
                    }
                }
            }

            Console.WriteLine(string.Concat("Errors: ", errors, "/", examples));
            margin = 0.25;

            if (errors > examples * margin)
                return false;
            else
                return true;
        }

        Matrix Run(Matrix input, int layer)
        {
            var signal = AddBias(input);
            var output = Matrix.Transpose(this.nn[layer]) * signal;

            return ActivationFunction(output);
        }

        public Matrix Run(Matrix signal)
        {
            var layers = this.nn.Length;

            for(var i = 0; i < layers - 1; i++)
            {
                signal = Run(signal, i);
            }

            return Run(signal, layers - 1);
        }

        Matrix[] RunAndReturnSignals(Matrix signal)
        {
            var layers  = this.nn.Length;
            var signals = new Matrix[layers+1];
            signals[0]  = AddBias(signal);

            for(var i = 0; i < layers; i++)
            {
                signal = Run(signal, i);

                if(i == layers - 1)
                    signals[i+1] = signal;
                else
                    signals[i+1] = AddBias(signal);
            }

            return signals;
        }

        Matrix ActivationFunction(Matrix signal)
        {
            // sigmoid: 1/(1+exp(-BETA*U))
            var activation = signal.Duplicate();

            for(var i = 0; i < activation.rows; i++)
            {
                for(var j = 0; j < activation.cols; j++)
                {
                    activation[i,j] = 1/(1+ System.Math.Exp(-BETA * signal[i,j]));
                }
            }

            return activation;
        }

        public void SaveNeuralNetwork(string name)
        {
            var network = String.Empty;
            for(var i = 0; i < nn.Length; i++)
            {
                if(i > 0)
                    network += ";";
                network += this.nn[i].ToString();
            }
            Save.ToFile(name, network);
        }

        public static NeuralNetwork LoadNeuralNetwork() {
            if(!Load.CheckForFile("nn"))
                throw new AccessViolationException("File not found");

            var loaded = Load.FromFile("nn.txt");
            var layers = loaded.Split(';');
            var nnet = new Matrix[layers.Length];

            for(int i = 0; i < layers.Length; i++)
            {
                nnet[i] = Matrix.Parse(layers[i]);
            }

            return new NeuralNetwork(nnet);
        }
    }
}