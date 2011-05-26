Imports System.ComponentModel
Imports DotNetNuke.Entities.Modules

Namespace Apollo.DNN.Modules.PageLocalization.Components


    Public Class RedirectsInfo
        Implements IHydratable


#Region "Private Members"

        Dim _itemId As Integer
        Dim _srcTabId As Integer
        Dim _srcLang As String
        Dim _reTabId As Integer
        Dim _reLang As String

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal itemId As Integer, ByVal srcTabId As Integer, ByVal srcLang As String, _
                       ByVal reTabId As Integer, ByVal reLang As String)
            Me.ItemId = itemId
            Me.SrcTabId = srcTabId
            Me.SrcLang = srcLang
            Me.ReTabId = reTabId
            Me.ReLang = reLang
        End Sub

#End Region

#Region "Public Properties"

        <DataObjectField(True)> _
        Public Property ItemId() As Integer
            Get
                Return _itemId
            End Get
            Set(ByVal Value As Integer)
                _itemId = Value
            End Set
        End Property

        Public Property SrcTabId() As Integer
            Get
                Return _srcTabId
            End Get
            Set(ByVal Value As Integer)
                _srcTabId = Value
            End Set
        End Property

        Public Property SrcLang() As String
            Get
                Return _srcLang
            End Get
            Set(ByVal Value As String)
                _srcLang = Value
            End Set
        End Property

        Public Property ReTabId() As Integer
            Get
                Return _reTabId
            End Get
            Set(ByVal Value As Integer)
                _reTabId = Value
            End Set
        End Property

        Public Property ReLang() As String
            Get
                Return _reLang
            End Get
            Set(ByVal Value As String)
                _reLang = Value
            End Set
        End Property

#End Region

        Public Sub Fill(ByVal dr As IDataReader) Implements IHydratable.Fill
            ItemId = Convert.ToInt32(Null.SetNull(dr("ItemId"), ItemId))
            SrcTabId = Convert.ToInt32(Null.SetNull(dr("SrcTabId"), SrcTabId))
            SrcLang = Convert.ToString(Null.SetNull(dr("SrcLang"), SrcLang))
            ReTabId = Convert.ToInt32(Null.SetNull(dr("ReTabId"), ReTabId))
            ReLang = Convert.ToString(Null.SetNull(dr("ReLang"), ReLang))

        End Sub

        Public Property KeyID() As Integer Implements IHydratable.KeyID
            Get
                Return ItemId
            End Get
            Set(ByVal value As Integer)
                ItemId = value
            End Set
        End Property
    End Class
End Namespace
