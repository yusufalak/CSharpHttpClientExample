namespace Commons.Exceptions
{
    [System.Serializable]
    public class SingleMessageArgument
    {
        public string[] InnerArguments { get; set; }
        public SingleMessageArgument(string[] innerArguments)
        {
            this.InnerArguments = innerArguments;
        }
    }
}
