﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_Minebot.Packets
{
    class TabComplete
    {
        public TabComplete(Wrapped.Wrapped socket, Form1 mainform)
        {
            socket.readString();
        }
    }
}
