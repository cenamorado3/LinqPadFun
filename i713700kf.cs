async void Main()
{
	int lowerBound = 10000;
	int upperBound = 1000000;//if you see negatives, you are overflowing


	Thread[] pool = new Thread[24];
	Random rng = new();
	for(int i = 0; i < 24; i += 1)
	{
		pool[i] = new Thread(A.Fib);
	}
	
	Stopwatch sw = new();
	
	sw.Start();
	Parallel.ForEach(pool, thread => 
	{
		thread.Start(rng.Next(lowerBound, upperBound));
	});
	sw.Stop();
	sw.ElapsedMilliseconds.Dump(">>>In Parallel<<<");
	
	
	
	sw.Start();
	for(int i = 0; i < 24; i += 1)
	{
		await A.AsyncFib(rng.Next(lowerBound, upperBound));
	}
	
	sw.Stop();
	sw.ElapsedMilliseconds.Dump(">>>Async Task<<<");
}

// You can define other methods, fields, classes and namespaces here
class A
{

	public static void Fib(object o)
	{
		int n = (int)o;
	    ulong a = 0, b = 1, c = 0;

	    // To return the first Fibonacci number
	    if (n == 0)
	        a.Dump($"fib at {n}");

	    for (int i = 2; i <= n; i++) {
	        c = a + b;
	        a = b;
	        b = c;
	    }

	    b.Dump($"fib at {n}");
	}
	
	public static Task AsyncFib(object o)
	{
		return Task.Run( () => 
		{
			int n = (int)o;
		    ulong a = 0, b = 1, c = 0;

		    // To return the first Fibonacci number
		    if (n == 0)
		        a.Dump($"fib at {n}");

		    for (int i = 2; i <= n; i++) {
		        c = a + b;
		        a = b;
		        b = c;
		    }
			b.Dump($"fib at {n}");
		});
	}
}
