using UnityEngine;
public class ExamplesInv : MonoBehaviour
{
    public InventorySimple MyInventory = new InventorySimple("UniqueNameInventory", 4);
    void Start()
    {
        //Добавляем предмет в инвентарь
        MyInventory.Slot[0].SetItem(new GunExample("Minigun",5f,20f));
        MyInventory.Slot[1].SetItem(new GunExample("MP5",2f,10f));

        //Посмотреть что в инвентаре
        for (int i = 0; i < MyInventory.Slot.Length; i++)
        {
            if (MyInventory.Slot[i].Empty==false)
            {
                Debug.Log(i+"="+MyInventory.Slot[i].GetItem<GunExample>().GunName);
            }
            else
            {
                Debug.Log(i+"=EMPTY");
            }
        }

        //Вывести дебаг инвентаря
        MyInventory.DebugInv();

        //Добавить в слот если он свободен SetItemIfEmpty
        bool SomeBool1;
        SomeBool1=MyInventory.Slot[0].SetItemIfEmpty(new GunExample("Shootgun",100f,2f));
        //False потому что слот [0] занят
        Debug.Log("SomeBool="+SomeBool1);

        bool SomeBool2;
        SomeBool2=MyInventory.Slot[3].SetItemIfEmpty(new GunExample("Shootgun",100f,2f));
        //True потому что слот [1] свободен
        Debug.Log("SomeBool2="+SomeBool2);
        
        //Название типа класса текущего обьекта в слоте
        Debug.Log("TypeClass="+MyInventory.Slot[3].TypeClass);

        //Вывести дебаг инвентаря
        MyInventory.DebugInv();
        
        //Очистить инвентарь (удаляет предмет из каждого слота)
        MyInventory.ClearInventory();
        Debug.Log("Инвентарь очищен");

        //Количество пустых слотов int
        Debug.Log("Количество пустых слотов="+MyInventory.CountEmpty());

        //Получить индексы пустых слотов в виде массива "int[]"
        int[] ExampleIntsArr = MyInventory.GetEmptySlotsIndexes();
        for (int i = 0; i < ExampleIntsArr.Length; i++)
        {
            Debug.Log("Пустой слот с индексом="+ExampleIntsArr[i]);
        }

    }
}

//[System.Serializable] Обязательно!!!
[System.Serializable]
public class GunExample
{
    public string GunName;
    public float Damage;
    public float ShootRate;

    public GunExample(string GunName, float Damage, float ShootRate)
    {
        this.GunName = GunName;
        this.Damage = Damage;
        this.ShootRate = ShootRate;
    }
}

