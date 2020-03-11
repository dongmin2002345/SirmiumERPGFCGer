﻿using SirmiumERPGFC.Scanners;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using static SirmiumERPGFC.Scanners.WIAScanner;

namespace SirmiumERPGFC.Views.Home
{
    /// <summary>
    /// Interaction logic for Scanner_List.xaml
    /// </summary>
    public partial class Scanner_List : UserControl, INotifyPropertyChanged
    {
        #region Images
        private ObservableCollection<ScannedImageViewModel> _Images = new ObservableCollection<ScannedImageViewModel>();
        public ObservableCollection<ScannedImageViewModel> Images
        {
            get { return _Images; }
            set
            {
                if (_Images != value)
                {
                    _Images = value;
                    NotifyPropertyChanged("Images");
                }
            }
        }
        #endregion

        #region ScanTypeOptions
        private ObservableCollection<ScannerTypeOptionViewModel> _ScanTypeOptions = new ObservableCollection<ScannerTypeOptionViewModel>()
        {
            new ScannerTypeOptionViewModel() { Type = WiaDataType.Grayscale, Name = (string)Application.Current.FindResource("Grayscale") },
            new ScannerTypeOptionViewModel() { Type = WiaDataType.Color, Name = (string)Application.Current.FindResource("Color") },
            new ScannerTypeOptionViewModel() { Type = WiaDataType.BlackAndWhite, Name = (string)Application.Current.FindResource("BlackAndWhite") },
        };

        public ObservableCollection<ScannerTypeOptionViewModel> ScanTypeOptions
        {
            get { return _ScanTypeOptions; }
            set
            {
                if (_ScanTypeOptions != value)
                {
                    _ScanTypeOptions = value;
                    NotifyPropertyChanged("ScanTypeOptions");
                }
            }
        }
        #endregion

        #region SelectedScanType
        private ScannerTypeOptionViewModel _SelectedScanType;

        public ScannerTypeOptionViewModel SelectedScanType
        {
            get { return _SelectedScanType; }
            set
            {
                if (_SelectedScanType != value)
                {
                    _SelectedScanType = value;
                    NotifyPropertyChanged("SelectedScanType");
                }
            }
        }
        #endregion



        #region CanInteractWithForm
        private bool _CanInteractWithForm = true;

        public bool CanInteractWithForm
        {
            get { return _CanInteractWithForm; }
            set
            {
                if (_CanInteractWithForm != value)
                {
                    _CanInteractWithForm = value;
                    NotifyPropertyChanged("CanInteractWithForm");
                }
            }
        }
        #endregion

        #region SelectedPath
        private string _SelectedPath;

        public string SelectedPath
        {
            get { return _SelectedPath; }
            set
            {
                if (_SelectedPath != value)
                {
                    _SelectedPath = value;
                    NotifyPropertyChanged("SelectedPath");
                }
            }
        }
        #endregion

        #region DocumentName
        private string _DocumentName;

        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                if (_DocumentName != value)
                {
                    _DocumentName = value;
                    NotifyPropertyChanged("DocumentName");
                }
            }
        }
        #endregion


        public Scanner_List()
        {
            InitializeComponent();
            this.DataContext = this;

            SelectedScanType = ScanTypeOptions.FirstOrDefault();
        }

        private void btnSavePdf_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(DocumentName))
            {
                MainWindow.ErrorMessage = (string)Application.Current.FindResource("MorasUnetiNazivDokumentaUzicnik");
                return;
            }
            if(Images == null || Images.Where(x => x.IsSelected).Count() < 1)
            {
                MainWindow.ErrorMessage = (string)Application.Current.FindResource("MorasSkeniratiNestoIOznacitiStavkeUzvicnik");
                return;
            }
            if (String.IsNullOrEmpty(SelectedPath))
            {
                MainWindow.ErrorMessage = (string)Application.Current.FindResource("MorasOdabratiFolderUzvicnik");
                return;
            }
            CanInteractWithForm = false;
            Thread td = new Thread(() =>
            {
                try
                {
                    var generator = new PDFGenerator(Images
                            .Where(x => x.IsSelected)
                            .OrderBy(x => x.CreatedAt)
                            .Select(x => x.ImagePath)
                            .ToList(), 
                            SelectedPath, DocumentName, 
                        MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);

                    generator.Generate();

                    MainWindow.SuccessMessage = (string)Application.Current.FindResource("DokumentJeUspesnoSacuvanUzvicnik");
                } catch(Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                } finally
                {
                    CanInteractWithForm = true;
                }
            });
            td.IsBackground = true;
            td.Start();
        }

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

        private void btnChoosePath_Click(object sender, RoutedEventArgs e)
        {
            CanInteractWithForm = false;

            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var selectedResult = dialog.ShowDialog();

                if(selectedResult == System.Windows.Forms.DialogResult.OK)
                {
                    SelectedPath = dialog.SelectedPath;
                }
            }
            CanInteractWithForm = true;
        }

        private void btnStartScanning_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedScanType == null)
            {
                MainWindow.ErrorMessage = (string)Application.Current.FindResource("MorasOdabratiVrstuSkeniranjaUzvicnik");
                return;
            }

            CanInteractWithForm = false;


            Thread td = new Thread(() =>
            {
                try
                {
                    var scanner = new WIAScanner();
                    scanner.InitializeProperties(new Dictionary<WIAScanner.WiaProperty, dynamic>()
                    {
                        { WiaProperty.DataType, (uint)SelectedScanType.Type }
                    });

                    var scannedData = scanner.Scan();

                    if (scannedData != null && scannedData.Count() > 0)
                    {
                        var images = Images.ToList();
                        foreach(var item in scannedData)
                        {
                            images.Add(new ScannedImageViewModel()
                            {
                                Identifier = Guid.NewGuid(),
                                ImagePath = item,
                                IsSelected = false,
                                CreatedAt = DateTime.Now,
                            });
                        }
                        Images = new ObservableCollection<ScannedImageViewModel>(images.OrderBy(x => x.UpdatedAt));
                    }
                }
                catch (WiaScannerDeviceNotFoundException exc)
                {
                    MainWindow.ErrorMessage = exc.Message;
                }
                catch (IOException exc)
                {
                    throw exc;
                }
                catch (Exception ex)
                {
                    if(ex.GetType() != typeof(COMException))
                    {
                        MainWindow.ErrorMessage = ex.Message;
                    } else
                    {
                        MainWindow.ErrorMessage = (string)Application.Current.FindResource("GreskaPriUcitavanjuSkeneraProbajOpetUzvicnik");
                    }
                } finally
                {
                    CanInteractWithForm = true;
                }
            });

            td.IsBackground = true;
            td.Start();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Images)
                item.IsSelected = true;
        }

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            CanInteractWithForm = false;
            Thread td = new Thread(() =>
            {
                try
                {
                    var selected = Images.Where(x => x.IsSelected).ToList();
                    if (selected != null && selected.Count() > 0)
                        foreach (var item in selected)
                            Dispatcher.BeginInvoke((Action)(() => { Images.Remove(item); }));
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
                finally
                {
                    CanInteractWithForm = true;
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        private void btnDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Images)
                item.IsSelected = false;
        }
    }
}
