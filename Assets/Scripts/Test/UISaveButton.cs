using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISaveButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _text;

    GlobalData _save;

    DeckInfo _currentData;
    int _currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        _currentIndex = 0;
        _save = SaveManager.Instance.load(1);
        LoadCard();
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
    }

    public void SaveButton()
    {
        SaveManager.Instance.save(1, _save);
    }

    public void LoadButton()
    {
        _save = SaveManager.Instance.load(1);
        LoadCard();
    }

    private void SetText()
    {
        _text.text = _currentData.data.name + ":" + _currentData.count.ToString();
    }

    public void AddMoney()
    {
        _currentData.count++;
    }

    public void DecreaseMoney()
    {
        _currentData.count--;
    }

    public void NextData()
    {
        _currentIndex++;
        int max = Manager.DeckManager.Instance.GetMaxCardData();
        if(max <= _currentIndex)
        {
            _currentIndex = 0;
        }
        LoadCard();
    }

    public void BackData()
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            int max = Manager.DeckManager.Instance.GetMaxCardData();
            _currentIndex = max - 1;
        }
        LoadCard();
    }

    private void LoadCard()
    {
        _currentData.data = Manager.DeckManager.Instance.GetCardData(_currentIndex);
        _currentData.count = 0;

        bool isExist = false;
        int index = 0;
        foreach (DeckInfo deckData in _save._Deck)
        {
            if (deckData.data == _currentData.data)
            {
                isExist = true;
                break;
            }
            index++;
        }

        if (isExist)
        {
            _currentData = _save._Deck[index];
        }
    }

    public void AddCard()
    {
        bool isExist = false;
        int index = 0;
        foreach (DeckInfo deckData in _save._Deck)
        {
            if (deckData.data == _currentData.data)
            {
                isExist = true;
                break;
            }
            index++;
        }

        if (isExist)
        {
            _save._Deck[index] = _currentData;
        }
        else
        {
            _save._Deck.Add(_currentData);
        }
    }
}
