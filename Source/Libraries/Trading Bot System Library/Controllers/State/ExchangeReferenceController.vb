﻿Option Compare Text
Option Explicit On

Imports System.Web.Http
Imports TradingBotSystemModelsLibrary.AreaModel.Exchange
Imports CHCModelsLibrary.AreaModel.Network.Response
Imports CHCModelsLibrary.AreaModel.Information




Namespace Controllers


    ' GET: api/{GUID service}/state/trade/exchangeReferenceController
    <Route("StateBaseApi")>
    Public Class ExchangeReferenceController

        Inherits ApiController



        ''' <summary>
        ''' This method provide to return all exchange reference
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValues(ByVal securityToken As String, ByVal exchangeId As Integer) As ExchangeReferenceListResponseModel
            Dim ownerId As String = "ExchangeReferenceController-" & Guid.NewGuid.ToString
            Dim result As New ExchangeReferenceListResponseModel
            Dim response As String = ""
            Dim enter As Boolean = False
            Try
                If (CHCSidechainServiceLibrary.AreaCommon.Main.serviceInformation.currentStatus = InternalServiceInformation.EnumInternalServiceState.started) Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeReferenceController.GetValues", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                    enter = True
                    response = CHCSidechainServiceLibrary.AreaCommon.Main.environment.adminToken.check(securityToken)

                    If (response.Length = 0) Then
                        result.value = AreaCommon.state.exchangeReferencesEngine.list(exchangeId, ownerId)

                        CHCSidechainServiceLibrary.AreaCommon.Main.updateLastGetServiceInformation = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.missingAuthorization
                        result.errorDescription = response
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                result.responseStatus = RemoteResponse.EnumResponseStatus.inError
                result.errorDescription = "503 - Generic Error"

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeReferenceController.GetValues", ownerId, ex.Message)
            Finally
                If enter Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeReferenceController.GetValues", ownerId)
                End If
            End Try

            result.responseTime = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()

            Return result
        End Function

        ''' <summary>
        ''' This method provide to get an information response model of an exchange 
        ''' </summary>
        ''' <param name="securityToken"></param>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Function GetValue(ByVal securityToken As String, ByVal exchangeId As Integer, ByVal urlType As ExchangeReferenceStructure.TypeReferenceEnumeration) As ExchangeReferenceResponseModel
            Dim ownerId As String = "ExchangeReferenceController-" & Guid.NewGuid.ToString
            Dim result As New ExchangeReferenceResponseModel
            Dim response As String = ""
            Dim enter As Boolean = False
            Try
                If (CHCSidechainServiceLibrary.AreaCommon.Main.serviceInformation.currentStatus = InternalServiceInformation.EnumInternalServiceState.started) Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeReferenceController.GetValue", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                    enter = True
                    response = CHCSidechainServiceLibrary.AreaCommon.Main.environment.adminToken.check(securityToken)

                    If (response.Length = 0) Then
                        result.value = AreaCommon.state.exchangeReferencesEngine.select(exchangeId, urlType, ownerId)

                        CHCSidechainServiceLibrary.AreaCommon.Main.updateLastGetServiceInformation = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.missingAuthorization
                        result.errorDescription = response
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                result.responseStatus = RemoteResponse.EnumResponseStatus.inError
                result.errorDescription = "503 - Generic Error"

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeReferenceController.GetValue", ownerId, ex.Message)
            Finally
                If enter Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeReferenceController.GetValue", ownerId)
                End If
            End Try

            result.responseTime = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()

            Return result
        End Function

        ''' <summary>
        ''' This method provide to add a new exchange reference into list
        ''' </summary>
        ''' <param name="securityToken"></param>
        ''' <param name="[name]"></param>
        Public Function PostValue(ByVal securityToken As String, <FromBody()> ByVal data As ExchangeReferenceStructure) As BaseRemoteResponse
            Dim ownerId As String = "ExchangeReferenceController-" & Guid.NewGuid.ToString
            Dim result As New BaseRemoteResponse
            Dim response As String = ""
            Dim enter As Boolean = False
            Try
                If (CHCSidechainServiceLibrary.AreaCommon.Main.serviceInformation.currentStatus = InternalServiceInformation.EnumInternalServiceState.started) Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeReferenceController.PostValue", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                    enter = True
                    response = CHCSidechainServiceLibrary.AreaCommon.Main.environment.adminToken.check(securityToken)

                    If (response.Length = 0) Then
                        If Not AreaCommon.state.exchangeReferencesEngine.addIfMissing(data, ownerId) Then
                            result.responseStatus = BaseRemoteResponse.EnumResponseStatus.inError

                            result.errorDescription = "Problem during add a new exchange reference"
                        End If

                        CHCSidechainServiceLibrary.AreaCommon.Main.updateLastGetServiceInformation = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.missingAuthorization
                        result.errorDescription = response
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                result.responseStatus = RemoteResponse.EnumResponseStatus.inError
                result.errorDescription = "503 - Generic Error"

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeReferenceController.PostValue", ownerId, ex.Message)
            Finally
                If enter Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeReferenceController.PostValue", ownerId)
                End If
            End Try

            Return result
        End Function

        ''' <summary>
        ''' This method provide to update an existing element
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="value"></param>
        Public Function PutValue(ByVal securityToken As String, <FromBody()> ByVal data As ExchangeReferenceStructure) As BaseRemoteResponse
            Dim ownerId As String = "ExchangeReferenceController-" & Guid.NewGuid.ToString
            Dim result As New BaseRemoteResponse
            Dim response As String = ""
            Dim enter As Boolean = False
            Try
                If (CHCSidechainServiceLibrary.AreaCommon.Main.serviceInformation.currentStatus = InternalServiceInformation.EnumInternalServiceState.started) Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeReferenceController.PutValue", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                    enter = True
                    response = CHCSidechainServiceLibrary.AreaCommon.Main.environment.adminToken.check(securityToken)

                    If (response.Length = 0) Then
                        If Not AreaCommon.state.exchangeReferencesEngine.updateExchangeReference(data.exchangeId, data.urlType, data.url, ownerId) Then
                            result.responseStatus = RemoteResponse.EnumResponseStatus.inError

                            result.errorDescription = "999 - Cannot update data (check id)"
                        End If

                        CHCSidechainServiceLibrary.AreaCommon.Main.updateLastGetServiceInformation = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.missingAuthorization
                        result.errorDescription = response
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeReferenceController.PutValue", ownerId, ex.Message)
            Finally
                If enter Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeReferenceController.PutValue", ownerId)
                End If
            End Try

            Return result
        End Function

        ''' <summary>
        ''' This method provide to delete an existing element
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="value"></param>
        Public Function DeleteValue(ByVal securityToken As String, <FromBody()> ByVal data As ExchangeReferenceStructure) As BaseRemoteResponse
            Dim ownerId As String = "ExchangeReferenceController-" & Guid.NewGuid.ToString
            Dim result As New BaseRemoteResponse
            Dim response As String = ""
            Dim enter As Boolean = False
            Try
                If (CHCSidechainServiceLibrary.AreaCommon.Main.serviceInformation.currentStatus = InternalServiceInformation.EnumInternalServiceState.started) Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeReferenceController.DeleteValue", ownerId,, CHCModelsLibrary.AreaModel.Log.AccessTypeEnumeration.api)

                    enter = True
                    response = CHCSidechainServiceLibrary.AreaCommon.Main.environment.adminToken.check(securityToken)

                    If (response.Length = 0) Then
                        If Not AreaCommon.state.exchangeReferencesEngine.delete(data.exchangeId, data.urlType, ownerId) Then
                            result.responseStatus = RemoteResponse.EnumResponseStatus.inError

                            result.errorDescription = "999 - Cannot delete data (check id or it used)"
                        End If

                        CHCSidechainServiceLibrary.AreaCommon.Main.updateLastGetServiceInformation = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.missingAuthorization
                        result.errorDescription = response
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeReferenceController.DeleteValue", ownerId, ex.Message)
            Finally
                If enter Then
                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeReferenceController.DeleteValue", ownerId)
                End If
            End Try

            Return result
        End Function

    End Class


End Namespace
