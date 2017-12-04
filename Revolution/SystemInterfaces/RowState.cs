namespace SystemInterfaces
{
    public enum RowState
    {
        Loaded, Added, Modified, Deleted,
        Unchanged
    }

    public enum ViewModelState
    {
       Intialized, NotIntialized
    }

    

    public enum EntityRelationshipOrdinality
    {
        One = 1, Many = 2
    }
}
