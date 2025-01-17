﻿Option Compare Text
Option Explicit On

Imports CHCCommonLibrary.AreaEngine.Encryption






Namespace AreaCommon.Masternode

    ''' <summary>
    ''' This enumerate contain the evaluation response
    ''' </summary>
    Public Enum EvaluationResponse
        notDeterminate
        approval
        rejected
        abstained
    End Enum

    ''' <summary>
    ''' This class is used to track a senders (broadcast) of a request
    ''' </summary>
    Public Class MasternodeSenders

        Public Class MasternodeSender

            Public publicAddress As String = ""
            Public publicAddressIP As String = ""

            Public deliveryTimeStamp As Double = 0
            Public lastTry As Double = 0
            Public tryNumber As Byte = 0

        End Class

        Private Property _Masternodes As New List(Of MasternodeSender)

        ''' <summary>
        ''' This method provide to return a list of a active masternode
        ''' </summary>
        ''' <param name="includeMe">Specify if this masternode will include</param>
        ''' <returns></returns>
        Public Shared Function createMasterNodeList(Optional ByVal includeMe As Boolean = False) As MasternodeSenders
            Try
                log.track("MasternodeSenders.createMasterNodeList", "Begin")

                Dim result As New MasternodeSenders
                Dim singleItem As MasternodeSender

                For Each item In state.runTimeState.activeChain.internalNodeList.Values
                    If (item.itsMe And includeMe) Or Not item.itsMe Then
                        singleItem = New MasternodeSender

                        singleItem.publicAddress = item.identityPublicAddress
                        singleItem.publicAddressIP = item.ipAddress

                        result.add(singleItem)
                    End If
                Next

                log.track("MasternodeSenders.createMasterNodeList", "Completed")

                Return result
            Catch ex As Exception
                log.track("MasternodeSenders.createMasterNodeList", ex.Message, "fatal")

                Return New MasternodeSenders
            End Try
        End Function

        ''' <summary>
        ''' This method provide to create a single master node list
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function createSingleMasterNodeList(ByVal publicAddress As String, ByVal publicIPAddress As String) As MasternodeSenders
            Try
                log.track("MasternodeSenders.createMasterNodeList", "Begin")

                Dim result As New MasternodeSenders
                Dim singleItem As MasternodeSender

                singleItem = New MasternodeSender

                singleItem.publicAddress = publicAddress
                singleItem.publicAddressIP = publicIPAddress

                result.add(singleItem)

                log.track("MasternodeSenders.createMasterNodeList", "Completed")

                Return result
            Catch ex As Exception
                log.track("MasternodeSenders.createMasterNodeList", ex.Message, "fatal")

                Return New MasternodeSenders
            End Try
        End Function

        ''' <summary>
        ''' This method provide to add a masternode sender
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function add(ByRef value As MasternodeSender) As Boolean
            Try
                _Masternodes.Add(value)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This property return the count of masternode list
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property count() As Integer
            Get
                Return _Masternodes.Count
            End Get
        End Property

        ''' <summary>
        ''' This property return get a first masternode of a list
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property getFirst() As MasternodeSender
            Get
                If (_Masternodes.Count > 0) Then
                    Return _Masternodes(0)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' This property return getItem in masternode sender
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Public ReadOnly Property getItem(ByVal index As Integer) As MasternodeSender
            Get
                Try
                    Return _Masternodes(index)
                Catch ex As Exception
                    Return New MasternodeSender
                End Try
            End Get
        End Property

        ''' <summary>
        ''' This method provide to remove a first element
        ''' </summary>
        ''' <returns></returns>
        Public Function removeFirst() As Boolean
            Try
                If (_Masternodes.Count > 0) Then
                    _Masternodes.RemoveAt(0)
                End If

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class

    ''' <summary>
    ''' This class is used to collect e manage an information relative an assessment power of a masternode
    ''' </summary>
    Public Class MinimalDataMasternodeList

        Public Class MinimalDataMasternode

            Public Property publicAddress As String = ""
            Public Property publicIPAddress As String = ""
            Public Property power As Double = 0
            Public Property assessmentTimeStamp As Double = 0
            Public Property hash As String = ""
            Public Property signature As String = ""

            ''' <summary>
            ''' This method provide to create a string resultant of a initial request
            ''' </summary>
            ''' <param name="requestHash"></param>
            ''' <param name="model"></param>
            ''' <returns></returns>
            Public Function toString(ByVal requestHash As String, ByVal model As AreaConsensus.EnumModel) As String
                Dim result As String = ""

                result += requestHash

                Select Case model
                    Case AreaConsensus.EnumModel.approved : result += "1"
                    Case AreaConsensus.EnumModel.abstained : result += "2"
                    Case AreaConsensus.EnumModel.rejected : result += "3"
                    Case AreaConsensus.EnumModel.absented : result += "4"
                End Select

                result += publicAddress
                result += assessmentTimeStamp.ToString()
                result += power.ToString()

                Return result
            End Function

            ''' <summary>
            ''' This method provide to create an hash resultant
            ''' </summary>
            ''' <returns></returns>
            Public Function getHash(ByVal requestHash As String, ByVal model As AreaConsensus.EnumModel) As String
                Return HashSHA.generateSHA256(Me.toString(requestHash, model))
            End Function

        End Class

        Private Property _Items As New Dictionary(Of String, MinimalDataMasternode)

        Public Property totalPower As Double = 0

        ''' <summary>
        ''' This method provide to add a new element into internal collection
        ''' </summary>
        ''' <param name="node"></param>
        ''' <param name="assessmentTimeStamp"></param>
        ''' <returns></returns>
        Public Function addNew(ByVal node As ContactDataMasternodeList.ContactDataMasternode, Optional ByVal assessmentTimeStamp As Double = 0) As Boolean
            Try
                Dim item As New MinimalDataMasternode

                item.publicAddress = node.publicAddress
                item.publicIPAddress = node.publicIPAddress
                item.power = node.power

                item.assessmentTimeStamp = assessmentTimeStamp

                If _Items.ContainsKey(node.publicAddress) Then
                    Return True
                End If
                _Items.Add(node.publicAddress, item)

                totalPower += node.power

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public ReadOnly Property count() As Integer
            Get
                Return _Items.Count
            End Get
        End Property

        Public Function getItem(ByVal publicAddress As String) As MinimalDataMasternode
            If _Items.ContainsKey(publicAddress) Then
                Return _Items.Item(publicAddress)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' This method provide to get an item
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Public Function getItem(ByVal index As Integer) As MinimalDataMasternode
            If (index <= _Items.Count) Then
                Return _Items.ElementAt(index - 1).Value
            Else
                Return New MinimalDataMasternode
            End If
        End Function

        ''' <summary>
        ''' This method provide to assign a timestamp of a bulletin into record without assessmentTime
        ''' </summary>
        ''' <param name="assessmentTimeStamp"></param>
        ''' <returns></returns>
        Public Function assignAssessment(ByVal assessmentTimeStamp As Double) As Boolean
            Try
                For Each item In _Items.Values
                    If (item.assessmentTimeStamp = 0) Then
                        item.assessmentTimeStamp = assessmentTimeStamp
                    End If
                Next

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class

    ''' <summary>
    ''' This class is used to collect e manage an information of a list of a node without a response
    ''' </summary>
    Public Class ContactDataMasternodeList

        Public Class ContactDataMasternode

            Inherits MinimalDataMasternodeList.MinimalDataMasternode

            Public Property tryNumberOfRequest As Byte = 0
            Public Property tryFirstTimeStamp As Double = 0

        End Class

        Private Property _Items As New Dictionary(Of String, ContactDataMasternode)

        Public Property totalPower As Double = 0

        ''' <summary>
        ''' This method provide to add a new element in dictionary
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="publicIPAddress"></param>
        ''' <param name="power"></param>
        ''' <returns></returns>
        Public Function addNew(ByVal publicAddress As String, ByVal publicIPAddress As String, ByVal power As Double) As Boolean
            Try
                Dim item As New ContactDataMasternode

                item.publicAddress = publicAddress
                item.publicIPAddress = publicIPAddress
                item.power = power

                If _Items.ContainsKey(publicAddress) Then
                    _Items.Remove(publicAddress)
                End If
                _Items.Add(publicAddress, item)

                totalPower += power

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to add a new element in the list
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function addNew(ByRef value As ContactDataMasternode) As Boolean
            Try
                _Items.Add(value.publicAddress, value)

                totalPower += value.power

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This property return the number of the element of the collection
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property count() As Integer
            Get
                Return _Items.Count
            End Get
        End Property

        ''' <summary>
        ''' This property provide to get an item by index from a collection
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Public Function getItem(ByVal index As Integer) As ContactDataMasternode
            If (index <= _Items.Count) Then
                Return _Items.Values(index)
            Else
                Return New ContactDataMasternode
            End If
        End Function

        ''' <summary>
        ''' This method provide to get an item in a collection
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <returns></returns>
        Public Function getItem(ByVal publicAddress As String) As ContactDataMasternode
            If _Items.ContainsKey(publicAddress) Then
                Return _Items.Item(publicAddress)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' This method provide to check if the public address conteined into collection
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <returns></returns>
        Public Function containKey(ByVal publicAddress As String) As Boolean
            Return _Items.ContainsKey(publicAddress)
        End Function

        ''' <summary>
        ''' This method provide to remove an element into collection
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <returns></returns>
        Public Function remove(ByVal publicAddress As String) As Boolean
            If _Items.ContainsKey(publicAddress) Then
                Dim power As Double = _Items.Item(publicAddress).power

                _Items.Remove(publicAddress)

                totalPower -= power

                Return True
            Else
                Return False
            End If
        End Function

    End Class

    ''' <summary>
    ''' This class contains the expression to power
    ''' </summary>
    Public Class MasternodeEvaluations

        Public Property rejectedNote As String = ""
        Public Property notifyRejected As New AreaCommon.Masternode.MasternodeNotifyRejectedList
        Public Property notExpressed As ContactDataMasternodeList
        Public Property approved As New MinimalDataMasternodeList
        Public Property rejected As New MinimalDataMasternodeList
        Public Property abstained As New MinimalDataMasternodeList
        Public Property absents As New MinimalDataMasternodeList

        Public Property currentChainNodeTotalPower As Double = 0

        ''' <summary>
        ''' This method provide to set a result of evaluation
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="approvedTime"></param>
        ''' <param name="operation"></param>
        ''' <returns></returns>
        Private Function setResult(ByVal publicAddress As String, Optional ByVal operation As EvaluationResponse = EvaluationResponse.notDeterminate, Optional ByVal assessmentTimeStamp As Double = 0) As Boolean
            Try
                Dim proceed As Boolean = False

                log.track("MasternodeEvaluations.setResult", "Begin")

                If Not notExpressed.containKey(publicAddress) Then
                    Return True
                Else
                    Select Case operation
                        Case EvaluationResponse.approval : proceed = approved.addNew(notExpressed.getItem(publicAddress), assessmentTimeStamp)
                        Case EvaluationResponse.rejected : proceed = rejected.addNew(notExpressed.getItem(publicAddress), assessmentTimeStamp)
                        Case EvaluationResponse.abstained : proceed = abstained.addNew(notExpressed.getItem(publicAddress), assessmentTimeStamp)
                        Case Else : proceed = absents.addNew(notExpressed.getItem(publicAddress), assessmentTimeStamp)
                    End Select

                    If proceed Then
                        log.track("MasternodeEvaluations.setResult", "Completed")

                        Return notExpressed.remove(publicAddress)
                    End If
                End If
            Catch ex As Exception
                log.track("MasternodeEvaluations.setResult", ex.Message, "fatal")

                Return False
            Finally
                log.track("MasternodeEvaluations.setResult", "Completed")
            End Try

            Return False
        End Function

        ''' <summary>
        ''' This method provide to set a request as approved
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="approvedTime"></param>
        ''' <returns></returns>
        Public Function setApproved(ByVal publicAddress As String, Optional ByVal approvedTimeStamp As Double = 0) As Boolean
            Return setResult(publicAddress, EvaluationResponse.approval, approvedTimeStamp)
        End Function

        ''' <summary>
        ''' This method provide to set a rejected the request
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="rejectedTimeStamp"></param>
        ''' <returns></returns>
        Public Function setRejected(ByVal publicAddress As String, Optional ByVal rejectedTimeStamp As Double = 0) As Boolean
            Return setResult(publicAddress, EvaluationResponse.rejected, rejectedTimeStamp)
        End Function

        ''' <summary>
        ''' This method provide to set an abstained the request
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="abstainedTimeStamp"></param>
        ''' <returns></returns>
        Public Function setAbstained(ByVal publicAddress As String, Optional abstainedTimeStamp As Double = 0) As Boolean
            Return setResult(publicAddress, EvaluationResponse.abstained, abstainedTimeStamp)
        End Function

        ''' <summary>
        ''' This method provide to set an absent the request
        ''' </summary>
        ''' <param name="publicAddress"></param>
        ''' <param name="absentTimeStamp"></param>
        ''' <returns></returns>
        Public Function setAbsent(ByVal publicAddress As String, Optional ByVal absentTimeStamp As Double = 0) As Boolean
            Return setResult(publicAddress, EvaluationResponse.notDeterminate, absentTimeStamp)
        End Function

    End Class

    ''' <summary>
    ''' This class manage a delivery of a message rejected
    ''' </summary>
    Public Class MasternodeNotifyRejectedList

        Public Property notDelivery As ContactDataMasternodeList
        Public Property delivery As New MinimalDataMasternodeList

    End Class

End Namespace