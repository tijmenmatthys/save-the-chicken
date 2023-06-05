using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;

    public void SetChickenName(string name)
    {
        _nameText.text = name;
    }
}
