using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine;

public class DenseDataLoggerController : MonoBehaviour
{
    [SerializeField] private DenseSparseExpController denseSparseExpController;
    [SerializeField] private ChaseGameController chaseGameController;
    private StreamWriter writer;
    private string folder;

    private string subjectName;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(float interval, DenseOrSparse denseOrSparse, ExpScene expScene)
    {
        subjectName = denseSparseExpController.GetSubjectName();

        folder = $"C:\\Users\\takaharayota\\Research\\{expScene}\\{subjectName}";
        Directory.CreateDirectory(folder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string fileName = $"{subjectName}_{denseOrSparse}_{interval}_{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, fileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);

    }
    public void InitializeAsChaseGame()
    {
        subjectName = chaseGameController.GetSubjectName();

        folder = $"C:\\Users\\takaharayota\\Research\\ChaseGame\\{subjectName}";
        Directory.CreateDirectory(folder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string fileName = $"{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, fileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);

    }
    public void InitializeAsVRKeyboard(float interval)
    {
        subjectName = denseSparseExpController.GetSubjectName();

        folder = $"C:\\Users\\takaharayota\\Research\\VRKeyboard\\{subjectName}";
        Directory.CreateDirectory(folder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string fileName = $"{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, fileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);

    }

    public void InitializeAsSurgeryExp(float interval, DenseOrSparse denseOrSparse)
    {
        subjectName = denseSparseExpController.GetSubjectName();

        folder = $"C:\\Users\\takaharayota\\Research\\Surgery-Exp\\{subjectName}\\{denseOrSparse}";
        Directory.CreateDirectory(folder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string fileName = $"{subjectName}_{denseOrSparse}_{interval}_{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, fileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);

    }
    public void WriteInformation(Vector3 controllerPosition)
    {

        writer.WriteLine($"{controllerPosition.x},{controllerPosition.y},{controllerPosition.z}");

    }
    public void ReflectPlaceChange(
        int placeIndex
    )
    {
        writer.WriteLine("place index:" + placeIndex);
    }
    public void ReflectAlphabetChange(
        string alphabet
    )
    {
        writer.WriteLine("alphabet index:" + alphabet);
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void Close()
    {
        writer.Close();
    }
    // private void OnDestroy()
    // {

    // }
}
