using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Простой инвентарь
public class InventorySimple
{
    public string NameInventory;//Имя инвентаря(должно быть уникальное)
    public SlotInventorySimple[] Slot;//Массив слотов инвентаря
    
    //Количество пустых слотов
    public int CountEmpty()
    {
        int CountEmpty = 0;
        for (int i = 0; i < Slot.Length; i++)
        {
            if (Slot[i].Empty==true)
            {
                CountEmpty++;
            }
        }
        return CountEmpty;
    }
    
    //Получить индексы пустых слотов (experimental)
    public int[] GetEmptySlotsIndexes()
    {
        List<int> IntsReturn=new List<int>(0);
        for (int i = 0; i < Slot.Length; i++)
        {
            if (Slot[i].Empty==true)
            {
                IntsReturn.Add(i);
            }
        }
        return IntsReturn.ToArray();
    }
    
    //Очистить весь инвентарь(удалить предмет из каждого слота)
    public void ClearInventory()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].DeleteItem(); 
        }
    }

    //Инициализировать весь инвентарь
    public void InstallInventory()
    {

        for (int i = 0; i < Slot.Length; i++)
        {
            //Загрузка каждого слота
            Slot[i] = new SlotInventorySimple("" + NameInventory + "_Slot" + i);
        }
    }


    //Посмотреть содержимое инвентаря (Debug)
    public void DebugInv(bool ShowAdressSlot=false,string SomeTextFirst="")
    {    
        
        Debug.Log(SomeTextFirst+" \n DebugInv ["+NameInventory+"].Slot.Length=" + Slot.Length + " ########################################################");
        int Empty = 0;
        int Full = 0;
        for (int i = 0; i < Slot.Length; i++)
        {
            string AfterText="";
            if (ShowAdressSlot==true)
            {
                AfterText += " [ADRESS]="+Slot[i].AddressNameInventory;
            }
            if (Slot[i].Empty==true)
            {
                Debug.Log("#Slot ["+i+"]=EMPTY"+AfterText);//Пустой слот
                Empty++;
            }
            else
            {
                /*
                Type TypeSimpleSlot = Type.GetType(Slot[i].TypeClass, true, true);//Получаем тип
                object downcastObj = Slot[i].GetItem<object>();
                Debug.Log(downcastObj);
                var upcastObject = System.Convert.ChangeType (downcastObj, TypeSimpleSlot);
                */
                Debug.Log(  "#Slot[" + i + "]=" + Slot[i].DataJson + AfterText);//Заполненый слот
                Full++;
            }
        }
        Debug.Log("DebugInv ["+NameInventory+"] Empty= " + Empty + " Full=" + Full + " #############END#############");

    }
    
    //Конструктор
    public InventorySimple(string NameInventory, int SizeInventory)
    {

        this.NameInventory = NameInventory;
        this.Slot = new SlotInventorySimple[SizeInventory];
        
        InstallInventory();
    }
}

//Слот простого инвентаря
[System.Serializable]
public class SlotInventorySimple
{
    public string AddressNameInventory;//Адрес в плеер префс в формате Inv + InvName + SlotId , пример "Inv_Towers_17"

    private bool Loaded_TypeClass = false;//Загружен ли слот
    private string TypeClass_Cache;//Кеш

    //Название типа класса текущего обьекта в слоте
    public string TypeClass
    {
        get
        {
            if (Loaded_TypeClass == false)
            {
                Loaded_TypeClass = true;
                TypeClass_Cache = GetSlotTypeClass();//Если не загружено с диска
                return TypeClass_Cache;
            }
            return TypeClass_Cache;//Если загружено с диска
        }
        set
        {
            SetSlotTypeClass(value);
            TypeClass_Cache = value;//Если не загружено с диска
            Loaded_TypeClass = true;
        }
    }

    private bool Loaded_DataJson = false;//Загружен ли слот
    private string DataJson_Cache;//Кеш

    //Данные JSON
    public string DataJson
    {
        get
        {
            if (Loaded_DataJson == false)
            {
                Loaded_DataJson = true;
                DataJson_Cache = GetSlotDataJson();//Если не загружено с диска
                return DataJson_Cache;
            }
            return DataJson_Cache;//Если загружено с диска
        }
        set
        {
            SetSlotDataJson(value);
            DataJson_Cache = value;
            Loaded_DataJson = true;
            Empty = false;//Слот не пустой

        }
    }

    private bool Loaded_Empty = false;//Загружен или нет
    private bool Empty_Cache;//Кеш

    //Пустой слот или нет true пустой,false заполеный
    public bool Empty
    {
        get
        {
            if (Loaded_Empty == false)
            {
                Loaded_Empty = true;
                Empty_Cache = GetSlotEmpty();//Если не загружено с диска
                return Empty_Cache;
            }
            return Empty_Cache;//Если загружено с диска
        }
        set
        {
            if (value == true)//Если делаем пустым слот
            {
                SetSlotDataJson("");
                SetSlotTypeClass("");
            }

            SetSlotEmpty(value);
            Empty_Cache = value;
            Loaded_Empty = true;
        }
    }

    //Поместить предмет
    public void SetItem(object InputItem)
    {
        DataJson = JsonUtility.ToJson(InputItem);
        TypeClass = "" + InputItem.GetType();
    }

    //Поместить предмет если слот пустой (возвращает true если успешно помещен)
    public bool SetItemIfEmpty(object InputItem)
    {
        if (Empty==true)
        {
            SetItem(InputItem);
            return true;
        }
        else
        {
            return false;
        }

    }

    //Удалить предмет 
    public void DeleteItem()
    {
        Empty = true;
    }

    //Взять предмет и преобразовать в нужный класс
    public T GetItem<T>()
    {
        return JsonUtility.FromJson<T>(DataJson); //Десериализация и возвращение обьекта
    }
    
    //true пустой,false заполеный
    private void SetSlotEmpty(bool InputEmpty)
    {
        if (InputEmpty == true)
        {
            PlayerPrefs.SetString(AddressNameInventory + "_Empty", "");
        }
        else
        {
            PlayerPrefs.SetString(AddressNameInventory + "_Empty", "false");
        }
    }

    //true пустой,false заполеный
    private bool GetSlotEmpty()
    {
        if (PlayerPrefs.GetString(AddressNameInventory + "_Empty") == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Загрузить данные json
    private string GetSlotDataJson()
    {
        return PlayerPrefs.GetString(AddressNameInventory + "_DataJson");
    }

    //Загрузить название класса(тип)
    private string GetSlotTypeClass()
    {
        return PlayerPrefs.GetString(AddressNameInventory + "_TypeClass");
    }

    //Установить данные json
    private void SetSlotDataJson(string InputDataJson)
    {
        PlayerPrefs.SetString(AddressNameInventory + "_DataJson", InputDataJson);
    }

    //Установить название класса(тип)
    private void SetSlotTypeClass(string InputTypeClass)
    {
        PlayerPrefs.SetString(AddressNameInventory + "_TypeClass", InputTypeClass);
    }

    public SlotInventorySimple(string AddressNameInventory)
    {
        this.AddressNameInventory = "Inv_" + AddressNameInventory;
    }
}
