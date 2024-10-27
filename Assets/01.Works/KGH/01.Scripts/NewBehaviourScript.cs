using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] StatIcon _statIcon;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(File.Exists(Path.Combine(Application.dataPath,$"00.ForEveryone/02.SOs/03.ItemSOs/{_statIcon.name}.asset")));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
