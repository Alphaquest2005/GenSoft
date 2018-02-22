namespace SystemInterfaces
{
    public enum RowState
    {
        Loaded, Added, Modified, Deleted,
        Unchanged
    }

    public enum ViewModelState
    {
       Initialized, NotInitialized,LoadingData,StopLoadingData
       
    }

    

    public enum EntityRelationshipOrdinality
    {
        One = 1, Many = 2
    }
}
