using Lab04.ModelView;
using System;
using System.Windows.Controls;

namespace Lab04.View
{
    /// <summary>
    /// Interaction logic for PersonListView.xaml
    /// </summary>
    public partial class PersonListView : UserControl
    {
        public PersonListView(Action showPersonAddAction, Action showPersonEditAction)
        {
            InitializeComponent();
            DataContext = new PersonListViewModel(showPersonAddAction, showPersonEditAction);
        }
    }
}
