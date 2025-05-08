using UnityEngine;

public class DNA {
    public float speed;
    public float size;
    public float vision;
    public float metabolism;
    public NeuralNetwork brain;

    public DNA CloneAndMutate() {
        return new DNA {
            speed = MutateGene(speed, 0.1f, 0.5f, 3f),
            size = MutateGene(size, 0.1f, 0.3f, 3f),
            vision = MutateGene(vision, 0.2f, 2f, 10f),
            metabolism = MutateGene(metabolism, 0.01f, 0.01f, 0.2f),
            brain = brain.CloneAndMutate(0.1f)
        };
    }

    float MutateGene(float value, float rate, float min, float max) {
        return Mathf.Clamp(value + Random.Range(-rate, rate), min, max);
    }
}
