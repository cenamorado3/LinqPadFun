//https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/saga/saga
//is this right?
Atomic atom = new();
///this function is severly abstracted
void Saga()//invocation
{
	if(atom.balance == 33)
	{ 
		atom.balance = 0; //reset the balance if its 33, the compensating transaction, which undoes the work, i.e "send it back, cancel the order, refund, do not proceed"
	}
	else
	{
		//GIVE ME MY QUANTUM
		//https://referencesource.microsoft.com/#System.Core/system/threading/ReaderWriterLockSlim/ReaderWriterLockSlim.cs,1666
		atom.balance += 1;//otherwise, perform the transaction, increment the balance. "service completed, process payment, send to shipping"
	}
}
async void Main()
{
	atom.Update += Saga;
	for (int t = 0; t < 500; t+=1)
	{
		await atom.Work();
	}
}
class Atomic
{
	async public Task Work()
	{
        var tasks = new Task[99];
        for (int i = 0; i < tasks.Length; i+=3) 
        {
            tasks[i] = DoA();
			tasks[i+1] = DoB();
			tasks[i+2] = DoC();
        }
        await Task.WhenAll(tasks);
		
	}
	public event Action Update;
	public void fire() => Update?.Invoke(); //invoke the delegate when fired, during the call to DoC
	int bal = 0;
	public int balance
	{
		get
		{
			return bal;
		}
		set
		{
			bal = value;
		}
	}

	
	public Task<List<string>> DoA()
	{
		return Task.Run( () =>
		{
			return new List<string>() { "IDK" };
		} );
	}

	public Task<List<string>> DoB()
	{
		return Task.Run( () =>
		{
			return new List<string>() { "IDK" };
		} );
	}

	public Task<List<string>> DoC()
	{
		return Task.Run( () =>
		{
			fire(); //Invoke
			return new List<string>() { "IDK" };
		} );
	}
}
