using UnityEngine;

public class PixelCounter : MonoBehaviour {

    [SerializeField] private RenderTexture renderTexture = null;

    [SerializeField] private int numParts = 1;

    private int texWidth, texHeight;
    private int numPixels;
    private int numPixelsInPart;

    private Texture2D readTexture;
    private Color32[] colors;

    private int currentPart = 0;

    private int[] numTransparentPixelsInParts;

    private int numTransparentPixels;

    private void Start() {
        numTransparentPixelsInParts = new int[numParts];

        texWidth = renderTexture.width;
        texHeight = renderTexture.height;

        numPixels = texWidth * texHeight;
        numPixelsInPart = numPixels / numParts;

        readTexture = new Texture2D(texWidth, texHeight);
    }

    private void Update() {
        UpdateColors();

        UpdateCounter();
        Debug.Log("Num transparent pixels: " + numTransparentPixels);

        currentPart = (currentPart + 1) % numParts;
    }

    private void UpdateColors() {
        UpdateReadTexture();
        colors = readTexture.GetPixels32();
    }

    private void UpdateReadTexture() {
        RenderTexture.active = renderTexture;
        readTexture.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        readTexture.Apply();
        RenderTexture.active = null;
    }

    private void UpdateCounter() {
        numTransparentPixels -= numTransparentPixelsInParts[currentPart];
        numTransparentPixelsInParts[currentPart] = 0;

        CountTransparentPixels();

        numTransparentPixels += numTransparentPixelsInParts[currentPart];
    }

    private void CountTransparentPixels() {
        int startPixelIndex = currentPart * numPixelsInPart;
        int endPixelIndex = (currentPart + 1) * numPixelsInPart;

        for (int i = startPixelIndex; i < endPixelIndex; ++i) {
            if (colors[i].a < 255) { numTransparentPixelsInParts[currentPart]++; }
        }
    }

}