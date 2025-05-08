using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour {
    public GameObject creaturePrefab;
    public int initialCount = 100;
    public List<Creature> allCreatures = new();

    void Start() {
        for (int i = 0; i < initialCount; i++) {
            SpawnRandomCreature();
        }
    }

    public void SpawnRandomCreature() {
        DNA dna = RandomDNA();
        Vector2 pos = GetSafeSpawnPosition();

        GameObject obj = Instantiate(creaturePrefab, pos, Quaternion.identity);
        Creature c = obj.GetComponent<Creature>();
        c.Initialize(dna);
        allCreatures.Add(c);
    }

    DNA RandomDNA() {
        return new DNA {
            speed = Random.Range(0.5f, 2f),
            size = Random.Range(0.5f, 2f),
            vision = Random.Range(3f, 8f),
            metabolism = Random.Range(0.01f, 0.05f),
            brain = new NeuralNetwork(new int[] { 5, 6, 2 })
        };
    }

    Vector2 GetSafeSpawnPosition(float radius = 0.5f, int maxTries = 20) {
        for (int i = 0; i < maxTries; i++) {
            Vector2 candidate = Random.insideUnitCircle * 15f;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(candidate, radius);
            if (colliders.Length == 0) return candidate;
        }
        return Random.insideUnitCircle * 15f;
    }
}
