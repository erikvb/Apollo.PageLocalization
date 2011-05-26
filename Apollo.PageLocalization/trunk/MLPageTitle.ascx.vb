'
' DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports Apollo.DNN_Localization
Imports Apollo.DNN.Modules.PageLocalization.Components
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Tabs
Imports System.Threading


Namespace Apollo.DNN.SkinObjects
    Partial Class MLPageTitle
        Inherits SkinObjectBase

#Region "Controls"

#End Region

#Region "Private Members"

        Private _cssClass As String = "skinobject"
        Private _visible As Boolean = False

#End Region

#Region "Public Members"

        Public Property CssClass() As String
            Get
                Return _cssClass
            End Get
            Set(ByVal Value As String)
                _cssClass = Value
            End Set
        End Property

        Public Overrides Property Visible() As Boolean
            Get
                Return _visible
            End Get
            Set(ByVal Value As Boolean)
                _visible = Value
            End Set
        End Property

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Try
                Dim objTab As LocalizedTabInfo = Nothing
                Dim objDNNTab As TabInfo

                Dim strTitle As String = PortalSettings.PortalName

                For Each objDNNTab In PortalSettings.ActiveTab.BreadCrumbs
                    objTab = LocalizeTab.getLocalizedTab(objDNNTab)
                    strTitle += " > " & objTab.TabName
                Next

                If objTab IsNot Nothing Then
                    ' tab title override
                    If objTab.Title <> "" Then
                        strTitle = objTab.Title
                    End If
                End If
                Dim BasePage As CDefault = CType(Me.Page, CDefault)
                BasePage.Title = strTitle
                BasePage.KeyWords = objTab.KeyWords
                BasePage.Description = objTab.Description
                BasePage.Comment += vbCrLf & "<META HTTP-EQUIV=""Content-Language"" content=""" & _
                                    Thread.CurrentThread.CurrentUICulture.Name & """>" & vbCrLf

                If Not Page.IsPostBack Then
                    ' public attributes
                    If CssClass <> "" Then
                        lblPageTitle.CssClass = CssClass
                    End If

                    If objTab.Title = "" Then
                        lblPageTitle.Text = objTab.TabName
                    Else
                        lblPageTitle.Text = objTab.Title
                    End If

                    lblPageTitle.Visible = Visible
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init
            Dim _
                redirInfo As RedirectsInfo = _
                    RedirectsController.Get(Globalization.CultureInfo.CurrentCulture.Name, _
                                             PortalSettings.ActiveTab.TabID)
            If redirInfo IsNot Nothing Then
                PermanentRedirect(NavigateURL(redirInfo.ReTabId, False, PortalSettings, "", redirInfo.ReLang, Nothing))

            End If
        End Sub

#End Region
    End Class
End Namespace
