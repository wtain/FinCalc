using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public class Person
	{
		public string name;
		public string fullName;
		public long personId;

		public static readonly Person DefaultPerson = new Person("Name", "Full Name", 0);

		public Person(string name, string fullName, long personId)
		{
			this.name = name;
			this.fullName = fullName;
			this.personId = personId;
		}
	}
}
