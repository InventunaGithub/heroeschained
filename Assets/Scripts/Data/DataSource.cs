using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSource : MonoBehaviour
{
    public enum DataSourceType { Firebase, Amazon }
    public DataSourceType Source = DataSourceType.Firebase;
    public string GameId = "Heroes Chained";

    DataProvider dataProvider = null;
    private void Start()
    {
        dataProvider = null;

        switch(Source)
        {
            case DataSourceType.Firebase:
                dataProvider = GetComponent<DataProviderFirebase>();
                break;
            case DataSourceType.Amazon:
                dataProvider = GetComponent<DataProviderAmazon>();
                break;
        }

        if (dataProvider == null)
        {
            Debug.LogError("DataProvider " + Source + " could not be initialized");
        }
        else
        {
            if (!dataProvider.Connect(GameId))
            {
                Debug.LogError("DataProvider " + dataProvider.Vendor() + " could not be connected");
            }

            if (dataProvider != null)
            {
                Debug.Log("Data source provider: " + dataProvider.Vendor());
            }
        }
    }

    public DataProvider GetProvider()
    {
        return dataProvider;
    }
}
