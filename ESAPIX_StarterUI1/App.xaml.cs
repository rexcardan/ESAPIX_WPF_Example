using ESAPIX.AppKit.Overlay;
using ESAPIX.Common;
using ESAPIX.Common.Args;
using ESAPIX.Interfaces;
using ESAPIX.Services;
using ESAPX_StarterUI.ViewModels;
using ESAPX_StarterUI.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ESAPX_StarterUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IESAPIService _esapiServ;
        private string[] _args;

        //Disable if you don't want patient selection
        public bool IsPatientSelectionEnabled { get; } = true;

        protected override void OnStartup(StartupEventArgs e)
        {
            _args = e.Args;
            ConfigureEsapiService();

            var win = CreateShell();
            base.OnStartup(e);

            MainWindow = win;
            win.Show();
        }

        private MainView CreateShell()
        {
            var win = new MainView
            {
                DataContext = new MainViewModel(_esapiServ)
            };

            EventHandler addSelection = null;

            addSelection = new EventHandler((o, args) =>
            {
                //Adds the patient selection UI on main window
                var currentContent = (UIElement)win.Content;
                var stackPanel = new DockPanel();
                stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
                win.Content = stackPanel;
                var selectPat = new SelectPatient();
                var selectPatContent = (FrameworkElement)selectPat.Content;
                selectPatContent.DataContext = selectPat;
                selectPat.Content = null;
                stackPanel.Children.Add(selectPatContent);
                stackPanel.Children.Add(currentContent);
                DockPanel.SetDock(selectPatContent, Dock.Top);
                DockPanel.SetDock(currentContent, Dock.Top);
                win.WindowState = WindowState.Maximized;
                win.Content = stackPanel;
                win.ContentRendered -= addSelection;
            });

            if (IsPatientSelectionEnabled)
            {
                win.ContentRendered += addSelection;
            }

            return win;
        }

        private void ConfigureEsapiService()
        {
            _esapiServ = new ESAPIService(() => VMS.TPS.Common.Model.API.Application.CreateApplication());
            //args = ContextIO.ReadArgsFromFile(@"context.txt");

            if (_args != null)
            {
                _esapiServ.Execute(sac =>
                {
                    ArgContextSetter.Set(sac, _args);
                });
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                AppComThread.Instance.Dispose();
            }
            catch (Exception)
            {

            }

            base.OnExit(e);
        }
    }
}
