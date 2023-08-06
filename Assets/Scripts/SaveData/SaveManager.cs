using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    // �Z�[�u�f�[�^�̕ۑ���f�B���N�g��
    const string SAVE_DIRECTORY = "save";
    // �Z�[�u�t�@�C���̖��O
    const string SAVE_FILE_NAME = "save";
    // �Z�[�u�t�@�C���̊g���q
    const string SAVE_FILE_TAIL = ".json";
    // �Z�[�u�f�[�^�̈ꗗ
    public static Dictionary<int, GlobalData> saveDatas
      = new Dictionary<int, GlobalData>();

    // �N���X�N������Save�t�@�C����ǂݎ���Ă���
    private void Awake()
    {
        // �v���W�F�N�g�f�B���N�g�����擾    
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory();
#else
      string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif

        // �Z�[�u�f�[�^�̕ۑ���f�B���N�g�����擾
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
        sd.name = "�Z�[�u�f�[�^" + index.ToString();
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
        // �Z�[�u�f�[�^���g�p���Ȃ��ꍇ�̏����l
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

