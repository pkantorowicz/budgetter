using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Budgetter.Wpf.Views.Controls;

public partial class NavigationControl : UserControl
{
    public static readonly DependencyProperty GoBackCommandProperty = DependencyProperty.Register(
        "GoBackCommand", typeof(ICommand),
        typeof(NavigationControl)
    );

    public static readonly DependencyProperty GoNextCommandProperty = DependencyProperty.Register(
        "GoNextCommand", typeof(ICommand),
        typeof(NavigationControl)
    );

    public NavigationControl()
    {
        InitializeComponent();
    }

    public ICommand GoBackCommand
    {
        get => (ICommand)GetValue(GoBackCommandProperty);
        set => SetValue(GoBackCommandProperty, value);
    }

    public ICommand GoNextCommand
    {
        get => (ICommand)GetValue(GoNextCommandProperty);
        set => SetValue(GoNextCommandProperty, value);
    }
}