﻿Option Compare Text
Option Explicit On

' ****************************************
' File: Numberic Text
' Release Engine: 1.0 
' 
' Date last successfully test: 03/10/2021
' ****************************************


Imports CHCProtocolLibrary.AreaWallet.Support








Public Class WalletAddress

    Public Shadows Event TextChanged()

    Private _NoChange As Boolean = False
    Private _DataPath As String = ""


    Private Function getPrivateKey() As String
        Try
            Dim dialogForm As New PrivateKey

            dialogForm.dataPath = dataPath

            If dialogForm.ShowDialog() = DialogResult.OK Then
                Dim checkKey As WalletAddressEngine.KeyPairComplete = WalletAddressEngine.createNew(dialogForm.value, False)

                If (checkKey.official.public = value) Then
                    Return checkKey.raw.private
                Else
                    MessageBox.Show("The private key is not root to Public Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            dialogForm.Dispose()

            dialogForm = Nothing
        Catch ex As Exception
            MessageBox.Show("Error during a adminWalletAddress_GetPrivateKey - " & Err.Description, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return ""
    End Function

    ''' <summary>
    ''' This property get/let the Data Path
    ''' </summary>
    ''' <returns></returns>
    Public Property dataPath() As String
        Get
            Return _DataPath
        End Get
        Set(value As String)
            _DataPath = value

            keyStoreManagerButton.Enabled = (_DataPath.Length > 0)
        End Set
    End Property
    ''' <summary>
    ''' This property get/let the caption
    ''' </summary>
    ''' <returns></returns>
    Public Property caption As String
        Get
            Return addressLabel.Text
        End Get
        Set(value As String)
            addressLabel.Text = value
        End Set
    End Property
    ''' <summary>
    ''' This property get/let the value
    ''' </summary>
    ''' <returns></returns>
    Public Property value As String
        Get
            Return addressText.Text
        End Get
        Set(value As String)
            _NoChange = True
            addressText.Text = value
            _NoChange = False
        End Set
    End Property
    ''' <summary>
    ''' This property get a private key
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property privateKey() As String
        Get
            If (value.Trim.Length > 0) Then

                If WalletAddressEngine.SingleKeyPair.checkFormatPublicAddress(value.Trim) Then
                    Return getPrivateKey()
                ElseIf (value.ToLower.Substring(0, 11).CompareTo("keystoreid:") = 0) Then
                    Return getPrivateKeyFromStore()
                Else
                    MessageBox.Show("Wallet address wrong or missing.", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                Return ""
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' This property set or let the Font Bold value    
    ''' </summary>
    ''' <returns></returns>
    Public Property useFontBold() As Boolean
        Get
            Return addressLabel.Font.Bold
        End Get
        Set(value As Boolean)
            If value Then
                addressLabel.Font = New Font(addressLabel.Font, FontStyle.Bold)
            Else
                addressLabel.Font = New Font(addressLabel.Font, FontStyle.Regular)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This property get a keyPair structure RAW
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property keyPairRAW() As WalletAddressEngine.SingleKeyPair
        Get
            Dim result As New WalletAddressEngine.SingleKeyPair

            If (value.Trim.Length > 0) Then

                If WalletAddressEngine.SingleKeyPair.checkFormatPublicAddress(value.Trim) Then
                    result.public = value.Trim()
                    result.private = getPrivateKey()
                ElseIf (value.ToLower.Substring(0, 11).CompareTo("keystoreid:") = 0) Then
                    Return getKeyPairRAWFromStore()
                Else
                    MessageBox.Show("Wallet address wrong or missing.", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            End If

            Return New WalletAddressEngine.SingleKeyPair
        End Get
    End Property

    ''' <summary>
    ''' This method provide to request to the user the Access Security Key
    ''' </summary>
    ''' <param name="securityValue"></param>
    ''' <returns></returns>
    Private Function requestAccessSecurityKey(ByRef securityValue As String) As Boolean
        Dim response As Boolean = False

        Try
            Dim dialogForm As New RequestPassword

            If (dialogForm.ShowDialog = DialogResult.OK) Then
                securityValue = dialogForm.accessKey

                response = True
            End If

            dialogForm.Dispose()

            dialogForm = Nothing
        Catch ex As Exception
        End Try

        Return response
    End Function
    ''' <summary>
    ''' This method provide to request Authorization Key
    ''' </summary>
    ''' <param name="securityValue"></param>
    ''' <returns></returns>
    Private Function requestAuthorizationKey(ByRef securityValue As String) As Boolean
        Dim response As Boolean = False

        Try
            Dim dialogForm As New RequestPassword

            dialogForm.Text = "Request Authorization Key"
            dialogForm.accessKey = securityValue

            If (dialogForm.ShowDialog = DialogResult.OK) Then
                securityValue = dialogForm.accessKey

                response = True
            End If

            dialogForm.Dispose()

            dialogForm = Nothing
        Catch ex As Exception
        End Try

        Return response
    End Function
    ''' <summary>
    ''' This method provide to read a User Keystore Path
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Function readUserKeyStorePath(ByVal value As String) As String
        Dim path As String = IO.Path.Combine(value, "define.path")

        If IO.File.Exists(path) And (value.Trim.Length > 0) Then
            Return IO.File.ReadAllText(path)
        Else
            Return value
        End If
    End Function
    ''' <summary>
    ''' This method provide to get a Private Key from Keystore
    ''' </summary>
    ''' <returns></returns>
    Private Function getPrivateKeyFromStore() As String
        Try
            Dim engine As New WalletAddressDataEngine
            Dim dataLoaded As Boolean = False
            Dim securityValue As String = ""

            engine.fileName = IO.Path.Combine(readUserKeyStorePath(_DataPath), value.ToLower.Substring(11), "walletAddress.private")

            If Not engine.load() Then
                If requestAccessSecurityKey(securityValue) Then
                    engine.securityKey = securityValue

                    dataLoaded = engine.load()

                    If Not dataLoaded Then
                        MessageBox.Show("The secret key is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                dataLoaded = True
            End If

            If dataLoaded Then
                If engine.data.authorizationKey <> "" Then
                    If requestAuthorizationKey(engine.data.authorizationKey) Then
                        Return WalletAddressEngine.createNew(engine.data.privateRAWKey, True).official.private
                    End If
                Else
                    Return WalletAddressEngine.createNew(engine.data.privateRAWKey, True).official.private
                End If
            End If

            Return ""
        Catch ex As Exception
            MessageBox.Show("Problem during read KeyStore List information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return False
        End Try
    End Function
    ''' <summary>
    ''' This method provide to get a Private Key RAW from Keystore
    ''' </summary>
    ''' <returns></returns>
    Private Function getPrivateKeyRAWFromStore() As String
        Try
            Dim engine As New WalletAddressDataEngine
            Dim dataLoaded As Boolean = False
            Dim securityValue As String = ""

            engine.fileName = IO.Path.Combine(readUserKeyStorePath(_DataPath), value.ToLower.Substring(11), "walletAddress.private")

            If Not engine.load() Then
                If requestAccessSecurityKey(securityValue) Then
                    engine.securityKey = securityValue

                    dataLoaded = engine.load()

                    If Not dataLoaded Then
                        MessageBox.Show("The secret key is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                dataLoaded = True
            End If

            If dataLoaded Then
                If engine.data.authorizationKey <> "" Then
                    If requestAuthorizationKey(engine.data.authorizationKey) Then
                        Return engine.data.privateRAWKey
                    End If
                Else
                    Return engine.data.privateRAWKey
                End If
            End If

            Return ""
        Catch ex As Exception
            MessageBox.Show("Problem during read KeyStore List information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return False
        End Try
    End Function
    ''' <summary>
    ''' This method provide to get Public Address RAW from Keystore
    ''' </summary>
    ''' <returns></returns>
    Private Function getPublicAddressRAWFromStore() As String
        Try
            Dim engine As New WalletAddressDataEngine
            Dim dataLoaded As Boolean = False
            Dim securityValue As String = ""

            engine.fileName = IO.Path.Combine(readUserKeyStorePath(_DataPath), value.ToLower.Substring(11), "walletAddress.private")

            If Not engine.load() Then
                If requestAccessSecurityKey(securityValue) Then
                    engine.securityKey = securityValue

                    dataLoaded = engine.load()

                    If Not dataLoaded Then
                        MessageBox.Show("The secret key is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                dataLoaded = True
            End If

            If dataLoaded Then
                If (engine.data.authorizationKey <> "") Then
                    If requestAuthorizationKey(engine.data.authorizationKey) Then
                        Return engine.data.publicRAWAddress
                    End If
                Else
                    Return engine.data.publicRAWAddress
                End If
            End If

            Return ""
        Catch ex As Exception
            MessageBox.Show("Problem during read KeyStore List information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return False
        End Try
    End Function
    ''' <summary>
    ''' This method provide to get Key pair RAW from Keystore
    ''' </summary>
    ''' <returns></returns>
    Private Function getKeyPairRAWFromStore() As WalletAddressEngine.SingleKeyPair
        Try
            Dim engine As New WalletAddressDataEngine
            Dim dataLoaded As Boolean = False
            Dim securityValue As String = ""
            Dim result As New WalletAddressEngine.SingleKeyPair

            engine.fileName = IO.Path.Combine(readUserKeyStorePath(_DataPath), value.ToLower.Substring(11), "walletAddress.private")

            If Not engine.load() Then
                If requestAccessSecurityKey(securityValue) Then
                    engine.securityKey = securityValue

                    dataLoaded = engine.load()

                    If Not dataLoaded Then
                        MessageBox.Show("The secret key is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                dataLoaded = True
            End If

            If dataLoaded Then
                If (engine.data.authorizationKey <> "") Then
                    If requestAuthorizationKey(engine.data.authorizationKey) Then
                        result.private = engine.data.privateRAWKey
                        result.public = engine.data.publicRAWAddress

                        Return result
                    End If
                Else
                    result.private = engine.data.privateRAWKey
                    result.public = engine.data.publicRAWAddress

                    Return result
                End If
            End If

            Return result
        Catch ex As Exception
            MessageBox.Show("Problem during read KeyStore List information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return New WalletAddressEngine.SingleKeyPair
        End Try
    End Function
    ''' <summary>
    ''' This method provide to get data from a Keystore
    ''' </summary>
    ''' <returns></returns>
    Private Function getDataFromKeyStore() As String
        Try
            Dim keyStoreManager = New KeyStoreEngine

            keyStoreManager.fileName = IO.Path.Combine(dataPath, "keyAddress.list")

            If keyStoreManager.read() Then
                Return keyStoreManager.getFileDetail(value.ToLower.Substring(12))
            End If

            keyStoreManager = Nothing
        Catch ex As Exception
        End Try

        Return ""
    End Function
    ''' <summary>
    ''' This method provide to resize all control
    ''' </summary>
    Private Sub resizeControls()
        Try
            addressLabel.Left = 4
            addressLabel.Width = 129

            keyStoreManagerButton.Left = Width - 68
            keyStoreManagerButton.Width = 65

            addressText.Left = 139
            addressText.Width = Width - 213
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' This event provide to notify the resize of a form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub WalletAddress_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        resizeControls()
    End Sub
    ''' <summary>
    ''' This event notify when the size changed of a form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub WalletAddress_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        resizeControls()
    End Sub
    ''' <summary>
    ''' This event notify when the control repain
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub WalletAddress_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        resizeControls()
    End Sub
    ''' <summary>
    ''' This event's method manage the click in the keyStore manager button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub keyStoreManagerButton_Click(sender As Object, e As EventArgs) Handles keyStoreManagerButton.Click
        Try
            Dim frm As New KeyStoreManager

            frm.dataPath = _DataPath

            If (frm.ShowDialog = DialogResult.OK) Then
                addressText.Text = frm.addressValue
            End If

            frm.Close()
            frm.Dispose()

            frm = Nothing
        Catch ex As Exception
            MessageBox.Show("Error during a keyStoreManagerButton_Click - " & Err.Description, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    ''' This event's method provide to manage a text changed from address text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub addressText_TextChanged(sender As Object, e As EventArgs) Handles addressText.TextChanged
        If Not _NoChange Then
            RaiseEvent TextChanged()
        End If
    End Sub

End Class
