using System.ComponentModel;
using System.Windows;

namespace Lab04.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PersonListView _personListView;
        private PersonEditView _personEditView;
        private PersonEditView _personAddView;
        public MainWindow()
        {
            InitializeComponent();
            ShowPersonListView();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            
            base.OnClosing(e);
        }
        private void ShowPersonListView()
        {

            //if (_personListView == null)
            //{
                _personListView = new PersonListView(ShowPersonAddView,ShowPersonEditView);
            //}
            ShowView(_personListView);
        }
        private void ShowPersonEditView()
        {

            //if (_personEditView == null)
            //{
                _personEditView = new PersonEditView(false,ShowPersonListView, ShowPersonListView);
            //}
            ShowView(_personEditView);
        }
        private void ShowPersonAddView()
        {

            //if (_personAddView == null)
            //{
                _personAddView = new PersonEditView(true,ShowPersonListView, ShowPersonListView);
            //}
            ShowView(_personAddView);
        }
        private void ShowView(UIElement element)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(element);
        }
    }
}
