using System;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI
{
    public interface IViewFactory
    {
        void RegisterStartupPage<TViewModel, TView>() where TView : Page;

        void Register<TViewModel, TView>() where TView : FrameworkElement;

        Page GetStartupPage();

        void Show<TViewModel>(TViewModel viewModel);

        void ShowDialog<TViewModel>(TViewModel viewModel);

        void ShowDialog<TViewModel>();

        Page GetPage<TViewModel>();

        UserControl GetUserControl<TViewModel>(TViewModel viewModel);

        UserControl GetUserControl(Type viewModelType, object viewModel);
    }
}