using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterViewModelBuilder
    {
        private readonly ParameterSettingsViewModelBuilder _parameterSettingsViewModelBuilder;

        public ParameterViewModelBuilder(ParameterSettingsViewModelBuilder parameterSettingsViewModelBuilder)
        {
            _parameterSettingsViewModelBuilder = parameterSettingsViewModelBuilder;
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
                    var paramViewModel = new ParameterViewModel(match.Value, _parameterSettingsViewModelBuilder);

                    newViewModels.Add(paramViewModel);
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
