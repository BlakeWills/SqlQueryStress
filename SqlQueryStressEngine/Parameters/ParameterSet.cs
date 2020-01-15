using System.Collections.Generic;

namespace SqlQueryStressEngine.Parameters
{
    // TODO: This should be a collection type (if we keep it)
    public class ParameterSet
    {
        private List<ParameterValue> _parameters = new List<ParameterValue>();

        public IEnumerable<ParameterValue> Parameters
        {
            get => _parameters.ToArray();
        }

        public void Add(ParameterValue paramValue) => _parameters.Add(paramValue);
    }
}
