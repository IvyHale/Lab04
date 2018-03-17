using Lab04.Annotations;
using Lab04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab04.ModelView
{
    class PersonListViewModel : INotifyPropertyChanged
    {
        List<string> SortEnum  = new List<string>()
         {"No sorting", "Name", "Surname", "Email", "BirthDate", "IsAdult", "SunSign", "ChineseSign", "IsBirthday"};
        private ObservableCollection<Person> _people;
        private Person _selectedPerson;
        private readonly Action _showPersonEditAction;
        private readonly Action _showPersonAddAction;
        private RelayCommand _personEditCommand;
        private RelayCommand _personAddCommand;
        private RelayCommand _personRemoveCommand;
        private RelayCommand _sortNameCommand;
        private RelayCommand _filterAdultCommand;
        private RelayCommand _unfilterCommand;
        private RelayCommand _saveCommand;
        internal PersonListViewModel(Action showPersonAddAction, Action showPersonEditAction)
        {
            _people = new ObservableCollection<Person>(PersonDBAdapter.Persons);
            _showPersonEditAction = showPersonEditAction;
            _showPersonAddAction = showPersonAddAction;
        }
        public ObservableCollection<Person> People
        {
            get => _people;
            private set
            {
                _people = value;
                OnPropertyChanged();
            }
        }
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AddCommand
        {
            get { return _personAddCommand ?? (_personAddCommand = new RelayCommand(o => _showPersonAddAction.Invoke())); }
        }
        public RelayCommand EditCommand
        {
            get { return _personEditCommand ?? (_personEditCommand = new RelayCommand(EditImpl, o => _selectedPerson != null)); }
        }

        private async void EditImpl(object o)
        {
            await Task.Run((() =>
            {
                StationManager.CurrentPerson = PersonDBAdapter.GetPerson(_selectedPerson.Email);
                Thread.Sleep(500);
            }));
            if (StationManager.CurrentPerson == null)
                MessageBox.Show("Something is wrong.");
            else
            {
                _showPersonEditAction.Invoke();
            }
        }
        public RelayCommand RemoveCommand
        {
            get
            {
                return _personRemoveCommand ?? (_personRemoveCommand = new RelayCommand(RemoveImpl, o => _selectedPerson!=null));
            }
        }
        private async void RemoveImpl(object o)
        {
            await Task.Run((() =>
            {
                PersonDBAdapter.RemovePerson(_selectedPerson.Email);
                _people = new ObservableCollection<Person>(PersonDBAdapter.Persons);

                OnPropertyChanged("People");
                Thread.Sleep(500);
            }));
            MessageBox.Show("Successfully removed");
        }
        public RelayCommand SortNameCommand
        {
            get { return _sortNameCommand ?? (_sortNameCommand = new RelayCommand(SortNameImpl, o => _people!=null)); }
        }
        private async void SortNameImpl(object o)
        {
            await Task.Run((() =>
            {
                _people = new ObservableCollection<Person>(_people.OrderBy(person => person.Name));

                OnPropertyChanged("People");
                Thread.Sleep(500);
            }));
            MessageBox.Show("The list is sorted by name.");
        }
        public RelayCommand FilterAdultCommand
        {
            get { return _filterAdultCommand ?? (_filterAdultCommand = new RelayCommand(FilterAdultImpl, o => _people != null)); }
        }
        private async void FilterAdultImpl(object o)
        {
            await Task.Run((() =>
            {
                List<Person> temp = _people.Where(p => p.IsAdult).ToList();
                _people= new ObservableCollection<Person>();
                foreach (Person p in temp)
                    _people.Add(p);

                OnPropertyChanged("People");
                Thread.Sleep(500);
            }));
            MessageBox.Show("The list is successfully filtered.");
        }
        public RelayCommand UnfilterCommand
        {
            get { return _unfilterCommand ?? (_unfilterCommand = new RelayCommand(UnfilterImpl, o => _people != null)); }
        }
        private async void UnfilterImpl(object o)
        {
            await Task.Run((() =>
            {
                _people = new ObservableCollection<Person>(PersonDBAdapter.Persons);

                OnPropertyChanged("People");
                Thread.Sleep(500);
            }));
            MessageBox.Show("The list is successfully unfiltered.");
        }
        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveImpl, o => _people != null)); }
        }
        private async void SaveImpl(object o)
        {
            await Task.Run((() =>
            {
                PersonDBAdapter.SaveData();
                Thread.Sleep(500);
            }));
            MessageBox.Show("The list is successfully saved.");
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
