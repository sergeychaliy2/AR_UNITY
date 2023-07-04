using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

public class SaveDataAtFile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textOut;
    private static string fileName = "/note.txt";
    private string realPath = Application.persistentDataPath + fileName;
    // Start is called before the first frame update
    // Update is called once per frame
    public void SaveDataAtFileButtonClick()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + fileName);

        using (writer)
        {
            writer.WriteLine(_textOut.ToString());
        }
        writer.Close();
    }
    public void OpenAndReadFileDataAsync()
    {
        StreamReader streamReader = new StreamReader(Application.persistentDataPath + fileName);
        using (streamReader)
        {
            string texts = streamReader.ReadLine();
            _textOut.SetText(texts.ToString());
        }
        Application.OpenURL(streamReader.ToString());
        streamReader.Close();
        Application.OpenURL(realPath);
    }
}
