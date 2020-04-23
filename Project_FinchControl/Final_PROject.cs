using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;
using System.IO;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Robot Distance-Based Light and Temperature Recorder
    // Description: An application to move the Finch robot to a new location and record the light and temperature levels there.
    // Application Type: Console
    // Author: Adam Kabat
    // Dated Created: 4/14/2020
    // Last Modified: 4/23/2020
    //
    // **************************************************

    class Program
    {

        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }
        // Theme
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.White;
        }
        // Menu
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Run Data Collection");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        RunDataCollection(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void RunDataCollection(Finch finchRobot)
        {
            // Variable Collection
            DisplayScreenHeader("Enter Desired Variables");

            string UserInput;
            int Distance;
            int Speed;

            Console.WriteLine();
            Console.WriteLine("\tEnter the time in seconds you would like the Finch robot to travel.");
            UserInput = Console.ReadLine();
            int.TryParse(UserInput, out Distance);
            Console.WriteLine("\tEnter the speed at which you would like the Finch robot to travel. Max 255.");
            UserInput = Console.ReadLine();
            int.TryParse(UserInput, out Speed);
            Console.Clear();

            // Execution

            int time = Distance * 1000;
            finchRobot.setMotors(Speed, Speed);
            finchRobot.wait(time);
            finchRobot.setMotors(0, 0);

            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(2000);

            int LightSensorValue = 0;
            double TempSensorValue = 0;

            LightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
            TempSensorValue = (finchRobot.getTemperature());

            finchRobot.noteOn(50);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(5000);

            finchRobot.setMotors(0, 255);
            finchRobot.wait(1700);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(1000);
            finchRobot.setMotors(Speed, Speed);
            finchRobot.wait(time);
            finchRobot.setMotors(0, 0);

            Console.Clear();

            // Results

            DisplayScreenHeader("Data Recording Results");

            Console.WriteLine();
            Console.WriteLine($"\tLight level: {LightSensorValue}");
            Console.WriteLine($"\tCurrent temperature: {TempSensorValue.ToString("n2")}");

            DisplayContinuePrompt();
        }

        #region FINCH ROBOT MANAGEMENT

        // Disconnect the Finch
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main");
        }

        // Connect to the Finch
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome!");

            DisplayContinuePrompt();
        }

        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using my application!");

            DisplayContinuePrompt();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
