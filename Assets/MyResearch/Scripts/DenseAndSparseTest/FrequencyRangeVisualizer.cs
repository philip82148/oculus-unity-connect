using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyRangeVisualizer : MonoBehaviour
{
    [SerializeField] private Vector3 cubeSize = new Vector3(0.30f, 0.3f, 0.1f);
    [SerializeField] private DenseSparseExpController denseSparseExpController;
    [SerializeField] private CalculateDistance calculateDistance;
    private Vector3 cubePosition;

    private List<GameObject> frequencyCubes = new List<GameObject>();
    private float previousInterval;

    void Start()
    {
        previousInterval = denseSparseExpController.GetInterval();
        cubePosition = denseSparseExpController.GetStartCoordinate();
        SetCubeSize();
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            CreateFrequencyCube(cubePosition);
        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            List<Vector3> cubePositions = denseSparseExpController.GetTargetCoordinates();
            for (int i = 0; i < cubePositions.Count; i++)
            {
                CreateFrequencyCube(cubePositions[i]);
            }
        }
    }

    void Update()
    {
        float currentInterval = denseSparseExpController.GetInterval();
        if (currentInterval != previousInterval)
        {
            SetCubeSize();
            UpdateCubePositions();
            previousInterval = currentInterval;
        }
    }

    private void SetCubeSize()
    {
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            float xLength = (float)calculateDistance.GetCentralRequiredLength() * 2;
            float yLength = (float)calculateDistance.GetCentralRequiredLength() * 2;
            float zLength = (float)calculateDistance.GetCentralRequiredLength() * 2;
            cubeSize = new Vector3(xLength, yLength, zLength);
        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            float xLength = (float)calculateDistance.GetRequiredLength() * 2;
            float yLength = (float)calculateDistance.GetRequiredLength() * 2;
            float zLength = (float)calculateDistance.GetRequiredLength() * 2;
            cubeSize = new Vector3(xLength, yLength, zLength);
        }
    }

    public void CreateFrequencyCube(Vector3 cubePosition)
    {
        GameObject frequencyCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frequencyCube.transform.localScale = cubeSize;
        frequencyCube.transform.position = cubePosition;

        // Get the Renderer of the cube
        Renderer cubeRenderer = frequencyCube.GetComponent<Renderer>();

        // Create a new material that supports transparency
        Material transparentMaterial = new Material(Shader.Find("Standard"));
        ChangeRenderMode(transparentMaterial, BlendMode.Fade);

        // Set alpha value to 0.5 for semi-transparency
        Color cubeColor = new Color(1f, 1f, 1f, 0.5f); // White color, 50% transparency
        transparentMaterial.color = cubeColor;

        // Apply the material to the cube
        cubeRenderer.material = transparentMaterial;

        // Add the cube to the list for later updates
        frequencyCubes.Add(frequencyCube);
    }

    private void UpdateCubePositions()
    {
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            if (frequencyCubes.Count > 0)
            {
                frequencyCubes[0].transform.position = denseSparseExpController.GetStartCoordinate();
                frequencyCubes[0].transform.localScale = cubeSize;
            }
        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            List<Vector3> cubePositions = denseSparseExpController.GetTargetCoordinates();
            for (int i = 0; i < frequencyCubes.Count; i++)
            {
                frequencyCubes[i].transform.position = cubePositions[i];
                frequencyCubes[i].transform.localScale = cubeSize;
            }
        }
    }

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public void ChangeRenderMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case BlendMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case BlendMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }
}
