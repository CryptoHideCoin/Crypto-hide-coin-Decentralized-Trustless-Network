﻿Option Compare Text
Option Explicit On

Imports CHCCommonLibrary.AreaEngine.CommandLine




Namespace AreaCommon.Command

    ''' <summary>
    ''' This class manage the command info 
    ''' </summary>
    Public Class CommandInfo : Implements CommandModel

        Private Property _Command As CommandStructure

        Public Event WriteLine(ByVal message As String) Implements CommandModel.WriteLine
        Public Event Process(ByVal applicationName As String, ByVal commandLine As String) Implements CommandModel.Process
        Public Event IntegrityApplication(ByVal fileName As String) Implements CommandModel.IntegrityApplication
        Public Event RaiseError(ByVal message As String) Implements CommandModel.RaiseError
        Public Event ReadKey() Implements CommandModel.ReadKey


        Private Property CommandModel_command As CommandStructure Implements CommandModel.command
            Get
                Return _Command
            End Get
            Set(value As CommandStructure)
                _Command = value
            End Set
        End Property

        Private Function CommandModel_run() As Boolean Implements CommandModel.run
            RaiseEvent WriteLine("Crypto Hide Coin Decentralized Trustless Network - Command Line Executor")
            RaiseEvent WriteLine("Free open source software")
            RaiseEvent WriteLine("(2022) Crypto Technology Alliances")
            RaiseEvent WriteLine("Release " & My.Application.Info.Version.ToString())

            Return True
        End Function

    End Class

End Namespace
