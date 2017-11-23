using System;
using UnityEngine;

/// <summary>
/// Small and simple Neural Network library
/// </summary>
public class NeuralNetwork{
    // private int inputs;
    // private int[] layers;
    private Matrix[] nn;

    private int beta = 5;

    /// <summary>
    /// Constructor takes number of inputs and an array of numbers of neurons in each layer.
    /// Initialize neural network with random wages.
    /// </summary>
    public NeuralNetwork(int inputs, int[] layers){
        // inputs = _inputs;
        // layers = _layers;

        nn = new Matrix[layers.Length];
        nn[0] = Matrix.RandomMatrix(inputs + 1, layers[0], 0.1f);    
        for(int i = 1; i < layers.Length; i++){
            nn[i] = Matrix.RandomMatrix(layers[i-1] + 1, layers[i], 0.1f);                 
        } 

        // Display your nn
        // for(int i = 0; i < nn.Length; i++){
        //     Debug.Log("layer " + i + " rows: " + nn[i].rows + " cols: " + nn[i].cols);     
        //     Debug.Log(nn[i].ToString());                  
        // }
    }

    /// <summary>
    /// Under work
    /// </summary>
    // IN:
    // OUT:
    public void Learn(Matrix P, Matrix T, int n){
        int examples = P.cols; // n.o. examples
        int nl = nn.Length; // n.o. layers
        float lr = .05f; // learning rate

        for(int l = 0; l < n; l++){
            // ============= Choosing an example ==============
            int next = UnityEngine.Random.Range(0, examples);
            string example = String.Empty;
            for(int i = 0; i < P.rows; i++)
                example += P[i,next] + "\r\n";
            Matrix signal = Matrix.Parse(example); 
            // Debug.Log("example: " + example);
            // ================================================
            
            // ==== Signal running through neural network =====
            Matrix[] signals = new Matrix[nl+1];
            signals[0] = Matrix.Parse("-1\r\n" + signal.ToString()); // bias
 
            for(int i = 0; i < nl; i++) {
                signal = Run(signal, i);
                if(i == nl - 1)
                    signals[i+1] = signal;   
                else
                    signals[i+1] = Matrix.Parse("-1\r\n" + signal.ToString());    
            // TODO: think about adding Matrix method, ToString() is cutting informations                                                  
            }              
            // ================================================

            // ================ Correct anwser ================
            string anwser = String.Empty;
            for(int i = 0; i < T.rows; i++)
                anwser += T[i,next] + "\r\n";
            // Debug.Log("anwser: " + anwser);
            // ================================================            

            // ===== Error and back propagation algorithm =====
            Matrix delta = Matrix.Parse(anwser) - signals[nl]; 
            Matrix error = delta.Duplicate(); // to have the same dimensions

            for(int k = 0; k < nl; k++){
                signal = signals[nl - k];
                if(signal.rows > error.rows){
                    signal = Matrix.RemoveRow(signal, 0); // remove bias if needed
                }             
                
                // Debug.Log("signal rows: " + signal.rows + " cols " + signal.cols);
                // Debug.Log("delta rows: " + delta.rows + " cols " + delta.cols + "\r\n" + delta.ToString());
                // Debug.Log("error rows: " + error.rows + " cols " + error.cols + "\r\n" + error.ToString());

                for(int i = 0; i < error.rows; i++){
                    for(int j = 0; j < error.cols; j++){
                        error[i,j] = delta[i,j] * beta * signal[i,j] * (1 - signal[i,j]); // for sigmoid activation function
                    }
                }

                Matrix dnn = lr * signals[nl - 1 - k] * Matrix.Transpose(error);
                // Debug.Log("dnn rows: " + dnn.rows + " cols " + dnn.cols + "\r\n" + dnn.ToString());    
                // Debug.Log("nn rows " + nn[nl - 1 - k].rows + " cols " + nn[nl - 1 - k].cols + "\r\n" + nn[nl-1-k].ToString());                            
                nn[nl - 1 - k] += dnn; 
                      

                delta = Matrix.RemoveRow(nn[nl - 1 - k], 0) * error; // backpropagation
                error = delta.Duplicate(); // same dimensions
            }
            // ================================================
        }
    }

    private Matrix Run(Matrix signal, int index){
        // Debug.Log("-1\r\n" + signal.ToString());
         Matrix X = Matrix.Parse("-1\r\n" + signal.ToString()); // adding bias      
        // Debug.Log("index: " + index +"\r\n\r\n" +
        //             nn[index].ToString() + "\r\n\r\n" +
        //             "nn rows: " + nn[index].rows + " nn cols: " + nn[index].cols);

        // Debug.Log(X.ToString() + "\r\n\r\n" + "signal rows: " + X.rows + " nn cols: " + X.cols);
        Matrix U = Matrix.Transpose(nn[index]) * X;
        return ActivationFunction(U);  
    }
    
    public Matrix Run(Matrix signal){
        for(int i = 0; i < nn.Length-1; i++){
            signal = Run(signal, i);             
        }
        return Run(signal, nn.Length-1);
    }

    private Matrix ActivationFunction(Matrix U){
        // sigmoid:
        // 1/(1+exp(-beta*U))
        Matrix Y = U.Duplicate();
        for(int i = 0; i < Y.rows; i++){
            for(int j = 0; j < Y.cols; j++){
                Y[i,j] = 1/(1+ System.Math.Exp(-beta * U[i,j]));
            }
        }
        return Y;
    }
    
    private Matrix ActivationFunction2(Matrix U){
        // ReLU
        Matrix Y = U.Duplicate(); // its already Y(k,l) = U(k,l);
        for(int i = 0; i < Y.rows; i++){
            for(int j = 0; j < Y.cols; j++){
                if(U[i,j] < 0)
                    Y[i,j] = 0;
            }
        }
        return Y;
    }

    public void SaveNeuralNetwork(){
        string network = String.Empty;
        for(int i = 0; i < nn.Length; i++) {
            if( i > 0)
                network += ";";            
            network += nn[i].ToString();
        }
        Save.ToFile("nn", network);
    }

    public void LoadNeuralNetwork(){
        if(!Load.CheckForFile("nn"))
            return;
        string loaded = Load.FromFile("nn");
        string[] network = loaded.Split(';');

        nn = new Matrix[network.Length];
        for(int i = 0; i < network.Length; i++){
            nn[i] = Matrix.Parse(network[i]);                 
        } 
    }
}