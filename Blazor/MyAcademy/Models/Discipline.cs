using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAcademy.Models
{

	public class Discipline
	{	
		[Key]
		//[Column(TypeName = "TINYINT")]
		public short discipline_id { get; set; }
		public string discipline_name { get; set; }
	}
}
