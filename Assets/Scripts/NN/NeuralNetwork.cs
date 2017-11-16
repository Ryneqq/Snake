using System;
using UnityEngine;

public class NeuralNetwork{
    private int inputs;
    private int[] layers;
    private Matrix[] nn;

    /// <summary>
    /// Constructor takes number of inputs and an array of numbers of neurons in each layer.
    /// Initialize neural network with random wages.
    /// </summary>
    public NeuralNetwork(int _inputs, int[] _layers){
        inputs = _inputs;
        layers = _layers;

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
    /// Learning
    /// </summary>
    // IN:
    // OUT:
    public void Learn(Matrix P, Matrix T, int n){
        int examples = P.cols; // n.o. examples
        int nl = nn.Length; // n.o. layers
        float lr = .1f; // learning rate

        for(int l = 0; l < n; l++){
            // ============= Choosing an example ==============
            int next = UnityEngine.Random.Range(0, examples);
            string example = String.Empty;
            for(int i = 0; i < P.rows; i++)
                example += P[i,next] + "\r\n";
            Matrix signal = Matrix.Parse(example); 
            // ================================================
            
            // ==== Signal running through neural network =====
            Matrix[] signals = new Matrix[nl+1];
            signals[0] = Matrix.Parse("-1\r\n" + signal.ToString()); // bias
 
            for(int i = 0; i < nl; i++){
                // Debug.Log(" nl " + nl + " i " + i);
                signal = Run(signal, i);
                signals[i+1] = signal;             
            }
            // signal = Run(signal, nl - 1); // response of the network
            // signals[nl - 1] = signal;

            Array.Reverse(signals); // make it easy to use                 
            // ================================================

            // ================ Correct anwser ================
            string anwser = String.Empty;
            for(int i = 0; i < T.rows; i++)
                anwser += T[i,next] + "\r\n";
            // ================================================            

            // ===== Error and back propagation algorithm =====
            Matrix error;         
            Matrix delta = Matrix.Parse(anwser) - signals[0]; 

            for(int k = 0; k < nl; k++){
                signal = signals[k];
                error = ActivationFunction(signal);
                Debug.Log("signal rows: " + signal.rows + " cols " + signal.cols);
                Debug.Log("error rows: " + error.rows + " cols " + error.cols); 
 
                for(int i = 0; i < delta.rows; i++){
                    for(int j = 0; j < delta.cols; j++){
                        error[i,j] *= delta[i,j] * (1 - signal[i,j]);
                    }
                }
                
                Matrix dnn = signal * Matrix.Transpose(error); // to chyba nie ten sygnał            
                Debug.Log("dnn rows: " + dnn.rows + " cols " + dnn.cols + "\r\n" + dnn.ToString());
                Debug.Log("nn rows " + nn[nl - 1 - k].rows + " cols " + nn[nl - 1 - k].cols + "\r\n" + nn[nl-1-k].ToString());
                nn[nl - 1 - k] += dnn;

                delta = Matrix.RemoveRow(nn[nl - 1 - k], 0) * error; // backpropagation
            }
            // ================================================
        }
    }

    public Matrix Run(Matrix signal, int index){
        Debug.Log("-1\r\n" + signal.ToString());
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
        int beta = 5;
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
}