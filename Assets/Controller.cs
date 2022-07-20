using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_Dropdown typeW;
    [SerializeField] TMP_Dropdown type2;

    [SerializeField] GameObject[] listShape;

    [SerializeField] TMP_InputField weight;
    [SerializeField] TMP_InputField ET;


    [SerializeField] TextMeshProUGUI horsepower;
    [SerializeField] TextMeshProUGUI horsepowerResult;
    [SerializeField] TextMeshProUGUI horsepowerWatts;


    [SerializeField] GameObject listFieldParent;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private int amount = 0;
    private double result = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 0;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
        typeW.options.Clear();
        type2.options.Clear();
        List<string> items = new List<string>();
        List<string> itemETs = new List<string>();

        items.Add("Pound");
        items.Add("Ton");
        items.Add("Kilogram");

        itemETs.Add("Second");
        itemETs.Add("Minute");
        itemETs.Add("Hour");
        itemETs.Add("Day");

        foreach (var item in items)
        {
            typeW.options.Add(new TMP_Dropdown.OptionData() { text = item });
        } 
        
        foreach (var item2 in itemETs)
        {
            type2.options.Add(new TMP_Dropdown.OptionData() { text = item2 });
        }

        //typeW.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
        //typeET.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
        typeW.value = 0;
        //type2.value = 0;
        type = 0;
        //listShape[0].SetActive(true);
    }

    double[] poundConvert = { 1, 2204.62262, 2.20462262 };
    double[] secondConvert = {1, 60, 3600, 86400 };

    private void DropdownitemSelected()
    {
        //SwitchToVolume();
    }



    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    private bool CheckValidate()
    {
        if (weight.text == "" || ET.text == "")
            return false;

        //return text.All(char.IsDigit);
        return true;
    }


    public void Sum()
    {
        CalWithAdult();
        //listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        double we = double.Parse(weight.text);
        double et = double.Parse(ET.text);

        var weToKG = we * poundConvert[typeW.value];
        var ETToSec = et * secondConvert[type2.value];

        var pow = ETToSec / 5.825f;
        var result = weToKG / (pow * pow * pow);

        horsepower.text = "Horsepower = " + weToKG + " / (" + ETToSec + "/5.825)^3";
        horsepowerResult.text = result.ToString("0.0000");
        horsepowerWatts.text = (result * 745.697153779f).ToString("0.0000");
    }

    void SwitchToVolume()
    {
        for(int i = 0; i < listShape.Length; i++)
        {
            listShape[i].SetActive(i==type);
        }
    }

    double m2toft2 = 10.7639104;
    double m2toin2 = 1550.0031;


    double M2ToFt2(double m2)
    {
        return m2 * m2toft2;
    }
    
    double M2ToIn2(double m2)
    {
        return m2 * m2toin2;
    }

    public void Continue()
    {
        Clear();
    }

    public void Clear()
    {
        listFieldParent.SetActive(false);

        typeW.value = 0;
        //type2.value = 0;

        weight.text = "";
        ET.text = "";

        calButton.interactable = false;
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
