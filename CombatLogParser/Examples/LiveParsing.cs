using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CombatLogParser;
using CombatLogParser.EventInfo;

namespace CombatLogParser.Examples
{
    public class LiveParsing
    {
        private FileInfo _fileInfo;
        private CombatLogParser _clp;
        private FileSystemWatcher _fsw;

        /// <summary>
        /// This class will live-read a combat log found at FilePath and print to the console any time it finds a UNIT_DIED event
        /// </summary>
        public LiveParsing(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Combat log not found", FilePath);
            }

            _fileInfo = new FileInfo(FilePath);
            _clp = new CombatLogParser(FilePath);

            //Set initial position to 0 to parse existing file before live monitoring, or _fileInfo.Length to skip existing file and only read future changes
            long lastSeekPos = _fileInfo.Length;
            _clp.PreParseEvent += (clp, fs) => { fs.Position = lastSeekPos; };
            _clp.PostParseEvent += (clp, fs) => { lastSeekPos = fs.Position; };

            _clp.RegisterEvent((s, e) =>
            {
                string name = e.Get(UNIT_DIED.UnitName);
                Console.WriteLine($"Unit {name} died");
            }, Events.UNIT_DIED);

            _fsw = new FileSystemWatcher();
            _fsw.Path = _fileInfo.DirectoryName;
            _fsw.NotifyFilter = NotifyFilters.LastWrite;
            _fsw.Filter = _fileInfo.Name;
            _fsw.EnableRaisingEvents = true;
            _fsw.Changed += (x, y) =>
            {
                _clp.ParseToEnd();
            };

            
            if (lastSeekPos != _fileInfo.Length)
            {
                _clp.ParseToEnd();
            }
        }
    }
}
