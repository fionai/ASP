using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyAcademy.Models
{
	public class Student
	{
		[Key]
		public int stud_id { get; set; }
		public string last_name { get; set; }
		public string first_name { get; set; }
		public string middle_name { get; set; }
		public int group { get; set; }
	}
}
