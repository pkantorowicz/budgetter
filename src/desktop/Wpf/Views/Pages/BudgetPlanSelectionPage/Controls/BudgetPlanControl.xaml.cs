using System.Windows;
using System.Windows.Controls;

namespace Budgetter.Wpf.Views.Pages.BudgetPlanSelectionPage.Controls;

public partial class BudgetPlanControl : UserControl
{
    public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register(
        "CanGoBack", typeof(bool),
        typeof(BudgetPlanControl)
    );

    public BudgetPlanControl()
    {
        InitializeComponent();
    }

    public bool CanGoBack
    {
        get => (bool)GetValue(CanGoBackProperty);
        set => SetValue(CanGoBackProperty, value);
    }
}