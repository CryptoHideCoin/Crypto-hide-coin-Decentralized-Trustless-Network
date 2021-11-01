﻿Option Compare Text
Option Explicit On




Namespace AreaData


    Partial Module Network

        Public dataNetwork As New CHCProtocolLibrary.AreaCommon.Models.Network.BuildNetworkModel
        Private _Proceed As Boolean = True
        Private _CompleteProcess As Boolean = True


        ''' <summary>
        ''' This method provide to rebuild command list after start a genesis state
        ''' </summary>
        ''' <returns></returns>
        Private Function rebuildCommandList() As Boolean
            With AreaCommon.state.currentService
                .listAvailableCommand.Clear()

                .listAvailableCommand.Add(CHCProtocolLibrary.AreaCommon.Models.Administration.EnumActionAdministration.cancelCurrentAction)
            End With

            Return True
        End Function

        ''' <summary>
        ''' This method provide to initialize a property of the network
        ''' </summary>
        ''' <returns></returns>
        Private Function setGenesisNetworkState() As Boolean
            Try
                AreaCommon.log.track("BuildNetwork.setNetworkState", "Begin")

                With AreaCommon.state.network
                    .publicAddressIdentity = AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity).publicAddress
                    .publicAddressRefund = AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.refund).publicAddress
                    .publicAddresstWarranty = AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.warranty).publicAddress
                    .coinWarranty = 1
                    .connectedMoment = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    .role = CHCRuntimeChainLibrary.AreaRuntime.AppState.EnumMasternodeRole.fullRole
                End With

                AreaCommon.log.track("BuildNetwork.setNetworkState", "Complete")

                Return True
            Catch ex As Exception
                AreaCommon.log.track("BuildNetwork.setNetworkState", ex.Message, "fatal")

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to create a new Ledger 
        ''' </summary>
        ''' <returns></returns>
        Private Function createLedger() As Boolean
            Try
                AreaCommon.log.track("BuildNetwork.createLedger", "Begin")

                With AreaCommon.state
                    .runtimeState.activeNetwork.networkCreationDate = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    .currentBlockLedger.log = AreaCommon.log
                    .currentBlockLedger.identifyBlockChain = "B0"

                    Return .currentBlockLedger.init(AreaCommon.paths.workData.currentVolume.ledger, AreaCommon.state.runtimeState.activeNetwork.networkCreationDate)
                End With

                AreaCommon.log.track("BuildNetwork.createLedger", "Complete")

                Return True
            Catch ex As Exception
                AreaCommon.log.track("BuildNetwork.createLedger", ex.Message, "fatal")

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to create a new state
        ''' </summary>
        ''' <returns></returns>
        Private Function createState() As Boolean
            Return AreaCommon.state.runtimeState.init(AreaCommon.paths.workData.state.db)
        End Function

        ''' <summary>
        ''' This method provide to create a virtual node list
        ''' </summary>
        ''' <returns></returns>
        Private Function createVirtualNodeList() As Boolean
            Try
                AreaCommon.log.track("BuildNetwork.createVirtualNodeList", "Begin")

                With AreaCommon.state.runtimeState.addNewNode("Primary")
                    .dayConnection = 0

                    If AreaCommon.settings.data.intranetMode Then
                        .ipAddress = AreaCommon.state.localIpAddress
                    Else
                        .ipAddress = AreaCommon.state.publicIpAddress
                    End If

                    .lastConnectionTimeStamp = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    .role = AreaState.ChainStateEngine.DataMasternode.roleMasterNode.fullService
                    .startConnectionTimeStamp = CHCCommonLibrary.AreaEngine.Miscellaneous.timeStampFromDateTime()
                    .votePoint = 1
                    .warrantyCoin = 1
                    .warrantyPublicAddress = .identityPublicAddress
                    .refundPublicAddress = .identityPublicAddress
                End With

                AreaCommon.log.track("BuildNetwork.createVirtualNodeList", "Complete")

                Return True
            Catch ex As Exception
                AreaCommon.log.track("BuildNetwork.createVirtualNodeList", ex.Message, "fatal")

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to create the request and wait the approvation
        ''' </summary>
        ''' <returns></returns>
        Private Function createAndWaitRequest(ByRef requestCode As String) As Boolean
            Try
                Do While (AreaCommon.flow.getRequest(requestCode).position.process <> AreaFlow.EnumOperationPosition.inWork)
                    System.Threading.Thread.Sleep(10)
                Loop

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function


        Private Sub manageA0x3()
            If _Proceed Then
                Dim commandA0x3 As New AreaProtocol.A0x3.Manager

                commandA0x3.log = AreaCommon.log
                commandA0x3.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA0x3.init(AreaCommon.paths, dataNetwork.primaryAsset, .publicAddress, .privateKey)
                End With
            End If
        End Sub

        Private Sub manageA0x4()
            If _Proceed Then
                Dim commandA0x4 As New AreaProtocol.A0x4.Manager

                commandA0x4.log = AreaCommon.log
                commandA0x4.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA0x4.init(AreaCommon.paths, dataNetwork.transactionChainParameter, .publicAddress, .privateKey)
                End With
            End If
        End Sub

        Private Sub manageA0x5()
            If _Proceed Then
                Dim commandA0x5 As New AreaProtocol.A0x5.Manager

                commandA0x5.log = AreaCommon.log
                commandA0x5.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA0x5.init(AreaCommon.paths, dataNetwork.privacyPolicy.content, .publicAddress, .privateKey)
                End With
            End If
        End Sub

        Private Sub manageA0x6()
            If _Proceed Then
                Dim commandA0x6 As New AreaProtocol.A0x6.Manager

                commandA0x6.log = AreaCommon.log
                commandA0x6.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA0x6.init(AreaCommon.paths, dataNetwork.generalCondition.content, .publicAddress, .privateKey)
                End With
            End If
        End Sub

        Private Sub manageA0x7()
            If _Proceed Then
                Dim commandA0x7 As New AreaProtocol.A0x7.Manager

                commandA0x7.log = AreaCommon.log
                commandA0x7.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA0x7.init(AreaCommon.paths, dataNetwork.refundPlan, .publicAddress, .privateKey)
                End With
            End If
        End Sub

        Private Sub manageA1x0()
            If _Proceed Then
                Dim commandA1x0 As New AreaProtocol.A1x0.Manager

                commandA1x0.log = AreaCommon.log
                commandA1x0.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x0.init(AreaCommon.paths, AreaCommon.state.internalInformation.chainName, .publicAddress, .privateKey)
                End With

                commandA1x0 = Nothing
            End If
        End Sub

        Private Sub manageA1x1()
            If _Proceed Then
                Dim commandA1x1 As New AreaProtocol.A1x1.Manager

                commandA1x1.log = AreaCommon.log
                commandA1x1.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x1.init(AreaCommon.paths, AreaCommon.Customize.chainDescription, .publicAddress, .privateKey)
                End With

                commandA1x1 = Nothing
            End If
        End Sub

        Private Sub manageA1x2()
            If _Proceed Then
                Dim commandA1x2 As New AreaProtocol.A1x2.Manager

                commandA1x2.log = AreaCommon.log
                commandA1x2.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x2.init(AreaCommon.paths, AreaCommon.Customize.chainProtocolDocument, .publicAddress, .privateKey)
                End With

                commandA1x2 = Nothing
            End If
        End Sub

        Private Sub manageA1x3()
            If _Proceed Then
                Dim commandA1x3 As New AreaProtocol.A1x3.Manager

                commandA1x3.log = AreaCommon.log
                commandA1x3.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x3.init(AreaCommon.paths, "BaseCommonServiceChain", .publicAddress, .privateKey)
                End With

                commandA1x3 = Nothing
            End If
        End Sub

        Private Sub manageA1x4()
            If _Proceed Then
                Dim commandA1x4 As New AreaProtocol.A1x4.Manager

                commandA1x4.log = AreaCommon.log
                commandA1x4.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x4.init(AreaCommon.paths, New CHCProtocolLibrary.AreaCommon.Models.Network.ItemPriceTableListModel, .publicAddress, .privateKey)
                End With

                commandA1x4 = Nothing
            End If
        End Sub

        Private Sub manageA1x5()
            If _Proceed Then
                Dim commandA1x5 As New AreaProtocol.A1x5.Manager

                commandA1x5.log = AreaCommon.log
                commandA1x5.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x5.init(AreaCommon.paths, "", .publicAddress, .privateKey)
                End With

                commandA1x5 = Nothing
            End If
        End Sub

        Private Sub manageA1x6()
            If _Proceed Then
                Dim commandA1x6 As New AreaProtocol.A1x6.Manager

                commandA1x6.log = AreaCommon.log
                commandA1x6.currentService = AreaCommon.state.currentService

                With AreaCommon.state.keys.key(TransactionChainLibrary.AreaEngine.KeyPair.KeysEngine.KeyPair.enumWalletType.identity)
                    _Proceed = commandA1x6.init(AreaCommon.paths, "", .publicAddress, .privateKey)
                End With

                commandA1x6 = Nothing

                _CompleteProcess = True
            End If
        End Sub

        Public Function buildNetwork() As Boolean
            Dim proceed As Boolean = True
            Try
                AreaCommon.log.trackIntoConsole("Build Network start")
                AreaCommon.log.track("BuildNetwork.run", "Begin")

                AreaCommon.state.network.position = CHCRuntimeChainLibrary.AreaRuntime.AppState.EnumConnectionState.genesisOperation

                If proceed Then proceed = AreaCommon.flow.init()
                If proceed Then proceed = setGenesisNetworkState()
                If proceed Then proceed = createLedger()
                If proceed Then proceed = rebuildCommandList()
                If proceed Then proceed = createState()
                If proceed Then proceed = createVirtualNodeList()
                If proceed Then proceed = createAndWaitRequest(AreaProtocol.A0x0.Manager.createRequest(dataNetwork.name))
                If proceed Then proceed = createAndWaitRequest(AreaProtocol.A0x1.Manager.createRequest(dataNetwork.whitePaper.content))
                If proceed Then proceed = createAndWaitRequest(AreaProtocol.A0x2.Manager.createRequest(dataNetwork.yellowPaper.content))

                'manageA0x3()
                'manageA0x4()
                'manageA0x5()
                'manageA0x6()
                'manageA0x7()

                'manageA1x0()
                'manageA1x1()
                'manageA1x2()
                'manageA1x3()
                'manageA1x4()
                'manageA1x5()
                'manageA1x6()

                If proceed Then
                    AreaCommon.log.trackIntoConsole("Build Network complete")
                Else
                    AreaCommon.log.trackIntoConsole("Build Network failed")
                End If
                AreaCommon.log.track("BuildNetwork.run", "Complete")

                Return True
            Catch ex As Exception
                AreaCommon.log.track("BuildNetwork.run", ex.Message, "fatal")

                Return False
            Finally
                If proceed Then
                    AreaCommon.state.currentService.currentAction.reset()
                End If

                AreaCommon.state.currentService.currentRunCommand = CHCProtocolLibrary.AreaCommon.Models.Administration.EnumActionAdministration.notDefined
                AreaCommon.state.currentService.requestCancelCurrentRunCommand = False
            End Try
        End Function

    End Module

End Namespace