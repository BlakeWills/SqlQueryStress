using System.Collections.Generic;

namespace SqlQueryStressEngine.Parameters
{
    public interface IParameterProvider
    {
        /// <summary>
        /// Returns a set of parameters for a single query execution.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParameterSet> GetParameterSets(IEnumerable<IParameterValueBuilder> parameterBuilders, int numberOfSets);
    }
}
