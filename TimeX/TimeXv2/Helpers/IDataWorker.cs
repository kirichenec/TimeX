namespace TimeXv2.Helpers
{
    interface IDataWorker
    {
        T Load<T>(string path);
        bool Save<T>(T data, string path);
    }
}
