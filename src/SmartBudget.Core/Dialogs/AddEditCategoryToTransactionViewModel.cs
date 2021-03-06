﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SmartBudget.Core.Dialogs
{
    public class AddEditCategoryToTransactionViewModel : BindableBase, IDialogAware
    {
        private readonly ICategoryService _categoryService;

        private int _categoryId;

        public int CategoryId
        {
            get { return _categoryId; }
            set { SetProperty(ref _categoryId, value); }
        }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public DelegateCommand SaveDialogCommand { get; }
        public DelegateCommand CancelDialogCommand { get; }

        public string Title => "Add Category";

        public event Action<IDialogResult> RequestClose;

        public AddEditCategoryToTransactionViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            Categories = new ObservableCollection<Category>();

            SaveDialogCommand = new DelegateCommand(SaveDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);
        }

        private void SaveDialog()
        {
            var result = ButtonResult.OK;

            var p = new DialogParameters
            {
                { "categoryid", CategoryId },
                { "amount", Amount }
            };

            RequestClose?.Invoke(new DialogResult(result, p));
        }

        private void CancelDialog()
        {
            var result = ButtonResult.Cancel;

            RequestClose?.Invoke(new DialogResult(result));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var categoryId = parameters.GetValue<int>("categoryid");
            var amount = parameters.GetValue<decimal>("amount");

            CategoryId = categoryId;
            await GetCategories();
            Amount = amount;
        }

        public async Task GetCategories()
        {
            var categories = await _categoryService.GetAll();

            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }
    }
}