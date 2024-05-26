//coding is a reflection of your logic, and your logic is an expression
void Main()
{

	int beats = 40;//number of beats/notes in the lick...a large number will give you more ideas to play with
	string[,] lick = GenerateMeasures(beats);
	
	
	Random rnd = new();
	int pickAString = rnd.Next(0, 7);
	int pickANote = rnd.Next(5, 8);//where do you want to be on the fretboard...if youre smart youll know what key it is, i dont
	
	int stringDeviation = 2;//for string skipping https://www.youtube.com/watch?v=oKvIAuXMl7U
	int noteDeviation = 5;
	//basing it off the dominant degree
	
	string previousNote = "";
	for(int i = 0; i < beats; i += 1)
	{
		if (i == 0)//tonic note
		{
			lick[pickAString, i] = pickANote.ToString();
			previousNote = lick[pickAString, i];
		}
		
		else
		{
			try
			{
				pickAString = H0(stringDeviation, pickAString, previousNote, noteDeviation, rnd);
				lick[pickAString, i] = Note(noteDeviation, i, previousNote, rnd);
				previousNote = lick[pickAString, i];
			}
			catch(IndexOutOfRangeException e)
			{
				lick[0,i] = "0";//chug
				//lick[0,i] = pickANote.ToString(); //return to tonic
				previousNote = pickANote.ToString();
				pickAString = 3;
			}
		}
		
	}
	
	lick.Dump();
	
	
	
}

// You can define other methods, fields, classes and namespaces here
private int H0(int stringDeviation, int prevString, string previousNote, int noteDeviation, Random rnd)
{
	int s = rnd.Next(prevString - stringDeviation, prevString + stringDeviation);
	if(int.Parse(previousNote) % 8 != 0)//play with this, in mcase im sticking with mods on 5 and 8 to mess with fifths and octaves
	{
		return s; //+ or - 1 will here will push your strings up or down which can crete some interesting shapes or resets
	}
	else
	{
		return int.MaxValue;
	}
}

private string Note(int noteDeviation, int i, string previousNote, Random rnd)
{
	try
	{
		int note = rnd.Next(int.Parse(previousNote) - noteDeviation, int.Parse(previousNote) + noteDeviation);
		if(note < 0)
		{
			return "0";//if we hit negatives, just chug, https://www.youtube.com/watch?v=NhZgR1aT8D8&list=PLXMwtXJnJ93ge3F-ATMTn_wQQo1_f6opr&index=31
		}
		
		if(note > noteDeviation * 4)//this is acting as a mathematical ceiling/fret limit, essentially if we get too wild, we will stay above this fret, ie 20 here, or hard code it
		{
			return previousNote;//with the cap in place, the next note is up in the air, will either ascend or descend 
		}
		return note.ToString();//intervals
	}
	catch(IndexOutOfRangeException e)
	{
		return previousNote;
	}
}

private string[,] GenerateMeasures(int beats)
{
	string[,] measures = new string[8, beats];
	for(int s = 0; s < 8; s += 1)
	{
		for(int i = 0; i < beats; i += 1)
		{
			measures[s, i] = "";
		}
	}
	return measures;
}
