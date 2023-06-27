void Main()
{
	Provider _provider = new();
	for(int i = 0; i < 5; i += 1)
	{
		Observer subscriber = new(i);
		subscriber.Subscribe(_provider);
	}
	_provider.Subscriptions.Dump();
	
	
	for(int i = 0; i < _provider.Subscriptions.Count; i += 1)
	{
		if((i + 1) % 2 == 0)
		{
			_provider.Subscriptions[i].Unsubscribe();
		}
	}
	_provider.Subscriptions.Dump();
	

	int c = 0;
	while(c < 2)
	{
		for(int i = 0; i < _provider.Subscriptions.Count; i += 1)
		{
			_provider.Subscriptions[i].OnNext(DateTime.Now.ToString());
		}
		c+=1;
	}
	
	_provider.Subscriptions[0].OnError(new TargetInvocationException(new ArgumentException("Strong, fruit of weak. Weak, seed of strong."))); ////https://youtu.be/m0bBBKoc_bg?t=1017
	
}

// You can define other methods, fields, classes and namespaces here
class Provider : IObservable<string> 
{
   	public Provider()
   	{
		Subscriptions = new();
   	}
	
	public List<Observer> Subscriptions {get;set;}
	private Observer _subscriber;
	
	public IDisposable Subscribe(IObserver<string> subscriber)
	{
		
		if (!Subscriptions.Contains(subscriber))
		{
			_subscriber = (Observer)subscriber;//must cast, safely, for a proper implementation.
			Subscriptions.Add(_subscriber);
		}

		return new Unsubscriber(Subscriptions, _subscriber);
	}
	
	protected sealed class Unsubscriber : IDisposable 
	{
		private List<Observer> _Subscriptions;
		private Observer _subscriber;

		public Unsubscriber(List<Observer> Subscriptions, Observer subscriber)
		{
			_Subscriptions = Subscriptions;
			_subscriber = subscriber;
		}

		public void Dispose()
		{
			if(_subscriber != null)
			{
				_Subscriptions.Remove((Observer)_subscriber);
			}
		}
	}
}

class Observer : IObserver<string>
{
	public Observer(int? priority)
	{
		if (priority.HasValue)
			_priority = priority.Value;
		else
			priority = int.MaxValue;
	}
	
	private IDisposable _unsubscriber;
  	private int _priority;
	private bool _done = false;
	private Guid _id = Guid.NewGuid();

	public virtual void Subscribe(IObservable<string> provider)
	{
	  	_unsubscriber = provider.Subscribe(this);
	}
	
	public virtual void Unsubscribe()
	{
		$"{_id} unsubscribed".Dump();
		_unsubscriber.Dispose();
	}
	
	public virtual void OnCompleted()
	{
		if (_done)
			$"\nvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv\n>{_id} completed with priority {_priority}<\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n".Dump();
	}
	
	public virtual void OnError(Exception ex)
	{
		ex.Dump("WYSIWYG");
		if (!_done)
			Unsubscribe();
	}

	public virtual void OnNext(string msg)
	{
		_priority -= 3;//thats odd
		$"Current priority of {_id} is {_priority}".Dump();
		if(_priority < 0 && !_done)
		{
			OnError(new UnsatisfiedException(_id.ToString()));
		}
		
		if(_priority == 0)
		{
			_done = true;
			OnCompleted();
		}
	}
}

class UnsatisfiedException : Exception
{
	public UnsatisfiedException(string message): base(message)
	{}
}
