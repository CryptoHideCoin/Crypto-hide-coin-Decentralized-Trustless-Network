﻿Option Compare Text
Option Explicit On

Imports System.Net








Namespace AreaJob

    ''' <summary>
    ''' This engine provide to get the asset from a Webservice to the provider
    ''' convert to currency structure and research into the archive
    ''' </summary>
    Public Class GetCurrenciesFromWEB

        Private Property _Initialize As Boolean = False
        Private Property _OwnerId As String = ""
        Private Property _Supplier As String = ""
        Private Property _CurrentExchange As String = ""


        ''' <summary>
        ''' This method provide to download a currency from a provider
        ''' </summary>
        Private Async Sub downloadCurrencies(ByVal supplier As String, ByVal exchangeId As Integer)
            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("GetCurrenciesFromWEB.downloadCurrencies", _OwnerId)

                Select Case _CurrentExchange.ToLower
                    Case "Coinbase".ToLower : Await Task.Run(Sub() AreaProvider.CoinbaseProvider.Worker.searchNewCurrenciesAsync(supplier, exchangeId))
                End Select
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("GetCurrenciesFromWEB.downloadCurrencies", _OwnerId, ex.Message)
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("GetCurrenciesFromWEB.downloadCurrencies", _OwnerId)
            End Try
        End Sub

        ''' <summary>
        ''' This method provide to download from exchange (automatic)
        ''' </summary>
        ''' <param name="exchangeId"></param>
        ''' <returns></returns>
        Public Function downloadFromExchange(ByVal id As Integer, ByVal [name] As String, ByVal manual As Boolean) As Boolean
            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("GetCurrenciesFromWEB.automaticDownloadFromExchange", _OwnerId)

                If _Initialize Then
                    _CurrentExchange = [name]

                    If manual Then
                        _Supplier = "Manual job"
                    Else
                        _Supplier = "Automatic job"
                    End If

                    downloadCurrencies(_Supplier, id)

                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("GetCurrenciesFromWEB.automaticDownloadFromExchange", _OwnerId, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("GetCurrenciesFromWEB.automaticDownloadFromExchange", _OwnerId)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to initialize the engine
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Public Function init() As Boolean
            Try
                _OwnerId = "JobDownloadCurrencies"

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("GetCurrenciesFromWEB.init", _OwnerId)

                _Initialize = True

                Return True
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("GetCurrenciesFromWEB.init", _OwnerId, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("GetCurrenciesFromWEB.init", _OwnerId)
            End Try
        End Function

    End Class

End Namespace
