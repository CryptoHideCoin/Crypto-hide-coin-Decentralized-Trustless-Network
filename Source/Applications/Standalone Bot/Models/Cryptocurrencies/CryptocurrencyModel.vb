﻿Option Compare Text
Option Explicit On


Namespace AreaCommon.Models.Products

    Public Class ProductBaseModel

        Public Property key As String = ""
        Public Property name As String = ""

        Public Property baseCurrency As String = ""
        Public Property quoteCurrency As String = ""

        Public Property baseIncrement As String = ""
        Public Property quoteIncrement As String = ""

        Public Property minMarketFunds As String = ""

        Public Property postOnly As Boolean = False
        Public Property limitOnly As Boolean = False

        Public Property tradingDisabled As Boolean = False
        Public Property status As String = ""
        Public Property statusMessage As String = ""

    End Class

    Public Class ProductValueModel

        Public Property automaticMinValue As Boolean = False
        Public Property automaticMaxValue As Boolean = False

        Public Property minValue As Double = 0
        Public Property dateLast As Double = 0

        Public Property maxValue As Double = 0
        Public Property dateMax As Double = 0

        Public Property current As Double = 0

        Public ReadOnly Property averageWork As Double
            Get
                Return (maxValue - minValue) / 2
            End Get
        End Property

        Public Function bottomPercentPosition(Optional ByVal currentValue As Double = 0) As Double
            If currentValue = 0 Then
                currentValue = current
            End If

            If (averageWork = 0) Then
                Return 0
            Else
                Return (currentValue - minValue) / (maxValue - minValue) * 100
            End If
        End Function

    End Class

    Public Class ProductUserDataModel

        Public Enum StateEnumeration
            undefined = -1
            deep = 0
            work = 1
            high = 2
        End Enum

        Public Enum PreferenceEnumeration
            undefined = -1
            ignore = 0
            userOnly = 1
            toWork = 2
            prefered = 3
            automaticDisabled = 4
        End Enum

        Public Property state As StateEnumeration = StateEnumeration.undefined
        Public Property preference As PreferenceEnumeration = PreferenceEnumeration.undefined

        Public Property isCustomized As Boolean = False

    End Class

    Public Class ProductOrderModel

        Public Property dateAcquire As Double = 0
        Public Property orderNumber As String = ""
        Public Property amount As Double = 0
        Public Property tcoQuote As Double = 0
        Public Property maxPrice As Double = 0
        Public Property feeCost As Double = 0
        Public Property orderState As Models.Bot.BotOrderModel.OrderStateEnumeration = Bot.BotOrderModel.OrderStateEnumeration.undefined

        Public Property internalOrderId As String = ""
        'Public Property ordinary As Boolean = False

    End Class

    Public Class ProductActivityModel

        Private m_dbl_Target As Double = 0

        Public Property inUse As Boolean = False
        Public Property dateLastCheck As Double = 0

        Public Property buys As New List(Of ProductOrderModel)

        Public Property sell As New ProductOrderModel

        Public Property target As Double
            Get
                If AreaState.defaultUserDataAccount.useVirtualAccount Then
                    Return sell.tcoQuote
                Else
                    Return m_dbl_Target
                End If
            End Get
            Set(value As Double)
                m_dbl_Target = value
            End Set
        End Property

        Public ReadOnly Property earn As Double
            Get
                Return sell.tcoQuote - totalInvestment
            End Get
        End Property

        Public ReadOnly Property totalAmount As Double
            Get
                Dim total As Double = 0
                Dim repeat As Boolean = True

                Do While repeat
                    repeat = False

                    Try
                        For Each buy In buys
                            If (buy.orderState = Bot.BotOrderModel.OrderStateEnumeration.filled) Then
                                total += CDec(buy.amount)
                            End If
                        Next
                    Catch ex As Exception
                        repeat = True
                    End Try
                Loop

                Return total
            End Get
        End Property

        Public ReadOnly Property totalInvestment As Double
            Get
                Dim total As Double = 0

                For Each buy In buys
                    If (buy.orderState = Bot.BotOrderModel.OrderStateEnumeration.filled) Then
                        total += CDec(buy.tcoQuote)
                    End If
                Next

                Return total
            End Get
        End Property

        Public ReadOnly Property totalFee As Double
            Get
                Dim total As Double = 0

                For Each buy In buys
                    If (buy.orderState = Bot.BotOrderModel.OrderStateEnumeration.filled) Then
                        total += CDec(buy.feeCost)
                    End If
                Next

                Return total
            End Get
        End Property

        Public ReadOnly Property openBuy() As ProductOrderModel
            Get
                For Each buy In buys
                    If (buy.orderState = Bot.BotOrderModel.OrderStateEnumeration.placed) Then
                        Return buy
                    End If
                Next

                Return New ProductOrderModel
            End Get
        End Property

        Public ReadOnly Property lastBuy() As ProductOrderModel
            Get
                Dim lastOrderBuy As New ProductOrderModel

                For Each buy In buys
                    If (buy.orderState = Bot.BotOrderModel.OrderStateEnumeration.filled) Then
                        If (lastOrderBuy.dateAcquire < buy.dateAcquire) Then
                            lastOrderBuy = buy
                        End If
                    End If
                Next

                Return lastOrderBuy
            End Get
        End Property

    End Class


    Public Class ProductModel

        Public Property header As New ProductBaseModel
        Public Property value As New ProductValueModel
        Public Property userData As New ProductUserDataModel
        Public Property activity As New ProductActivityModel

        Public Property minTarget As Double

        Public ReadOnly Property pairID As String
            Get
                Return $"{header.baseCurrency}-{header.quoteCurrency}"
            End Get
        End Property

        Public ReadOnly Property currentValue As Double
            Get
                Return CDec(value.current) * activity.totalAmount
            End Get
        End Property

        Public ReadOnly Property currentSpread() As Double
            Get
                Return CDec(currentValue) - activity.totalInvestment
            End Get
        End Property

        Public ReadOnly Property currentSpreadPerc() As Double
            Get
                Return currentSpread / activity.totalInvestment * 100
            End Get
        End Property

        Public ReadOnly Property maxTarget() As Double
            Get
                Return activity.target
            End Get
        End Property

        Public ReadOnly Property maxTargetReached() As Boolean
            Get
                Return (currentValue > activity.target)
            End Get
        End Property

        Public Function inDeal(ByVal dealPercValue As Double) As Boolean
            Dim perc As Double = 0

            If (activity.totalInvestment = 0) Then
                Return False
            Else
                perc = currentSpread / activity.totalInvestment * 100
                If (perc) < 0 Then
                    perc = Math.Abs(perc)
                End If

                Return (perc < dealPercValue)
            End If
        End Function

        Public Function resetData() As Boolean
            If userData.isCustomized Then
                activity.buys = New List(Of ProductOrderModel)
                activity.sell = New ProductOrderModel

                activity.dateLastCheck = 0
                activity.inUse = False

                activity.target = 0

                minTarget = 0
            End If

            Return True
        End Function

    End Class



    Public Class ProductsModel

        Private Property _Index As New Dictionary(Of String, ProductModel)

        Public Property items As New List(Of ProductModel)

        Public Function addNew(ByVal key As String) As ProductModel
            Dim newItem As New ProductModel

            Try
                newItem.header.key = key

                items.Add(newItem)
                _Index.Add(key, newItem)
            Catch ex As Exception
            End Try

            Return newItem
        End Function

        Public Function addNew(ByVal key As String, ByRef data As ProductModel) As Boolean
            Try
                items.Add(data)
                _Index.Add(key, data)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function exist(ByVal key As String) As Boolean
            Return _Index.ContainsKey(key)
        End Function

        Public Function getCurrency(ByVal key As String) As ProductModel
            If _Index.ContainsKey(key) Then
                Return _Index(key)
            Else
                Return New ProductModel
            End If
        End Function

    End Class

End Namespace