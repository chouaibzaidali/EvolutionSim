using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour {
    public CreatureManager creatureManager;

    void Update() {
        if (creatureManager.allCreatures.Count <= 10)
            Evolve();
    }

    void Evolve() {
        List<DNA> survivors = new();
        foreach (Creature c in FindObjectsOfType<Creature>())
            if (c.energy > 100)
                survivors.Add(c.dna);

        foreach (Creature c in FindObjectsOfType<Creature>())
            Destroy(c.gameObject);

        creatureManager.allCreatures.Clear();

        for (int i = 0; i < 100; i++) {
            DNA parent = survivors[Random.Range(0, survivors.Count)];
            DNA child = parent.CloneAndMutate();
            GameObject obj = Instantiate(creatureManager.creaturePrefab, Random.insideUnitCircle * 10f, Quaternion.identity);
            Creature c = obj.GetComponent<Creature>();
            c.dna = child;
            creatureManager.allCreatures.Add(c);
        }
    }
}
