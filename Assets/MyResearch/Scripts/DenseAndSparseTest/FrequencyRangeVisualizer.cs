using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyRangeVisualizer : MonoBehaviour
{
    [SerializeField] private Vector3 cubeSize = new Vector3(0.30f, 0.3f, 0.1f);
    [SerializeField] private DenseSparseExpController denseSparseExpController;
    [SerializeField] private CalculateDistance calculateDistance;
    private Vector3 cubePosition;

    private GameObject frequencyCube;

    void Start()
    {
        cubePosition = denseSparseExpController.GetStartCoordinate();
        SetCubeSize();
        CreateFrequencyCube();
    }

    void Update()
    {

    }


    private void SetCubeSize()
    {
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            float xYLength = (float)calculateDistance.GetCentralRequiredLength() * 2;
            float zLength = (float)calculateDistance.GetDepthRequiredLength() * 2;
            cubeSize = new Vector3(xYLength, xYLength, zLength);

        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            // soundLength = (denseSparseExpController.GetObjectCount() - 1) * denseSparseExpController.GetInterval() + 2 *
            // calculateDistance.GetRequiredLength();
        }
    }

    public void CreateFrequencyCube()
    {
        frequencyCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frequencyCube.transform.localScale = cubeSize;
        frequencyCube.transform.position = cubePosition;

        // キューブのRendererを取得
        Renderer cubeRenderer = frequencyCube.GetComponent<Renderer>();

        // 透過をサポートする新しいマテリアルを作成
        Material transparentMaterial = new Material(Shader.Find("Standard"));
        ChangeRenderMode(transparentMaterial, BlendMode.Fade);

        // アルファ値を0.5に設定（半透明）
        Color cubeColor = new Color(1f, 1f, 1f, 0.5f); // 白色、透明度50%
        transparentMaterial.color = cubeColor;

        // マテリアルをキューブに適用
        cubeRenderer.material = transparentMaterial;
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
