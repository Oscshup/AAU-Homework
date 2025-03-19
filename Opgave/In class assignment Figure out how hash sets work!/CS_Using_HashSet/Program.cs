using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CS_Using_HashSet
{


    class Program
    {
        static void Main(string[] args)
        {
            //section 2
            // the HashSet eliminate the duplicat
            /*Console.WriteLine("Using HashSet");
            //1. Defining String Array (Note that the string "mahesh" is repeated) 
            string[] names = new string[] {
                "mahesh",
                "vikram",
                "mahesh",
                "mayur",
                "suprotim",
                "saket",
                "manish"
            };
            //2. Length of Array and Printing array
            Console.WriteLine("Length of Array " + names.Length);
            Console.WriteLine();
            Console.WriteLine("The Data in Array");
            foreach (var n in names)
            {
                Console.WriteLine(n);
            }

            Console.WriteLine();
            //3. Defining HashSet by passing an Array of string to it
            HashSet<string> hSet = new HashSet<string>(names);
            //4. Count of Elements in HashSet
            Console.WriteLine("Count of Data in HashSet " + hSet.Count);
            Console.WriteLine();
            //5. Printing Data in HashSet, this will eliminate duplication of "mahesh" 
            Console.WriteLine("Data in HashSet");
            foreach (var n in hSet)
            {
                Console.WriteLine(n);
            }
            Console.ReadLine();*/



            //Section 3
            // HashSet eliminate duplicats
            /*string[] names1 = new string[]
           {
                "mahesh","sabnis","manish","sharma","saket","karnik"
           };

            string[] names2 = new string[]
            {
                "suprotim","agarwal","vikram","pendse","mahesh","mitkari"
            };

            //2.

            HashSet<string> hSetN1 = new HashSet<string>(names1);
            Console.WriteLine("Data in First HashSet");
            foreach (var n in hSetN1)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine("_______________________________________________________________");
            HashSet<string> hSetN2 = new HashSet<string>(names2);
            Console.WriteLine("Data in Second HashSet");
            foreach (var n in hSetN2)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine("________________________________________________________________");
            //3.
            Console.WriteLine("Data After Union");
            hSetN1.UnionWith(hSetN2);
            foreach (var n in hSetN1)
            {
                Console.WriteLine(n);
            }


            // section 4
            // it remove mahesh   
            Console.WriteLine();
            Console.WriteLine("_________________________________");
            Console.WriteLine("Data in HashSet before using Except With");
            Console.WriteLine("_________________________________");
            //storing data of hSetN3 in temporary HashSet
            HashSet<string> hSetN3 = new HashSet<string>(names1);
            foreach (var n in hSetN3)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine();
            Console.WriteLine("_________________________________");
            Console.WriteLine("Using Except With");
            Console.WriteLine("_________________________________");
            hSetN3.ExceptWith(hSetN2);
            foreach (var n in hSetN3)
            {
                Console.WriteLine(n);
            }

            //Section 5
            // mahesh is remove in both HashSet
            HashSet<string> hSetN4 = new HashSet<string>(names1);
            Console.WriteLine("_________________________________");
            Console.WriteLine("Elements in HashSet before using SymmetricExceptWith");
            Console.WriteLine("_________________________________");
            Console.WriteLine("HashSet 1");
            foreach (var n in hSetN4)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine("HashSet 2");
            foreach (var n in hSetN2)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine("_________________________________");
            Console.WriteLine("Using SymmetricExceptWith");
            Console.WriteLine("_________________________________");
            hSetN4.SymmetricExceptWith(hSetN2);
            foreach (var n in hSetN4)
            {
                Console.WriteLine(n);
            }*/


            //
            Get_Add_Performance_HashSet_vs_List();

            //
            Get_Contains_Performance_HashSet_vs_List();

            //
            Get_Remove_Performance_HashSet_vs_List();
        }


        //I had chatgpt give me a list of 500 names, and I used that list to test the performance of HashSet vs List.
        static string[] names = new string[] {
    "Ram", "Tejas", "Sita", "Neema", "Neema", "GundaRam", "Ram", "Agarwal", "Ramesh", "Pendse",
    "Saket", "Mahesh", "Tejas", "Ramesh", "Leena", "Neema", "Karnik", "Mitkari", "Tejas", "Agarwal",
    "Leena", "Agarwal", "Saket", "Neema", "Manish", "Pendse", "Sita", "Tejas", "Sabnis", "Saket",
    "Rajesh", "Sita", "GundaRam", "Leena", "Rajesh", "Ram", "Ramesh", "Suprotim", "Ram", "Mayur",
    "Mayur", "Mitkari", "Sita", "Mahesh", "Manish", "Agarwal", "Ram", "Suprotim", "Ramesh", "Agarwal",
    "Vikram", "Mitkari", "Mayur", "Pendse", "Leena", "Ramesh", "Mahesh", "Neema", "Vikram", "Ramesh",
    "Neema", "Ram", "Suprotim", "Sita", "Manish", "Mayur", "Sabnis", "Mayur", "Mayur", "Leena",
    "Sita", "Ramesh", "Mitkari", "Sabnis", "Agarwal", "Neema", "Sabnis", "Manish", "Suprotim", "Sita",
    "Agarwal", "Neema", "Rajesh", "Mahesh", "Neema", "Mahesh", "Rajesh", "Suprotim", "Sita", "Ramesh",
    "Leena", "Pendse", "Rajesh", "Leena", "Sharma", "Suprotim", "Manish", "GundaRam", "Sita", "GundaRam",
    "Neema", "Agarwal", "Agarwal", "Sita", "Pendse", "Saket", "Pendse", "Suprotim", "Mayur", "Neema",
    "GundaRam", "Karnik", "Sharma", "Ramesh", "Mahesh", "Ram", "GundaRam", "Sabnis", "Saket", "Mitkari",
    "Ramesh", "Suprotim", "Suprotim", "Mitkari", "Manish", "Karnik", "Sita", "Agarwal", "Tejas", "Ram",
    "Agarwal", "Sita", "Rajesh", "Ram", "Vikram", "Saket", "Sabnis", "Manish", "Tejas", "Sita",
    "Karnik", "Sabnis", "Karnik", "Ram", "Vikram", "Karnik", "Mitkari", "Leena", "GundaRam", "Mayur",
    "Sabnis", "Agarwal", "Karnik", "Tejas", "Mitkari", "Rajesh", "Sharma", "Tejas", "Ram", "Mayur",
    "Vikram", "Neema", "Mahesh", "Neema", "Pendse", "Ramesh", "Ramesh", "Sharma", "Ramesh", "Agarwal",
    "GundaRam", "GundaRam", "Sharma", "Agarwal", "Sabnis", "Sita", "Karnik", "Mitkari", "Saket", "Leena",
    "Agarwal", "Leena", "Vikram", "Suprotim", "Mayur", "Manish", "Karnik", "Manish", "Ram", "Neema",
    "Neema", "Ramesh", "Rajesh", "Tejas", "Pendse", "Agarwal", "Neema", "Pendse", "Neema", "Tejas",
    "Ramesh", "Mahesh", "Neema", "Ramesh", "Mahesh", "Rajesh", "Ramesh", "Karnik", "Neema", "Sita",
    "Sharma", "Leena", "Agarwal", "GundaRam", "Pendse", "Pendse", "Sharma", "Neema", "Sharma", "Saket",
    "Leena", "Ram", "Ram", "Saket", "Mayur", "Saket", "Saket", "Manish", "Mahesh", "Ram", "Mahesh",
    "Suprotim", "Rajesh", "Ram", "Neema", "Leena", "Leena", "Agarwal", "Manish", "GundaRam", "Saket",
    "Sabnis", "Sita", "Manish", "Neema", "Ramesh", "Manish", "Agarwal", "Ram", "Mahesh", "Agarwal",
    "Tejas", "Ramesh", "Neema", "Sabnis", "Saket", "Sharma", "Sharma", "Leena", "Suprotim", "Mahesh",
    "Sabnis", "Suprotim", "Tejas", "Suprotim", "Sita", "Manish", "Vikram", "Saket", "Agarwal", "Sharma",
    "GundaRam", "Leena", "Vikram", "Leena", "Mahesh", "Pendse", "Agarwal", "Mahesh", "Rajesh", "Mahesh",
    "Mahesh", "Pendse", "Sharma", "Karnik", "Karnik", "Sabnis", "Mahesh", "Karnik", "Ramesh", "Sabnis",
    "Ramesh", "Mitkari", "Ramesh", "Neema", "Suprotim", "Ram", "Pendse", "Neema", "Pendse", "Mitkari",
    "Mahesh", "Mitkari", "Ramesh", "Saket", "Pendse", "Pendse", "Karnik", "Rajesh", "Sita", "Leena",
    "Rajesh", "Neema", "Sita", "Suprotim", "GundaRam", "Vikram", "Manish", "Rajesh", "Ramesh", "Tejas",
    "Manish", "Mitkari", "Pendse", "Ram", "Ramesh", "Agarwal", "Leena", "Karnik", "Sita", "GundaRam",
    "Mayur", "Ramesh", "Neema", "Mayur", "Vikram", "Sabnis", "Manish", "Agarwal", "Vikram", "Mitkari",
    "Karnik", "Tejas", "Agarwal", "Vikram", "Ram", "GundaRam", "Sita", "Ram", "Ram", "Agarwal",
    "GundaRam", "Sita", "Vikram", "Mitkari", "Leena", "Rajesh", "Leena", "Sita", "Karnik", "Sharma",
    "Sita", "Mahesh", "Ramesh", "Saket", "Sita", "Mahesh", "Tejas", "Rajesh", "GundaRam", "Sita",
    "Sabnis", "Manish", "Agarwal", "Saket", "Agarwal", "Tejas", "Ram", "Ramesh", "GundaRam", "Agarwal",
    "Mahesh", "Mayur", "Pendse", "Agarwal", "GundaRam", "Saket", "GundaRam", "Mahesh", "Vikram", "Mayur",
    "Mahesh", "Mayur", "Leena", "Neema", "Ram", "Mayur", "Agarwal", "Saket", "Mitkari", "GundaRam",
    "Neema", "Sabnis", "Sabnis", "Saket", "Tejas", "Sabnis", "Rajesh", "Saket", "Neema", "Sita",
    "Sabnis", "Ram", "Suprotim", "Mahesh", "Sharma", "Neema", "Leena", "Manish", "Mayur", "Vikram",
    "Neema", "Neema", "Tejas", "Leena", "Suprotim", "Rajesh", "Sita", "Ramesh", "Sita", "Mayur",
    "Karnik", "Suprotim", "Agarwal", "Rajesh", "Tejas", "Ram", "Sita", "Sabnis", "Pendse", "Sita"
        };


        //section 6
        //
        static void Get_Add_Performance_HashSet_vs_List()
        {

            Console.WriteLine("____________________________________");
            Console.WriteLine("List Performance while Adding Item");
            Console.WriteLine();
            List<string> lstNames = new List<string>();
            var s2 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                lstNames.Add(s);
            }
            s2.Stop();

            Console.WriteLine(s2.Elapsed.TotalMilliseconds.ToString("0.000 ms")); Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine();
            Console.WriteLine("____________________________________");
            Console.WriteLine("HashSet Performance while Adding Item");
            Console.WriteLine();

            HashSet<string> hStringNames = new HashSet<string>(StringComparer.Ordinal);
            var s1 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                hStringNames.Add(s);
            }
            s1.Stop();

            Console.WriteLine(s1.Elapsed.TotalMilliseconds.ToString("0.000 ms")); Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine("____________________________________");
            Console.WriteLine();
        }

        static void Get_Contains_Performance_HashSet_vs_List()
        {

            Console.WriteLine("____________________________________");
            Console.WriteLine("List Performance while checking Contains operation");
            Console.WriteLine();
            List<string> lstNames = new List<string>();
            var s2 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                lstNames.Contains(s);
            }
            s2.Stop();

            Console.WriteLine(s2.Elapsed.TotalMilliseconds.ToString("0.000 ms")); Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine();
            Console.WriteLine("____________________________________");
            Console.WriteLine("HashSet Performance while checking Contains operation");
            Console.WriteLine();

            HashSet<string> hStringNames = new HashSet<string>(StringComparer.Ordinal);
            var s1 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                hStringNames.Contains(s);
            }
            s1.Stop();

            Console.WriteLine(s1.Elapsed.TotalMilliseconds.ToString("0.000 ms"));
            Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine("____________________________________");
            Console.WriteLine();

        }

        static void Get_Remove_Performance_HashSet_vs_List()
        {

            Console.WriteLine("____________________________________");
            Console.WriteLine("List Performance while performing Remove item operation");
            Console.WriteLine();
            List<string> lstNames = new List<string>();
            var s2 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                lstNames.Remove(s);
            }
            s2.Stop();

            Console.WriteLine(s2.Elapsed.TotalMilliseconds.ToString("0.000 ms")); Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine();
            Console.WriteLine("____________________________________");
            Console.WriteLine("HashSet Performance while performing Remove item operation");
            Console.WriteLine();

            HashSet<string> hStringNames = new HashSet<string>(StringComparer.Ordinal);
            var s1 = Stopwatch.StartNew();
            foreach (string s in names)
            {
                hStringNames.Remove(s);
            }
            s1.Stop();

            Console.WriteLine(s1.Elapsed.TotalMilliseconds.ToString("0.000 ms")); Console.WriteLine();
            Console.WriteLine("Ends Here");
            Console.WriteLine("____________________________________");
            Console.WriteLine();

        }
    }
}

