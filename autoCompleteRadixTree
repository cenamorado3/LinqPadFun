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
	
	List<Node> tree = new List<Node>();
	foreach(int i in dict.Keys)
	{
		for(int q = 0; q < dict[i].Count; q += 1)
		{
			tree.Add(BuildTreeFromString(dict[i][q]));
		}
	}
	//printInorder(tree.Where(t => t.Value == 'g').ToList()[0]).Dump();
	//
	List<Node> possbilities = new List<Node>();
	string input = "";
	bool autoComplete = false;
	while(!autoComplete)
	{
		input += Console.ReadLine();
		input.Dump("current text");
		//USING TREES
			if(tree.Where(l => printInorder(l).Length >= input.Length).Any(t => input == printInorder(t).Substring(0, input.Length)))
			{
				possbilities.AddRange(tree.Where(t => input == printInorder(t).Substring(0, input.Length)).ToList());// == input.Substring(0,  printInorder(t).Length)).ToList().Dump();
			}
possbilities.Count.Dump();
		if(possbilities.Count == 1)
		{
			input = printInorder(possbilities[0]);//this limits input to one of the remaining possbilities
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


public record Node
{
	public char ?Value {get;set;}
	public Node Left {get;set;}
	public Node Right {get;set;}
};
// You can define other methods, fields, classes and namespaces here
Node BuildTreeFromString(string s)
{
	Node root = new Node{Value = s[0], Left = null, Right = null};
	Node prev = new Node{Value = null, Left = null, Right = null};
	for(int i = s.Length -1; i >= 0; i -= 1)
	{
		if(i == 0)
		{
			root.Left = null;
		}
		

		else
		{
			prev = root.Right;
			root.Right = new Node{Value = s[i], Left = null, Right = prev};
		}
	}
	return root;
}


string printInorder(Node node)
{
	string s = "";
    if (node == null)
        return s;

    /* first recur on left child */
    s += printInorder(node.Left);

    /* then print the data of node */
    s+= node.Value;

    /* now recur on right child */
    s += printInorder(node.Right);
	return s;
}
