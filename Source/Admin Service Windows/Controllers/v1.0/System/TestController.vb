﻿Imports System.Web.Http



Namespace Controllers


    ' GET: api/v1.0/System/TestServiceController
    ''' <summary>
    ''' This API permit a client application of know if the masternode is alive
    ''' </summary>
    <Route("SystemApi")>
    Public Class TestServiceController

        Inherits ApiController



        Public Function GetValue() As CHCProtocol.AreaCommon.Models.General.BooleanModel

            Return New CHCProtocol.AreaCommon.Models.General.BooleanModel With {.value = True}

        End Function


    End Class


End Namespace