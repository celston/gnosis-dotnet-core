namespace Gnosis.Data
{
    public class SelectQueryMultipleTablesException : GException
    {
        public SelectQueryMultipleTablesException(string newTable, string oldTable)
            : base("Cannot add table {0}, {1} already set as primary", newTable, oldTable)
        {
        }   
    }

    public class SelectQueryEmptyTablesOnJoinException : GException
    {
        public SelectQueryEmptyTablesOnJoinException(string table)
            : base("Cannot join {0}, no primary table set")
        {
        }
    }
}