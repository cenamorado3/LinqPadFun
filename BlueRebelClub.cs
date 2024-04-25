//authenticates against the national weather service, retriving information with the intent of having the logic tell me if its suitable riding weather
async void Main()
{

	HttpClient _http = new();
	_http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
	_http.DefaultRequestHeaders.Add("User-Agent", "Blue Rebel Biking");
	HttpResponseMessage weatherResponse = await _http.GetAsync("https://api.weather.gov/zones/county/TNZ026/forecast");

	string msg = "";
	Forecast forecast = new();
	if(weatherResponse.IsSuccessStatusCode)
	{
	    forecast = JsonSerializer.Deserialize<Forecast>(await weatherResponse.Content.ReadAsStringAsync());
		
		foreach(Period period in forecast.Properties.Periods.Where(x => x.Name == "Today" || x.Name == "Tonight" || x.Name.Contains(DateTime.Now.AddDays(1).DayOfWeek.ToString())))
		{
			msg += $"{period.Name}: {period.Forecast}\n"; //could have a bit of fun here with NLP at a very simple level, is it sunny, clear, over 40F, then decide if its suitable riding weather...should i wear leather or mesh? inner liner or no liner? based on the weather
		}
	}
	
	msg.Dump();
	forecast.Properties.Periods.Dump();
	
}	

// You can define other methods, fields, classes and namespaces here

public class Forecast
{
	[JsonPropertyName("properties")]
	public Properties Properties {get;set;}
}

public class Properties
{
	public DateTime Updated {get;set;}
	[JsonPropertyName("periods")]
	public List<Period> Periods {get;set;}
}

public class Period
{
  [JsonPropertyName("number")]
	public int Number {get;set;}
	[JsonPropertyName("name")]
	public string Name {get;set;}
	[JsonPropertyName("detailedForecast")]
	public string Forecast {get;set;}
}
