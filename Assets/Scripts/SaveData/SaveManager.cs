using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    // セーブデータの保存先ディレクトリ
    const string SAVE_DIRECTORY = "save";
    // セーブファイルの名前
    const string SAVE_FILE_NAME = "save";
    // セーブファイルの拡張子
    const string SAVE_FILE_TAIL = ".json";
    // セーブデータの一覧
    public static Dictionary<int, GlobalData> saveDatas
      = new Dictionary<int, GlobalData>();

    // クラス起動時にSaveファイルを読み取っておく
    private void Awake()
    {
        // プロジェクトディレクトリを取得    
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory();
#else
      string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        // セーブデータの保存先ディレクトリを取得
        path += ("/" + SAVE_DIRECTORY + "/");
        createDirectory(Path.GetDirectoryName(path));
        string[] names = Directory.GetFiles(path, SAVE_FILE_NAME + "*" + SAVE_FILE_TAIL);
        foreach (string name in names)
        {
            try
            {
                FileInfo info = new FileInfo(name);
                StreamReader reader = new StreamReader(info.OpenRead(), Encoding.GetEncoding("UTF-8"));
                string json = reader.ReadToEnd();
                reader.Close();
                GlobalData sd = JsonUtility.FromJson<GlobalData>(json);
                saveDatas.Add(sd.fileNo, sd);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
            }
        }
    }

    public void save(int index, GlobalData sd)
    {
        sd.fileNo = index;
        sd.updateTime = DateTime.Now.ToString();
        sd.name = "セーブデータ" + index.ToString();
        string json = JsonUtility.ToJson(sd);
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory();
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
        path += ("/" + SAVE_DIRECTORY + "/" + SAVE_FILE_NAME + index.ToString() + SAVE_FILE_TAIL);
        createDirectory(Path.GetDirectoryName(path));
        StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8"));
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();

        UpdateDictionary(sd);
    }
    private GlobalData setInitValue(GlobalData sd)
    {
        // セーブデータを使用しない場合の初期値
        sd._Deck = new List<DeckInfo>();
        sd.updateTime = DateTime.Now.ToString();
        sd.name = "";
        return sd;
    }

    public GlobalData load(int index)
    {
        GlobalData sd = new GlobalData();
        if (saveDatas.ContainsKey(index))
        {
            sd = saveDatas[index];
        }
        else
        {
            sd = setInitValue(new GlobalData());
        }
        return sd;
    }

    private  void UpdateDictionary(GlobalData sd)
    {
        if (saveDatas.ContainsKey(sd.fileNo))
        {
            saveDatas[sd.fileNo] = sd;
        }
    }

    public void createDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

