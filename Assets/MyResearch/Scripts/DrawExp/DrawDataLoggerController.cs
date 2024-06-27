using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DrawDataLoggerController : MonoBehaviour
{
    [Header("Experiment Setting")]
    [SerializeField]
    private string subjectName = "高原陽太";
    [SerializeField] private int whichAudioParameter = 0;

    private string folder;
    private StreamWriter writer;

    private string fileName;

    // Start is called before the first frame update
    void Start()
    {
        folder = $"C:\\Users\\takaharayota\\Research\\Exp2-data\\{subjectName}\\{whichAudioParameter}";
        Directory.CreateDirectory(folder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        fileName = $"{whichAudioParameter}_{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, fileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

        writer = new StreamWriter(fileStream, Encoding.UTF8);
    }


    public void WriteResultInformation(float[] resultArray)
    {
        Debug.Log("this place");
        writer.WriteLine($"score:{resultArray[0]},accuracy:{resultArray[1]}");
    }
    public void Close()
    {
        writer.Close();
    }
}
