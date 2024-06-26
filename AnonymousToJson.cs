//This script creates a C# anonymous object with JSON serialization in mind, while deserializing the "return" object as a deep clone as Dictionary<string, Dictionary<string, object>>
//It needs refinement, though this should provide a lightweight solution similar to Pythons loads and dumps functions, which I do not currently know of a way to natively do in C#

void Main()
{
	var anon = new object[]{
		new {
			that = "is", 
			json = new string[]{"this", "is", "an", "array", "of", "objects"},
			numbers = new int[]{1,2,3,4,5},
			blah1 = new idk("blah1", 4)
		},
		new {
			str = "is", 
			strarr = new string[]{"this", "is", "an", "array", "of", "objects"},
			intarr = new int[]{1,2,3,4,5},
			blah2 = new idk("blah2", 4)
		},
		new { a = new { b = new { c = 3}}}
	};
	
	
	
	Dictionary<string, Dictionary<string, object>> d = new();
	for(int i = 0;anon.Length > i; i+=1)
	{
		string payload = JsonSerializer.Serialize(anon[i]);
		using (JsonDocument document = JsonDocument.Parse(payload))
		{
		    JsonElement root = document.RootElement;
			d.Add(i.ToString(), GetChildElement(root.EnumerateObject(), i.ToString()).First()) ;
		}
		
	}
	foreach(var kvp in ((Dictionary<string, object>)d["0"]["blah1"]))
	{
		kvp.Dump();
	}
	((Dictionary<string, object>)d["0"]["blah1"])["t"] = Convert.ToInt32(((Dictionary<string, object>)d["0"]["blah1"])["t"]) * 2;//clunky casting but we can "dot walk" to access keys, no need to itter if you know what you are looking for

	d.Dump();
}


private IEnumerable<Dictionary<string, object>> GetChildElement(IEnumerable ele, string key = null)
{
	Dictionary<string, object> child = new();
	IEnumerator itter = ele.GetEnumerator();
	while(itter.MoveNext())
	{
		if(itter.Current is JsonProperty)
		{
			JsonValueKind jprop = ((JsonProperty)itter.Current).Value.ValueKind;
			switch(jprop)
			{
				case JsonValueKind.Object:
					//the below is handling recursion edge case, consolidate later...
					string payload = JsonSerializer.Serialize(((JsonProperty)itter.Current).Value);
					child.Add(((JsonProperty)itter.Current).Name, ParseDoc(payload, ((JsonProperty)itter.Current).Name).First());
				break;
				case JsonValueKind.String:
					child.Add(((JsonProperty)itter.Current).Name, ((JsonProperty)itter.Current).Value.ToString());
				break;
				case JsonValueKind.Number:
					if (!child.ContainsKey(key))
					{
						child.Add(((JsonProperty)itter.Current).Name, ((JsonProperty)itter.Current).Value.GetInt32());
					}
					
					else if (child.ContainsKey(key))
					{
						child[key] += ((JsonProperty)itter.Current).Value.ToString();
					}
				break;
				case JsonValueKind.Array:
						JsonValueKind arrType = ((JsonProperty)itter.Current).Value[0].ValueKind;
							switch(arrType)
							{
								case JsonValueKind.String:
									IEnumerable arrEnumS = ((JsonProperty)itter.Current).Value.EnumerateArray();
									IEnumerator arrItterS = arrEnumS.GetEnumerator();
									List<string> s = new();
									while(arrItterS.MoveNext())
									{
										s.Add(arrItterS.Current.ToString());
									}
									child.Add(((JsonProperty)itter.Current).Name, s.ToArray());
								break;
								case JsonValueKind.Number:
									IEnumerable arrEnumI = ((JsonProperty)itter.Current).Value.EnumerateArray();
									IEnumerator arrItterI = arrEnumI.GetEnumerator();
									List<int> i = new();
									while(arrItterI.MoveNext())
									{
										i.Add(((JsonElement)arrItterI.Current).GetInt32());
									}
									child.Add(((JsonProperty)itter.Current).Name, i.ToArray());
								break;
								default:
									child.Add(((JsonProperty)itter.Current).Name, new string[] {((JsonProperty)itter.Current).Value.ToString()}); //perform safe cast via EnumerateArray ?
								break;
							}
					break;
				default:
					((JsonProperty)itter.Current).Value.Dump(">>>>>>>>>>>>>>>>>>>UNDEFINED<<<<<<<<<<<<<<");
					child.Add(((JsonProperty)itter.Current).Name, JsonValueKind.Undefined);
				break;
			}
		}
	}
	
	//if(key is not null)
	//{
	//	d.Add(key, child);
	//}
	yield return child;
}
// You can define other methods, fields, classes and namespaces here
public record idk
(
	string random,
	int t
);


IEnumerable<Dictionary<string, object>> ParseDoc(string payload, string key = "0")
{
	using (JsonDocument document = JsonDocument.Parse(payload))
	{
	    JsonElement root = document.RootElement;
		yield return GetChildElement(root.EnumerateObject(), key).First();
	}
}
