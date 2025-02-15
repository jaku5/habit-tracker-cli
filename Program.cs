﻿List<string> habitsToTrack = new List<string>(); //= ["Meditate", "Read", "Walk", "Code"];
DayOfWeek[] daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
List<string> habitsCompleted = new List<string>();
var currentDate = DateTime.Now;
var selectedDate = currentDate;
DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

// User input helper vriables
string? userInput;
string menuSelection = "";
bool exit = false;

do
{
    LoadUserData();
    SetFirstDayOfWeek(firstDayOfWeek);
    ShowWeekGrid();

    userInput = Console.ReadLine();

    if (userInput != null && userInput.ToLower().Contains("exit"))
    {
        exit = true;
    }

    else if (userInput != null && (userInput.ToLower().Contains("menu") || userInput.ToLower().Contains('m')))
    {
        ShowMenu();
    }
}
while (exit == false);

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine($"Select option (selected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}):\n");
    Console.WriteLine("1. Show week grid for selected date.");
    Console.WriteLine("2. Select date.");
    Console.WriteLine("3. Mark habit as done for selected date.");
    Console.WriteLine("4. Add or remove habit.");
    Console.WriteLine("5. Set first day of the week");

    // @TODO Handle invalid input
    userInput = Console.ReadLine();

    if (userInput != null)
    {
        menuSelection = userInput.ToLower();
    }

    switch (menuSelection)
    {
        case "1":

            ShowWeekGrid();

            break;

        case "2":

            bool validInput = false;
            int year = currentDate.Year;
            int month = currentDate.Month;
            int day = currentDate.Day;

            // @TODO Handle invalid input
            Console.WriteLine($"Type year and press enter(default is {year}):");
            userInput = Console.ReadLine();

            if (userInput != null)
            {
                validInput = int.TryParse(userInput, out year);
            }

            // @TODO Handle invalid input
            Console.WriteLine($"Type month and press enter(default is {month}):");
            userInput = Console.ReadLine();

            if (userInput != null)
            {
                validInput = int.TryParse(userInput, out month);
            }

            // @TODO Handle invalid input
            Console.WriteLine($"Type day and press enter(default is {day}):");
            userInput = Console.ReadLine();

            if (userInput != null)
            {
                validInput = int.TryParse(userInput, out day);
            }

            SetCustomDate(year, month, day);

            break;

        case "3":

            validInput = false;

            // @TODO Handle invalid input
            // @TODO Add suport for unmark option
            Console.WriteLine($"Type habit name youand press enter. Use \"false\" options to unmark habit done status. Selected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}):");
            userInput = Console.ReadLine();

            do
            {
                if (userInput != null && habitsToTrack.Contains(userInput))
                {
                    string habitsCompletedID = userInput + DateOnly.FromDateTime(selectedDate);
                    MarkHabitDone(habitsCompletedID);
                    validInput = true;
                }

                else
                {
                    Console.WriteLine("Habit not on the list. Please enter a valid habit name or add a new habit to the list.");
                    userInput = Console.ReadLine();
                }
            } while (validInput == false);

            break;

        case "4":

            ShowWeekGrid();

            break;

        case "5":

            ShowWeekGrid();

            break;

        case "6":

            ShowWeekGrid();

            break;

        default:

            ShowWeekGrid();

            break;
    }
}

void LoadUserData()
{
    if (File.Exists("./habit_data.txt"))
    {
        //@ TODO Handle empty data file
        string habits = "";
        string habitsCompletedID = "";
        StreamReader sr = new StreamReader("./habit_data.txt");

        habits = sr.ReadLine();
        habitsCompletedID = sr.ReadLine();
        habitsToTrack = habits.Replace(" ", "").Split(',').ToList();

        if (habitsCompletedID != "" && habitsCompletedID != null)
        {
            habitsCompleted = habitsCompletedID.Replace(" ", "").Split(',').ToList();
        }

        sr.Close();
    }

    else
    {
        do
        {
            //@TODO Handle invalid input
            Console.WriteLine("Welcome to Habit Tracker. To start tracking, add your first habit. Type the name of the habit you want to track and press enter");

            userInput = Console.ReadLine();

            if (userInput != null && userInput != "")
            {
                habitsToTrack.Add(userInput);
                SaveUserData();
            }
        } while (userInput == null || userInput == "");
    }

}

void SaveUserData()
{
    string userData = "";
    string habitList = "";
    string habitCompletedList = "";
    StreamWriter sw = new StreamWriter("./habit_data.txt");

    // @TODO Handle empty habit array
    if (habitsToTrack.Count > 0)
    {
        foreach (string habit in habitsToTrack)
        {
            habitList += $"{habit}, ";
        }

        if (habitsCompleted.Count > 0)
        {
            foreach (string habitCompletedID in habitsCompleted)
            {
                habitCompletedList += $"{habitCompletedID}, ";
            }

            userData = $"{habitList.Remove(habitList.Length - 2)}\n{habitCompletedList.Remove(habitCompletedList.Length - 2)}";
        }

        else
        {
            userData = $"{habitList.Remove(habitList.Length - 2)}";
        }

        sw.WriteLine(userData);
        sw.Close();
    }
}

void ModifyHabitList(string habit)
{
    if (habitsToTrack.Contains(habit))
    {
        for (int i = 0; i < habitsCompleted.Count; i++)
        {
            while (habitsCompleted[i].ToString().Contains(habit))
            {
                MarkHabitDone(habitsCompleted[i], false);
            }
        }

        habitsToTrack.Remove(habit);
    }

    else
    {
        habitsToTrack.Add(habit);
    }

    SaveUserData();
    ShowWeekGrid();
}

void ShowWeekGrid()
{
    Console.Clear();
    Console.WriteLine($"\n\t\tWelcome to Habit Tracker. Today is {currentDate.DayOfWeek} {DateOnly.FromDateTime(currentDate)}.\n\t\tSelected date is {selectedDate.DayOfWeek} {DateOnly.FromDateTime(selectedDate)}. Type \"menu\" or \"m\" to display options menu.\n");

    ShowGridHeader();
    ShowGridBody();
}

void ShowGridHeader()
{
    string weekGridHeader = "";
    string weekGridHeaderDates = "";

    for (int i = 0; i < daysOfWeek.Length; i++)
    {
        string currentWeekDay = $"{daysOfWeek[i].ToString()} ";
        string currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM")} ";

        if (i != 0)
            currentWeekDayDate = $"{CalculateGridDates(habitsToTrack[0], i).ToString("dd MMM").PadLeft(daysOfWeek[i - 1].ToString().Length)} ";

        weekGridHeader += currentWeekDay;
        weekGridHeaderDates += currentWeekDayDate;
    }
    Console.WriteLine($"\t\t{weekGridHeader} Current Record");
    Console.WriteLine($"\t\t{weekGridHeaderDates}\n");
}

void ShowGridBody()
{
    foreach (string habit in habitsToTrack)
    {
        int currentStreak = CalculateCurrentStreak(habit);
        int recordStreak = CalculateRecordStreak(habit);

        string habitCheckRow = "";
        string streaksRow = "";
        string habitEntryID = "";

        for (int i = 0; i < daysOfWeek.Length; i++)
        {
            string currentWeekDay = "";
            DateOnly habitDate = CalculateGridDates(habit, i);
            string checkIcon = "- [x] ";
            string uncheckIcon = "- [ ] ";
            string icon = uncheckIcon;
            bool habitDone = false;
            habitEntryID = $"{habit}{habitDate}";

            if (habitsCompleted.Contains(habitEntryID))
            {
                habitDone = true;
                icon = checkIcon;
            }

            if (i == 0)
            {
                currentWeekDay = icon;
                habitCheckRow += currentWeekDay; // + habitDate;
            }

            else if (i == daysOfWeek.Length - 1)
            {
                currentWeekDay = $"{icon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay;

                streaksRow = $"{currentStreak}".ToString().PadLeft(daysOfWeek[i].ToString().Length - 3) + $"{recordStreak}".ToString().PadLeft(8);
            }

            else
            {
                currentWeekDay = $"{icon}".PadLeft(daysOfWeek[i - 1].ToString().Length + 1);
                habitCheckRow += currentWeekDay;
            }
        }
        Console.Write($"{habit}");
        // @TODO: Handle long habit names.
        Console.WriteLine($"{habitCheckRow}".PadLeft(72 - habit.Length) + $"{streaksRow}\n");
    }
}

DateOnly CalculateGridDates(string habit, int weekDay)
{

    DateOnly habitGridEntryDate = new DateOnly();
    int dateDaysDifference = 0;


    if (daysOfWeek[weekDay] == selectedDate.DayOfWeek)
    {
        habitGridEntryDate = DateOnly.FromDateTime(selectedDate);
    }

    else
    {
        dateDaysDifference = weekDay - Array.IndexOf(daysOfWeek, selectedDate.DayOfWeek);
        DateTime habitEntryCalculatedDate = new DateTime();

        habitEntryCalculatedDate = selectedDate.AddDays(dateDaysDifference);
        habitGridEntryDate = DateOnly.FromDateTime(habitEntryCalculatedDate);
    }

    return habitGridEntryDate;
}

void SetFirstDayOfWeek(DayOfWeek customFirstDay)
{
    int dayOfWeekOffset = 0;

    if (customFirstDay != DayOfWeek.Sunday)
    {
        dayOfWeekOffset = 7 - (int)customFirstDay;

        daysOfWeek[0] = customFirstDay;
        for (int i = 1; i < dayOfWeekOffset; i++)
        {
            daysOfWeek[i] = customFirstDay + i;
        }

        daysOfWeek[dayOfWeekOffset] = DayOfWeek.Sunday;
        int counter = 0;

        for (int i = dayOfWeekOffset; i < daysOfWeek.Length; i++)
        {
            daysOfWeek[i] = DayOfWeek.Sunday + counter;
            counter++;
        }
    }
}

void MarkHabitDone(string habitEntryID, bool habitDone = true)
{
    // @TODO: Handle marking future dates
    if (habitDone == false)
    {
        habitsCompleted.Remove(habitEntryID);
    }
    else
        habitsCompleted.Add(habitEntryID);

    SaveUserData();
    ShowWeekGrid();
}

int CalculateCurrentStreak(string habit)
{
    int currentStreak = 0;
    DateOnly currentHabitDate = DateOnly.FromDateTime(currentDate);
    string currentHabitID = $"{habit}{currentHabitDate}";

    while (habitsCompleted.Contains(currentHabitID))
    {
        currentHabitDate = currentHabitDate.AddDays(-1);
        currentHabitID = habit + currentHabitDate;
        currentStreak++;
    }

    return currentStreak;
}

int CalculateRecordStreak(string habit)
{
    int recordStreak = 0;

    foreach (string habitCompletedID in habitsCompleted)
    {
        DateOnly currentHabitDate;
        DateOnly.TryParse(habitCompletedID.Substring(habit.Length), out currentHabitDate);
        string currentHabitID = $"{habit}{currentHabitDate}";

        int tempRecordStreak = 0;

        if (habitCompletedID.ToString().Contains(habit))
        {
            while (habitsCompleted.Contains(currentHabitID))
            {
                currentHabitDate = currentHabitDate.AddDays(-1);
                currentHabitID = habit + currentHabitDate;
                tempRecordStreak++;
            }

            if (tempRecordStreak > recordStreak)
                recordStreak = tempRecordStreak;
        }
    }

    return recordStreak;
}

void SetCustomDate(int year, int month, int day)
{
    DateTime customDate = new DateTime(year: year, month: month, day: day);
    selectedDate = customDate;
}