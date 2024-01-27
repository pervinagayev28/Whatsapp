using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Whatsapp.Commands
{
    public class CommandAsync : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Func<object,Task>? AsyncAction { get; set; }
        public Predicate<object>? Predicate { get; set; }

        public CommandAsync(Func<object,Task> asyncAction, Predicate<object> predicate = null)
        {
            AsyncAction = asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));
            Predicate = predicate;

            if (Predicate == null)
                Predicate = (obj) => true;
        }

        public bool CanExecute(object? parameter)
        {
            return Predicate?.Invoke(parameter!) ?? false;
        }

        public async void Execute(object? parameter)
        {
            if (AsyncAction != null)
            {
                await AsyncAction.Invoke(parameter!);
            }
        }



     
    }
}
