//I am a servant of the Secret Fire
void Main()
{
	Barrier barrier = new(3);
	Parallel.Invoke
	(
		( () =>
			{
				"Regression".Dumplings();
				barrier.SignalAndWait();
			}
		),
		( () =>
			{
				"Classification".Dumplings();
				barrier.SignalAndWait();
			}
		),
		( () =>
			{
				"Model".Dumplings();
				barrier.SignalAndWait();
			}
		)
	);
	 barrier.Dispose();
}

// You can define other methods, fields, classes and namespaces here
//YOU SHALL NOT PASS!
public static class DumplingExtension
{
	public static void Dumplings(this object obj)
	{
		obj.Dump();
	}
}
