using SqlQueryStressGUI.ViewModels;
using SqlQueryStressGUI.Views.ParameterSettings;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlQueryStressGUI
{
    public class QueryParameterViewModelBuilder
    {
        private readonly QueryParameterSettingsFactory _queryParameterSettingsFactory;
        private readonly QueryParameterSettingsViewFactory _queryParameterSettingsViewFactory;

        public QueryParameterViewModelBuilder(
            QueryParameterSettingsFactory queryParameterSettingsFactory,
            QueryParameterSettingsViewFactory queryParameterSettingsViewFactory)
        {
            _queryParameterSettingsFactory = queryParameterSettingsFactory;
            _queryParameterSettingsViewFactory = queryParameterSettingsViewFactory;
        }

        private static readonly Regex _paramRegex = new Regex("@\\w*");

        public void UpdateQueryParameterViewModels(string query, ref List<QueryParameterViewModel> viewModels)
        {
            var paramMatches = _paramRegex.Matches(query);
            var newViewModels = new List<QueryParameterViewModel>();

            foreach(Match match in paramMatches)
            {
                var existingViewModel = viewModels.FirstOrDefault(x => x.Name == match.Value);

                if (existingViewModel == null)
                {
                    newViewModels.Add(new QueryParameterViewModel(_queryParameterSettingsFactory, _queryParameterSettingsViewFactory)
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
