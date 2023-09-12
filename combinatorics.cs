void Main()
{
	int comboLength = 4;
	int numberOfCombos = 2;
	int sideToStart = (int)Side.Lead;
	//totalPermutations = ((len(moves) * 2) -1)! / (((len(moves) * 2) - 1)! - 4)!...ie...roughly 212,520 unique 4 hit combos? (non-alternating)
	//EulersZigZag/André's problem...Zn = 2An...101,042 of those would be alernating with the given moveset...so a 1/101,042-th chance to properly predict the entire 4 move combo
	//is this entropy?
	List<string> moves = new() 
	{
		"Jab",
		"Jab Feint",
		"Uppercut",
		"Horizontal Elbow",
		"Upward Elbow",
		"Downward Elbow",
		"Catfish Elbow",
		"Body Kick",
		"Leg Kick",
		"Thigh Kick",
		"Feint Kick",
    //"Head Kick",
    //"...Clinch..."
		"Knee",
		"Faint Knee",
		"Faint Teep",
		"Teep"
	};

	Dictionary<int, List<string>> menu = new();
	
	for(int m = 0; m < numberOfCombos; m += 1)
	{
		bool startWithLead = (int)sideToStart == 0 ? true : false;
		Random rng = new();
		List<string> combo = new();
		for(int t = 0; t < comboLength; t += 1)
		{
			string strike = moves[rng.Next(0, moves.Count)];
			if(startWithLead && strike != moves[0])
			{
				combo.Add("Lead " + strike);
				startWithLead = !startWithLead;
			}
			else if(startWithLead && strike == moves[0] && rng.Next(0, 100) >= 75)
			{
				combo.Add(moves[0]);
				combo.Add("Flicker " + moves[0]);
				startWithLead = !startWithLead;
			}
			else if(startWithLead && strike == moves[0] && rng.Next(0, 100) < 75)
			{
				combo.Add(moves[0]);
				startWithLead = !startWithLead;
			}
			else
			{
				combo.Add(strike.Contains(moves[0]) ? "Cross" : "Rear " + strike);
				startWithLead = !startWithLead;
			}
		}
		if(!menu.Values.Any(c => c.SequenceEqual(combo)))
		{
			menu.Add(menu.Count, combo);
		}
	}
	menu.Dump();
}
// You can define other methods, fields, classes and namespaces here

enum Side
{
	Lead, Rear
}
