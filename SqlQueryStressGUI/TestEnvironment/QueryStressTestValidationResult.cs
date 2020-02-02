namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryStressTestValidationResult
    {
        public bool ThreadCountValid { get; set; }

        public bool IterationsValid { get; set; }

        public bool ConnectionValid { get; set; }

        public bool QueryParametersValid { get; set; }

        public bool QueryValid { get; set; }

        public bool IsTestValid()
        {
            return ThreadCountValid && IterationsValid && ConnectionValid && QueryParametersValid && QueryValid;
        }
    }
}
