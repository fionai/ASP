using System.ComponentModel.DataAnnotations;

namespace MyAcademy.Models
{
	public class Group
	{

		[Key]
		public int group_id {  get; set; }
		public string group_name { get; set; }
	}
}
