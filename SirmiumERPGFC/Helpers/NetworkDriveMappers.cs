using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Helpers
{
    public abstract class NetworkDriveMapper
    {
        protected virtual string DriveLetter { get; set; }
        protected virtual string DriveNetworkPath { get; set; }
        protected virtual string SubDir { get; set; }
        protected virtual string Username { get; set; }
        protected virtual string Password { get; set; }
        protected virtual bool Persistent { get; set; }
        public NetworkDriveMapper(string driveLetter, string driveNetworkPath = "", string subDir = "", string username = "", string password = "", bool isPersistent = true)
        {
            DriveLetter = driveLetter;
            DriveNetworkPath = driveNetworkPath;
            SubDir = subDir;
            Username = username;
            Password = password;
            Persistent = isPersistent;
        }

        public NetworkDriveMapper(string driveLetter)
        {
            DriveLetter = driveLetter;
        }

        public abstract void MapDrive();
        public abstract void UnmapDrive();

        public bool IsDriveAndExists()
        {
            var drives = DriveInfo.GetDrives();


            return drives.Any(x => x.Name == $"{DriveLetter}\\");
        }

        protected int ExecuteCommand(string processPath, string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{processPath}";
            cmd.StartInfo.Arguments = $"{command}";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.WaitForExit();
            return cmd.ExitCode;
        }

        public class NetworkDriveException : Exception
        {
            public NetworkDriveException(string exception) : base(exception) { }
        }
    }

    public class AzureNetworkDriveMapper : NetworkDriveMapper
    {
        public AzureNetworkDriveMapper(string DriveLetter, string DriveNetworkPath = "", string SubDir = "", string Username = "", string Password = "", bool IsPersistent = true)
            : base(DriveLetter, DriveNetworkPath, SubDir, Username, Password, IsPersistent)
        {
        }

        public override void MapDrive()
        {
            if (!IsDriveAndExists())
            {
                var processCode = ExecuteCommand("cmdKey", $"/add:{DriveNetworkPath} /user:{Username} /pass:{Password}");

                if (processCode == 0)
                {
                    string isPersistent = Persistent ? "Yes" : "No";
                    var processMapCode = ExecuteCommand("net", $"use {DriveLetter} \\\\{DriveNetworkPath}{SubDir} /Persistent:{isPersistent}");
                    if (processMapCode != 0) throw new NetworkDriveException($"Couldn't map network drive {DriveNetworkPath} to letter {DriveLetter}!");
                }
                else throw new NetworkDriveException($"Couldn't add network drive credentials!");
            }
            else throw new NetworkDriveException($"Drive with name {DriveLetter} already exists!");
        }

        public override void UnmapDrive()
        {
            if (IsDriveAndExists())
            {
                var code = ExecuteCommand("net", $"use ${DriveLetter} /delete");
            }
            else throw new NetworkDriveException($"Drive with letter {DriveLetter} not found!");
        }
    }
}
