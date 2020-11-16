namespace YoYo.Model
{
    public class Result<T>
    {
        public T Data { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}
