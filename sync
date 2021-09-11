//synschonization of task/threads with locks, SAGA design pattern?
async void Main()
{
    for (int t = 0; t < 50000; t+=1)
    {
        var tasks = new Task[99];
        for (int i = 0; i < tasks.Length; i+=3)
        {
            tasks[i] = DoA();
			tasks[i+1] = DoB();
			tasks[i+2] = DoC();
        }
        await Task.WhenAll(tasks);
		if(bal != 33) bal.Dump(); //<----this will only occur when the value is not correct for a single unit of work
		bal = 0;
	}
}
object Lock = new object();
int bal = 0;

//this function is called everytime "DoC" has completed, signaling that the inner for loop, i, representing the work, composed of, in this case, 99 task.
//The outer loop, t, represents an epoch, the completion of one unit of work,
//abstractly this could signal the succes or failure of any action or event
void update()
{
//what happens if you remove this lock?
	lock( Lock )
	{
		bal += 1;
	}
//after seeing the results, consider the frequency of updates, add a few figures to the t
}
// You can define other methods, fields, classes and namespaces here
public Task<List<string>> DoA()
{
	lock( Lock )
	{
		return Task.Run( () =>
		{
			return new List<string>() { "TODO" };
		} );
	}
}
public Task<List<string>> DoB()
{
	lock( Lock )
	{
		return Task.Run( () =>
		{
			return new List<string>() { "TODO" };
		} );
	}
}
public Task<List<string>> DoC()
{
	lock( Lock )
	{
		return Task.Run( () =>
		{
			update();
			return new List<string>() { "TODO" };
		} );
	}
}
