using Lab04.Annotations;
using Lab04.Exceptions;
using Lab04.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab04.ModelView
{
    class PersonEditViewModel : INotifyPropertyChanged
    {
        private bool _isOK;
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate = DateTime.Now;
        private RelayCommand _proceedCommand;
        private RelayCommand _cancelCommand;
        private readonly Action _editSuccessAction;
        private readonly Action _cancelAction;
        internal PersonEditViewModel(Action editSuccessAction, Action cancelAction)
        {
            _editSuccessAction = editSuccessAction;
            _cancelAction = cancelAction;

            _name = StationManager.CurrentPerson.Name;
            _surname = StationManager.CurrentPerson.Surname;
            _email = StationManager.CurrentPerson.Email;
            _birthDate = StationManager.CurrentPerson.BirthDate;

            _isOK = true;
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
                                !String.IsNullOrWhiteSpace(_email)));
            }
        }

        private async void ProceedImpl(object o)
        {
            await Task.Run((() =>
            {
                try
                {
                    PersonDBAdapter.RemovePerson(StationManager.CurrentPerson.Email);
                    StationManager.CurrentPerson = PersonDBAdapter.CreatePerson(_name, _surname, _email, _birthDate);
                    _isOK = true;
                }
                catch (WrongNameException e)
                {
                    MessageBox.Show("WrongNameException: " + e.Message);
                    _isOK = false;
                }
                catch (EmailException e)
                {
                    MessageBox.Show("EmailException: " + e.Message);
                    _isOK = false;
                }
                catch (PastException e)
                {
                    MessageBox.Show("PastException: " + e.Message);
                    _isOK = false;
                }
                catch (FutureException e)
                {
                    MessageBox.Show("FutureException: " + e.Message);
                    _isOK = false;
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show("Something gone really wrong: " + e.Message);
                    _isOK = false;
                }
                Thread.Sleep(500);
            }));

            if (_isOK)
            {
                _editSuccessAction.Invoke();
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
