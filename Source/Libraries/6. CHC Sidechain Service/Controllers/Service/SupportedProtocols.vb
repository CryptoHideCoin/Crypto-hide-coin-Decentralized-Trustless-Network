﻿Option Compare Text
Option Explicit On


Imports System.Web.Http
Imports CHCModelsLibrary.AreaModel.Network.Response
Imports CHCModelsLibrary.AreaModel.Information





Namespace Controllers

    ' GET: api/{GUID service}/service/supportedProtocolsController
    <Route("ServiceApi")>
    Public Class supportedProtocolsController

        Inherits ApiController



        ''' <summary>
        ''' This method provide to return the protocols list supported of this service
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValue() As SupportedProtocolsResponseModel
            Dim ownerId As String = "SupportedProtocolsGet-" & Guid.NewGuid.ToString
            Dim result As New SupportedProtocolsResponseModel
            Dim enter As Boolean = False
            Try
                AreaCommon.Main.environment.log.trackEnter("supportedProtocols.GetValue", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                enter = True

                result.protocols.Add("SuperminimalAdmin")

                result.responseTime = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
            Catch ex As Exception
                result.responseStatus = RemoteResponse.EnumResponseStatus.inError
                result.errorDescription = "503 - Generic Error"

                AreaCommon.Main.environment.log.trackException("supportedProtocols.GetValue", ownerId, ex.Message)
            Finally
                If enter Then
                    AreaCommon.Main.environment.log.trackExit("supportedProtocols.GetValue", ownerId)
                End If
            End Try

            Return result
        End Function

    End Class

End Namespace
