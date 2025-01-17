﻿Option Compare Text
Option Explicit On

' ****************************************
' Engine: Log Model
' Release Engine: 1.0 
' 
' Date last successfully test: 21/02/2022
' ****************************************



Namespace AreaModel.Network.Communication

    ''' <summary>
    ''' This class contain the component of a service information communication
    ''' </summary>
    Public Class CommunicationServiceInformation

        Public Property useSecure As Boolean = False
        Public Property url As String = ""
        Public Property certificate As String = ""

        ''' <summary>
        ''' This method provide to return a string decode of a secure flag
        ''' </summary>
        ''' <returns></returns>
        <DebuggerHiddenAttribute()> Public Function secureToString() As String
            If useSecure Then
                Return "https://"
            Else
                Return "http://"
            End If
        End Function

        ''' <summary>
        ''' This method provide to compose a URL by parameter
        ''' </summary>
        ''' <param name="intermediateValue"></param>
        ''' <param name="useCertificate"></param>
        ''' <returns></returns>
        <DebuggerHiddenAttribute()> Public Function composeURL(ByVal intermediateValue As String, Optional ByVal useCertificate As Boolean = False) As String
            If useCertificate Then
                Return secureToString() & url & intermediateValue & "?certificate=" & certificate
            Else
                Return secureToString() & url & intermediateValue
            End If
        End Function

    End Class

End Namespace