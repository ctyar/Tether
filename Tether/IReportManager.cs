﻿namespace Tether
{
    internal interface IReportManager
    {
        void Print(string message, MessageType messageType);
        
        void PrintLine();
    }
}