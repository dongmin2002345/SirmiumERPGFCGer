using Ninject;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ToDos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.Home
{
    /// <summary>
    /// Interaction logic for ToDoStatus_AddEdit.xaml
    /// </summary>
    public partial class ToDoStatus_AddEdit : UserControl, INotifyPropertyChanged
        {
            #region Attributes

            #region Services
            IToDoStatusService ToDoStatusService;
            #endregion

            #region Events
            public event ToDoStatusHandler ToDoStatusCreatedUpdated;
            #endregion


            #region CurrentToDoStatus
            private ToDoStatusViewModel _CurrentToDoStatus = new ToDoStatusViewModel();

            public ToDoStatusViewModel CurrentToDoStatus
            {
                get { return _CurrentToDoStatus; }
                set
                {
                    if (_CurrentToDoStatus != value)
                    {
                        _CurrentToDoStatus = value;
                        NotifyPropertyChanged("CurrentToDoStatus");
                    }
                }
            }
            #endregion


            #region IsCreateProcess
            private bool _IsCreateProcess;

            public bool IsCreateProcess
            {
                get { return _IsCreateProcess; }
                set
                {
                    if (_IsCreateProcess != value)
                    {
                        _IsCreateProcess = value;
                        NotifyPropertyChanged("IsCreateProcess");
                    }
                }
            }
            #endregion

            #region IsPopup
            private bool _IsPopup;

            public bool IsPopup
            {
                get { return _IsPopup; }
                set
                {
                    if (_IsPopup != value)
                    {
                        _IsPopup = value;
                        NotifyPropertyChanged("IsPopup");
                    }
                }
            }
            #endregion


            #region SubmitButtonContent
            private string _SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));

            public string SubmitButtonContent
            {
                get { return _SubmitButtonContent; }
                set
                {
                    if (_SubmitButtonContent != value)
                    {
                        _SubmitButtonContent = value;
                        NotifyPropertyChanged("SubmitButtonContent");
                    }
                }
            }
            #endregion

            #region SubmitButtonEnabled
            private bool _SubmitButtonEnabled = true;

            public bool SubmitButtonEnabled
            {
                get { return _SubmitButtonEnabled; }
                set
                {
                    if (_SubmitButtonEnabled != value)
                    {
                        _SubmitButtonEnabled = value;
                        NotifyPropertyChanged("SubmitButtonEnabled");
                    }
                }
            }
            #endregion

            #endregion

            #region Constructor

            public ToDoStatus_AddEdit(ToDoStatusViewModel ToDoStatusViewModel, bool isCreateProcess, bool isPopup = false)
            {
                ToDoStatusService = DependencyResolver.Kernel.Get<IToDoStatusService>();

                InitializeComponent();

                this.DataContext = this;

                CurrentToDoStatus = ToDoStatusViewModel;
                IsCreateProcess = isCreateProcess;
                IsPopup = isPopup;
            }

            #endregion

            #region  Submit and Cancel button

            private void btnSubmit_Click(object sender, RoutedEventArgs e)
            {
                #region Validation

                if (String.IsNullOrEmpty(CurrentToDoStatus.Name))
                {
                    MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_polje_naziv"));
                    return;
                }

                #endregion

                Thread th = new Thread(() =>
                {
                    SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                    SubmitButtonEnabled = false;

                    CurrentToDoStatus.IsSynced = false;
                    CurrentToDoStatus.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                    CurrentToDoStatus.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                    ToDoStatusResponse response = new ToDoStatusSQLiteRepository().Delete(CurrentToDoStatus.Identifier);
                    response = new ToDoStatusSQLiteRepository().Create(CurrentToDoStatus);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_čuvanja_na_serveruUzvičnik"));
                        SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                        SubmitButtonEnabled = true;
                        return;
                    }

                    response = ToDoStatusService.Create(CurrentToDoStatus);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik")) + response.Message;
                        SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                        SubmitButtonEnabled = true;
                    }

                    if (response.Success)
                    {
                        MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                        SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                        SubmitButtonEnabled = true;

                        ToDoStatusCreatedUpdated();

                        if (IsCreateProcess)
                        {
                            CurrentToDoStatus = new ToDoStatusViewModel();
                            CurrentToDoStatus.Identifier = Guid.NewGuid();

                            Application.Current.Dispatcher.BeginInvoke(
                                System.Windows.Threading.DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    txtName.Focus();
                                })
                            );
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(
                                System.Windows.Threading.DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    if (IsPopup)
                                        FlyoutHelper.CloseFlyoutPopup(this);
                                    else
                                        FlyoutHelper.CloseFlyout(this);
                                })
                            );
                        }
                    }
                });
                th.IsBackground = true;
                th.Start();
            }

            private void btnCancel_Click(object sender, RoutedEventArgs e)
            {
                if (IsPopup)
                    FlyoutHelper.CloseFlyoutPopup(this);
                else
                    FlyoutHelper.CloseFlyout(this);
            }

            #endregion

            #region INotifyPropertyChanged implementation
            public event PropertyChangedEventHandler PropertyChanged;

            // This method is called by the Set accessor of each property.
            // The CallerMemberName attribute that is applied to the optional propertyName
            // parameter causes the property name of the caller to be substituted as an argument.
            private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }
    }