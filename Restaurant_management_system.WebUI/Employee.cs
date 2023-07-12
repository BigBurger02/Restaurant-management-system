using System;
namespace Restaurant_management_system.WebUI
{
	public class Employee
	{
		public Employee(string firstName, string lastName, string middleName, int employeeNumber, int birthDay)
		{
			FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			MiddleName = middleName ?? throw new ArgumentNullException(nameof(middleName));
			EmployeeNumber = employeeNumber;
			BirthDaty = birthDay;
		}
		public string FirstName
		{
			get;
			set;
		}
		public string LastName
		{
			get;
			set;
		}
		public string MiddleName
		{
			get;
			set;
		}
		public int EmployeeNumber
		{
			get;
			set;
		}
		public int BirthDaty
		{
			get;
			set;
		}
		public string FullName
		{
			get
			{
				return $"{FirstName} {MiddleName} {LastName}";
			}
		}
	}
}

