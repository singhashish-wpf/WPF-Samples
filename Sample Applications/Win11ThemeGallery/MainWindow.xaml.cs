﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.ViewModels;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider, INavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();

        Toggle_TitleButtonVisibility();

        _navigationService = navigationService;
        _navigationService.SetFrame(this.RootContentFrame);
        _navigationService.NavigateTo(typeof(DashboardPage));

        WindowChrome.SetWindowChrome(
            this,
            new WindowChrome
            {
                CaptionHeight = 50,
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = true
            }
        );
    }

    private IServiceProvider _serviceProvider;
    private INavigationService _navigationService;

    public MainWindowViewModel ViewModel { get; }

    private void ControlsList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (ControlsList.SelectedItem is NavigationItem navItem)
        {
            _navigationService.NavigateTo(navItem.PageType);
        }
    }

    private void Toggle_TitleButtonVisibility()
    {
        var appContextBackdropData = AppContext.GetData("Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop");
        bool disableFluentThemeWindowBackdrop = false;

        if (appContextBackdropData != null)
        {
            disableFluentThemeWindowBackdrop = bool.Parse(Convert.ToString(appContextBackdropData));
        }


        if (!disableFluentThemeWindowBackdrop)
        {
            foreach (ResourceDictionary mergedDictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (mergedDictionary.Source != null && mergedDictionary.Source.ToString().EndsWith("Fluent.xaml"))
                {
                    MinimizeButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Collapsed;
                    CloseButton.Visibility = Visibility.Collapsed;
                    break;
                }
            }
        }
    }

    private void SearchBox_KeyUp(object sender, KeyEventArgs e)
    {
        ViewModel.UpdateSearchText(SearchBox.Text);
    }

    private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        ViewModel.UpdateSearchText(SearchBox.Text);
    }

    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        Console.WriteLine(MaximizeIcon.Text);
        if(this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            MaximizeIcon.Text = "\uE922";
        }
        else
        {
            this.WindowState = WindowState.Maximized;
            MaximizeIcon.Text = "\uE923";
        }
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}