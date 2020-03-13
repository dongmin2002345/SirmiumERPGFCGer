using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SirmiumERPGFC.Scanners
{
    public class WIAScanner
    {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";

        WIA.Device SelectedDevice { get; set; } = null;

        Dictionary<uint, dynamic> PropertyValues = new Dictionary<uint, dynamic>();

        Dictionary<uint, dynamic> PreviousPropertyValues = new Dictionary<uint, dynamic>();

        public bool SelectDevice()
        {
            WIA.CommonDialog dialog = new WIA.CommonDialog();
            WIA.Device device = dialog.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, true, false);

            SelectedDevice = device;

            return SelectedDevice != null;
        }


        public List<string> Scan(WiaDocumentHandlingType handlingType = WiaDocumentHandlingType.Feeder, WiaDocumentHandlingType? duplexOrOtherMode = null)
        {
            SelectDevice();

            if (SelectedDevice == null)
                return new List<string>();


            List<string> images = new List<string>();
            bool hasMorePages = true;

            while (hasMorePages)
            {
                // select the correct scanner using the provided scannerId parameter
                WIA.DeviceManager manager = new WIA.DeviceManager();
                WIA.Device device = null;

                if (manager.DeviceInfos == null || manager.DeviceInfos.Count < 1)
                    throw new WiaScannerDeviceNotFoundException((string)Application.Current.FindResource("NemaDostupnihSkeneraUzvicnik"));

                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == SelectedDevice.DeviceID)
                    {
                        // connect to scanner
                        device = info.Connect();
                        break;
                    }
                }
                // device was not found
                if (device == null)
                    throw new WiaScannerDeviceNotFoundException((string)Application.Current.FindResource("OdabraniSkenerNijeDostupanUzvicnik"));


                if(handlingType == WiaDocumentHandlingType.Feeder)
                {
                    if (duplexOrOtherMode != null)
                        SetProperty(device.Properties, (uint)WiaProperty.DocumentHandlingSelect, (uint)(handlingType|duplexOrOtherMode));
                    else
                        SetProperty(device.Properties, (uint)WiaProperty.DocumentHandlingSelect, (uint)handlingType);
                }
                WIA.Item item = device.Items[1] as WIA.Item;
                try
                {

                    PreviousPropertyValues.Clear();

                    foreach (var property in PropertyValues)
                    {
                        WIA.Property prop = GetProperty(item.Properties, property.Key);
                        PreviousPropertyValues.Add(property.Key, prop?.get_Value());

                        var supported = prop.SubTypeValues;
                        SetProperty(item.Properties, property.Key, property.Value);
                    }


                    // scan image
                    WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();


                    WIA.ImageFile image = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatBMP, false);
                    // save to temp file
                    string fileName = Path.GetTempFileName();
                    if(image != null)
                    {
                        File.Delete(fileName);
                        image.SaveFile(fileName);
                        image = null;
                        // add file to output list
                        images.Add(fileName);
                    }
                }
                catch(WiaScannerDeviceNotFoundException exc)
                {
                    throw exc;
                }
                catch(IOException exc)
                {
                    throw exc;
                }
                catch (COMException exc)
                {
                    if ((uint)exc.ErrorCode == 0x80210003)
                        throw new WiaScannerInsertPaperException(exc.Message);
                    else
                        throw exc;
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    item = null;

                    WIA.Property documentHandlingSelect = null;
                    WIA.Property documentHandlingStatus = null;
                    foreach (WIA.Property prop in device.Properties)
                    {
                        if (prop.PropertyID == (uint)WiaProperty.DocumentHandlingSelect)
                            documentHandlingSelect = prop;
                        if (prop.PropertyID == (uint)WiaProperty.DocumentHandlingStatus)
                            documentHandlingStatus = prop;
                    }
                    hasMorePages = false;

                    if(documentHandlingSelect != null)
                    {
                        if((documentHandlingSelect.get_Value() & ((uint)WiaDocumentHandlingType.Feeder)) != 0)
                        {
                            hasMorePages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
                        }
                    }
                }
            }
            return images;
        }

        void SetProperty(WIA.Properties props, uint property, dynamic value)
        {
            object propName = property.ToString();
            object propValue = value.ToString();
            WIA.Property prop = props.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }

        WIA.Property GetProperty(WIA.Properties props, uint property)
        {
            foreach (WIA.Property prop in props)
                if (prop.PropertyID == property)
                    return prop;

            return null;
        }

        public void InitializeProperties(Dictionary<WiaProperty, dynamic> properties)
        {
            if (properties != null)
            {
                foreach (var item in properties)
                    InitializeProperty(item.Key, item.Value);
            }
        }

        public void InitializeProperty(WiaProperty property, dynamic val)
        {
            PropertyValues.Add((uint)property, val);
        }

        public class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 0x00000001;
            public const uint FLATBED = 0x00000002;
        }
        public class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEED_READY = 0x00000001;
        }

        public enum WiaProperty : uint
        {
            DeviceId = 2,
            Manufacturer = 3,
            Description = 4,
            Type = 5,
            Port = 6,
            Name = 7,
            Server = 8,
            RemoteDevId = 9,
            UIClassId = 10,
            FirmwareVersion = 1026,
            ConnectStatus = 1027,
            DeviceTime = 1028,
            PicturesTaken = 2050,
            PicturesRemaining = 2051,
            ExposureMode = 2052,
            ExposureCompensation = 2053,
            ExposureTime = 2054,
            FNumber = 2055,
            FlashMode = 2056,
            FocusMode = 2057,
            FocusManualDist = 2058,
            ZoomPosition = 2059,
            PanPosition = 2060,
            TiltPostion = 2061,
            TimerMode = 2062,
            TimerValue = 2063,
            PowerMode = 2064,
            BatteryStatus = 2065,
            Dimension = 2070,
            HorizontalBedSize = 3074,
            VerticalBedSize = 3075,
            HorizontalSheetFeedSize = 3076,
            VerticalSheetFeedSize = 3077,
            SheetFeederRegistration = 3078,         // 0 = LEFT_JUSTIFIED, 1 = CENTERED, 2 = RIGHT_JUSTIFIED
            HorizontalBedRegistration = 3079,       // 0 = LEFT_JUSTIFIED, 1 = CENTERED, 2 = RIGHT_JUSTIFIED
            VerticalBedRegistraion = 3080,          // 0 = TOP_JUSTIFIED, 1 = CENTERED, 2 = BOTTOM_JUSTIFIED
            PlatenColor = 3081,
            PadColor = 3082,
            FilterSelect = 3083,
            DitherSelect = 3084,
            DitherPatternData = 3085,

            DocumentHandlingCapabilities = 3086,    // FEED = 0x01, FLAT = 0x02, DUP = 0x04, DETECT_FLAT = 0x08, 
                                                    // DETECT_SCAN = 0x10, DETECT_FEED = 0x20, DETECT_DUP = 0x40, 
                                                    // DETECT_FEED_AVAIL = 0x80, DETECT_DUP_AVAIL = 0x100
            DocumentHandlingStatus = 3087,          // FEED_READY = 0x01, FLAT_READY = 0x02, DUP_READY = 0x04, 
                                                    // FLAT_COVER_UP = 0x08, PATH_COVER_UP = 0x10, PAPER_JAM = 0x20
            DocumentHandlingSelect = 3088,          // FEEDER = 0x001, FLATBED = 0x002, DUPLEX = 0x004, FRONT_FIRST = 0x008
                                                    // BACK_FIRST = 0x010, FRONT_ONLY = 0x020, BACK_ONLY = 0x040
                                                    // NEXT_PAGE = 0x080, PREFEED = 0x100, AUTO_ADVANCE = 0x200
            DocumentHandlingCapacity = 3089,
            HorizontalOpticalResolution = 3090,
            VerticalOpticalResolution = 3091,
            EndorserCharacters = 3092,
            EndorserString = 3093,
            ScanAheadPages = 3094,                  // ALL_PAGES = 0
            MaxScanTime = 3095,
            Pages = 3096,                           // ALL_PAGES = 0
            PageSize = 3097,                        // A4 = 0, LETTER = 1, CUSTOM = 2
            PageWidth = 3098,
            PageHeight = 3099,
            Preview = 3100,                         // FINAL_SCAN = 0, PREVIEW = 1
            TransparencyAdapter = 3101,
            TransparecnyAdapterSelect = 3102,
            ItemName = 4098,
            FullItemName = 4099,
            ItemTimeStamp = 4100,
            ItemFlags = 4101,
            AccessRights = 4102,
            DataType = 4103, // WiaDataType
            BitsPerPixel = 4104,
            PreferredFormat = 4105,
            Format = 4106,
            Compression = 4107, // WiaCompression
            MediaType = 4108,
            ChannelsPerPixel = 4109,
            BitsPerChannel = 4110,
            Planar = 4111,
            PixelsPerLine = 4112,
            BytesPerLine = 4113,
            NumberOfLines = 4114,
            GammaCurves = 4115,
            ItemSize = 4116,
            ColorProfiles = 4117,
            BufferSize = 4118,
            RegionType = 4119,
            ColorProfileName = 4120,
            ApplicationAppliesColorMapping = 4121,
            StreamCompatibilityId = 4122,
            ThumbData = 5122,
            ThumbWidth = 5123,
            ThumbHeight = 5124,
            AudioAvailable = 5125,
            AudioFormat = 5126,
            AudioData = 5127,
            PicturesPerRow = 5128,
            SequenceNumber = 5129,
            TimeDelay = 5130,
            CurrentIntent = 6146,
            HorizontalResolution = 6147,
            VerticalResolution = 6148,
            HorizontalStartPosition = 6149,
            VerticalStartPosition = 6150,
            HorizontalExtent = 6151,
            VerticalExtent = 6152,
            PhotometricInterpretation = 6153,
            Brightness = 6154,
            Contrast = 6155,
            Orientation = 6156, // WiaOrientation
            Rotation = 6157, // WiaRotation
            Mirror = 6158,
            Threshold = 6159,
            Invert = 6160,
            LampWarmUpTime = 6161,
        }

        /// <summary>
        /// Page sizes:
        /// A4 (21x29.7): 0, default
        /// Letter: 1
        /// </summary>
        public enum WiaPageSize : uint
        {
            A4 = 0,
            Letter = 1
        }

        /// <summary>
        /// Compressions:
        /// None: 0,
        /// Jpeg: 5,
        /// Png: 8
        /// </summary>
        public enum WiaCompression : uint
        {
            None = 0,
            Jpeg = 5,
            Png = 8
        }

        /// <summary>
        /// Portrait: 0 degrees,
        /// Landscape: 90 degrees
        /// Flip90: Same as Landscape
        /// Flip180: Flip by 180 degrees, upside-down
        /// Flip270: Rotate 270 degrees, rotate to left side for -90 degrees
        /// </summary>
        public enum WiaOrientation : uint
        {
            Portrait = 0, Landscape = 1, Flip90 = Landscape, Flip180 = 2, Flip270 = 3
        }
        /// <summary>
        /// Portrait: 0 degrees,
        /// Landscape: 90 degrees
        /// Flip90: Same as Landscape
        /// Flip180: Flip by 180 degrees, upside-down
        /// Flip270: Rotate 270 degrees, rotate to left side for -90 degrees
        /// </summary>
        public enum WiaRotation : uint
        {
            Portrait = 0, Landscape = 1, Flip90 = Landscape, Flip180 = 2, Flip270 = 3
        }

        /// <summary>
        /// Data Types for scanner:
        /// Auto: Automatically select
        /// Color: Color image
        /// Grayscale: Gray color tones
        /// Threshold: Black and white
        /// </summary>
        public enum WiaDataType : uint
        {
            BlackAndWhite = 0,
            Grayscale = 2,
            Color = 3,
        }

        public class WiaScannerDeviceNotFoundException : Exception
        {
            public WiaScannerDeviceNotFoundException(string exceptionMessage) : base(exceptionMessage) { }
        }
        public class WiaScannerInsertPaperException : Exception
        {
            public WiaScannerInsertPaperException(string exceptionMessage) : base(exceptionMessage) { }
        }



        public enum WiaDocumentHandlingType : uint
        {
            Feeder = 0x001, 
            Flatbed = 0x002,


            Duplex = 0x004,
            FrontFirst = 0x008,
            BackFirst = 0x010,
            FrontOnly = 0x020,
            BackOnly = 0x040,
            NextPage = 0x080,
            Prefeed = 0x100,
            AutoAdvance = 0x200
        }
    }
}
