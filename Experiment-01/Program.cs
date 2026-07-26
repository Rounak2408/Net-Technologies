// Experiment-01: Student Admission Management System
using System;

class Student
{
    // Private Data Members (Access Modifiers)
    private string name;
    private double pcmPercentage;
    private string branch;
    private double courseFee;
    private double scholarship;
    private double busFee;

    // Constructor
    public Student(string name, double pcmPercentage)
    {
        this.name = name;
        this.pcmPercentage = pcmPercentage;
        this.branch = "";
        this.courseFee = 0;
        this.scholarship = 0;
        this.busFee = 0;
    }

    // Check Admission Eligibility
    public bool IsEligible()
    {
        return pcmPercentage >= 40;
    }

    // Branch Selection
    public void SelectBranch()
    {
        Console.WriteLine("\n--- AVAILABLE BRANCHES ---");
        Console.WriteLine("1. Computer Science Engineering - Rs. 100000");
        Console.WriteLine("2. Mechanical Engineering       - Rs. 80000");
        Console.WriteLine("3. Civil Engineering            - Rs. 70000");
        Console.WriteLine("4. Electrical Engineering       - Rs. 75000");

        Console.Write("\nSelect Branch (1-4): ");
        int choice;
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            choice = 0; // Will fallback to default case
        }

        switch (choice)
        {
            case 1:
                branch = "Computer Science Engineering";
                courseFee = 100000;
                break;

            case 2:
                branch = "Mechanical Engineering";
                courseFee = 80000;
                break;

            case 3:
                branch = "Civil Engineering";
                courseFee = 70000;
                break;

            case 4:
                branch = "Electrical Engineering";
                courseFee = 75000;
                break;

            default:
                Console.WriteLine("Invalid Choice! Default branch selected.");
                branch = "Computer Science Engineering";
                courseFee = 100000;
                break;
        }
    }

    // Scholarship
    public void CalculateScholarship()
    {
        if (pcmPercentage >= 90)
            scholarship = 100;
        else if (pcmPercentage >= 80)
            scholarship = 50;
        else if (pcmPercentage >= 70)
            scholarship = 30;
        else if (pcmPercentage >= 60)
            scholarship = 20;
        else
            scholarship = 0;
    }

    // Bus Facility
    public void AddBusFacility()
    {
        Console.Write("\nDo you want Bus Facility? (yes/no): ");
        string choice = Console.ReadLine();

        if (choice.ToLower() == "yes" || choice.ToLower() == "y")
        {
            double distance = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.Write("Enter Distance from College (KM): ");
                string input = Console.ReadLine();
                if (input == null) break;
                input = input.ToLower().Replace("km", "").Trim();
                if (double.TryParse(input, out distance))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a valid number (e.g., 40).");
                }
            }

            if (distance <= 5)
                busFee = 5000;
            else if (distance <= 10)
                busFee = 8000;
            else if (distance <= 20)
                busFee = 12000;
            else if (distance <= 30)
                busFee = 15000;
            else
                busFee = 20000;
        }
        else
        {
            busFee = 0;
        }
    }

    // Display Details
    public void DisplayAdmissionDetails()
    {
        double scholarshipAmount = courseFee * scholarship / 100;
        double feeAfterScholarship = courseFee - scholarshipAmount;
        double finalFee = feeAfterScholarship + busFee;

        Console.WriteLine("\n======================================");
        Console.WriteLine("       FINAL ADMISSION DETAILS");
        Console.WriteLine("======================================");

        Console.WriteLine("Student Name         : " + name);
        Console.WriteLine("PCM Percentage       : " + pcmPercentage + "%");
        Console.WriteLine("Selected Branch      : " + branch);
        Console.WriteLine("Course Fee           : Rs. " + courseFee);
        Console.WriteLine("Scholarship          : " + scholarship + "%");
        Console.WriteLine("Scholarship Amount   : Rs. " + scholarshipAmount);
        Console.WriteLine("Bus Fee              : Rs. " + busFee);
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("Final Payable Fee    : Rs. " + finalFee);
        Console.WriteLine("======================================");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("======================================");
        Console.WriteLine("   STUDENT ADMISSION MANAGEMENT");
        Console.WriteLine("======================================");

        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine();

        double percentage = 0;
        bool isValidPercentage = false;
        while (!isValidPercentage)
        {
            Console.Write("Enter 12th PCM Percentage: ");
            string input = Console.ReadLine();
            if (input == null) break;
            input = input.Replace("%", "").Trim();
            if (double.TryParse(input, out percentage))
            {
                isValidPercentage = true;
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid percentage number (e.g., 85).");
            }
        }

        Student student = new Student(name, percentage);

        if (!student.IsEligible())
        {
            Console.WriteLine("\nAdmission Rejected!");
            Console.WriteLine("Minimum 40% PCM is required.");
            return;
        }

        Console.WriteLine("\nCongratulations! You are eligible for admission.");

        student.SelectBranch();
        student.CalculateScholarship();
        student.AddBusFacility();
        student.DisplayAdmissionDetails();

        Console.WriteLine("\nAdmission Process Completed Successfully!");
        Console.ReadKey();
    }
}
