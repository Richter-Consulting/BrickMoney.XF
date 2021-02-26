using System;
using Prism.Commands;
using Syncfusion.ListView.XForms;

namespace BrickMoney.Commands
{
    public class ListItemSelectedCommand<TItem> : DelegateCommand<TItem>
    {
        public ListItemSelectedCommand(Action<TItem> executeMethod, Func<TItem, bool> canExecuteMethod = null) : base(executeMethod, canExecuteMethod ?? (i => true))
        {
        }

        protected override bool CanExecute(object parameter)
        {
            return parameter is ItemTappedEventArgs item && item.ItemType == ItemType.Record && item.ItemData is TItem typedItem
                ? CanExecute(typedItem)
                : CanExecute(null);
        }

        protected override void Execute(object parameter)
        {
            if (parameter is ItemTappedEventArgs item && item.ItemType == ItemType.Record && item.ItemData is TItem typedItem)
            {
                Execute(typedItem);
            }
            else
            {
                Execute(null);
            }
        }
    }
}
