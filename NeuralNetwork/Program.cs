using System;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new int[] {10, 8, 6, 4, 2};
            var nn = new NeuralNetwork(configuration);
            var n = 1;
            var learning = true;
            var e = 100;
            var examples = GeneratExamples(e);

            // for(var i = 0; i < 10; i++)
            // {
            //     Console.WriteLine("Learning examples: " + i);
            //     nn.Learn(examples[0], examples[1], examples[2], examples[3]);
            // }

            // while(learning)
            // {
            //     Console.WriteLine("Learning examples: " + n++);
            //     var examples = GeneratExamples(e);
            //     if(nn.Learn(examples[0], examples[1], examples[2], examples[3]))
            //     {
            //         learning = false;
            //     }
            // }

            // nn.SaveNeuralNetwork("nn");

            // var ex = GeneratExamples(e);

            // for(int i = 0; i < e; i++)
            // {
            //     var response = nn.Run(ex[0].GetCol(i));
            //     Console.WriteLine("Correct: \n" + ex[1].GetCol(i).ToString() + "Network response: \n" + response.ToString());
            // }

            // XOR();
            var evo = new Evolution();

            evo.Run();
        }

        public static Matrix[] GeneratExamples(int n) {
            var _examples = CreateEmptyExamples(n);
            var _tests    = CreateEmptyExamples(n);
            Save.ToFile("examples/examples_generated", _examples);
            Save.ToFile("examples/test_generated", _tests);
            
            var examples      = LoadExamples("examples/examples_generated.txt");
            var questions     = examples[0];
            var anwsers       = examples[1];

            examples          = LoadExamples("examples/test_generated.txt");
            var testQuestions = examples[0];
            var testAnwasers  = examples[1];

            return new Matrix[] {questions, anwsers, testQuestions, testAnwasers};
        }

        static void XOR()
        {
            Matrix questions = Matrix.Parse("0 0 1 1\r\n0 1 0 1");
            Matrix anwsers   = Matrix.Parse("0 1 1 0");

            var nn = new NeuralNetwork(new int[] {2, 4, 2, 1});

            Console.WriteLine("Before learning");
            Console.WriteLine("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
            Console.WriteLine("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
            Console.WriteLine("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());
            Console.WriteLine("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());

            nn.Learn(questions, anwsers, questions, anwsers);
            nn.Learn(questions, anwsers, 1000);
            Console.WriteLine("After learning");
            Console.WriteLine("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
            Console.WriteLine("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
            Console.WriteLine("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());
            Console.WriteLine("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());
        }

        static Matrix[] LoadExamples(string path)
        {
            var questions = string.Empty;
            var anwsers   = string.Empty;
            var question  = new string[10];
            var anwser    = new string[2];
            var newLine   = "\r\n";

            var loaded    = Load.FromFile(path);
            var examples  = loaded.Split(";");
            var length    = examples.GetLength(0);

            for (var i = 0; i < length - 1; i++)
            {
                var example = examples[i].Split(",");

                Array.Copy(example, 0,  question, 0, 10);
                Array.Copy(example, 10, anwser,   0, 2);

                if(i >= length - 2)
                    newLine = String.Empty;

                questions += String.Join(" ", question) + newLine;
                anwsers   += String.Join(" ", anwser)   + newLine;
            }

            questions = DotToComma(questions);
            anwsers   = DotToComma(anwsers);

            var Q = Matrix.Transpose(Matrix.Parse(questions));
            var A = Matrix.Transpose(Matrix.Parse(anwsers));

            return new Matrix[] {Q, A};
        }

        static string DotToComma(string input)
        {
            return input.Replace('.', ',');
        }

        static string CreateEmptyExamples(int n) {
            var zeros = ".00,";
            var view  = "0.00,0.00,0.00,0.00,2.00,0.00,";
            var side  = new string[] {"0.00,1.00,", "0.00,-1.00,", "1.00,0.00,", "-1.00,0.00,"};
            var save  = string.Empty;

            for(int i = 0, s = 0; i < n; i++, s++)
            {
                if (s > 3)
                    s = 0;

                var x = Rand(); var y = Rand();
                var anwser = Normalize(Anwser(x, y, s), s);

                save += view + side[s];
                save += x + zeros + y + zeros;
                save += anwser[0] + zeros + anwser[1] + ".00;";
            }

            return save;
        }

        static int[] Anwser(int x, int y, int side)
        {
            bool foodIsBehind = false;

            switch(side)
            {
                case 0:
                    foodIsBehind = (y <= 0);
                    break;
                case 1: 
                    foodIsBehind = (y >= 0);
                    break;
                case 2: 
                    foodIsBehind = (x <= 0);
                    break;
                case 3: 
                    foodIsBehind = (x >= 0);
                    break;
            }

            if (side < 2)
            {
                if (foodIsBehind || Abs(x) > Abs(y))
                    return new int[] {x/Abs(x), 0};
                else
                    return new int[] {0, 0};
            }
            else 
            {
                if (foodIsBehind || Abs(x) < Abs(y))
                    return new int[] {0, y/Abs(y)};
                else
                    return new int[] {0, 0};
            }
        }

        static int[] Normalize(int[] face, int side)
        {
            var left    = new int[] {0, 1};
            var right   = new int[] {1, 0};
            var forward = new int[] {1, 1};

            var none = face[0] == 0 && face[1] == 0;

            switch(side)
            {
                case 0:
                    if (none)
                        return forward;
                    else if (face[0] < 0)
                        return left;
                    else 
                        return right;
                case 1:
                    if (none)
                        return forward;
                    else if (face[0] < 0)
                        return right;
                    else 
                        return left;
                case 2:
                    if (none)
                        return forward;
                    else if (face[1] < 0)
                        return right;
                    else 
                        return left;
                case 3:
                    if (none)
                        return forward;
                    else if (face[1] < 0)
                        return left;
                    else 
                        return right;
            }

            return new int[] {0, 0};
        }

        static int Abs(int n)
        {
            return Math.Abs(n);
        }

        static int Rand() {
            var rand   = new Random();
            var range  = new int[] {-40, 40};
            var number = 0;

            while(number >= -1 && number <= 1)
            {
                number = rand.Next(range[0], range[1]); 
            }

            return number;
        }
    }
}
