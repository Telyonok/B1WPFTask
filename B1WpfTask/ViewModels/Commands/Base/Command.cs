using System.Windows.Input;

namespace B1WPFTask.Commands.Base;
internal abstract class Command : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public virtual bool CanExecute(object? parameter) => true;
    public abstract void Execute(object? parameter);

    public abstract class WithParams<T> : Command where T : class
    {
        public override void Execute(object? parameter)
        {
            if (parameter is not T parameters || !ValidateParams(parameters))
            {
                return;
            }

            Execute(parameters);
        }

        public override bool CanExecute(object? parameter) =>
            parameter is T parameters && ValidateParams(parameters);

        protected abstract bool ValidateParams(T parameters);
        protected abstract void Execute(T parameters);
    }
}
