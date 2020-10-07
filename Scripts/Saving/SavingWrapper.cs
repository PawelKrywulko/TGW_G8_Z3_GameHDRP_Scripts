using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    private const string DefaultSaveFile = "save";

    //IEnumerator Start()
    //{
    //    //yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    //}

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }*/

    public void Load()
    {
        GetComponent<SavingSystem>().Load(DefaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(DefaultSaveFile);
    }
}
