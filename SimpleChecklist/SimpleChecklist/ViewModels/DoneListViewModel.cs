﻿using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
{
    public class DoneListViewModel : Screen
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly TaskListObservableCollection _taskList;

        public DoneListViewModel(DoneListObservableCollection doneList, TaskListObservableCollection taskList,
            IDialogUtils dialogUtils)
        {
            DoneList = doneList;
            _taskList = taskList;
            _dialogUtils = dialogUtils;
        }

        public DoneListObservableCollection DoneList { get; }

        public ICommand RemoveClickCommand => new Command(async item =>
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.RemoveTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                DoneList.RemoveDoneItem((DoneItem) item);
            }
        });

        public ICommand UndoneClickCommand => new Command(async item =>
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.UndoneTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                var doneItem = (DoneItem) item;
                _taskList.Add((ToDoItem) item);
                DoneList.RemoveDoneItem(doneItem);
            }
        });
    }
}