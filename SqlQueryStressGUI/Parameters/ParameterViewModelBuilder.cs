using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterViewModelBuilder
    {
        private readonly ParameterSettingsFactory _queryParameterSettingsFactory;

        public ParameterViewModelBuilder(ParameterSettingsFactory queryParameterSettingsFactory)
        {
            _queryParameterSettingsFactory = queryParameterSettingsFactory;
        }

        private static readonly Regex _paramRegex = new Regex("@\\w*");

        public void UpdateQueryParameterViewModels(string query, ref List<ParameterViewModel> viewModels)
        {
            var paramMatches = _paramRegex.Matches(query);
            var newViewModels = new List<ParameterViewModel>();

            foreach(Match match in paramMatches)
            {
                var existingViewModel = viewModels.FirstOrDefault(x => x.Name == match.Value);

                if (existingViewModel == null)
                {
                    newViewModels.Add(new ParameterViewModel(_queryParameterSettingsFactory)
                    {
                        Name = match.Value
                    });
                }
                else
                {
                    newViewModels.Add(existingViewModel);
                }
            }

            viewModels = newViewModels;
        }
    }
}
