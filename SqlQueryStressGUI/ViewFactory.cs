﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI
{
    public sealed class ViewFactory : IViewFactory
    {
        private readonly Dictionary<Type, Type> _viewModelMap;
        private KeyValuePair<Type, Type> _startupPage;

        public ViewFactory()
        {
            _viewModelMap = new Dictionary<Type, Type>();
        }

        public void RegisterStartupPage<TViewModel, TView>() where TView : Page
        {
            _startupPage = new KeyValuePair<Type, Type>(typeof(TViewModel), typeof(TView));
        }

        public Page GetStartupPage()
        {
            if(_startupPage.Equals(default(KeyValuePair<Type, Type>)))
            {
                throw new InvalidOperationException("Startup page not registered.");
            }

            var viewModel = DiContainer.Instance.ServiceProvider.GetRequiredService(_startupPage.Key);
            var page = (Page)DiContainer.Instance.ServiceProvider.GetRequiredService(_startupPage.Value);
            page.DataContext = viewModel;

            return page;
        }

        public void Show<TViewModel>(TViewModel viewModel) => GetWindow(viewModel).Show();

        public void ShowDialog<TViewModel>(TViewModel viewModel) => GetWindow(viewModel).ShowDialog();

        public void ShowDialog<TViewModel>()
        {
            var viewModel = DiContainer.Instance.ServiceProvider.GetRequiredService<TViewModel>();
            GetWindow(viewModel).ShowDialog();
        }

        private Window GetWindow<TViewModel>(TViewModel viewModel) => (Window)GetFrameworkElement(viewModel);

        public UserControl GetUserControl<TViewModel>(TViewModel viewModel) => (UserControl)GetFrameworkElement(viewModel);

        public Page GetPage<TViewModel>()
        {
            var viewModel = DiContainer.Instance.ServiceProvider.GetRequiredService<TViewModel>();
            return (Page)GetFrameworkElement(viewModel);
        }

        public UserControl GetUserControl(Type viewModelType, object viewModel) => (UserControl)GetFrameworkElement(viewModelType, viewModel);

        public void Register<TViewModel, TView>() where TView : FrameworkElement
        {
            _viewModelMap.Add(typeof(TViewModel), typeof(TView));
        }

        private object GetFrameworkElement(Type viewModelType, object viewModel)
        {
            return GetViewInstance(viewModelType, viewModel);
        }

        private object GetFrameworkElement<TViewModel>(TViewModel viewModel)
        {
            return GetViewInstance(typeof(TViewModel), viewModel);
        }

        private object GetViewInstance(Type viewModelType, object viewModel)
        {
            if (_viewModelMap.TryGetValue(viewModelType, out Type viewType))
            {
                var view = (FrameworkElement)DiContainer.Instance.ServiceProvider.GetRequiredService(viewType);
                view.DataContext = viewModel;
                return view;
            }
            else
            {
                throw new InvalidOperationException($"No view registered for view model type {viewModelType.Name}");
            }
        }
    }
}
