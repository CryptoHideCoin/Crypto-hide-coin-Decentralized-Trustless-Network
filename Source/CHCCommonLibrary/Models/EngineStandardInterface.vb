﻿Option Explicit On
Option Compare Text



Public Interface StandardInterface

    Function start() As Boolean
    Function [stop]() As Boolean
    Function maintenance() As Boolean

End Interface