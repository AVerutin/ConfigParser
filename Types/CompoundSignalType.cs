﻿namespace ConfigParser.Types
{
    public enum CompoundSignalType
    {
        SIMPLE_SIGNAL = 0,
        VIRTUAL_SIGNAL,
        COMPLEX_SIGNAL_SUM,
        COMPLEX_SIGNAL_AVERAGE,
        COMPLEX_SIGNAL_BOOL_AND,
        COMPLEX_SIGNAL_BOOL_OR,
        COMPLEX_SIGNAL_POINTER,
        COMPLEX_SIGNAL_PPOINTER,
        COMPLEX_SIGNAL_SELECT,
        COMPLEX_SIGNAL_DELAYOFF,
        COMPLEX_SIGNAL_DELAYON
    }
}