﻿Option Compare Text
Option Explicit On




Namespace AreaCommon


    Module StartUp


        ''' <summary>
        ''' This method provide to print a welcome message into console
        ''' </summary>
        Private Sub printWelcome()

            Dim strTitle As String = "Crypto Hide Coin Decentralized Trustless Peer Admin Service"

            strTitle += " rel." & My.Application.Info.Version.ToString()

            log.trackIntoConsole(strTitle)
            log.trackIntoConsole(New String("=", Len(strTitle)))
            log.trackIntoConsole()
            log.trackIntoConsole("System bootstrap " & CHCCommonLibrary.CHCEngines.Miscellaneous.atMomentGMT() & " (gmt)")

            state.currentApplication = AppState.enumStateApplication.waitingToStart

        End Sub


        ''' <summary>
        ''' This method provide to copy a command Parameters
        ''' </summary>
        ''' <param name="command"></param>
        Private Sub copyCommandParameters(ByRef command As ManageCommandLine)

            Try

                With command.parameters.data

                    If (.certificateClient.Trim.Length > 0) Then settings.data.certificateClient = .certificateClient
                    If (.certificateMasternodeEngine.Trim.Length > 0) Then settings.data.certificateMasternodeEngine = .certificateMasternodeEngine
                    If (.certificateMasternodeStart.Trim.Length > 0) Then settings.data.certificateMasternodeStart = .certificateMasternodeStart
                    If (.dataPath.Trim.Length > 0) Then settings.data.dataPath = .dataPath
                    If (.gui) Then settings.data.gui = .gui
                    If (.noConsoleMessage) Then settings.data.noConsoleMessage = .noConsoleMessage
                    If (.portNumber <> 1122) Then settings.data.portNumber = .portNumber
                    If (.recallStarter) Then settings.data.recallStarter = .recallStarter
                    If (.urlMasternodeEngine.Trim.Length > 0) Then settings.data.urlMasternodeEngine = .urlMasternodeEngine
                    If (.urlMasternodeStart.Trim.Length > 0) Then settings.data.urlMasternodeStart = .urlMasternodeStart
                    If (.walletPublicAddress.Trim.Length > 0) Then settings.data.walletPublicAddress = .walletPublicAddress

                End With

            Catch ex As Exception

            End Try

        End Sub



        ''' <summary>
        ''' This method provide to prepare a startup of application
        ''' </summary>
        ''' <returns></returns>
        Private Function firstProcedureStartup() As Boolean

            Try

                Dim command As New ManageCommandLine
                Dim definePath As String = paths.searchDefinePath()

                command.run(Environment.CommandLine.Split("/"))

                log.noPrintConsole = settings.data.noConsoleMessage

                printWelcome()

                If Not command.haveParameters Then

                    Dim haveSettings As Boolean = False

                    If (definePath.Length() = 0) Then

                        MessageBox.Show("File settings is missing", "Notify problem", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End

                    End If

                    paths.pathBaseData = paths.readDefinePath()

                    settings.fileName = IO.Path.Combine(paths.pathBaseData, "Settings", paths.settingFileName)

                    If settings.read() Then

                        paths.pathBaseData = settings.data.dataPath

                        haveSettings = Not settings.data.gui

                    Else

                        settings.data.dataPath = paths.pathBaseData

                    End If

                    If Not haveSettings Then

                        state.uiVisible = True

                    Else

                        If command.useLastSettings Then

                            copyCommandParameters(command)

                            state.uiVisible = False

                            paths.pathBaseData = settings.data.dataPath

                        End If

                    End If

                Else

                    settings.data = command.parameters.data

                    state.uiVisible = settings.data.gui

                    paths.pathBaseData = settings.data.dataPath

                End If

                command.parameters = Nothing
                command = Nothing

                log.trackIntoConsole("Parameters read")

                If (paths.pathBaseData.Trim().Length = 0) Then

                    If (definePath.Length() = 0) Then

                        MessageBox.Show("File settings is missing", "Notify problem", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End

                    End If

                    paths.pathBaseData = paths.readDefinePath()

                    settings.data.dataPath = paths.pathBaseData

                End If

                log.trackIntoConsole("Root paths set " & paths.pathBaseData)

                If state.uiVisible Then

                    Dim tmp As New Main

                    log.trackIntoConsole("User interface active")

                    tmp.ShowDialog()

                    Return False

                End If

                log.trackIntoConsole("Silent mode active")

                Return True

            Catch ex As Exception

                MessageBox.Show("An error occurrent during firstProcedureStartup " & Err.Description, "Notify problem", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return False

            End Try

        End Function


        ''' <summary>
        ''' This method provide to recall the Starter Service
        ''' </summary>
        Private Sub recallStarter()

            Try

                log.track("moduleMain.recallStarter", "Begin")

                Dim handShakeEngine As New CHCCommonLibrary.CHCEngines.Communication.ProxyWS(Of Models.General.BooleanModel)

                handShakeEngine.url = "http://" & AreaCommon.settings.data.urlMasternodeStart & "/api/v1.0/System/handShake/?serviceAdministrative=true&serviceEngine=false&certificateValue=" & settings.data.certificateMasternodeStart

                If handShakeEngine.getData() Then

                    If Not handShakeEngine.data.value Then

                        log.track("moduleMain.recallStarter", "Cannot connection with the service or wrong certificate", "fatal")

                    End If

                End If

            Catch ex As Exception

                log.track("moduleMain.recallStarter", "Error:" & ex.Message, "fatal")

                CloseApplication()

            Finally

                log.track("moduleMain.recallStarter", "Complete")

            End Try

        End Sub


        ''' <summary>
        ''' This method provide to run the service
        ''' </summary>
        Sub run(Optional ByVal saveSettings As Boolean = False)

            Try

                log.trackIntoConsole("Start Service")

                state.currentApplication = AppState.enumStateApplication.inStarting

                log.track("moduleMain.run", "Begin")
                log.track("moduleMain.run", "Commandline process execute is " & Environment.CommandLine)
                log.track("moduleMain.run", "DataPath is " & paths.pathBaseData)
                log.track("moduleMain.run", "User Interface visible = " & IIf(state.uiVisible, "true", "false"))

                paths.init()

                registry.noSave = Not settings.data.useEventRegistry

                registry.init(paths.pathEvents)
                registry.addNew(CHCServerSupport.Support.RegistryEngine.RegistryData.TypeEvent.applicationStartUp)

                log.track("moduleMain.run", "System Registry is running")

                log.noSave = (settings.data.useTrack = AppSettings.TrackRuntimeModeEnum.dontTrack)

                log.init(paths.pathLogs, "main", registry)

                log.track("moduleMain.run", "Log engine is running")

                With settings.data.trackRotate

                    .frequency = CHCServerSupport.Support.LogRotateEngine.LogRotateConfig.FrequencyEnum.everyDay
                    .keepFile = CHCServerSupport.Support.LogRotateEngine.LogRotateConfig.KeepFileEnum.onlyMainTracks
                    .keepLast = CHCServerSupport.Support.LogRotateEngine.LogRotateConfig.KeepEnum.lastWeek

                    logRotate.configuration.frequency = .frequency
                    logRotate.configuration.keepFile = .keepFile
                    logRotate.configuration.keepLast = .keepLast

                End With

                logRotate.path = paths.pathLogs

                logRotate.run(log)

                log.track("moduleMain.run", "Trackrotate is running")

                counter.init(paths.pathCounters)

                log.track("moduleMain.run", "Counter is running")

                log.noSave = (settings.data.useTrack = AppSettings.TrackRuntimeModeEnum.dontTrack)

                log.track("moduleMain.run", "Log.noSave = " & log.noSave)

                If saveSettings Then

                    settings.save()

                    log.track("moduleMain.run", "Settings saved")

                End If

                If webserviceThread() Then

                    log.trackIntoConsole("Service is in run")

                Else

                    log.trackIntoConsole("Problem during start service")

                    End

                End If

                AreaApplication.Application.assets.init()

                If settings.data.recallStarter Then

                    recallStarter()

                End If

                state.currentApplication = AppState.enumStateApplication.inRunning

            Catch ex As Exception

                log.track("moduleMain.run", "Error:" & ex.Message, "fatal")

                CloseApplication()

            Finally

                log.track("moduleMain.run", "Completed")

            End Try

        End Sub


        ''' <summary>
        ''' This method provide to prepare to start the application
        ''' </summary>
        Sub Main()

            Try

                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)

                If firstProcedureStartup() Then

                    run()

                End If

            Catch ex As Exception

                MessageBox.Show("An error occurrent during moduleMain.startup " & Err.Description, "Notify problem", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        End Sub


        ''' <summary>
        ''' This method provide to stop the application
        ''' </summary>
        Sub [stop]()

            Try

                log.trackIntoConsole("Start the system shutdown operations")

                log.track("moduleMain.[Stop]", "Begin")

                state.currentApplication = AppState.enumStateApplication.inShutDown

                log.track("moduleMain.[Stop]", "Complete")

                state.currentApplication = AppState.enumStateApplication.waitingToStart

                log.trackIntoConsole("System is shutdown")

            Catch ex As Exception

            End Try

        End Sub


    End Module


End Namespace