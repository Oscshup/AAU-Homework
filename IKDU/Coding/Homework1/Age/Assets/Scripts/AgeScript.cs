using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeScript : MonoBehaviour
{
    // Variables 
    public int age = 24;
    public int additionalAge = 4;

    // Start is called before the first frame update
    public void Start()
    {
        // Test increasing age by 1 year (default)
        IncreaseAge(age);

        // Test increasing age by 1 year (default)
        IncreaseAge(age, additionalAge);
    }

    // Method that increases age by a specified number of years (default is 1 year)
    public void IncreaseAge(int currentAge, int yearToAdd = 1)
    {

        currentAge += yearToAdd;
        logAge(currentAge, yearToAdd);

    }

    // Helper function to log the results
    public void logAge(int newAge, int addedYear)
    {
        if (addedYear == 1)
        {
            Debug.Log("Age increased by 1 year: " + newAge);
        }
        else
        {
            Debug.Log("Age increased by " + addedYear + " year: " + newAge);
        }
    }
}
