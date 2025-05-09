using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour {
    int  idindex =0 ;
    public int simspeed=1;
    public int MaxCreatureBeforeEvolving=70;
    public int currentDeaths = 0;
    public GameObject creaturePrefab;
    public int initialCount = 100;
    public List<Creature> allCreatures = new();
    public static CreatureManager instance;
    public int CreaturesCount=0;
    public List<Creature> survivors = new(); 
    
    void Awake(){
      instance=this;
    }
    void Start() {
        Time.timeScale = simspeed;
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
        c.id=idindex;
        idindex++;
        allCreatures.Add(c);
    }

    DNA RandomDNA() {
    return new DNA {
        speed = Random.Range(0.5f, 2f),
        size = Random.Range(0.5f, 2f),
        vision = Random.Range(3f, 8f),
        metabolism = Random.Range(0.1f, 0.2f),
        dietType = (DietType)Random.Range(0, 3),
        aggression = (AggressionLevel)Random.Range(0, 3),
        socialBehavior = Random.Range(0f, 1f),
        curiosity = Random.Range(0f, 1f),
        predationInstinct = Random.Range(0f, 1f),
        brain = new NeuralNetwork(new int[] { 9, 10, 10, 3 }) // Updated to 9 inputs for new traits
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
     public void RemoveCreature (int id){
          Creature dead = allCreatures.Find(c => c.id == id);
    if (dead != null)
    {
        // Save DNA if energy was high enough
       // if (dead.energy >= 100)
          //  survivors.Add(dead);

        allCreatures.Remove(dead);
        CreaturesCount = allCreatures.Count;
        currentDeaths++;

        // Check if all creatures in this wave are dead
        if (currentDeaths >= MaxCreatureBeforeEvolving)
        {
            Evolve();
        }
    }

     }



void Evolve()
{
    Debug.Log("Evolving ......");

    List<DNA> survivorDNA = new();
    
    foreach (Creature c in allCreatures)
    {
        if(c.energy>99||c.dna.dietType==DietType.Herbivore)
        survivorDNA.Add(c.dna);
    }

    Debug.Log("Survivor count: " + survivorDNA.Count);

    // Cleanup old survivors
    foreach (Creature c in allCreatures)
    {
        if (c != null)
            Destroy(c.gameObject);
    }

    allCreatures.Clear();
    survivors.Clear();
    currentDeaths = 0;

    // Reproduce new generation
    for (int i = 0; i < 100; i++)
    {
        DNA childDNA;

        if (survivorDNA.Count > 0)
        {
            DNA parent = survivorDNA[Random.Range(0, survivorDNA.Count)];
            childDNA = parent.CloneAndMutate();
        }
        else
        {
            childDNA = RandomDNA(); // Fallback to random
        }

        Vector2 pos = GetSafeSpawnPosition();
        GameObject obj = Instantiate(creaturePrefab, pos, Quaternion.identity);
        Creature c = obj.GetComponent<Creature>();
        c.Initialize(childDNA);
        c.id = idindex++;
        allCreatures.Add(c);
    }

    CreaturesCount = allCreatures.Count;
}

     
}
