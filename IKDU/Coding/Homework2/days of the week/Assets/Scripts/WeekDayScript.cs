using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekDayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        
        WeekDay(days);

        Debug.Log("_________________________________");

        IfWeek(days);

    }

    private void WeekDay(string[] dayOfWeek)
    {
        for(int i = 0; i < dayOfWeek.Length; i++)
        {
            switch (dayOfWeek[i])
            {
                case "Monday":
                    Debug.Log("Go to school");
                    break;

                case "Tuesday":
                    Debug.Log("HomeWork");
                    break;

                case "Wednesday":
                    Debug.Log("Fitness");
                    break;

                case "Thursday":
                    Debug.Log("Go out with Girlfriend");
                    break;

                case "Friday":
                    Debug.Log("Party with Friends");
                    break;

                case "Saturday":
                    Debug.Log("Netflix and chill");
                    break;

                case "Sunday":
                    Debug.Log("Prepare school for next day");
                    break;

                default:
                    Debug.Log("Can not find the day!");
                    break;
            }
        }
    }


    private void IfWeek(string[] dayOfWeek)
    {
        for (int i = 0; dayOfWeek.Length > i; i++)
        {
            if (dayOfWeek[i] == "Monday")
            {
                Debug.Log("Walk to school");
            }
            else if (dayOfWeek[i] == "Tuesday")
            {
                Debug.Log("Take Bus to school");
            }
            else if (dayOfWeek[i] == "Wednesday")
            {
                Debug.Log("Bike to school");
            }
            else if (dayOfWeek[i] == "Thursday")
            {
                Debug.Log("Stay home and make homework");
            }
            else if (dayOfWeek[i] == "Friday")
            {
                Debug.Log("Go out with family");
            }
            else if (dayOfWeek[i] == "Saturday")
            {
                Debug.Log("Can not find the day!");
            }
            else if (dayOfWeek[i] == "Sunday")
            {
                Debug.Log("Sunbath day");
            }            
            else
            {
                Debug.Log("Can not find the day!");
            }

        }
    }
}
