using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExpenseTracker
{
    #region Enums

    /// <summary>
    /// Categorization for user expenses.
    /// </summary>
    public enum ExpenseCategory
    {
        Food = 1,
        Travel,
        Utilities,
        Entertainment,
        Healthcare,
        Education,
        Miscellaneous
    }

    /// <summary>
    /// Standard payment methods.
    /// </summary>
    public enum PaymentMethod
    {
        Cash = 1,
        Card,
        UPI,
        NetBanking
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// Contract defining Expense storage operations.
    /// </summary>
    public interface IExpenseRepository
    {
        void AddExpense(Expense expense);
        void UpdateExpense(string id, Expense updatedExpense);
        void DeleteExpense(string id);
        Expense GetExpenseById(string id);
        List<Expense> GetAllExpenses();
        List<Expense> GetExpensesByCategory(ExpenseCategory category);
        List<Expense> GetExpensesByPaymentMethod(PaymentMethod method);
        void SaveToFile();
        void LoadFromFile();
    }

    /// <summary>
    /// Contract for generating visual or summary reports.
    /// </summary>
    public interface IReportGenerator
    {
        void ShowCategoryBreakdown(List<Expense> expenses);
        void ShowBudgetUtilization(List<Expense> expenses, double budgetLimit);
        void ShowPaymentMethodBreakdown(List<Expense> expenses);
    }

    #endregion

    #region Custom Exception Hierarchy

    /// <summary>
    /// Base exception class for all Expense Tracker errors.
    /// </summary>
    public class ExpenseTrackerException : Exception
    {
        public ExpenseTrackerException(string message) : base(message) { }
        public ExpenseTrackerException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when validation fails on any expense fields.
    /// </summary>
    public class ExpenseValidationException : ExpenseTrackerException
    {
        public ExpenseValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when an invalid monetary amount is supplied.
    /// </summary>
    public class InvalidAmountException : ExpenseValidationException
    {
        public InvalidAmountException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when a requested expense ID does not exist.
    /// </summary>
    public class ExpenseNotFoundException : ExpenseTrackerException
    {
        public ExpenseNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when budget constraints are violated in strict enforcement mode.
    /// </summary>
    public class BudgetExceededException : ExpenseTrackerException
    {
        public double BudgetLimit { get; private set; }
        public double AttemptedTotal { get; private set; }

        public BudgetExceededException(string message, double limit, double attemptedTotal) 
            : base(message)
        {
            BudgetLimit = limit;
            AttemptedTotal = attemptedTotal;
        }
    }

    /// <summary>
    /// Exception thrown when saving or loading data fails.
    /// </summary>
    public class DataPersistenceException : ExpenseTrackerException
    {
        public DataPersistenceException(string message, Exception inner) : base(message, inner) { }
    }

    #endregion

    #region Audit Logger

    /// <summary>
    /// Static logging utility for system tracking, diagnostics, and persistent log writing.
    /// </summary>
    public static class AuditLogger
    {
        private static readonly string LogFilePath = "audit_log.txt";

        static AuditLogger()
        {
            // Ensure log file starts fresh or is appended
            try
            {
                File.AppendAllText(LogFilePath, string.Format("--- System Log Started at {0:yyyy-MM-dd HH:mm:ss} ---\n", DateTime.Now));
            }
            catch
            {
                // Fallback gracefully if file logging is blocked
            }
        }

        private static void WriteToFile(string level, string message)
        {
            try
            {
                File.AppendAllText(LogFilePath, string.Format("[{0}] [{1:yyyy-MM-dd HH:mm:ss}] {2}\n", level, DateTime.Now, message));
            }
            catch
            {
                // Silently ignore log file write failures to prevent app crashing
            }
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[INFO] " + message);
            Console.ResetColor();
            WriteToFile("INFO", message);
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARNING] " + message);
            Console.ResetColor();
            WriteToFile("WARNING", message);
        }

        public static void Error(string message, Exception ex = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + message);
            if (ex != null)
            {
                Console.WriteLine("Details: " + ex.Message);
            }
            Console.ResetColor();
            WriteToFile("ERROR", message + (ex != null ? " -> " + ex.ToString() : ""));
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SUCCESS] " + message);
            Console.ResetColor();
            WriteToFile("SUCCESS", message);
        }

        public static void ShowLogContents()
        {
            Console.WriteLine("\n--- SYSTEM AUDIT LOG ---");
            if (File.Exists(LogFilePath))
            {
                string[] lines = File.ReadAllLines(LogFilePath);
                // Show last 30 lines
                int start = Math.Max(0, lines.Length - 30);
                for (int i = start; i < lines.Length; i++)
                {
                    Console.WriteLine(lines[i]);
                }
            }
            else
            {
                Console.WriteLine("No logs found.");
            }
            Console.WriteLine("------------------------\n");
        }
    }

    #endregion

    #region Domain Entities

    /// <summary>
    /// Represents an individual expense record. Encapsulates properties and enforces validation rules.
    /// </summary>
    public class Expense
    {
        private string _id;
        private string _description;
        private double _amount;
        private DateTime _date;

        public string Id
        {
            get { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ExpenseValidationException("Expense ID cannot be null or empty.");
                _id = value.Trim();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
                    throw new ExpenseValidationException("Description cannot be empty.");
                _description = value.Trim();
            }
        }

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (value <= 0)
                    throw new InvalidAmountException("Expense amount must be strictly greater than zero.");
                if (value > 10000000)
                    throw new InvalidAmountException("Expense amount cannot exceed 10,000,000 (sanity check).");
                _amount = value;
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value > DateTime.Today)
                    throw new ExpenseValidationException("Expense date cannot be in the future.");
                _date = value;
            }
        }

        public ExpenseCategory Category { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Expense(string id, string description, double amount, DateTime date, ExpenseCategory category, PaymentMethod paymentMethod)
        {
            this.Id = id;
            this.Description = description;
            this.Amount = amount;
            this.Date = date;
            this.Category = category;
            this.PaymentMethod = paymentMethod;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1:yyyy-MM-dd} | {2,-15} | {3,10:C} | Via: {4,-10} | {5}", Id, Date, Category, Amount, PaymentMethod, Description);
        }
    }

    /// <summary>
    /// Encapsulates budget operations and policies.
    /// </summary>
    public class BudgetManager
    {
        private double _monthlyBudgetLimit;
        
        public double Limit
        {
            get { return _monthlyBudgetLimit; }
            set
            {
                if (value < 0)
                    throw new ExpenseValidationException("Budget limit cannot be negative.");
                _monthlyBudgetLimit = value;
            }
        }

        public bool StrictEnforcement { get; set; }

        public BudgetManager(double initialLimit = 0, bool strict = false)
        {
            this.Limit = initialLimit;
            this.StrictEnforcement = strict;
        }

        /// <summary>
        /// Validates if an expense can be added relative to current spending and budget policy.
        /// </summary>
        public void EvaluateExpense(double currentTotalSpending, double newExpenseAmount)
        {
            if (Limit <= 0) return; // Budget not set or disabled

            double targetSpending = currentTotalSpending + newExpenseAmount;
            if (targetSpending > Limit)
            {
                string msg = string.Format("Transaction exceeds monthly budget. Limit: {0:C}, Current Total: {1:C}, Attempted: {2:C}.", Limit, currentTotalSpending, newExpenseAmount);
                if (StrictEnforcement)
                {
                    throw new BudgetExceededException(msg, Limit, targetSpending);
                }
                else
                {
                    AuditLogger.Warning("Budget Limit Warn Threshold Crossed! " + msg);
                }
            }
        }
    }

    #endregion

    #region Repositories

    /// <summary>
    /// In-memory repository with file-based (CSV) state persistence.
    /// Includes robust handling for corrupted files/records.
    /// </summary>
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly List<Expense> _expenses = new List<Expense>();
        private readonly BudgetManager _budgetManager;
        private readonly string _filePath;

        public ExpenseRepository(BudgetManager budgetManager, string filePath = "expenses.csv")
        {
            _budgetManager = budgetManager;
            _filePath = filePath;
        }

        public void AddExpense(Expense expense)
        {
            if (expense == null)
                throw new ArgumentNullException("expense", "Expense object cannot be null.");

            if (_expenses.Any(e => e.Id.Equals(expense.Id, StringComparison.OrdinalIgnoreCase)))
                throw new ExpenseTrackerException(string.Format("An expense with ID '{0}' already exists.", expense.Id));

            // Calculate current month's spending
            double currentMonthSpending = _expenses
                .Where(e => e.Date.Year == expense.Date.Year && e.Date.Month == expense.Date.Month)
                .Sum(e => e.Amount);

            // Let budget manager evaluate transaction
            _budgetManager.EvaluateExpense(currentMonthSpending, expense.Amount);

            _expenses.Add(expense);
            SaveToFile();
            AuditLogger.Success(string.Format("Expense successfully recorded: {0} ({1:C})", expense.Description, expense.Amount));
        }

        public void UpdateExpense(string id, Expense updatedExpense)
        {
            var existing = GetExpenseById(id);

            // Double check validation before replacing
            existing.Description = updatedExpense.Description;
            existing.Amount = updatedExpense.Amount;
            existing.Date = updatedExpense.Date;
            existing.Category = updatedExpense.Category;
            existing.PaymentMethod = updatedExpense.PaymentMethod;

            SaveToFile();
            AuditLogger.Success(string.Format("Expense ID '{0}' successfully updated.", id));
        }

        public void DeleteExpense(string id)
        {
            var existing = GetExpenseById(id);
            _expenses.Remove(existing);
            SaveToFile();
            AuditLogger.Success(string.Format("Expense ID '{0}' successfully deleted.", id));
        }

        public Expense GetExpenseById(string id)
        {
            var match = _expenses.FirstOrDefault(e => e.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (match == null)
                throw new ExpenseNotFoundException(string.Format("Expense with ID '{0}' not found in the system.", id));
            return match;
        }

        public List<Expense> GetAllExpenses()
        {
            return new List<Expense>(_expenses);
        }

        public List<Expense> GetExpensesByCategory(ExpenseCategory category)
        {
            return _expenses.Where(e => e.Category == category).ToList();
        }

        public List<Expense> GetExpensesByPaymentMethod(PaymentMethod method)
        {
            return _expenses.Where(e => e.PaymentMethod == method).ToList();
        }

        public void SaveToFile()
        {
            try
            {
                using (var writer = new StreamWriter(_filePath, false))
                {
                    // Write header
                    writer.WriteLine("Id,Description,Amount,Date,Category,PaymentMethod");
                    foreach (var exp in _expenses)
                    {
                        // Escape description to preserve comma in csv format
                        string escapedDesc = exp.Description.Replace("\"", "\"\"");
                        if (escapedDesc.Contains(",") || escapedDesc.Contains("\n"))
                        {
                            escapedDesc = "\"" + escapedDesc + "\"";
                        }
                        writer.WriteLine(string.Format("{0},{1},{2},{3:yyyy-MM-dd},{4},{5}", exp.Id, escapedDesc, exp.Amount, exp.Date, (int)exp.Category, (int)exp.PaymentMethod));
                    }
                }
            }
            catch (IOException ioEx)
            {
                throw new DataPersistenceException("Could not write database changes to disk.", ioEx);
            }
        }

        public void LoadFromFile()
        {
            _expenses.Clear();
            if (!File.Exists(_filePath))
            {
                AuditLogger.Info("Database file expenses.csv not found. A new one will be created upon save.");
                return;
            }

            int lineNum = 0;
            int corruptRows = 0;

            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    string header = reader.ReadLine(); // Skip header
                    lineNum++;

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNum++;
                        try
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            // Handle basic comma parsing, keeping in mind quotes
                            string[] tokens = ParseCsvLine(line);
                            if (tokens.Length < 6)
                            {
                                throw new FormatException("Missing required expense data columns.");
                            }

                            string id = tokens[0];
                            string description = tokens[1];
                            double amount = double.Parse(tokens[2]);
                            DateTime date = DateTime.Parse(tokens[3]);
                            ExpenseCategory category = (ExpenseCategory)int.Parse(tokens[4]);
                            PaymentMethod paymentMethod = (PaymentMethod)int.Parse(tokens[5]);

                            var expense = new Expense(id, description, amount, date, category, paymentMethod);
                            _expenses.Add(expense);
                        }
                        catch (Exception innerEx)
                        {
                            corruptRows++;
                            // Gracefully log corruption instead of failing execution (industry standard tolerance)
                            AuditLogger.Warning(string.Format("Corrupt record skipped at line {0}: {1}", lineNum, innerEx.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataPersistenceException("Critically failed to read expenses file.", ex);
            }

            if (corruptRows > 0)
            {
                AuditLogger.Warning(string.Format("Database file load complete with {0} corrupted records skipped.", corruptRows));
            }
            else
            {
                AuditLogger.Info(string.Format("Loaded {0} records successfully from file.", _expenses.Count));
            }
        }

        /// <summary>
        /// Simple CSV parser to extract elements and respect quotation marks containing commas.
        /// </summary>
        private string[] ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            int startPos = 0;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(UnescapeCsvToken(line.Substring(startPos, i - startPos)));
                    startPos = i + 1;
                }
            }
            // Add final token
            result.Add(UnescapeCsvToken(line.Substring(startPos)));

            return result.ToArray();
        }

        private string UnescapeCsvToken(string token)
        {
            token = token.Trim();
            if (token.StartsWith("\"") && token.EndsWith("\""))
            {
                token = token.Substring(1, token.Length - 2);
            }
            return token.Replace("\"\"", "\"");
        }
    }

    #endregion

    #region Report Generator

    /// <summary>
    /// Implementation of IReportGenerator. Generates clean console reports and progress charts.
    /// </summary>
    public class ReportGenerator : IReportGenerator
    {
        public void ShowCategoryBreakdown(List<Expense> expenses)
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No records to summarize.");
                return;
            }

            double grandTotal = expenses.Sum(e => e.Amount);

            Console.WriteLine("\n=============================================");
            Console.WriteLine("           CATEGORY-WISE BREAKDOWN           ");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-20} | {1,-12} | {2,-10}", "Category", "Total Cost", "Share (%)"));
            Console.WriteLine("---------------------------------------------");

            var groups = expenses.GroupBy(e => e.Category)
                                 .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                                 .OrderByDescending(g => g.Total);

            foreach (var group in groups)
            {
                double percentage = (group.Total / grandTotal) * 100;
                Console.WriteLine(string.Format("{0,-20} | {1,12:C} | {2,8:F1}%", group.Category, group.Total, percentage));
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine(string.Format("{0,-20} | {1,12:C} | 100.0%", "GRAND TOTAL", grandTotal));
            Console.WriteLine("=============================================\n");
        }

        public void ShowBudgetUtilization(List<Expense> expenses, double budgetLimit)
        {
            if (budgetLimit <= 0)
            {
                Console.WriteLine("\n[INFO] Monthly budget has not been set. Cannot generate utilization report.\n");
                return;
            }

            // Current month spending
            DateTime today = DateTime.Today;
            double currentMonthSpending = expenses
                .Where(e => e.Date.Year == today.Year && e.Date.Month == today.Month)
                .Sum(e => e.Amount);

            double percentage = (currentMonthSpending / budgetLimit) * 100;
            double remaining = budgetLimit - currentMonthSpending;

            Console.WriteLine("\n=============================================");
            Console.WriteLine(string.Format("    BUDGET UTILIZATION REPORT ({0:MMMM yyyy})", today));
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("Set Limit        : {0:C}", budgetLimit));
            Console.WriteLine(string.Format("Total Spending   : {0:C}", currentMonthSpending));
            Console.WriteLine(string.Format("Remaining Balance: {0:C}", remaining));
            Console.WriteLine("---------------------------------------------");

            // Render visual Progress Bar (20 steps)
            int barLength = 20;
            int filledSteps = Math.Min(barLength, (int)Math.Round((percentage / 100.0) * barLength));
            
            Console.Write("Usage Bar        : [");
            if (percentage >= 100.0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (percentage >= 80.0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            for (int i = 0; i < barLength; i++)
            {
                if (i < filledSteps) Console.Write("#");
                else Console.Write("-");
            }
            Console.ResetColor();
            Console.WriteLine(string.Format("] {0:F1}%", percentage));

            if (percentage >= 100.0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CRITICAL: You have exceeded your monthly limit!");
                Console.ResetColor();
            }
            else if (percentage >= 80.0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: You are nearing your monthly limit.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SAFE: Budget is healthy and well-managed.");
                Console.ResetColor();
            }
            Console.WriteLine("=============================================\n");
        }

        public void ShowPaymentMethodBreakdown(List<Expense> expenses)
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No records to summarize.");
                return;
            }

            double grandTotal = expenses.Sum(e => e.Amount);

            Console.WriteLine("\n=============================================");
            Console.WriteLine("        PAYMENT METHOD-WISE BREAKDOWN        ");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-20} | {1,-12} | {2,-10}", "Method", "Total Cost", "Share (%)"));
            Console.WriteLine("---------------------------------------------");

            var groups = expenses.GroupBy(e => e.PaymentMethod)
                                 .Select(g => new { Method = g.Key, Total = g.Sum(e => e.Amount) })
                                 .OrderByDescending(g => g.Total);

            foreach (var group in groups)
            {
                double percentage = (group.Total / grandTotal) * 100;
                Console.WriteLine(string.Format("{0,-20} | {1,12:C} | {2,8:F1}%", group.Method, group.Total, percentage));
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine(string.Format("{0,-20} | {1,12:C} | 100.0%", "GRAND TOTAL", grandTotal));
            Console.WriteLine("=============================================\n");
        }
    }

    #endregion

    #region Main Program Entry

    public class Program
    {
        private static BudgetManager _budgetManager;
        private static ExpenseRepository _repo;
        private static ReportGenerator _reporter;

        public static void Main(string[] args)
        {
            Console.Title = "Enterprise Expense Tracker Module";
            
            // Initialization
            _budgetManager = new BudgetManager(50000, false); // Default 50k warning-mode budget
            _repo = new ExpenseRepository(_budgetManager, "expenses.csv");
            _reporter = new ReportGenerator();

            AuditLogger.Info("Initializing Database...");
            try
            {
                _repo.LoadFromFile();
                // Add demo data if repository is completely empty
                if (_repo.GetAllExpenses().Count == 0)
                {
                    LoadMockData();
                }
            }
            catch (Exception ex)
            {
                AuditLogger.Error("Initialization failed. Starting with empty database.", ex);
            }

            bool exitApp = false;
            while (!exitApp)
            {
                try
                {
                    ShowHeader();
                    ShowMenu();
                    string choice = ReadTrimmedLine();

                    switch (choice)
                    {
                        case "1":
                            SetBudgetLimit();
                            break;
                        case "2":
                            AddNewExpense();
                            break;
                        case "3":
                            ViewAllExpenses();
                            break;
                        case "4":
                            FilterExpensesMenu();
                            break;
                        case "5":
                            EditExpenseDetails();
                            break;
                        case "6":
                            DeleteExpenseRecord();
                            break;
                        case "7":
                            ShowReportsMenu();
                            break;
                        case "8":
                            AuditLogger.ShowLogContents();
                            break;
                        case "9":
                            exitApp = true;
                            AuditLogger.Info("Terminating Expense Tracker session. Goodbye!");
                            break;
                        default:
                            AuditLogger.Warning("Invalid menu selection. Please select between [1-9].");
                            break;
                    }
                }
                catch (ExpenseValidationException ex)
                {
                    AuditLogger.Error("Validation Error: Please correct your inputs.", ex);
                }
                catch (BudgetExceededException ex)
                {
                    AuditLogger.Error("Budget Violation: Operation rejected.", ex);
                    Console.WriteLine(string.Format("Maximum Limit : {0:C}", ex.BudgetLimit));
                    Console.WriteLine(string.Format("Attempted sum : {0:C}", ex.AttemptedTotal));
                }
                catch (ExpenseNotFoundException ex)
                {
                    AuditLogger.Error("Lookup Failure", ex);
                }
                catch (DataPersistenceException ex)
                {
                    AuditLogger.Error("Database Storage Failure. Changes not committed.", ex);
                }
                catch (Exception ex)
                {
                    AuditLogger.Error("Critical unexpected runtime error.", ex);
                }

                if (!exitApp)
                {
                    Console.WriteLine("\nPress [ENTER] to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static string ReadTrimmedLine()
        {
            string input = Console.ReadLine();
            return input != null ? input.Trim() : string.Empty;
        }

        private static void ShowHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
┌────────────────────────────────────────────────────────┐
│           ENTERPRISE EXPENSE TRACKER SYSTEM            │
│       Robust Exception Handling & Persistence v1.0     │
└────────────────────────────────────────────────────────┘");
            Console.ResetColor();

            // Quick status overview
            double monthlyLimit = _budgetManager.Limit;
            double currentMonthSpending = _repo.GetAllExpenses()
                .Where(e => e.Date.Year == DateTime.Today.Year && e.Date.Month == DateTime.Today.Month)
                .Sum(e => e.Amount);

            Console.Write(" Spent This Month: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(string.Format("{0:C}", currentMonthSpending));
            Console.ResetColor();
            Console.Write(" | Monthly Limit: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(monthlyLimit > 0 ? string.Format("{0:C}", monthlyLimit) : "NOT SET");
            Console.ResetColor();
            
            Console.Write(" Budget Mode: ");
            Console.ForegroundColor = _budgetManager.StrictEnforcement ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(_budgetManager.StrictEnforcement ? "STRICT (BLOCKS EXPENDITURE)" : "FLEXIBLE (WARNINGS ONLY)");
            Console.ResetColor();
            Console.WriteLine("----------------────────────────────────────────────────");
        }

        private static void ShowMenu()
        {
            Console.WriteLine("1. Configure Monthly Budget & Threshold Policies");
            Console.WriteLine("2. Record New Expense");
            Console.WriteLine("3. View Complete Expense Ledger");
            Console.WriteLine("4. Filter & Search Ledger records");
            Console.WriteLine("5. Update Existing Expense Record");
            Console.WriteLine("6. Delete Expense Record");
            Console.WriteLine("7. View Summary & Analytical Reports");
            Console.WriteLine("8. Display System Audit Logs");
            Console.WriteLine("9. Safely Exit");
            Console.Write("\nChoose an option (1-9): ");
        }

        private static void SetBudgetLimit()
        {
            Console.WriteLine("\n--- BUDGET CONFIGURATION ---");
            Console.Write("Enter Monthly Budget Limit Amount (e.g. 15000): ");
            string inputAmount = ReadTrimmedLine();
            
            double limit;
            if (!double.TryParse(inputAmount, out limit) || limit < 0)
                throw new ExpenseValidationException("Invalid budget limit amount entered.");

            Console.Write("Enable Strict Enforcement (Blocks expenses exceeding budget)? (Y/N): ");
            string strictChoice = ReadTrimmedLine().ToUpper();
            bool strict = (strictChoice == "Y" || strictChoice == "YES");

            _budgetManager.Limit = limit;
            _budgetManager.StrictEnforcement = strict;
            
            AuditLogger.Success(string.Format("Monthly Budget Limit configured to {0:C} in {1} mode.", limit, strict ? "STRICT" : "FLEXIBLE"));
        }

        private static void AddNewExpense()
        {
            Console.WriteLine("\n--- RECORD NEW EXPENSE ---");

            // Generate unique, clean alphanumeric transaction ID
            string guidHex = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            string txId = "TX" + guidHex;

            Console.Write("Enter Expense Title/Description: ");
            string desc = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(desc))
                throw new ExpenseValidationException("Description cannot be null or empty.");

            Console.Write("Enter Amount ($): ");
            string inputAmount = ReadTrimmedLine();
            double amt;
            if (!double.TryParse(inputAmount, out amt))
                throw new InvalidAmountException("Entered amount must be a numeric value.");

            Console.Write("Enter Transaction Date (YYYY-MM-DD) [Default: Today]: ");
            string inputDate = ReadTrimmedLine();
            DateTime dt = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(inputDate))
            {
                if (!DateTime.TryParse(inputDate, out dt))
                    throw new ExpenseValidationException("Invalid Date format. Must be YYYY-MM-DD.");
            }

            // Category Selection
            Console.WriteLine("\nAvailable Categories:");
            foreach (ExpenseCategory cat in Enum.GetValues(typeof(ExpenseCategory)))
            {
                Console.WriteLine(string.Format("{0}. {1}", (int)cat, cat));
            }
            Console.Write("Select Category index: ");
            string catInput = ReadTrimmedLine();
            int catIdx;
            if (!int.TryParse(catInput, out catIdx) || !Enum.IsDefined(typeof(ExpenseCategory), catIdx))
                throw new ExpenseValidationException("Invalid Expense Category selected.");
            ExpenseCategory selectedCat = (ExpenseCategory)catIdx;

            // Payment Selection
            Console.WriteLine("\nAvailable Payment Methods:");
            foreach (PaymentMethod pay in Enum.GetValues(typeof(PaymentMethod)))
            {
                Console.WriteLine(string.Format("{0}. {1}", (int)pay, pay));
            }
            Console.Write("Select Payment Method index: ");
            string payInput = ReadTrimmedLine();
            int payIdx;
            if (!int.TryParse(payInput, out payIdx) || !Enum.IsDefined(typeof(PaymentMethod), payIdx))
                throw new ExpenseValidationException("Invalid Payment Method selected.");
            PaymentMethod selectedPay = (PaymentMethod)payIdx;

            // Instantiate and add to repo
            var newExpense = new Expense(txId, desc, amt, dt, selectedCat, selectedPay);
            _repo.AddExpense(newExpense);
        }

        private static void ViewAllExpenses()
        {
            Console.WriteLine("\n--- LEDGER LISTING ---");
            var items = _repo.GetAllExpenses().OrderByDescending(e => e.Date).ToList();
            if (items.Count == 0)
            {
                Console.WriteLine("No records in ledger database.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(string.Format("\nTotal Records: {0} | Cumulative Sum: {1:C}", items.Count, items.Sum(e => e.Amount)));
        }

        private static void FilterExpensesMenu()
        {
            Console.WriteLine("\n--- FILTER & SEARCH LEDGER ---");
            Console.WriteLine("1. Filter by Expense Category");
            Console.WriteLine("2. Filter by Payment Method");
            Console.WriteLine("3. Search by Description Keyword");
            Console.Write("Select filtering option (1-3): ");
            string choice = ReadTrimmedLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nAvailable Categories:");
                    foreach (ExpenseCategory cat in Enum.GetValues(typeof(ExpenseCategory)))
                    {
                        Console.WriteLine(string.Format("{0}. {1}", (int)cat, cat));
                    }
                    Console.Write("Select Category: ");
                    string catIn = ReadTrimmedLine();
                    int catIdx;
                    if (int.TryParse(catIn, out catIdx) && Enum.IsDefined(typeof(ExpenseCategory), catIdx))
                    {
                        var list = _repo.GetExpensesByCategory((ExpenseCategory)catIdx);
                        DisplayFilteredResults(list);
                    }
                    else
                    {
                        AuditLogger.Warning("Invalid selection.");
                    }
                    break;

                case "2":
                    Console.WriteLine("\nAvailable Payment Methods:");
                    foreach (PaymentMethod pay in Enum.GetValues(typeof(PaymentMethod)))
                    {
                        Console.WriteLine(string.Format("{0}. {1}", (int)pay, pay));
                    }
                    Console.Write("Select Payment Method: ");
                    string payIn = ReadTrimmedLine();
                    int payIdx;
                    if (int.TryParse(payIn, out payIdx) && Enum.IsDefined(typeof(PaymentMethod), payIdx))
                    {
                        var list = _repo.GetExpensesByPaymentMethod((PaymentMethod)payIdx);
                        DisplayFilteredResults(list);
                    }
                    else
                    {
                        AuditLogger.Warning("Invalid selection.");
                    }
                    break;

                case "3":
                    Console.Write("Enter description search phrase: ");
                    string q = ReadTrimmedLine();
                    if (!string.IsNullOrEmpty(q))
                    {
                        var list = _repo.GetAllExpenses()
                            .Where(e => e.Description.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToList();
                        DisplayFilteredResults(list);
                    }
                    else
                    {
                        AuditLogger.Warning("Search phrase cannot be empty.");
                    }
                    break;

                default:
                    AuditLogger.Warning("Unknown filter type.");
                    break;
            }
        }

        private static void DisplayFilteredResults(List<Expense> filtered)
        {
            Console.WriteLine("\nMatches Found:");
            if (filtered.Count == 0)
            {
                Console.WriteLine("No records match your filters.");
                return;
            }

            foreach (var item in filtered)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(string.Format("Total Matches: {0} | Total Cost: {1:C}", filtered.Count, filtered.Sum(e => e.Amount)));
        }

        private static void EditExpenseDetails()
        {
            Console.WriteLine("\n--- UPDATE EXPENSE DETAILS ---");
            Console.Write("Enter Target Expense Transaction ID to update (e.g. TXA1B2C3D4): ");
            string id = ReadTrimmedLine();

            // Get existing or throw Exception
            var current = _repo.GetExpenseById(id);

            Console.WriteLine(string.Format("\nFound record details: \n{0}", current));
            Console.WriteLine("Leave field empty to keep current configuration values.\n");

            Console.Write(string.Format("Enter New Description [{0}]: ", current.Description));
            string inputDesc = Console.ReadLine();
            if (inputDesc != null) inputDesc = inputDesc.Trim();
            string finalDesc = string.IsNullOrEmpty(inputDesc) ? current.Description : inputDesc;

            Console.Write(string.Format("Enter New Amount [{0}]: ", current.Amount));
            string inputAmount = ReadTrimmedLine();
            double finalAmount = current.Amount;
            if (!string.IsNullOrEmpty(inputAmount))
            {
                if (!double.TryParse(inputAmount, out finalAmount))
                    throw new InvalidAmountException("Entered amount must be numeric.");
            }

            Console.Write(string.Format("Enter New Date (YYYY-MM-DD) [{0:yyyy-MM-dd}]: ", current.Date));
            string inputDate = ReadTrimmedLine();
            DateTime finalDate = current.Date;
            if (!string.IsNullOrEmpty(inputDate))
            {
                if (!DateTime.TryParse(inputDate, out finalDate))
                    throw new ExpenseValidationException("Invalid Date syntax. Use YYYY-MM-DD.");
            }

            // Category Edit
            Console.WriteLine("\nAvailable Categories:");
            foreach (ExpenseCategory cat in Enum.GetValues(typeof(ExpenseCategory)))
            {
                Console.WriteLine(string.Format("{0}. {1}", (int)cat, cat));
            }
            Console.Write(string.Format("Select New Category index (Or Enter to keep '{0}'): ", current.Category));
            string catInput = ReadTrimmedLine();
            ExpenseCategory finalCat = current.Category;
            if (!string.IsNullOrEmpty(catInput))
            {
                int catIdx;
                if (!int.TryParse(catInput, out catIdx) || !Enum.IsDefined(typeof(ExpenseCategory), catIdx))
                    throw new ExpenseValidationException("Invalid Category selected.");
                finalCat = (ExpenseCategory)catIdx;
            }

            // Payment Method Edit
            Console.WriteLine("\nAvailable Payment Methods:");
            foreach (PaymentMethod pay in Enum.GetValues(typeof(PaymentMethod)))
            {
                Console.WriteLine(string.Format("{0}. {1}", (int)pay, pay));
            }
            Console.Write(string.Format("Select New Payment Method index (Or Enter to keep '{0}'): ", current.PaymentMethod));
            string payInput = ReadTrimmedLine();
            PaymentMethod finalPay = current.PaymentMethod;
            if (!string.IsNullOrEmpty(payInput))
            {
                int payIdx;
                if (!int.TryParse(payInput, out payIdx) || !Enum.IsDefined(typeof(PaymentMethod), payIdx))
                    throw new ExpenseValidationException("Invalid Payment Method selected.");
                finalPay = (PaymentMethod)payIdx;
            }

            var updated = new Expense(id, finalDesc, finalAmount, finalDate, finalCat, finalPay);
            _repo.UpdateExpense(id, updated);
        }

        private static void DeleteExpenseRecord()
        {
            Console.WriteLine("\n--- DELETE EXPENSE RECORD ---");
            Console.Write("Enter Expense Transaction ID to delete: ");
            string id = ReadTrimmedLine();

            // Confirm delete
            var existing = _repo.GetExpenseById(id);
            Console.WriteLine(string.Format("Found record:\n{0}", existing));
            Console.Write("Are you absolutely sure you want to delete this record? (Y/N): ");
            string confirmation = ReadTrimmedLine().ToUpper();
            if (confirmation == "Y" || confirmation == "YES")
            {
                _repo.DeleteExpense(id);
            }
            else
            {
                AuditLogger.Info("Delete action cancelled.");
            }
        }

        private static void ShowReportsMenu()
        {
            Console.WriteLine("\n--- ANALYTICAL REPORTS ---");
            Console.WriteLine("1. Category Distribution Summary");
            Console.WriteLine("2. Current Month Budget Utilization Bar Chart");
            Console.WriteLine("3. Payment Method Distribution Summary");
            Console.Write("Select report number (1-3): ");
            string choice = ReadTrimmedLine();

            var data = _repo.GetAllExpenses();

            switch (choice)
            {
                case "1":
                    _reporter.ShowCategoryBreakdown(data);
                    break;
                case "2":
                    _reporter.ShowBudgetUtilization(data, _budgetManager.Limit);
                    break;
                case "3":
                    _reporter.ShowPaymentMethodBreakdown(data);
                    break;
                default:
                    AuditLogger.Warning("Invalid report selection.");
                    break;
            }
        }

        private static void LoadMockData()
        {
            AuditLogger.Info("Database is currently empty. Pre-populating ledger with representative corporate demo records...");
            try
            {
                DateTime baseDate = DateTime.Today;
                _repo.AddExpense(new Expense("TXDEMO01", "Client Lunch - Taj Hotel", 4500, baseDate.AddDays(-5), ExpenseCategory.Food, PaymentMethod.Card));
                _repo.AddExpense(new Expense("TXDEMO02", "Uber Ride - Airport to Office", 1200, baseDate.AddDays(-4), ExpenseCategory.Travel, PaymentMethod.UPI));
                _repo.AddExpense(new Expense("TXDEMO03", "AWS Hosting Server Bill", 24500, baseDate.AddDays(-2), ExpenseCategory.Utilities, PaymentMethod.NetBanking));
                _repo.AddExpense(new Expense("TXDEMO04", "Team Dinner", 8500, baseDate.AddDays(-1), ExpenseCategory.Food, PaymentMethod.UPI));
                _repo.AddExpense(new Expense("TXDEMO05", "Office Stationery", 750, baseDate, ExpenseCategory.Miscellaneous, PaymentMethod.Cash));
                AuditLogger.Info("Demo dataset loaded successfully.");
            }
            catch (Exception ex)
            {
                AuditLogger.Error("Error populating mock data.", ex);
            }
        }
    }

    #endregion
}
