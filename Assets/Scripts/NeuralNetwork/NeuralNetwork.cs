using System;
using UnityEngine;

/// <summary>
/// Small and simple Neural Network library
/// </summary>
public class NeuralNetwork {
    private Matrix[] nn;
    private int beta = 5;

    NeuralNetwork() {}

    /// <summary>
    /// Constructor takes number of inputs and an array of numbers of neurons in each layer.
    /// Initialize neural network with random wages.
    /// </summary>
    public NeuralNetwork(int inputs, int[] layers) {
        nn = new Matrix[layers.Length];
        nn[0] = Matrix.RandomMatrix(inputs + 1, layers[0], 0.1f);
        for(int i = 1; i < layers.Length; i++){
            nn[i] = Matrix.RandomMatrix(layers[i-1] + 1, layers[i], 0.1f);
        } 
    }

    public void Display() {
        for(int i = 0; i < nn.Length; i++) {
            Debug.Log("layer " + i + " rows: " + nn[i].rows + " cols: " + nn[i].cols);
            Debug.Log(nn[i].ToString());
        }
    }

    Matrix AddBias(Matrix signal) {
        return Matrix.Parse("-1\r\n" + signal.ToString());
    }

    Matrix RemoveBias(Matrix signal) {
        return Matrix.RemoveRow(signal, 0);
    }

    Matrix ChooseExample(Matrix examples, int example) {
        return examples.GetCol(example);
    }

    Matrix GetAnwser(Matrix anwsers, int example) {
        return anwsers.GetCol(example);
    }

    Matrix FindError(Matrix signal, Matrix delta) {
        Matrix error = delta.Duplicate(); // to have the same dimensions
        for(int i = 0; i < error.rows; i++){
            for(int j = 0; j < error.cols; j++){
                error[i,j] = delta[i,j] * beta * signal[i,j] * (1 - signal[i,j]);
            }
        }
        return error;
    }

    void BackPropagation(Matrix[] signals, Matrix delta, int layers) {
        Matrix signal;
        float learningRate = .05f; // learning rate

        for(int i = layers; i > 0; i--) {
            signal = signals[i];
            if(signal.rows > delta.rows)
                signal = RemoveBias(signal); // remove bias if needed

            Matrix error = FindError(signal, delta);

            Matrix dnn = learningRate * signals[i-1] * Matrix.Transpose(error); // delta of nn weights
            nn[i-1] += dnn; // adjust weights in neural network

            delta = RemoveBias(nn[i-1]) * error; // delta for previous layer
        }
    }

    /// <summary>
    /// Teach your neural network by example.
    /// </summary>
    // IN: P - example input, T - example output, n - how many times repeat learning
    public void Learn(Matrix P, Matrix T, int n) {
        int example = 0;
        int layers = nn.Length; // number of layers

        for(int i = 0; i < n; i++) {
            example = UnityEngine.Random.Range(0, P.cols);
            Matrix signal = ChooseExample(P, example);
            Matrix[] signals = RunAndReturnSignals(signal, layers);
            Matrix delta = GetAnwser(T, example) - signals[layers];
            BackPropagation(signals, delta, layers);
        }
    }

    Matrix Run(Matrix signal, int index) {
        Matrix X = AddBias(signal);
        Matrix U = Matrix.Transpose(nn[index]) * X;
        return ActivationFunction(U);
    }
    
    public Matrix Run(Matrix signal) {
        for(int i = 0; i < nn.Length-1; i++){
            signal = Run(signal, i);
        }
        return Run(signal, nn.Length-1);
    }

    Matrix[] RunAndReturnSignals(Matrix signal, int layers) {
        Matrix[] signals = new Matrix[layers+1];
        signals[0] = AddBias(signal);

        for(int i = 0; i < layers; i++) {
            signal = Run(signal, i);
            if(i == layers - 1)
                signals[i+1] = signal;
            else
                signals[i+1] = AddBias(signal);
        }
        return signals;
    }

    Matrix ActivationFunction(Matrix U) {
        // sigmoid: 1/(1+exp(-beta*U))
        Matrix Y = U.Duplicate();
        for(int i = 0; i < Y.rows; i++){
            for(int j = 0; j < Y.cols; j++){
                Y[i,j] = 1/(1+ System.Math.Exp(-beta * U[i,j]));
            }
        }
        return Y;
    }

    public void SaveNeuralNetwork() {
        string network = String.Empty;
        for(int i = 0; i < nn.Length; i++) {
            if( i > 0)
                network += ";";
            network += nn[i].ToString();
        }
        Save.ToFile("nn", network);
    }

    public static NeuralNetwork LoadNeuralNetwork() {
        var nnet = new NeuralNetwork(); 
        if(!Load.CheckForFile("nn"))
            throw new AccessViolationException("File not found");
        string loaded = Load.FromFile("nn");
        string[] network = loaded.Split(';');

        nnet.nn = new Matrix[network.Length];
        for(int i = 0; i < network.Length; i++)
            nnet.nn[i] = Matrix.Parse(network[i]);
        return nnet;
    }
}