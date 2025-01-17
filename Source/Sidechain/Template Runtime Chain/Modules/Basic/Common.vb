﻿Option Compare Text
Option Explicit On

Imports CHCProtocolLibrary.AreaSystem
Imports CHCCommonLibrary.Support




Namespace AreaCommon

    Module moduleMain

        Public paths As New VirtualPathEngine
        Public log As New LogEngine
        Public logRotate As New LogRotateEngine
        Public counter As New CounterEngine
        Public registry As New RegistryEngine
        Public settings As New CHCRuntimeChainLibrary.AreaRuntime.AppSettings
        Public flow As New AreaFlow.FlowEngine
        Public support As New Engine.SupportEngine
        Public consensus As New AreaConsensus.ConsensusEngine
        Public state As New AppState




        ''' <summary>
        ''' This application provides to close application
        ''' </summary>
        Public Sub closeApplication(Optional ByVal notWait As Boolean = False)

            Console.WriteLine("")
            Console.WriteLine("")

            If Not notWait Then

                Console.Write("Press key to continue")
                Console.ReadKey()
                Console.WriteLine("")
                Console.WriteLine("")

            End If

            End

        End Sub


        ''' <summary>
        ''' This method provide to run an external application with parameters
        ''' </summary>
        ''' <param name="applicationName"></param>
        ''' <param name="parameterValue"></param>
        Public Sub executeExternalApplication(ByVal applicationName As String, Optional ByVal parameterValue As String = "")

            Try
                Shell(applicationName & " " & parameterValue, AppWinStyle.NormalFocus)
            Catch ex As Exception
                log.track("moduleMain.ExecuteExternalApplication", "Enable execute an external application; check admin authorizathion", "fatal")
            End Try

        End Sub


        Public Function refreshBatch(ByRef adapterLog As LogEngine) As Boolean
            Try
                adapterLog.track("moduleMain.refreshBatch", "Begin")

                Return logRotate.run(adapterLog)
            Catch ex As Exception
                adapterLog.track("moduleMain.refreshBatch", ex.Message, "Fatal")

                Return False
            Finally
                adapterLog.track("moduleMain.refreshBatch", "Completed")
            End Try
        End Function

    End Module

End Namespace