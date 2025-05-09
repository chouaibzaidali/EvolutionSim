using UnityEngine;

public class Creature : MonoBehaviour {
    public int id =00;
    public DNA dna;
    public float energy = 100f;
    public float age = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (dna == null) {
            Debug.LogError("DNA not set!");
            return;
        }

        transform.localScale = Vector3.one * dna.size;
        sr.color = ColorHelper.FromDNA(dna);
        
    }

    void Update() {
        if (dna == null) return;

        float[] inputs = GetInputs();
        float[] outputs = dna.brain.FeedForward(inputs);

        Move(outputs);
        TryEat();
       // TryReproduce();
       // if(dna.dietType!=DietType.Herbivore)//for testing until beter way harbivors doesnt lose energie so they can survive untile soimething like grass is implemented
        energy -= dna.metabolism * Time.deltaTime * 0.5f;
        age += Time.deltaTime;

        if (energy <= 0 || age > 1000f)
            Die();
    }

    float[] GetInputs() {
      float[] visionData = Sense();

    return new float[] {
        energy / 200f,
        age / 100f,
        visionData[0], // sees target
        visionData[1], // target energy
        visionData[2], // target size
        dna.socialBehavior,
        dna.curiosity,
        dna.predationInstinct,
        (float)dna.aggression / 2f // normalized: 0 (Passive), 0.5 (Neutral), 1 (Aggressive)
    };
    }

    float[] Sense() {
      Vector2[] directions = {
    transform.up,
    Quaternion.Euler(0, 0, 90) * transform.up,
    Quaternion.Euler(0, 0, -90) * transform.up,
    Quaternion.Euler(0, 0, -180) *transform.up
    
};

foreach (var dir in directions) {
    Debug.DrawRay(transform.position, dir * dna.vision, Color.cyan, 0.1f);

    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dna.vision);
    if (hit.collider != null && hit.collider.TryGetComponent(out Creature other) && other != this) {
        return new float[] { 1f, other.energy / 100f, other.dna.size / 3f };
    }
}


        return new float[] { 0f, 0f, 0f };
    }

    void Move(float[] outputs) {
           Vector2 moveDirection = new Vector2(outputs[0], outputs[1]).normalized;
    float rotationInput = outputs.Length > 2 ? outputs[2] : 0f;

    // Apply movement in the forward direction
    rb.linearVelocity = transform.up * dna.speed * moveDirection.magnitude;

    // Rotate creature based on neural output
    float rotationSpeed = 200f; // You can tweak this value
    transform.Rotate(Vector3.forward, -rotationInput * rotationSpeed * Time.deltaTime);
    }
   
 void TryEat() {
    Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 0.5f);
    foreach (var col in nearby) {
        if (col.TryGetComponent(out Creature other) && other != this) {
            // Don't eat same species or allies (example logic)
            if (dna.dietType == DietType.Herbivore) continue;

            bool canPredate = dna.dietType == DietType.Carnivore || dna.dietType == DietType.Omnivore;
            bool isSmallerPrey = this.dna.size > other.dna.size * 0.9f;
            bool isAggressiveEnough = dna.predationInstinct > 0.5f;

            if (canPredate && isSmallerPrey && isAggressiveEnough) {
                energy = Mathf.Min(energy + other.energy * 0.9f, 200f);
                other.Die();
                break;
            }
        }
    }
}

    void TryReproduce() {
        if (energy > 180f) {
            GameObject clone = Instantiate(gameObject, transform.position + (Vector3)(Random.insideUnitCircle * 1f), Quaternion.identity);

            Creature child = clone.GetComponent<Creature>();
            child.dna = dna.CloneAndMutate();
            child.energy = energy / 2f;
            this.energy /= 2f;
        }
    }

    public void Die() {
        CreatureManager.instance.RemoveCreature(id);
       // Debug.Log("deleted" + id);
        Destroy(gameObject);
    }

    public void Initialize(DNA assignedDNA) {
        dna = assignedDNA;
        transform.localScale = Vector3.one * dna.size;
        GetComponent<SpriteRenderer>().color = ColorHelper.FromDNA(dna);
    }
}
