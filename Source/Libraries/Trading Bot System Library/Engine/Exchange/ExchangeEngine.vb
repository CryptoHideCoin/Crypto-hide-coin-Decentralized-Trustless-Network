﻿Option Compare Text
Option Explicit On

Imports TradingBotSystemModelsLibrary.AreaModel.Exchange






Namespace AreaBusiness

    ''' <summary>
    ''' This engine contain all method relative of a exchange
    ''' </summary>
    Public Class ExchangeEngine

        Private Property _EngineDB As New CHCDataAccess.AreaCommon.Database.DatabaseGeneric
        Private Property _DBStateFileName As String = "EvaluationState.Db"
        Private Property _Initialize As Boolean = False
        Private Property _OwnerId As String = ""
        Private Property _CacheExchangeById As New Dictionary(Of Integer, ExchangeStructure)
        Private Property _CacheExchangeByName As New Dictionary(Of String, ExchangeStructure)

        Public Property useCache As Boolean = False



        ''' <summary>
        ''' This method provide to create an exchange table
        ''' </summary>
        ''' <returns></returns>
        Private Function createExchangeTable() As Boolean
            Try
                Dim sql As String = ""

                sql += "CREATE TABLE exchanges "
                sql += " ([id] INTEGER PRIMARY KEY AUTOINCREMENT, "
                sql += "  [name] NVARCHAR(512), "
                sql += "  isUsed INTEGER);"

                Return _EngineDB.executeDataTable(sql)
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.createExchangeTable", _OwnerId, ex.Message)

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to create a DB structures
        ''' </summary>
        ''' <returns></returns>
        Private Function createStructures() As Boolean
            Try
                Dim proceed As Boolean = True

                If proceed Then
                    proceed = createExchangeTable()
                End If

                Return proceed
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.createStructures", _OwnerId, ex.Message)

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method provide to add a new exchange into table
        ''' </summary>
        ''' <param name="[name]"></param>
        ''' <returns></returns>
        Private Function addNew(ByVal [name] As String, ByVal forceOwner As String) As Boolean
            Try
                Dim sql As String

                sql = $"INSERT INTO exchanges ([name], isUsed) VALUES ('[{name}]', 0)"

                Return _EngineDB.executeDataTableAndReturnID(sql, forceOwner)
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.addNew", forceOwner, ex.Message)

                Return False
            End Try
        End Function

        ''' <summary>
        ''' This method Add or Get the Id of a name of the exchange
        ''' </summary>
        ''' <param name="apiName"></param>
        ''' <returns></returns>
        Public Function addOrGetExchange(ByVal [name] As String, Optional ByVal forceOwner As String = "") As ExchangeStructure
            Dim response As New ExchangeStructure With {.name = [name]}

            If (forceOwner.Length = 0) Then
                forceOwner = _OwnerId
            End If

            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.addOrGetExchange", forceOwner)

                If _Initialize Then
                    Dim sql As String
                    Dim result As Object

                    If useCache Then
                        If Not _CacheExchangeByName.ContainsKey([name]) Then
                            response.id = addNew([name], forceOwner)

                            _CacheExchangeByName.Add([name], response)
                        End If

                        response = _CacheExchangeByName([name])
                    Else
                        Dim openDB As Object

                        sql = $"SELECT id, isUsed FROM exchanges WHERE [name] = '{name}'"

                        openDB = _EngineDB.openDatabase(forceOwner)

                        If Not IsNothing(openDB) Then
                            result = _EngineDB.selectDataReader(openDB, sql, forceOwner)

                            If Not IsNothing(result) Then
                                While result.read()
                                    response.id = result.GetInt32(0)
                                    response.isUsed = (result.GetInt32(1) <> 0)
                                End While
                            End If

                            openDB.close()
                        Else
                            response.id = addNew([name], forceOwner)
                        End If
                    End If
                End If

                Return response
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.addOrGetExchange", forceOwner, ex.Message)

                Return response
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.addOrGetExchange", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to modify into db and into cache data the name of an exchange
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="newName"></param>
        ''' <returns></returns>
        Public Function updateExchangeName(ByVal id As Integer, ByVal newName As String, Optional ByVal forceOwner As String = "") As Boolean
            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.modifyExchangeName", _OwnerId)

                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                If _Initialize Then
                    Dim sql As String

                    If useCache Then
                        Dim originalItem As ExchangeStructure

                        If _CacheExchangeById.ContainsKey(id) Then
                            originalItem = _CacheExchangeById(id)

                            _CacheExchangeByName.Remove(originalItem.name)

                            originalItem.name = newName

                            _CacheExchangeByName.Add(newName, originalItem)
                        Else
                            Return False
                        End If
                    End If

                    sql = $"UPDATE exchanges SET [name] = '{newName}' WHERE [id] = '{id}'"

                    Return _EngineDB.executeDataTable(sql, forceOwner)
                End If

                Return False
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.modifyExchangeName", forceOwner, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.modifyExchangeName", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to update to isUsed into db into cache data and into table of db
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="newName"></param>
        ''' <returns></returns>
        Public Function updateExchangeInUsed(ByVal id As Integer, Optional ByVal forceOwner As String = "") As Boolean
            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.updateExchangeInUsed", _OwnerId)

                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                If _Initialize Then
                    Dim sql As String

                    If useCache Then
                        If _CacheExchangeById.ContainsKey(id) Then
                            _CacheExchangeById(id).isUsed = True
                        Else
                            Return False
                        End If
                    End If

                    sql = $"UPDATE exchanges SET isUsed = 1 WHERE [id] = '{id}'"

                    Return _EngineDB.executeDataTable(sql, forceOwner)
                End If

                Return False
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.updateExchangeInUsed", forceOwner, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.updateExchangeInUsed", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to get an name of an Exchange from an Id
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Function [select](ByVal id As Integer, Optional ByVal forceOwner As String = "") As ExchangeStructure
            Dim response As New ExchangeStructure With {.id = id}
            Try
                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.[select]", forceOwner)

                If _Initialize Then
                    If useCache Then
                        Return _CacheExchangeById(id)
                    Else
                        Dim sql As String
                        Dim result As Object

                        sql = $"SELECT [name] FROM exchanges WHERE [id] = {id}"

                        result = _EngineDB.selectResultDataTable(sql, forceOwner)

                        If Not IsNothing(result) Then
                            response.name = result
                        End If
                    End If
                End If

                Return response
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.[select]", forceOwner, ex.Message)

                Return response
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.[select]", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to get an Id of an Exchange
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Function [select](ByVal [name] As String, Optional ByVal forceOwner As String = "") As ExchangeStructure
            Dim response As New ExchangeStructure With {.name = name}
            Try
                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.[select]", forceOwner)

                If _Initialize Then
                    If useCache Then
                        Return _CacheExchangeByName([name])
                    Else
                        Dim sql As String
                        Dim result As Object

                        sql = $"SELECT id FROM exchanges WHERE [name] = '{name}'"

                        result = _EngineDB.selectResultDataTable(sql, forceOwner)

                        If Not IsNothing(result) Then
                            response.id = result
                        End If
                    End If
                End If

                Return response
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.[select]", forceOwner, ex.Message)

                Return response
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.[select]", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to return a count of a exchange table
        ''' </summary>
        ''' <returns></returns>
        Public Function count(Optional ByVal forceOwner As String = "") As Integer
            Try
                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.count", forceOwner)

                If _Initialize Then
                    If useCache Then
                        Return _CacheExchangeById.Count
                    Else
                        Dim sql As String
                        Dim result As Object

                        sql = $"SELECT Count(id) as numExchange FROM exchanges"

                        result = _EngineDB.selectResultDataTable(sql, forceOwner)

                        If Not IsNothing(result) Then
                            Return result
                        End If
                    End If
                End If

                Return 0
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.count", forceOwner, ex.Message)

                Return 0
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.count", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to get a list of a table's exchange
        ''' </summary>
        ''' <returns></returns>
        Public Function list(Optional ByVal loadEntireCache As Boolean = False, Optional ByVal forceOwner As String = "") As List(Of ExchangeStructure)
            Dim sql As String
            Dim result As Object
            Dim openDB As Object
            Dim response As New List(Of ExchangeStructure)

            If (forceOwner.Length = 0) Then
                forceOwner = _OwnerId
            End If

            Try
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.list", forceOwner)

                If _Initialize Then
                    If useCache And Not loadEntireCache Then
                        Return _CacheExchangeById.Values.ToList()
                    Else
                        sql = $"SELECT id, name FROM exchanges "

                        openDB = _EngineDB.openDatabase(forceOwner)

                        If Not IsNothing(openDB) Then
                            result = _EngineDB.selectDataReader(openDB, sql, forceOwner)

                            If Not IsNothing(result) Then
                                Dim item As ExchangeStructure

                                While result.read()
                                    item = New ExchangeStructure

                                    item.id = result.GetInt32(0)
                                    item.name = result.GetString(1)

                                    If loadEntireCache Then
                                        _CacheExchangeById.Add(item.id, item)
                                        _CacheExchangeByName.Add(item.name, item)
                                    End If

                                    response.Add(item)
                                End While
                            End If

                            openDB.close()
                        End If
                    End If
                End If
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.list", forceOwner, ex.Message)

                Return New List(Of ExchangeStructure)
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.list", forceOwner)
            End Try

            Return response
        End Function

        ''' <summary>
        ''' This method provide to delete an unique id item's
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Function delete(ByVal id As Integer, Optional ByVal forceOwner As String = "") As Boolean
            Try
                If (forceOwner.Length = 0) Then
                    forceOwner = _OwnerId
                End If

                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.delete", forceOwner)

                If useCache Then
                    If _CacheExchangeById(id).isUsed Then
                        Return False
                    Else
                        _CacheExchangeByName.Remove(_CacheExchangeById(id).name)
                        _CacheExchangeById.Remove(id)
                    End If
                Else
                    If [select](id, forceOwner).isUsed Then
                        Return False
                    End If
                End If

                Return _EngineDB.executeDataTable($"DELETE exchanges WHERE [id] = {id}", forceOwner)
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.delete", forceOwner, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.delete", forceOwner)
            End Try
        End Function

        ''' <summary>
        ''' This method provide to initialize the engine
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Public Function init() As Boolean
            Try
                _EngineDB.path = CHCSidechainServiceLibrary.AreaCommon.Main.environment.paths.workData.state.path
                _EngineDB.fileName = _DBStateFileName
                _EngineDB.logInstance = CHCSidechainServiceLibrary.AreaCommon.Main.environment.log

                If Not IO.Directory.Exists(_EngineDB.path) Then
                    IO.Directory.CreateDirectory(_EngineDB.path)
                End If

                If _EngineDB.init("Evaluation", "Custom") Then
                    _OwnerId = "ExchangeEngine"

                    CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackEnter("ExchangeEngine.init", _OwnerId)

                    If Not _EngineDB.existTable("exchanges", _OwnerId) Then
                        CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.track("ExchangeEngine.init", _OwnerId, "Exchange table not exist")

                        If Not createStructures() Then
                            CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.track("ExchangeEngine.init", _OwnerId, "Problem during create db structures")

                            Return False
                        End If
                    ElseIf useCache Then
                        list(True)
                    End If

                    _Initialize = True

                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackException("ExchangeEngine.init", _OwnerId, ex.Message)

                Return False
            Finally
                CHCSidechainServiceLibrary.AreaCommon.Main.environment.log.trackExit("ExchangeEngine.init", _OwnerId)
            End Try
        End Function

    End Class

End Namespace