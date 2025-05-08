using UnityEngine;

public static class ColorHelper {
    public static Color FromDNA(DNA dna) {
        float r = Mathf.InverseLerp(0.5f, 3f, dna.speed);
        float g = Mathf.InverseLerp(0.3f, 3f, dna.size);
        float b = Mathf.InverseLerp(2f, 10f, dna.vision);
        return new Color(r, g, b);
    }
}
