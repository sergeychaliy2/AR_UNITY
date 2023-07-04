using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using System.IO;
using System;
using System.Globalization;

public class QRCodeScanner : MonoBehaviour
{
    [SerializeField] private RawImage _rawImageBakgraund;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private TextMeshProUGUI _textOut;
    [SerializeField] private TextMeshProUGUI _textDataLog;
    [SerializeField] private RectTransform _scanZone;
    [SerializeField] private RectTransform _fieldDataLog;
    private bool _isCamAvaible;
    private WebCamTexture _cameraTexture;
    private static string fileName = "/note.txt";
    private string realPath = Application.persistentDataPath + fileName;
    // Start is called before the first frame update
    public static DateTime Now { get; }
    private string DataTimeFoo()
    {
        DateTime localDate = DateTime.Now;
        DateTime utcDate = DateTime.UtcNow;
        String[] cultureNames = { "ru-RU" };
        string dataLogTime=null;

        foreach (var cultureName in cultureNames)
        {
            var culture = new CultureInfo(cultureName);
            dataLogTime =localDate.ToString(culture);
            
        }
        return dataLogTime;
    }
    void Start()
    {
        ActiveSceneDataLog(_fieldDataLog, false);
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }
    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            _isCamAvaible = false;
            return;
        }
        for(int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            }
        }
        _cameraTexture.Play();
        _rawImageBakgraund.texture = _cameraTexture;
        _isCamAvaible = true;
    }
    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }
        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;
        int orientation = -_cameraTexture.videoRotationAngle;
        bool mirrored = _cameraTexture.videoVerticallyMirrored;
        if (mirrored)
        {
            _rawImageBakgraund.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
        }
        else
        {
            _rawImageBakgraund.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
        }
    }
    public void OnClickScan()
    {
        Scan();
    }
    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                _textOut.text = result.Text;
                OKScanCube();
            }
            else
            {
                _textOut.text = "Failed scan QR";
                ErrorScanCube();
            }
        }
        catch
        {
            _textOut.text = "Failed in try";
        }   
    }
    private IEnumerator Turn(float timeSleep)
    {
        yield return new WaitForSeconds(timeSleep);
    }
    private void ColorScannerCube()
    {
        _scanZone.GetComponent<Outline>().effectColor = Color.white;
    }
    private void ErrorScanCube()
    {
        _scanZone.GetComponent<Outline>().effectColor = Color.red;
        Invoke("ColorScannerCube", 1);
    }
    private void OKScanCube()
    {
        _scanZone.GetComponent<Outline>().effectColor = Color.green;
        Invoke("ColorScannerCube", 1);
    }
    public void SaveDataAtFileButtonClick()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + fileName,true);

        using (writer)
        {
            writer.WriteLine(DataTimeFoo()+"\n"+_textOut.text);
        }
        writer.Close();
    }
    public void OpenAndReadFileDataAsync()
    {
        ActiveSceneDataLog(_fieldDataLog,true);
        StreamReader streamReader = new StreamReader(Application.persistentDataPath + fileName);
        using (streamReader)
        {
            string texts = streamReader.ReadToEnd();
            _textDataLog.SetText(texts.ToString());
        }
        Application.OpenURL(streamReader.ToString());
        streamReader.Close();
        Application.OpenURL(realPath);
    }
    public void ActiveSceneDataLog(RectTransform _fieldDataLogs,bool arg)
    {
        _fieldDataLogs.gameObject.SetActive(arg);
    }
    public void CloseSceneDataLog()
    {
        ActiveSceneDataLog(_fieldDataLog, false);
    }
    public void ClearFile()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + fileName, false);

        using (writer)
        {
            writer.WriteLine("");
        }
        _textDataLog.SetText("");
        writer.Close(); ;
    }
    public void InputMenu(int value)
    {
        try
        {
            if (value == 0)
            {
                
            }
            else if (value == 1)
            {
                SaveDataAtFileButtonClick();
            }
            else if (value == 2)
            {
                OpenAndReadFileDataAsync();
            }
            else if (value == 3)
            {
                ClearFile();
            }
        }
        catch(Exception ex)
        {
            _textOut.SetText(ex.Message);
        }     
    }
}
