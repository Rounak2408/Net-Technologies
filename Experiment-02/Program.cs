using System;
using System.Collections.Generic;

namespace EmployeePayrollSystem
{
    #region Enums
    
    /// <summary>
    /// Represents the employment type.
    /// </summary>
    public enum EmployeeType
    {
        FullTime,
        PartTime
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// Interface defining payable behavior for entities that earn a salary.
    /// </summary>
    public interface IPayable
    {
        double CalculateSalary();
    }

    /// <summary>
    /// Repository pattern interface for managing Employee storage.
    /// </summary>
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee emp);
        Employee GetEmployeeById(string id);
        List<Employee> GetAllEmployees();
        List<Employee> SearchEmployees(string query);
    }

    #endregion

    #region Custom Exception Hierarchy

    /// <summary>
    /// Base exception class for all Payroll System errors.
    /// </summary>
    public class PayrollException : Exception
    {
        public PayrollException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when employee validation fails.
    /// </summary>
    public class EmployeeValidationException : PayrollException
    {
        public EmployeeValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when a duplicate employee ID is added.
    /// </summary>
    public class DuplicateEmployeeException : PayrollException
    {
        public DuplicateEmployeeException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when an employee cannot be found.
    /// </summary>
    public class EmployeeNotFoundException : PayrollException
    {
        public EmployeeNotFoundException(string message) : base(message) { }
    }

    #endregion

    #region Audit Logger

    /// <summary>
    /// Simple utility class for logging system events.
    /// </summary>
    public static class AuditLogger
    {
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(string.Format("[INFO] [{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message));
            Console.ResetColor();
        }
    }

    #endregion

    #region Domain Entities

    /// <summary>
    /// Abstract base class representing a generic Employee in the system.
    /// </summary>
    public abstract class Employee
    {
        // Encapsulation: private backing fields and public properties
        private string _employeeId;
        private string _name;
        private double _basicSalary;

        public string EmployeeId
        {
            get { return _employeeId; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new EmployeeValidationException("Employee ID cannot be empty.");
                _employeeId = value.Trim();
            }
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new EmployeeValidationException("Employee Name cannot be empty.");
                _name = value.Trim();
            }
        }

        public double BasicSalary
        {
            get { return _basicSalary; }
            protected set
            {
                if (value < 0)
                    throw new EmployeeValidationException("Basic Salary cannot be negative.");
                _basicSalary = value;
            }
        }

        /// <summary>
        /// Protected constructor for Employee initialization.
        /// </summary>
        protected Employee(string employeeId, string name, double basicSalary)
        {
            this.EmployeeId = employeeId;
            this.Name = name;
            this.BasicSalary = basicSalary;
        }

        /// <summary>
        /// Abstract method to display specific employee details.
        /// </summary>
        public abstract void DisplayDetails();

        // Helper properties for reports
        public abstract double GrossEarnings { get; }
        public abstract double TotalDeductions { get; }
        public abstract EmployeeType Type { get; }
    }

    /// <summary>
    /// Concrete Full-Time Employee entity.
    /// </summary>
    public class FullTimeEmployee : Employee, IPayable
    {
        // Corporate formulas: HRA (20%), DA (10%), Special Allowance (Rs. 5000), PF (12%), PT (Rs. 200)
        public const double HraRate = 0.20;
        public const double DaRate = 0.10;
        public const double SpecialAllowance = 5000.00;
        public const double PfRate = 0.12;
        public const double ProfessionalTax = 200.00;

        public double HRA { get { return BasicSalary * HraRate; } }
        public double DA { get { return BasicSalary * DaRate; } }
        
        public double PF { get { return BasicSalary * PfRate; } }
        public double PT { get { return ProfessionalTax; } }
        
        /// <summary>
        /// Slab-based TDS calculation (10% if Basic > 50000, otherwise 5%)
        /// </summary>
        public double TDS
        {
            get
            {
                return BasicSalary > 50000.00 ? BasicSalary * 0.10 : BasicSalary * 0.05;
            }
        }

        public override EmployeeType Type { get { return EmployeeType.FullTime; } }

        public override double GrossEarnings
        {
            get { return BasicSalary + HRA + DA + SpecialAllowance; }
        }

        public override double TotalDeductions
        {
            get { return PF + PT + TDS; }
        }

        public FullTimeEmployee(string employeeId, string name, double basicSalary)
            : base(employeeId, name, basicSalary)
        {
        }

        /// <summary>
        /// Calculates net salary: Gross Earnings - Total Deductions
        /// </summary>
        public double CalculateSalary()
        {
            return GrossEarnings - TotalDeductions;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(string.Format("Employee Type        : Full-Time (ID: {0})", EmployeeId));
            Console.WriteLine(string.Format("Name                 : {0}", Name));
            Console.WriteLine("------------------ EARNINGS ----------------------");
            Console.WriteLine(string.Format("Basic Salary         : Rs. {0:F2}", BasicSalary));
            Console.WriteLine(string.Format("HRA (20%)            : Rs. {0:F2}", HRA));
            Console.WriteLine(string.Format("DA (10%)             : Rs. {0:F2}", DA));
            Console.WriteLine(string.Format("Special Allowance    : Rs. {0:F2}", SpecialAllowance));
            Console.WriteLine(string.Format("Gross Earnings       : Rs. {0:F2}", GrossEarnings));
            Console.WriteLine("------------------ DEDUCTIONS --------------------");
            Console.WriteLine(string.Format("Provident Fund (12%) : Rs. {0:F2}", PF));
            Console.WriteLine(string.Format("Professional Tax     : Rs. {0:F2}", PT));
            Console.WriteLine(string.Format("TDS (Tax)            : Rs. {0:F2}", TDS));
            Console.WriteLine(string.Format("Total Deductions     : Rs. {0:F2}", TotalDeductions));
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(string.Format("Net Salary Payable   : Rs. {0:F2}", CalculateSalary()));
            Console.WriteLine("--------------------------------------------------");
        }
    }

    /// <summary>
    /// Concrete Part-Time Employee entity.
    /// </summary>
    public class PartTimeEmployee : Employee, IPayable
    {
        private double _workingHours;
        private double _hourlyRate;

        public double WorkingHours
        {
            get { return _workingHours; }
            private set
            {
                if (value < 0)
                    throw new EmployeeValidationException("Working Hours cannot be negative.");
                _workingHours = value;
            }
        }

        public double HourlyRate
        {
            get { return _hourlyRate; }
            private set
            {
                if (value < 0)
                    throw new EmployeeValidationException("Hourly Rate cannot be negative.");
                _hourlyRate = value;
            }
        }

        public override EmployeeType Type { get { return EmployeeType.PartTime; } }

        /// <summary>
        /// Regular working hours cap is 40.
        /// </summary>
        public double RegularHours
        {
            get { return WorkingHours > 40.0 ? 40.0 : WorkingHours; }
        }

        /// <summary>
        /// Hours worked above 40 qualify for overtime.
        /// </summary>
        public double OvertimeHours
        {
            get { return WorkingHours > 40.0 ? WorkingHours - 40.0 : 0.0; }
        }

        public double RegularPay
        {
            get { return RegularHours * HourlyRate; }
        }

        /// <summary>
        /// Overtime is paid at 1.5x of hourly rate.
        /// </summary>
        public double OvertimePay
        {
            get { return OvertimeHours * HourlyRate * 1.5; }
        }

        public override double GrossEarnings
        {
            get { return RegularPay + OvertimePay; }
        }

        /// <summary>
        /// TDS is 10% if Gross > 30000, else 5%.
        /// </summary>
        public double TDS
        {
            get
            {
                return GrossEarnings > 30000.00 ? GrossEarnings * 0.10 : GrossEarnings * 0.05;
            }
        }

        public override double TotalDeductions
        {
            get { return TDS; }
        }

        public PartTimeEmployee(string employeeId, string name, double workingHours, double hourlyRate)
            : base(employeeId, name, 0.0) // Basic salary is 0 for hourly contractor
        {
            this.WorkingHours = workingHours;
            this.HourlyRate = hourlyRate;
        }

        /// <summary>
        /// Calculates net salary: Gross Earnings - TDS
        /// </summary>
        public double CalculateSalary()
        {
            return GrossEarnings - TotalDeductions;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(string.Format("Employee Type        : Part-Time (ID: {0})", EmployeeId));
            Console.WriteLine(string.Format("Name                 : {0}", Name));
            Console.WriteLine("------------------ EARNINGS ----------------------");
            Console.WriteLine(string.Format("Regular Hours worked : {0} hrs", RegularHours));
            Console.WriteLine(string.Format("Hourly Rate          : Rs. {0:F2}/hr", HourlyRate));
            Console.WriteLine(string.Format("Regular Pay          : Rs. {0:F2}", RegularPay));
            Console.WriteLine(string.Format("Overtime Hours       : {0} hrs", OvertimeHours));
            Console.WriteLine(string.Format("Overtime Pay (1.5x)  : Rs. {0:F2}", OvertimePay));
            Console.WriteLine(string.Format("Gross Earnings       : Rs. {0:F2}", GrossEarnings));
            Console.WriteLine("------------------ DEDUCTIONS --------------------");
            Console.WriteLine(string.Format("TDS (Tax Deducted)   : Rs. {0:F2}", TDS));
            Console.WriteLine(string.Format("Total Deductions     : Rs. {0:F2}", TotalDeductions));
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(string.Format("Net Salary Payable   : Rs. {0:F2}", CalculateSalary()));
            Console.WriteLine("--------------------------------------------------");
        }
    }

    #endregion

    #region Data Access Layer (In-Memory Repository)

    /// <summary>
    /// In-Memory implementation of employee repository.
    /// </summary>
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;

        public InMemoryEmployeeRepository()
        {
            _employees = new List<Employee>();
        }

        public void AddEmployee(Employee emp)
        {
            if (emp == null)
                throw new ArgumentNullException("emp");

            // Check uniqueness
            foreach (var existing in _employees)
            {
                if (existing.EmployeeId.Equals(emp.EmployeeId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DuplicateEmployeeException(string.Format("Employee ID '{0}' already exists.", emp.EmployeeId));
                }
            }

            _employees.Add(emp);
            AuditLogger.LogInfo(string.Format("Added {0} employee: {1} (ID: {2})", emp.Type, emp.Name, emp.EmployeeId));
        }

        public Employee GetEmployeeById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            foreach (var emp in _employees)
            {
                if (emp.EmployeeId.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
                    return emp;
            }
            return null;
        }

        public List<Employee> GetAllEmployees()
        {
            return new List<Employee>(_employees);
        }

        public List<Employee> SearchEmployees(string query)
        {
            List<Employee> results = new List<Employee>();
            if (string.IsNullOrEmpty(query))
                return results;

            string normalized = query.Trim().ToLower();
            foreach (var emp in _employees)
            {
                if (emp.EmployeeId.ToLower().Contains(normalized) || emp.Name.ToLower().Contains(normalized))
                {
                    results.Add(emp);
                }
            }
            return results;
        }
    }

    #endregion

    #region Presentation & Business Logic (Console UI)

    class Program
    {
        private static IEmployeeRepository _repository;

        static void Main(string[] args)
        {
            // Initialize in-memory repository
            _repository = new InMemoryEmployeeRepository();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("    ENTERPRISE PAYROLL & HR MANAGEMENT SYSTEM");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Register Full-Time Employee");
                Console.WriteLine("2. Register Part-Time Contractor");
                Console.WriteLine("3. Display All Employees & Pay Slips");
                Console.WriteLine("4. Search Employee by ID/Name");
                Console.WriteLine("5. HR Payroll & Tax Summary Report");
                Console.WriteLine("6. Exit System");
                Console.WriteLine("==================================================");
                Console.Write("Enter your command (1-6): ");

                string command = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (command)
                    {
                        case "1":
                            RegisterFullTime();
                            break;
                        case "2":
                            RegisterPartTime();
                            break;
                        case "3":
                            DisplayAllPayrolls();
                            break;
                        case "4":
                            SearchEmployee();
                            break;
                        case "5":
                            GenerateHrReport();
                            break;
                        case "6":
                            running = false;
                            Console.WriteLine("System shutting down. Goodbye!");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Invalid option! Please input a number between 1 and 6.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (PayrollException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Business Rule Exception: " + ex.Message);
                    Console.ResetColor();
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input Format Error: Please enter numeric characters only for rates, salaries, and hours.");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Critical System Error: " + ex.Message);
                    Console.ResetColor();
                }
            }
        }

        private static void RegisterFullTime()
        {
            Console.WriteLine(">>> Add New Full-Time Employee <<<");
            
            Console.Write("Employee ID: ");
            string id = Console.ReadLine();
            ValidateIdNotExists(id);

            Console.Write("Employee Name: ");
            string name = Console.ReadLine();

            Console.Write("Basic Monthly Salary (Rs.): ");
            double salary;
            if (!double.TryParse(Console.ReadLine(), out salary))
                throw new EmployeeValidationException("Basic salary must be a valid numeric quantity.");

            FullTimeEmployee emp = new FullTimeEmployee(id, name, salary);
            _repository.AddEmployee(emp);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Full-Time Employee saved in memory.");
            Console.ResetColor();
        }

        private static void RegisterPartTime()
        {
            Console.WriteLine(">>> Add New Part-Time Contractor <<<");

            Console.Write("Employee ID: ");
            string id = Console.ReadLine();
            ValidateIdNotExists(id);

            Console.Write("Employee Name: ");
            string name = Console.ReadLine();

            Console.Write("Hours Worked: ");
            double hours;
            if (!double.TryParse(Console.ReadLine(), out hours))
                throw new EmployeeValidationException("Hours worked must be a valid numeric quantity.");

            Console.Write("Hourly Compensation Rate (Rs.): ");
            double rate;
            if (!double.TryParse(Console.ReadLine(), out rate))
                throw new EmployeeValidationException("Hourly rate must be a valid numeric quantity.");

            PartTimeEmployee emp = new PartTimeEmployee(id, name, hours, rate);
            _repository.AddEmployee(emp);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Part-Time Contractor saved in memory.");
            Console.ResetColor();
        }

        private static void DisplayAllPayrolls()
        {
            Console.WriteLine(">>> Master Employee Payroll Records <<<");
            List<Employee> list = _repository.GetAllEmployees();

            if (list.Count == 0)
            {
                Console.WriteLine("No records exist. Please register employees first.");
                return;
            }

            // Demonstrating Runtime Polymorphism
            foreach (var emp in list)
            {
                // Dynamic call to DisplayDetails depending on underlying concrete object
                emp.DisplayDetails();

                // Dynamic call to CalculateSalary using IPayable interface type checking
                IPayable payable = emp as IPayable;
                if (payable != null)
                {
                    Console.WriteLine(string.Format("System Polymorphic Pay Calc: Rs. {0:F2}", payable.CalculateSalary()));
                }
                Console.WriteLine();
            }
        }

        private static void SearchEmployee()
        {
            Console.WriteLine(">>> Search Employee Database <<<");
            Console.Write("Enter ID or Name substring: ");
            string query = Console.ReadLine();

            List<Employee> results = _repository.SearchEmployees(query);
            Console.WriteLine(string.Format("\nSearch returned {0} matches:\n", results.Count));

            foreach (var emp in results)
            {
                emp.DisplayDetails();
                Console.WriteLine();
            }
        }

        private static void GenerateHrReport()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("         HR MASTER PAYROLL & TAX REPORT");
            Console.WriteLine("==================================================");

            List<Employee> list = _repository.GetAllEmployees();

            int totalCount = list.Count;
            int ftCount = 0;
            int ptCount = 0;
            double totalGross = 0.0;
            double totalNet = 0.0;
            double totalPF = 0.0;
            double totalPT = 0.0;
            double totalTDS = 0.0;

            foreach (var emp in list)
            {
                if (emp is FullTimeEmployee)
                {
                    ftCount++;
                    FullTimeEmployee ft = (FullTimeEmployee)emp;
                    totalPF += ft.PF;
                    totalPT += ft.PT;
                    totalTDS += ft.TDS;
                }
                else if (emp is PartTimeEmployee)
                {
                    ptCount++;
                    PartTimeEmployee pt = (PartTimeEmployee)emp;
                    totalTDS += pt.TDS;
                }

                totalGross += emp.GrossEarnings;
                IPayable payable = emp as IPayable;
                if (payable != null)
                {
                    totalNet += payable.CalculateSalary();
                }
            }

            Console.WriteLine(string.Format("Total Employees Enrolled : {0}", totalCount));
            Console.WriteLine(string.Format("  - Full-Time Employees  : {0}", ftCount));
            Console.WriteLine(string.Format("  - Part-Time Contractors: {0}", ptCount));
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(string.Format("Total Company Gross Pay  : Rs. {0:F2}", totalGross));
            Console.WriteLine(string.Format("Total Net Salaries Paid  : Rs. {0:F2}", totalNet));
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Collected Tax & Benefit Deductions:");
            Console.WriteLine(string.Format("  - Provident Fund (PF)  : Rs. {0:F2}", totalPF));
            Console.WriteLine(string.Format("  - Professional Tax (PT): Rs. {0:F2}", totalPT));
            Console.WriteLine(string.Format("  - TDS (Govt. Income Tax): Rs. {0:F2}", totalTDS));
            Console.WriteLine("==================================================");
        }

        private static void ValidateIdNotExists(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new EmployeeValidationException("Employee ID cannot be empty.");

            if (_repository.GetEmployeeById(id) != null)
            {
                throw new DuplicateEmployeeException(string.Format("Employee ID '{0}' already exists in records.", id.Trim()));
            }
        }
    }

    #endregion
}
