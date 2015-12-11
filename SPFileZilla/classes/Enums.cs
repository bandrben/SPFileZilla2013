using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BandR
{
    public class Enums
    {

        public class IconImages
        {
            public const int FOLDER = 0;
            public const int FILE = 1;
            public const int REFRESH = 2;
            public const int LIST = 3;
        }

        public class TreeNodeActions
        {
            public const string REFRESH = "0";
            public const string UP = "1";
        }

        public class TreeNodeTypes
        {
            public const int OTHER = -1;
            public const int FOLDER = 0;
            public const int FILE = 1;
            public const int LIST = 2;
        }

    }
}
