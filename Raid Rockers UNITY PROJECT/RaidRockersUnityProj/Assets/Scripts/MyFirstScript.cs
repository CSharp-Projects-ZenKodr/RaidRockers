using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstScript : MonoBehaviour
{
    //Variables begin
    int houseTemperature = 70;

    float temperaturePercent;

    bool changeTemperature = true;

    string hi = "Hello World";
    //Variables end

    // Start is called before the first frame update
    void Start()
    {
        if (changeTemperature)
        {
            HeatUp();
            CoolDown(); 
        }

        //Getting Percent
        temperaturePercent = (float)houseTemperature / 100f;
        print("The house's temperature is " + temperaturePercent + " out of 1");

        //Testing bool
        print("Change Temperature variable is set to " + changeTemperature);
        changeTemperature = false;
        print("Change Temperature variable is set to " + changeTemperature);

        //String
        print(hi);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Heats up our house
    void HeatUp ()
    {
        houseTemperature += 2;
        print("Our house is now at " + houseTemperature);
    }

    // Cools down our house
    void CoolDown()
    {
        houseTemperature -= 2;
        print("Our house is now at " + houseTemperature);
    }
}