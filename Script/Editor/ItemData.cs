using System;

[Serializable]
public class ItemData
{
	public int ID;

	public int ParentID = -1;

	public int Index;

	public ItemData(int id, int index)
	{
		ID = id;
		Index = index;
	}
}
