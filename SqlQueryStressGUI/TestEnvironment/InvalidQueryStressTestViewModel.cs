using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class InvalidQueryStressTestViewModel : ViewModel
    {
        public InvalidQueryStressTestViewModel()
        {
            CloseCommand = new CommandHandler((window) => Close((ICloseable)window));
        }

        public CommandHandler CloseCommand { get; }
        
        public QueryStressTestValidationResult ValidationResult { get; set; }

        public string Message { get => GetMessage(); }

        private string GetMessage()
        {
            return string.Join(Environment.NewLine, GetInvalidProperties());
        }

        private IEnumerable<string> GetInvalidProperties()
        {
            if (!ValidationResult.ThreadCountValid) { yield return "Invalid Thread Count"; }
            if (!ValidationResult.IterationsValid) { yield return "Invalid Iterations"; }
            if (!ValidationResult.ConnectionValid) { yield return "Invalid Connection"; }
            if (!ValidationResult.QueryParametersValid) { yield return "Invalid Query Parameters"; }
            if (!ValidationResult.QueryValid) { yield return "Invalid Query"; }
        }
    }
}
