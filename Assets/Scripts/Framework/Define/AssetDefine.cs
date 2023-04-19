namespace GreyFramework
{
    public enum eAssetMode
    {
        Editor,
        Mobile,
    }
    public enum eLoadState
    {
        Invalid = -1,
        Loading = 0,
        LoadComplete = 1,
        Unloaded = 2,
        LoadError = 3,
    }
}
