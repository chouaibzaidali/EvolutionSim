using UnityEngine;

public class NeuralNetwork {
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;

    public NeuralNetwork(int[] layers) {
        this.layers = layers;
        InitNeurons();
        InitWeights();
    }

    void InitNeurons() {
        neurons = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++)
            neurons[i] = new float[layers[i]];
    }

    void InitWeights() {
        weights = new float[layers.Length - 1][][];
        for (int i = 0; i < layers.Length - 1; i++) {
            weights[i] = new float[layers[i]][];
            for (int j = 0; j < layers[i]; j++) {
                weights[i][j] = new float[layers[i + 1]];
                for (int k = 0; k < layers[i + 1]; k++) {
                    weights[i][j][k] = Random.Range(-1f, 1f);
                }
            }
        }
    }

    public float[] FeedForward(float[] inputs) {
        for (int i = 0; i < inputs.Length; i++)
            neurons[0][i] = inputs[i];

        for (int i = 1; i < layers.Length; i++) {
            for (int j = 0; j < layers[i]; j++) {
                float sum = 0f;
                for (int k = 0; k < layers[i - 1]; k++)
                    sum += neurons[i - 1][k] * weights[i - 1][k][j];
                neurons[i][j] = (float)System.Math.Tanh(sum);
            }
        }

        return neurons[^1];
    }

    public float[][][] GetWeights() => weights;

    public void SetWeights(float[][][] newWeights) {
        weights = newWeights;
    }

    public NeuralNetwork CloneAndMutate(float rate) {
        NeuralNetwork clone = new NeuralNetwork(layers);
        float[][][] newWeights = new float[weights.Length][][];

        for (int i = 0; i < weights.Length; i++) {
            newWeights[i] = new float[weights[i].Length][];
            for (int j = 0; j < weights[i].Length; j++) {
                newWeights[i][j] = new float[weights[i][j].Length];
                for (int k = 0; k < weights[i][j].Length; k++) {
                    float w = weights[i][j][k];
                    w += Random.Range(-rate, rate);
                    newWeights[i][j][k] = Mathf.Clamp(w, -1f, 1f);
                }
            }
        }

        clone.SetWeights(newWeights);
        return clone;
    }
}
