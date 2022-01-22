﻿Option Compare Text
Option Explicit On

Imports System.Web.Http

Imports CHCCommonLibrary.AreaCommon.Models.General
Imports CHCProtocolLibrary.AreaCommon




Namespace Controllers

    ' GET: api/{GUID service}/supply/supplyInformationBaseController
    <Route("SupplyApi")>
    Public Class supplyInformationBaseController

        Inherits ApiController



        ''' <summary>
        ''' This method provide to get a value of Information Base of a Supply
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValue() As Models.Supply.Response.SupplyResponseModel
            Dim result As New Models.Supply.Response.SupplyResponseModel
            Dim privateKeyRAW As String
            Try
                result.requestTime = CHCCommonLibrary.AreaEngine.Miscellaneous.atMomentGMT()

                If (AreaCommon.state.serviceInformation.currentStatus = Models.Service.InternalServiceInformation.EnumInternalServiceState.started) Then
                    If (AreaCommon.state.network.position = CHCRuntimeChainLibrary.AreaRuntime.AppState.EnumConnectionState.genesisOperation) Or
                       (AreaCommon.state.network.position = CHCRuntimeChainLibrary.AreaRuntime.AppState.EnumConnectionState.onLine) Then

                        privateKeyRAW = AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity).privateKey

                        result.masterNodePublicAddress = AreaCommon.state.network.publicAddressIdentity
                        result.integrityTransactionChain.coordinate = AreaCommon.state.runTimeState.supply.coordinate
                        result.integrityTransactionChain.hash = AreaCommon.state.runTimeState.supply.hash
                        result.integrityTransactionChain.progressiveHash = AreaCommon.state.runTimeState.supply.progressiveHash
                        result.integrityTransactionChain.registrationTimeStamp = AreaCommon.state.runTimeState.supply.registrationTimeStamp
                        result.value = AreaCommon.state.runTimeState.supply.value

                        result.signature = CHCProtocolLibrary.AreaWallet.Support.WalletAddressEngine.createSignature(privateKeyRAW, result.getHash())
                    Else
                        result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                    End If
                Else
                    result.responseStatus = RemoteResponse.EnumResponseStatus.systemOffline
                End If
            Catch ex As Exception
                result.responseStatus = RemoteResponse.EnumResponseStatus.inError
                result.errorDescription = "503 - Generic Error"
            End Try

            result.responseTime = CHCCommonLibrary.AreaEngine.Miscellaneous.atMomentGMT()

            Return result
        End Function

    End Class

End Namespace