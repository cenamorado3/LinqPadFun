async void Main()
{

	HttpClient _http = new();

	LogDispatcher _dispatch = new(_http);

		
	ILogger _logger = null;//ILogger represents an empty service container for implementation registrations
	ILogger _jsonLogger = new JsonLogger();
	_logger = _jsonLogger;//represents IServiceProvider.GetService(type) via a "ptr" switch 
	_dispatch.Status += () => //the delegate to subscribe to for the current status of the logs via a getter of reports;
	{
		_dispatch.Storage.Add(_logger, _logger.Report());
	};
	
	
	//represents the call from anywhere/an endpoint
	for(int i = 0; i < 11; i += 1)
	{
		_logger.Log(LogLevel.WARNING, new Exception("Json warning " + i.ToString()), typeof(JsonLog));
	}
	await _dispatch.InvokeAsync();//invoke the delegate on line 9, whose work of reporting is done on a seperate thread, in other words
	//prior to this point the dispatcher was unaware of the logs which the ILogger gathered, they are now being reported to the dispatcher
	
	
	
	ILogger _objectLogger = new ObjectLogger();
	_logger = _objectLogger;//polymorphism in place of dep inj

	for(int i = 0; i < 11; i += 1)
	{
		_logger.Log(LogLevel.WARNING, new Exception("Object warning " + i.ToString()), typeof(Log));
	}
	
	
	
	await _dispatch.InvokeAsync();
	_logger = _jsonLogger;//we want the "json logs" in this case
	//_dispatch.ELKPush(_logger);//the actual push to elastic

	_dispatch.Status -= (() =>{});//unsub
	_dispatch.Storage[_logger].Dump();
}

public void Status(LogDispatcher _dispatch, ILogger _logger)
{
	_dispatch.Storage.Add(_logger, _logger.Report());
}

// You can define other methods, fields, classes and namespaces here
public class LogDispatcher
{
	public LogDispatcher(HttpClient http)
	{
		_http = http;
	}
	
	~LogDispatcher()
	{
		Status = null;//force unsub.
		_http.Dispose();//only the dispatcher will have access to the caller. The application should take care of disposal in a genuine enviornment
	}

	private readonly HttpClient _http;
	public event Action? Status;
	private void Report() => Status?.Invoke();

	public Dictionary<ILogger, LogStore> Storage {get; private set;} = new();
	
	
	public Task InvokeAsync()
	{
		return Task.Run(() =>
		{
			Report();
		});
	}
	
	public async void ELKPush(ILogger _logger)//will only push the logs for the respective ILogger based on the last invocation, it is possible to push all
	{
		Storage.TryGetValue(_logger, out LogStore stack);
		StringBuilder sb = new();
		string create = "{ \"create\":{ } }";
		Dictionary<string, string> contents = new();
		foreach(Log log in stack.Logs)
		{
			sb.Append(create);
			sb.AppendLine();
			sb.Append(JsonSerializer.Serialize(log));
			sb.AppendLine();
			//formatting the body for elastic _bulk consumption/ep
		}
		StringContent data = new (sb.ToString() +"\n", Encoding.UTF8, "application/json");
		//kibana endpoint
		string kib = "YOUR KIBANA EP";
		//elastic endpoint
		string el = "YOUR ELASTIC EP";
		string ep = "/your index/_bulk";//_doc or _bulk, _bulk insert to mitagate overhead/network cost/bandwidth, _doc for a single push
		_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", "YOUR KIBANA API KEY NOT ELASTIC"; //doesnt make sense to me either
		//"ApiKey {token} is the schema
		
		HttpResponseMessage response = await _http.PostAsync(el + ep, data); //yes, the elastic ep with kibana token
		await response.Content.ReadAsStringAsync().Dump();
	}
}

public class LogStore
{

	public Stack<Log> Logs{get;set;} = new(); //stack up
	
}

public enum LogLevel
{
	WARNING = 3
}

#nullable enable


public class JsonLogger : ILogger
{

	public JsonLogger()
	{
		Logs = new();
	}
	private LogStore Logs{get;set;}

	public void Log<TState>(LogLevel logLevel, Exception? ex)  where TState : Log, new()//mirror with clause: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger.log?view=dotnet-plat-ext-7.0#microsoft-extensions-logging-ilogger-log-1(microsoft-extensions-logging-loglevel-microsoft-extensions-logging-eventid-0-system-exception-system-func((-0-system-exception-system-string))) 
	{
		TState state = new TState();
		JsonSerializer.Serialize(state).Dump();
	}
	
	public void Log(LogLevel logLevel, Exception? ex, Type logType) //simplified
	{
		JsonLog log = new JsonLog();
		log.CallingObject = this.GetType().Name;
		log.CallingMethod = new StackTrace().GetFrames()[20].GetMethod().Name;//this is hacky
		log.Exception = ex;
		Logs.Logs.Push(log);
	}
	
	public LogStore Report()
	{
		return Logs;
	}
}


public class ObjectLogger : ILogger
{
	public ObjectLogger()
	{
		Logs = new();
	}
	private LogStore Logs{get;set;}
	
	public void Log<TState>(LogLevel logLevel, Exception? ex)  where TState : Log, new()
	{
		TState state = new TState();
		state.Dump();
	}
	
	public void Log(LogLevel logLevel, Exception? ex, Type logType)
	{
		Log log = (Log)Activator.CreateInstance(logType);
		log.CallingObject = this.GetType().Name;
		log.CallingMethod = new StackTrace().GetFrames()[10].GetMethod().Name;//this is hacky and probably best to just pass the whole trace
		log.Exception = ex;
		Logs.Logs.Push(log);
	}
	
	public LogStore Report()
	{
		return Logs;
	}
}


public interface ILogger
{
	public void Log<TState>(LogLevel logLevel, Exception? ex) where TState : Log, new();
	public void Log(LogLevel logLevel, Exception? ex, Type logType);
	public LogStore Report();
}

public class Log
{
	public Log()
	{
		LID = Guid.NewGuid();//https://www.youtube.com/watch?v=xrhkfADEtMU
	}
	public Guid LID {get;set;}
	public string CallingObject{get;set;}
	public string CallingMethod{get;set;}
	public Exception? Exception {get;set;}
}
public class JsonLog : Log
{
	public JsonLog()
	{
	}
}
#nullable disable
