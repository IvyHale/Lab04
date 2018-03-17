using Lab04.Annotations;
using Lab04.Exceptions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab04.ModelView
{
    class PersonAddViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate = DateTime.Now;
        private bool _dateChanged = false;
        private RelayCommand _proceedCommand;
        private RelayCommand _cancelCommand;
        private readonly Action _addSuccessAction;
        private readonly Action _cancelAction;
        internal PersonAddViewModel(Action addSuccessAction, Action cancelAction)
        {
            _addSuccessAction = addSuccessAction;
            _cancelAction = cancelAction;
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                _dateChanged = true;
                OnPropertyChanged();
            }
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(o => _cancelAction.Invoke())); }
        }

        public RelayCommand ProceedCommand
        {
            get
            {
                return _proceedCommand ?? (_proceedCommand = new RelayCommand(ProceedImpl,
                           o => !String.IsNullOrWhiteSpace(_name) &&
                                !String.IsNullOrWhiteSpace(_surname) &&
                                !String.IsNullOrWhiteSpace(_email) &&
                                _dateChanged));
            }
        }

        private async void ProceedImpl(object o)
        {
            await Task.Run((() =>
            {
                try
                {
                    StationManager.CurrentPerson = PersonDBAdapter.CreatePerson(_name, _surname, _email, _birthDate);
                }
                catch (WrongNameException e)
                {
                    MessageBox.Show("WrongNameException: " + e.Message);
                }
                catch (EmailException e)
                {
                    MessageBox.Show("EmailException: " + e.Message);
                }
                catch (PastException e)
                {
                    MessageBox.Show("PastException: " + e.Message);
                }
                catch (FutureException e)
                {
                    MessageBox.Show("FutureException: " + e.Message);
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show("Something gone really wrong: " + e.Message);
                }
                Thread.Sleep(500);
            }));

            if (StationManager.CurrentPerson != null)
            {
                _addSuccessAction.Invoke();
            }
        }

        #region Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
