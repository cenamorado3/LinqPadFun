void Main()
{
	string s = "string";
	List<string> guess = new List<string>() {"poss", "this", "is", "a", "list", "of", "possbilities"};
	

	
	Dictionary<int, List<string>> dict = new Dictionary<int, List<string>>()
	{
		{1, new List<string> {"s", "i"}},
		{2, new List<string> {"st", "ng"}},
		{3, new List<string> {"ing", "tri"}},
		{4, new List<string> {"ring", "stri", "root"}},
		{5, new List<string> {"strin", "gnirs"}},
		{6, new List<string> {"string", "gnirst"}}
	};
	foreach(string g in guess)
	{
		if(dict.ContainsKey(g.Length))
		{
			dict[g.Length].Add(g);
		}
		
		else
		{
			dict[g.Length] = new List<string> {g};
		}
	}
	//
	
	List<Node[]> tree = new List<Node[]>();
	foreach(int i in dict.Keys)
	{
		
		for(int q = 0; q < dict[i].Count; q += 1)
		{
			Node[] nodes = new Node[dict[i][q].Length];
			for(int c = 0; c < dict[i][q].Length; c += 1)
			{
				nodes[c] = new Node(dict[i][q][c], c == 0 ? null : dict[i][q][c - 1], c == dict[i][q].Length - 1 ? null : dict[i][q][c + 1]);
				nodes.Dump();
			}
			tree.Add(nodes);
		}
	}
	tree.Dump();
	List<string> possbilities = new List<string>();
	string input = "";
	bool autoComplete = false;
	while(!autoComplete)
	{
		input += Console.ReadLine();
		input.Dump("current text");
		
		
		//USING TREES
		foreach(Node[] trie in tree)
		{
			if(trie.Length >= input.Length && input == string.Join("", trie.Select(t => t.Value)).Substring(0, input.Length))
			{
				possbilities.Add(string.Join("", trie.Select(t => t.Value)));
				possbilities.Dump();
			}
		}
		
		////USING DICT
		//foreach(int key in dict.Keys)
		//{
		//	if(key >= input.Length)
		//	{
		//		foreach(string match in dict[key])
		//		{
		//			if(input == match.Substring(0, input.Length))
		//			{
		//				possbilities.Add(match);
		//				dict[key].Select(w => w.Substring(0, input.Length) == input).Dump();
		//			}
		//		}
		//	}
		//}
		if(possbilities.Count == 1)
		{
			input = possbilities[0];//this limits input to one of the remaining possbilities
			autoComplete = true;
		}
		if(possbilities.Count > 0)
		{
			possbilities.Clear();
		}
	}
	input.Dump("auto complete");
	//tree[19].Dump("tree");
}


record Node
(
	char Value,
	char ?Left,
	char ?Right
);
// You can define other methods, fields, classes and namespaces here
