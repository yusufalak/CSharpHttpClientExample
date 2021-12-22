namespace Commons.Exceptions
{
    [System.Serializable]
    public class ErrorCodes
    {

        private readonly string code;
        private readonly string message;
        private readonly int argumentCount;

        private ErrorCodes(string code, string message)
        {
            this.code = code;
            this.message = message;
            this.argumentCount = 0;
        }

        private ErrorCodes(string code, string message, int argumentCount)
        {
            this.code = code;
            this.message = message;
            this.argumentCount = argumentCount;
        }

        public string Code { get { return code; } }


        /// <summary> 
        /// Error explanation message in English. <br/>
        /// </summary>
        public string Message { get { return message; } }


        /// <summary> 
        /// Argument size of error explanation message. <br/>
        /// </summary>
        public int ArgumentCount { get { return argumentCount; } }


        /// <summary> 
        /// Only timeout problems will be resulted with this code. <br/>
        /// </summary>
        public static readonly ErrorCodes TIMEOUT_EXPIRED = new ErrorCodes("1000",
                "The request was not completed within the expected time interval. Please try again");

        /// <summary> 
        /// Connection reset, no route to host etc. errors will be resulted with this code. <br/>
        /// @arguments <b>error message</b> Free text error message from http connection exception. <br/>
        /// </summary>
        public static readonly ErrorCodes URL_CONNECTION_ERROR = new ErrorCodes("1001",
                "We are not able to access provider web service endpoint. Error message:{0}", 1);

        /// <summary> 
        /// <br/>
        /// @arguments <b>missing field</b>. 
        /// <br/>
        /// </summary>
        public static readonly ErrorCodes MISSING_MANDATORY_FIELD = new ErrorCodes("4000", "Missing mandatory field : {0}.", 1);

        /// <summary> 
        /// Explains a web service response payload root value is null. <br/>
        /// <b>cause</b> Free text error reason from subchannel implementation. <br/>
        /// </summary>
        public static readonly ErrorCodes SERVICE_RESPONSE_NULL = new ErrorCodes("5000",
                "Null or empty web service response received from provider web service. Cause:{0}", 1);

        /// <summary> 
        /// Explains a web service response payload contains invalid or null (empty) data. <br/>
        /// <b>cause</b> Free text error reason from subchannel implementation. <br/>
        /// </summary>
        public static readonly ErrorCodes EMPTY_OR_INVALID_DATA_RECEIVED = new ErrorCodes("5001", "Null or empty data received from provider web service. Cause:{0}",
                1);

        /// <summary> 
        /// Used for unidentified errors (or unmapped) from provider. <br/>
        /// <b>Error message</b> Free text error reason from subchannel implementation. <br/>
        /// </summary>
        public static readonly ErrorCodes UNKNOWN_ERROR = new ErrorCodes("9999", "Unmapped error has been occurred. Error message:{0} ", 1);

    }
}
