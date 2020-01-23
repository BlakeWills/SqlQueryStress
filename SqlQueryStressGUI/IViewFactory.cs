using System;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>() where TView : FrameworkElement;

        void Show<TViewModel>(TViewModel viewModel);

        void ShowDialog<TViewModel>(TViewModel viewModel);

        void ShowDialog<TViewModel>();

        UserControl GetUserControl<TViewModel>(TViewModel viewModel);

        UserControl GetUserControl(Type viewModelType, object viewModel);
    }
}