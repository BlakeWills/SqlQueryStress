using SqlQueryStressGUI.ViewModels;
using SqlQueryStressGUI.Views;
using System.Collections.Generic;

namespace SqlQueryStressGUI
{
    public sealed class ParameterWindowBuilder
    {
        public ParameterWindow Build(IEnumerable<QueryParameterViewModel> queryParameterViewModels)
        {
            var managerViewModel = new QueryParameterManagerViewModel(queryParameterViewModels);
            return new ParameterWindow(managerViewModel);
        }
    }
}
