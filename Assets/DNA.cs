using UnityEngine;

public enum DietType { Herbivore, Carnivore, Omnivore }
public enum AggressionLevel { Passive, Neutral, Aggressive }

public class DNA {
    public float speed;
    public float size;
    public float vision;
    public float metabolism;
    public DietType dietType;
    public AggressionLevel aggression;
    public float socialBehavior; // 0 = solitary, 1 = highly social
    public float curiosity;      // tendency to explore vs stay safe
    public float predationInstinct; // likelihood to hunt if carnivore
    public NeuralNetwork brain;

    public DNA CloneAndMutate() {
        return new DNA {
            speed = MutateGene(speed, 0.1f, 0.5f, 3f),
            size = MutateGene(size, 0.1f, 0.3f, 3f),
            vision = MutateGene(vision, 0.2f, 2f, 10f),
            metabolism = MutateGene(metabolism, 0.01f, 0.01f, 0.2f),
            dietType = MutateDiet(dietType),
            aggression = MutateAggression(aggression),
            socialBehavior = MutateGene(socialBehavior, 0.1f, 0f, 1f),
            curiosity = MutateGene(curiosity, 0.1f, 0f, 1f),
            predationInstinct = MutateGene(predationInstinct, 0.1f, 0f, 1f),
            brain = brain.CloneAndMutate(0.1f)
        };
    }

    float MutateGene(float value, float rate, float min, float max) {
        return Mathf.Clamp(value + Random.Range(-rate, rate), min, max);
    }

    DietType MutateDiet(DietType current) {
        if (Random.value < 0.05f) {
            return (DietType)Random.Range(0, 3); // small chance to switch diet
        }
        return current;
    }

    AggressionLevel MutateAggression(AggressionLevel current) {
        if (Random.value < 0.05f) {
            return (AggressionLevel)Random.Range(0, 3);
        }
        return current;
    }
}
