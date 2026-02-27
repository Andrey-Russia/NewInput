
using System.Collections.Generic;

public class DataTable
{
    public List<DataEntry> Entries { get; set; }

    public DataTable(List<DataEntry> entries)
    {
        Entries = entries;
    }
}
