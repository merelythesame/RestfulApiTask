using TestWebApi.Models;

namespace TestWebApi
{
	public class StaticData
	{
		public static List<User> Users { get; set; } = new List<User>
		{
			new User { Id = 1, Name = "Alex1", Email = "ipz232_pov@student.ztu.edu.ua" },
			new User { Id = 2, Name = "Alex2", Email = "ipz232_pov@student.ztu.edu.ua" },
			new User { Id = 3, Name = "Alex3", Email = "ipz232_pov@student.ztu.edu.ua" },
			new User { Id = 4, Name = "Alex4", Email = "ipz232_pov@student.ztu.edu.ua" },
			new User { Id = 5, Name = "Alex5", Email = "ipz232_pov@student.ztu.edu.ua" },
			new User { Id = 6, Name = "Alex6", Email = "ipz232_pov@student.ztu.edu.ua" }
		};

		public static List<Member> Members = new()
		{
			new Member { Id = 1, Username = "admin", Password = "admin", Role = "Admin" },
			new Member { Id = 2, Username = "user", Password = "user", Role = "User" }
		};
	}
}
