// Disable the "reference to volatile field not treated as volatile" error.
//the key may be to disable the pragma warning, since now we can ref a volatile variable since it will not be treated as volatile within the scope of the function.
//note to future self: it is likely you must still use some sort of readwritelock(slim) at the lowest level.
#https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0420
//https://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/Task.cs,31
#pragma warning disable 0420


//volatile int c = 0; //taking the volatile approach and was not far beind the interlock approach, but similiar results to using t without meticulous consideration
void Main()
{
	int c= 0;
	object o = new();
	Task[] tar = new Task[5];
	for(int t = 0; t < tar.Length; t ++)//using t is madness
	{
	    tar[t] = Task.Factory.StartNew( (Object obj ) => 
		{
		
			lock(o)
			{
			Metal m = ((Metal)obj);
			
			
			//switch(c)
			//{
			//	case 0:
			//		c= 1;
			//		break;
			//	case 1:
			//		c = 0;
			//		break;
			//	default:
			//		break;
			//}
			m.m = m.i % 2 == 0 ? "iron" : "carbon"; //if you use t instead of the initialized i, m.i, value, you have a race condition with far from expected results
			Interlocked.Increment(ref c);//by using the thread safe interlocked, an auxillary indexer/pointer, and a lock, expected results can be achieved, with inconsistency
			}//removing the lock will break the atomicity 
	   	},
	   	new Metal(){i=t} // using the init value yields the greatest consistency, due to its immutable nature which is thread safe by virtue, the same result can happen with meticulous care when setting a new value
		);
	}
	Task.WaitAll(tar);
	foreach(Task t in tar)
	{
		t.AsyncState.Forge();
	}
}

record Metal{public string m{get;set;}public int i{get;init;}=0;}

public static class Metallurgy
{
	public static void Forge(this object obj)
	{
		obj.Dump();
	}
}

//shits hot
https://referencesource.microsoft.com/#mscorlib/system/Runtime/InteropServices/ComEventsHelper.cs,198
