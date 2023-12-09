namespace Program
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine($"\tbxcrypt (d|e) (strings)");
			}
			else
			{
				switch (args[0])
				{
					case "d":
						for (int i = 1; i < args.Length; i++)
						{
							Console.WriteLine($"{args[i]}: {BX.App.Util.Crypt.Decrypt(args[i])}");
						}
						break;
					case "e":
						for (int i = 1; i < args.Length; i++)
						{
							Console.WriteLine($"{args[i]}: {BX.App.Util.Crypt.Encrypt(args[i])}");
						}
						break;
					default:
						Console.WriteLine("Invalid Crypt call type. Use `d` for decrypting and `e` for encrypting.");
						break;
				}
			}
		}
	}
}