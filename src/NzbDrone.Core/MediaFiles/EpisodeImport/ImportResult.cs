using System;
using System.Collections.Generic;
using System.Linq;
using NzbDrone.Common;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.MediaFiles.EpisodeImport
{
    public class ImportResult
    {
        public ImportDecision ImportDecision { get; private set; }
        public List<String> Errors { get; private set; }

        public bool Acceptable
        {
            get
            {
                return (Errors.Empty() && ImportDecision.Approved) || !ImportDecision.Approved;
            }
        }

        public bool Successful
        {
            get
            {
                return Errors.Empty();
            }
        } 

        public ImportResult(ImportDecision importDecision, params String[] errors)
        {
            ImportDecision = importDecision;
            Errors = errors.ToList();
        }
    }
}
