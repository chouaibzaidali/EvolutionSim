using UnityEngine;

public static class ColorHelper {
    public static Color FromDNA(DNA dna) {
    float r = Mathf.InverseLerp(0.5f, 3f, dna.speed);
    float g = Mathf.InverseLerp(0.3f, 3f, dna.size);
    float b = Mathf.InverseLerp(2f, 10f, dna.vision);

    // Modify based on aggression
    float aggressionBoost = (float)dna.aggression / 2f; // 0 = Passive, 0.5 = Neutral, 1 = Aggressive
    r = Mathf.Clamp01(r + aggressionBoost * 0.3f); // Increase red with aggression

    // Modify based on diet type
    switch (dna.dietType) {
        case DietType.Herbivore:
            g += 0.2f; // Make greener
            break;
        case DietType.Carnivore:
            r += 0.2f; // Emphasize red
            break;
        case DietType.Omnivore:
            b += 0.2f; // Add blue tint for mixed traits
            break;
    }

    return new Color(
        Mathf.Clamp01(r),
        Mathf.Clamp01(g),
        Mathf.Clamp01(b)
    );
}
}
