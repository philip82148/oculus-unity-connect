using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DrawDataLoggerController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private AudioSettingController audioSettingController;

    [Header("Experiment Setting")]
    [SerializeField]
    private string subjectName = "高原陽太";
    [SerializeField] private int whichAudioParameter = 0;

    private string folder;
    private string coordinateFolder;
    private StreamWriter expResultWriter;
    private StreamWriter coordinateResultWriter;

    private string resultFileName;

    // Start is called before the first frame update
    void Start()
    {
        whichAudioParameter = audioSettingController.GetWhichAudioParameter();
        folder = $"C:\\Users\\takaharayota\\Research\\Exp2-data\\{subjectName}\\Values";
        Directory.CreateDirectory(folder);
        coordinateFolder = $"C:\\Users\\takaharayota\\Research\\Exp2-data\\{subjectName}\\Coordinate";
        Directory.CreateDirectory(coordinateFolder);

        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        resultFileName = $"{whichAudioParameter}_{dateTime}.txt";
        string filePath = System.IO.Path.Combine(folder, resultFileName);
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        expResultWriter = new StreamWriter(fileStream, Encoding.UTF8);

        string timeFilePath = System.IO.Path.Combine(coordinateFolder, resultFileName);
        FileStream coordinateFileStream = new FileStream(timeFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

        coordinateResultWriter = new StreamWriter(coordinateFileStream, Encoding.UTF8);
    }


    // public void WriteResultInformation(float[] resultArray)
    // {
    //     Debug.Log("this place");
    //     writer.WriteLine($"score:{resultArray[0]},accuracy:{resultArray[1]}");
    // }

    public void WriteResultInformation(float consumedTime, float accuracy)
    {
        Debug.Log("this place");
        expResultWriter.WriteLine($"consumedTime:{consumedTime},accuracy:{accuracy}");
    }
    public void WriteCoordinateInformation(int targetPlaceIndex, Vector3 coordinate)
    {

        Debug.Log("called here");
        coordinateResultWriter.WriteLine($"target place:{targetPlaceIndex},result coordinate:{coordinate}");
    }
    public void Close()
    {
        expResultWriter.Close();
        coordinateResultWriter.Close();
    }
}
