using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System;
using Microsoft.Win32.SafeHandles;
public class TimeLoggerController : MonoBehaviour
{


    private StreamWriter writer;






    public void Initialize(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);
    }

    public void WriteTimeInformation(double time)
    {

        writer.WriteLine($"{time}");

    }
    public void ReflectPlaceChange(
        int placeIndex
    )
    {
        writer.WriteLine("place index:" + placeIndex);
    }


    public void Close()
    {
        writer.Close();
    }
}
