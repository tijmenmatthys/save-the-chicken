using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChickenNames
{
    private static string[] _chickenNames =
    {
        "Stijn",
        "Dave",
        "Alexander",
        "Luna",
        "Luc",
        "Jef",
        "Steven",

        "Joran",
        "Jason",
        "Giorgio",
        "Pablo",
        "Ben",
        "Ruben",
        "Lander",
        "Yvette",
        "Amelie",
        "Vital",
        "Thomas",
        "Laurens",
        "Eefje",
        "Oscar",
        "Emma",
        "Gunnar",
        "Noah",
        "Michiel",
        "Jasper"
    };

    public static Dictionary<Vector3, string> DiedChicken { get; private set; }
        = new Dictionary<Vector3, string>();

    private static string _lastDiedChicken = "";

    public static void ResetDiedChicken()
    {
        DiedChicken.Clear();
    }

    public static void AddDiedChicken(Vector3 position, string chickenName)
    {
        DiedChicken[position] = chickenName;
        _lastDiedChicken = chickenName;
    }

    public static string GetRandomChickenName()
    {
        string name;
        do
            name = _chickenNames.Shuffle().First();
        while (name == _lastDiedChicken);
        return name;
    }
}
