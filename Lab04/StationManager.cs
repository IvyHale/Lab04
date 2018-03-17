using Lab04.Model;
using System;
using System.IO;

namespace Lab04
{
    static class StationManager
    {
        internal static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LabData");
        public static Person CurrentPerson { get; set; }
    }
}
