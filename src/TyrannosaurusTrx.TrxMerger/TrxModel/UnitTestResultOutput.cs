﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.TrxModel
{
    public class UnitTestResultOutput
    {
        public string StdErr { get; set; }
        public string StdOut { get; set; }
        public ErrorInfo ErrorInfo { get; set; }
    }
}