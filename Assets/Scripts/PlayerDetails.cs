using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public List<int> Controller;
    public List<Material> Materials;
    public List<Color> Colors;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
