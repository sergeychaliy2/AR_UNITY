using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;

public class QRGenerator : MonoBehaviour
{
    [SerializeField] private RawImage _rawImageReciiver;
    [SerializeField] private TMP_InputField _textInputField;
    private Texture2D _storeEncodedTexture;
    // Start is called before the first frame update
    void Start()
    {
        _storeEncodedTexture = new Texture2D(256, 256); 
    }
    private Color32[] Encode(string textForEncoding,int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    /*  private void EncodeTextToQRCode()
      {
          string textWrite = string.IsNullOrEmpty(_textInputField.text) ? "You write" : _textInputField.text;
          Color32[] _convertPixelToTexture = Encode(textWrite, _storeEncodedTexture.width, _storeEncodedTexture.height);
          _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
          _storeEncodedTexture.Apply();
          _rawImageReciiver.texture = _storeEncodedTexture;
      }
    */
    private void EncodeTextToQRCode()
    {
        GameManager gameManager = new GameManager(1,2,3,4);
        string textInfo = gameManager.GetDeviceInfo();
       // string textWrite = string.IsNullOrEmpty(_textInputField.text) ? "You write" : _textInputField.text;
        Color32[] _convertPixelToTexture = Encode(textInfo, _storeEncodedTexture.width, _storeEncodedTexture.height);
        _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
        _storeEncodedTexture.Apply();
        _rawImageReciiver.texture = _storeEncodedTexture;
    }
    public void OnClicEncode()
    {
        EncodeTextToQRCode();
    }
}
public class GameManager
{
    int identificator,serie,manufacture, year;
    public GameManager(int identificators, int series, int manufactures, int years)
    {
        this.identificator = identificators;
        this.serie = series;
        this.manufacture = manufactures;
        this.years = years;
    }
    public int? identificators { get; set; }

    public int? series { get; set; }

    public int? manufactures { get; set; }

    public int? years { get; set; }
    public string GetDeviceInfo()
    {
        string infoText = ("|identificator:" + identificator + "\n" + "|serie:" + serie + "\n" +
        "|manufacture:" + manufactures + "\n" + "|year:" + years);
        return infoText;
    }
}
