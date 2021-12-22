using System.Text;

namespace Commons.Exceptions
{
    [System.Serializable]
    public class SubChannelException : Exception
    {
        private readonly ErrorCodes error;
        private readonly List<SingleMessageArgument> arguments;

        public SubChannelException(ErrorCodes error) : base(error.Message)
        {
            this.error = error;
            this.arguments = new List<SingleMessageArgument>();
        }

        public SubChannelException(ErrorCodes error, params string[] args) : base(error.Message)
        {
            this.error = error;
            this.arguments = new List<SingleMessageArgument>();
            if (args.Length != this.error.ArgumentCount)
            {
                throw new ArgumentException("Defined error code argument size can not be different than args size.");
            }

            AddArgument(args);
        }

        public ErrorCodes ErrorCode { get { return error; } }
        public List<SingleMessageArgument> Arguments { get { return arguments; } }
        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
        public string GetMessageFormat
        {
            get { return error.Message; }
        }
        public override string Message
        {
            get
            {
                StringBuilder builder = new StringBuilder(this.error.Code).Append(":");
                if (this.arguments != null && this.arguments.Count > 0)
                {
                    foreach (var singleMessageArgument in this.arguments)
                    {
                        builder.Append(String.Format(this.error.Message, singleMessageArgument.InnerArguments)).AppendLine();
                    }
                }
                else
                {
                    builder.Append(this.error.Message);
                }
                return builder.ToString();
            }
        }

        public void AddArgument(params string[] args)
        {
            if (args.Length != this.error.ArgumentCount)
            {
                throw new ArgumentException("Defined error code argument size can not be different than args size.");
            }
            this.arguments.Add(new SingleMessageArgument(args));
        }

    }
}
