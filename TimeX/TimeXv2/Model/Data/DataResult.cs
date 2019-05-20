namespace TimeXv2.Model.Data
{
    public class DataResult<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
