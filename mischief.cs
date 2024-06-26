//every vision, lies behind a door.
//just as every lock, has a key.
//yet there is no value, in this meaning.
void Main()
{
	ClassA aLock = new();
	aLock.secret = aLock.Encrypt("ClassA UNENC");
	ClassAB abLock = new();
	abLock.secret = abLock.Encrypt("ClassAB UNENC");
	ClassC cLock = new();
	cLock.secret = cLock.Encrypt("ClassC");
	
	
	IDecrpyt<ClassA> aKey = new ClassB();
	aKey.CastToT(aLock).secret.Dump(aLock.secret);
	IDecrpyt<ClassAB> abKey = new ClassB();
	abKey.CastToT(abLock).secret.Dump(abLock.secret);
	IDecrpyt<ClassC> cKey = new ClassBC();
	cKey.CastToT(cLock).secret.Dump(cLock.secret);
}

// You can define other methods, fields, classes and namespaces here


interface IEncrypt <out T>
{
	public string secret {get;set;}
	public string Encrypt(string s);
}

class ClassA : IEncrypt<ClassA>
{
	public string secret {get;set;}

	
	public string Encrypt(string s)
	{
		return Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
	}
}

class ClassAB : ClassA
{
}







class ClassC :  IEncrypt<ClassC>
{
	public string secret {get;set;}
	public string Encrypt(string s)
	{
		return s + "*salkdfhaskhdflaksdf";
	}
}




interface IDecrpyt <in T>
{
	public string secret {get;set;}
	public ClassB CastToT(T a);
	public string Decrypt(string s);
}
class ClassB : IDecrpyt<ClassA>
{
	public string secret {get;set;} = "ClassB";
	public ClassB CastToT(ClassA a)
	{
		ClassB aa = new();
		aa.secret = Decrypt(a.secret);
		return aa;
	}
	public string Decrypt(string s)
	{
		return Encoding.ASCII.GetString(Convert.FromBase64String(s));
	}
}

class ClassBC : IDecrpyt<ClassC>
{
	public string secret {get;set;} = "ClassB";
	public ClassB CastToT(ClassC c)
	{
		ClassB aa = new();
		aa.secret = Decrypt(c.secret);
		return aa;
	}
	public string Decrypt(string s)
	{
		return s.Split('*')[0];
	}
}
