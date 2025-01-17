﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoMaintenanceSettings
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.everyChangeFile = New CHCSupportUIControls.NumericText()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.everyLabel = New System.Windows.Forms.Label()
        Me.logRotateSettings = New System.Windows.Forms.GroupBox()
        Me.keepFile = New System.Windows.Forms.ComboBox()
        Me.keepFileLabel = New System.Windows.Forms.Label()
        Me.keepLast = New System.Windows.Forms.ComboBox()
        Me.keepLastLabel = New System.Windows.Forms.Label()
        Me.confirmButton = New System.Windows.Forms.Button()
        Me.registryEventsGroup = New System.Windows.Forms.GroupBox()
        Me.keepLastRegistry = New System.Windows.Forms.ComboBox()
        Me.keepLastRegistryLabel = New System.Windows.Forms.Label()
        Me.counterGroupBox = New System.Windows.Forms.GroupBox()
        Me.keepLastCounter = New System.Windows.Forms.ComboBox()
        Me.keepLastCounterLabel = New System.Windows.Forms.Label()
        Me.logRotateSettings.SuspendLayout()
        Me.registryEventsGroup.SuspendLayout()
        Me.counterGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'everyChangeFile
        '
        Me.everyChangeFile.currentFormat = ""
        Me.everyChangeFile.Location = New System.Drawing.Point(68, 17)
        Me.everyChangeFile.locationCode = "it-IT"
        Me.everyChangeFile.Name = "everyChangeFile"
        Me.everyChangeFile.Size = New System.Drawing.Size(100, 21)
        Me.everyChangeFile.TabIndex = 9
        Me.everyChangeFile.Text = "0"
        Me.everyChangeFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.everyChangeFile.useDecimal = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(174, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "hour/s"
        '
        'everyLabel
        '
        Me.everyLabel.AutoSize = True
        Me.everyLabel.Location = New System.Drawing.Point(22, 20)
        Me.everyLabel.Name = "everyLabel"
        Me.everyLabel.Size = New System.Drawing.Size(40, 13)
        Me.everyLabel.TabIndex = 7
        Me.everyLabel.Text = "Every"
        '
        'logRotateSettings
        '
        Me.logRotateSettings.Controls.Add(Me.keepFile)
        Me.logRotateSettings.Controls.Add(Me.keepFileLabel)
        Me.logRotateSettings.Controls.Add(Me.keepLast)
        Me.logRotateSettings.Controls.Add(Me.keepLastLabel)
        Me.logRotateSettings.Location = New System.Drawing.Point(12, 48)
        Me.logRotateSettings.Name = "logRotateSettings"
        Me.logRotateSettings.Size = New System.Drawing.Size(296, 95)
        Me.logRotateSettings.TabIndex = 10
        Me.logRotateSettings.TabStop = False
        Me.logRotateSettings.Text = "Log rotate"
        '
        'keepFile
        '
        Me.keepFile.FormattingEnabled = True
        Me.keepFile.Items.AddRange(New Object() {"Nothing", "Only main"})
        Me.keepFile.Location = New System.Drawing.Point(119, 56)
        Me.keepFile.Name = "keepFile"
        Me.keepFile.Size = New System.Drawing.Size(148, 21)
        Me.keepFile.TabIndex = 3
        '
        'keepFileLabel
        '
        Me.keepFileLabel.AutoSize = True
        Me.keepFileLabel.Location = New System.Drawing.Point(53, 59)
        Me.keepFileLabel.Name = "keepFileLabel"
        Me.keepFileLabel.Size = New System.Drawing.Size(57, 13)
        Me.keepFileLabel.TabIndex = 2
        Me.keepFileLabel.Text = "Keep file"
        '
        'keepLast
        '
        Me.keepLast.FormattingEnabled = True
        Me.keepLast.Items.AddRange(New Object() {"Day", "Week", "Month", "Year"})
        Me.keepLast.Location = New System.Drawing.Point(119, 29)
        Me.keepLast.Name = "keepLast"
        Me.keepLast.Size = New System.Drawing.Size(148, 21)
        Me.keepLast.TabIndex = 1
        '
        'keepLastLabel
        '
        Me.keepLastLabel.AutoSize = True
        Me.keepLastLabel.Location = New System.Drawing.Point(53, 32)
        Me.keepLastLabel.Name = "keepLastLabel"
        Me.keepLastLabel.Size = New System.Drawing.Size(60, 13)
        Me.keepLastLabel.TabIndex = 0
        Me.keepLastLabel.Text = "Keep last"
        '
        'confirmButton
        '
        Me.confirmButton.Location = New System.Drawing.Point(327, 12)
        Me.confirmButton.Name = "confirmButton"
        Me.confirmButton.Size = New System.Drawing.Size(75, 44)
        Me.confirmButton.TabIndex = 11
        Me.confirmButton.Text = "Confirm"
        Me.confirmButton.UseVisualStyleBackColor = True
        '
        'registryEventsGroup
        '
        Me.registryEventsGroup.Controls.Add(Me.keepLastRegistry)
        Me.registryEventsGroup.Controls.Add(Me.keepLastRegistryLabel)
        Me.registryEventsGroup.Location = New System.Drawing.Point(12, 149)
        Me.registryEventsGroup.Name = "registryEventsGroup"
        Me.registryEventsGroup.Size = New System.Drawing.Size(296, 66)
        Me.registryEventsGroup.TabIndex = 12
        Me.registryEventsGroup.TabStop = False
        Me.registryEventsGroup.Text = "Registry events"
        '
        'keepLastRegistry
        '
        Me.keepLastRegistry.FormattingEnabled = True
        Me.keepLastRegistry.Items.AddRange(New Object() {"Day", "Week", "Month", "Year"})
        Me.keepLastRegistry.Location = New System.Drawing.Point(119, 29)
        Me.keepLastRegistry.Name = "keepLastRegistry"
        Me.keepLastRegistry.Size = New System.Drawing.Size(148, 21)
        Me.keepLastRegistry.TabIndex = 1
        '
        'keepLastRegistryLabel
        '
        Me.keepLastRegistryLabel.AutoSize = True
        Me.keepLastRegistryLabel.Location = New System.Drawing.Point(53, 32)
        Me.keepLastRegistryLabel.Name = "keepLastRegistryLabel"
        Me.keepLastRegistryLabel.Size = New System.Drawing.Size(60, 13)
        Me.keepLastRegistryLabel.TabIndex = 0
        Me.keepLastRegistryLabel.Text = "Keep last"
        '
        'counterGroupBox
        '
        Me.counterGroupBox.Controls.Add(Me.keepLastCounter)
        Me.counterGroupBox.Controls.Add(Me.keepLastCounterLabel)
        Me.counterGroupBox.Location = New System.Drawing.Point(12, 221)
        Me.counterGroupBox.Name = "counterGroupBox"
        Me.counterGroupBox.Size = New System.Drawing.Size(296, 66)
        Me.counterGroupBox.TabIndex = 13
        Me.counterGroupBox.TabStop = False
        Me.counterGroupBox.Text = "Counter rotate"
        '
        'keepLastCounter
        '
        Me.keepLastCounter.FormattingEnabled = True
        Me.keepLastCounter.Items.AddRange(New Object() {"Day", "Week", "Month", "Year"})
        Me.keepLastCounter.Location = New System.Drawing.Point(119, 29)
        Me.keepLastCounter.Name = "keepLastCounter"
        Me.keepLastCounter.Size = New System.Drawing.Size(148, 21)
        Me.keepLastCounter.TabIndex = 1
        '
        'keepLastCounterLabel
        '
        Me.keepLastCounterLabel.AutoSize = True
        Me.keepLastCounterLabel.Location = New System.Drawing.Point(53, 32)
        Me.keepLastCounterLabel.Name = "keepLastCounterLabel"
        Me.keepLastCounterLabel.Size = New System.Drawing.Size(60, 13)
        Me.keepLastCounterLabel.TabIndex = 0
        Me.keepLastCounterLabel.Text = "Keep last"
        '
        'AutoMaintenanceSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(412, 308)
        Me.Controls.Add(Me.counterGroupBox)
        Me.Controls.Add(Me.registryEventsGroup)
        Me.Controls.Add(Me.confirmButton)
        Me.Controls.Add(Me.logRotateSettings)
        Me.Controls.Add(Me.everyChangeFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.everyLabel)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AutoMaintenanceSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Auto maintenance settings"
        Me.logRotateSettings.ResumeLayout(False)
        Me.logRotateSettings.PerformLayout()
        Me.registryEventsGroup.ResumeLayout(False)
        Me.registryEventsGroup.PerformLayout()
        Me.counterGroupBox.ResumeLayout(False)
        Me.counterGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents everyChangeFile As CHCSupportUIControls.NumericText
    Friend WithEvents Label2 As Label
    Friend WithEvents everyLabel As Label
    Friend WithEvents logRotateSettings As GroupBox
    Friend WithEvents keepFile As ComboBox
    Friend WithEvents keepFileLabel As Label
    Friend WithEvents keepLast As ComboBox
    Friend WithEvents keepLastLabel As Label
    Friend WithEvents confirmButton As Button
    Friend WithEvents registryEventsGroup As GroupBox
    Friend WithEvents keepLastRegistry As ComboBox
    Friend WithEvents keepLastRegistryLabel As Label
    Friend WithEvents counterGroupBox As GroupBox
    Friend WithEvents keepLastCounter As ComboBox
    Friend WithEvents keepLastCounterLabel As Label
End Class
