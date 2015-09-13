using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public struct Person
	{
		public string name;
		public string fullName;
		public int personId;

		public Person(string name, string fullName, int personId)
		{
			this.name = name;
			this.fullName = fullName;
			this.personId = personId;
		}
	}
}
