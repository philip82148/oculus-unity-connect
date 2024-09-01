using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FingerPaintController : MonoBehaviour
{
    [Header("UI display")]
    [SerializeField] private TextMeshProUGUI displayText;
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    [Header("Pen Properties")]
    public Material drawingMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.001f;
    public Color[] penColors = new Color[] { Color.black, Color.green, Color.blue };



    private LineRenderer currentDrawing;
    private List<Vector3> positions = new List<Vector3>();
    private int currentColorIndex = 1;

    private bool isDrawing = false;

    public void StartDrawing()
    {
        Vector3 fingerPosition = GetRightIndexFingerPosition();
        positions.Clear();
        positions.Add(fingerPosition);
        currentDrawing = new GameObject("Drawing").AddComponent<LineRenderer>();
        currentDrawing.material = drawingMaterial;
        currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
        currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
        currentDrawing.positionCount = 1;
        currentDrawing.SetPosition(0, fingerPosition);
        isDrawing = true;
    }

    private void Update()
    {
        if (isDrawing)
        {
            Debug.Log("start drawing here");
            ContinueDrawing();
        }


    }

    public void ContinueDrawing()
    {
        Vector3 fingerPosition = GetRightIndexFingerPosition();
        // if (Vector3.Distance(positions[positions.Count - 1], fingerPosition) > 0.01f)
        // {
        positions.Add(fingerPosition);
        currentDrawing.positionCount = positions.Count;
        currentDrawing.SetPositions(positions.ToArray());
        displayText.text = "drawing";
        // }
    }

    public void StopDrawing()
    {
        isDrawing = false;
        currentDrawing = null;
    }
    private Vector3 GetRightIndexFingerPosition()
    {

        return indexFinger.transform.position;

    }

}
