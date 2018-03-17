using Lab04.ModelView;
using System;
using System.Windows.Controls;

namespace Lab04.View
{
    /// <summary>
    /// Interaction logic for PersonEditView.xaml
    /// </summary>
    public partial class PersonEditView : UserControl
    {
        public PersonEditView(bool add,Action editSuccessAction, Action cancelAction)
        {
            InitializeComponent();
            if (add)
                DataContext = new PersonAddViewModel(editSuccessAction, cancelAction);
            else
                DataContext = new PersonEditViewModel(editSuccessAction, cancelAction);
        }
    }
}
