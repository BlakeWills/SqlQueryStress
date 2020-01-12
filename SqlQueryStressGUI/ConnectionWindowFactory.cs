using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressGUI.ViewModels;
using SqlQueryStressGUI.Views;

namespace SqlQueryStressGUI
{
    public class ConnectionWindowFactory
    {
        public ConnectionWindow Build(AddEditConnectionViewModel addEditConnectionViewModel = null)
        {
            if(addEditConnectionViewModel == null)
            {
                addEditConnectionViewModel = DiContainer.Instance.ServiceProvider.GetRequiredService<AddEditConnectionViewModel>();
            }

            return new ConnectionWindow(addEditConnectionViewModel);
        }
    }
}
