using System.Collections.Generic;

namespace SqlQueryStressEngine.Parameters
{
    public class ParameterProvider : IParameterProvider
    {
        public IEnumerable<ParameterSet> GetParameterSets(IEnumerable<IParameterValueBuilder> parameterBuilders, int numberOfSets)
        {
            var sets = new List<ParameterSet>();
            for (int i = 0; i < numberOfSets; i++)
            {
                var set = new ParameterSet();
                foreach (var builder in parameterBuilders)
                {
                    set.Add(builder.GetNextValue());
                }

                sets.Add(set);
            }

            return sets;
        }
    }
}
