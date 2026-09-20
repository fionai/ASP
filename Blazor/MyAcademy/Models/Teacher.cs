using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAcademy.Models
{
	public class Teacher
	{
		[Key]
		[Column(TypeName = "SMALLINT")]
		public int teacher_id { get; set; }
		public string last_name { get; set; }
		public string first_name { get; set; }
		public string middle_name { get; set; }
	}
}
