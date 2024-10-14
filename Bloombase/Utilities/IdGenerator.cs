namespace Bloombase.Utilities
{
	public static class IdGenerator
	{
		public static string GenerateRandomId<T>(int length, List<T> list, Func<T, string> idSelector)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();

			int i = 0;

			while (true)
			{
				i++;

				if (i > 1000)
				{
					break;
				}

				string randomId = new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());

				if (list.Any(p => idSelector(p) == randomId))
				{
					continue;
				}
				else
				{
					return randomId;
				}
			}

			return "";
		}
	}
}