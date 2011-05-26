'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2008
' by DotNetNuke Corporation
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
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.UI.WebControls

Namespace Apollo.DNN.Modules.PageLocalization.Components


    ''' <summary>
    ''' This class inherits the original DotNetNuke.UI.Skins.NavObjectBase class. This is needed because it calls the inherited Apollo.DNN.Modules.PageLocalization.Navigation class
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NavObjectBase

        Inherits Skins.NavObjectBase

        Private _parentID As String

        Public Property ParentID() As String
            Get
                Return _parentID
            End Get
            Set(ByVal Value As String)
                _parentID = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' returns a tab object of the requested level in the parent tree of requested tab.
        ''' we only look up, not down
        ''' </summary>
        ''' <param name="objTab"></param>
        ''' <param name="needLevel"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	9-2-2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function getTopAncestor(ByVal objTab As TabInfo, ByVal needLevel As Integer) As TabInfo
            If objTab Is Nothing Then
                Return Nothing
            Else
                If (objTab.Level = needLevel) Or (objTab.Level = 0) Then
                    If objTab.Level = needLevel Then
                        Return objTab
                    Else
                        Return Nothing
                    End If
                Else
                    Return _
                        getTopAncestor((New TabController()).GetTab(objTab.ParentId, PortalSettings.PortalId, False), _
                                        needLevel)
                End If
            End If
        End Function


        ''' <summary>
        ''' GetNavigationNodes gets a collection of DNNNodes, containing all tabinfo objects. This function shadows the function 
        ''' in the base class in order to be able to load localized tabinfo objects
        ''' </summary>
        ''' <param name="objNode"></param>
        ''' <returns>DNNNodeCollection</returns>
        ''' <remarks></remarks>
        Public Shadows Function GetNavigationNodes(ByVal objNode As DNNNode) As DNNNodeCollection
            Dim intRootParent As Integer = PortalSettings.ActiveTab.TabID
            Dim objNodes As DNNNodeCollection
            Dim eToolTips As DotNetNuke.UI.Navigation.ToolTipSource
            Dim intNavNodeOptions As Integer
            'Dim blnRootOnly As Boolean = Boolean.Parse(GetValue(RootOnly, "False"))
            Dim intDepth As Integer = ExpandDepth
            Dim _localParentID As Integer = -1
            If ParentID <> "" Then
                _localParentID = Integer.Parse(ParentID)
            End If

            If Level <> "" Then
                'This setting indicates the root level for the menu
                Select Case LCase(Level)
                    Case "child"
                    Case "parent"
                        intNavNodeOptions = DotNetNuke.UI.Navigation.NavNodeOptions.IncludeParent + _
                                            DotNetNuke.UI.Navigation.NavNodeOptions.IncludeSelf
                    Case "same"
                        intNavNodeOptions = DotNetNuke.UI.Navigation.NavNodeOptions.IncludeSiblings + _
                                            DotNetNuke.UI.Navigation.NavNodeOptions.IncludeSelf
                    Case Else 'root
                        If Level.ToLower.IndexOf("level") > -1 Then
                            Dim tempLevel As String = Level.ToLower.Replace("level", "")
                            Dim intLevel As Integer
                            Try
                                intLevel = Integer.Parse(tempLevel)
                            Catch
                                intLevel = -1
                            End Try
                            If intLevel >= 0 Then
                                Try
                                    Dim tempTab As TabInfo = getTopAncestor(PortalSettings.ActiveTab, intLevel)
                                    If Not (tempTab Is Nothing) Then
                                        intRootParent = tempTab.TabID
                                    Else
                                        intRootParent = -2
                                        'correct tab was not found .... menu will not render
                                    End If
                                Catch
                                    intRootParent = -1
                                End Try
                            End If
                        Else
                            intRootParent = -1
                        End If

                        intRootParent = -1
                        intNavNodeOptions = DotNetNuke.UI.Navigation.NavNodeOptions.IncludeSiblings + _
                                            DotNetNuke.UI.Navigation.NavNodeOptions.IncludeSelf
                End Select
            Else
                If _
                    (_localParentID <> -1) AndAlso _
                    ((New TabController()).GetTab(_localParentID, PortalSettings.PortalId, False) IsNot Nothing) Then
                    intRootParent = _localParentID
                Else
                    intRootParent = -1
                End If
            End If

            Select Case LCase(ToolTip)
                Case "name"
                    eToolTips = DotNetNuke.UI.Navigation.ToolTipSource.TabName
                Case "title"
                    eToolTips = DotNetNuke.UI.Navigation.ToolTipSource.Title
                Case "description"
                    eToolTips = DotNetNuke.UI.Navigation.ToolTipSource.Description
                Case Else
                    eToolTips = DotNetNuke.UI.Navigation.ToolTipSource.None
            End Select

            If Me.PopulateNodesFromClient AndAlso Control.SupportsPopulateOnDemand Then
                intNavNodeOptions += DotNetNuke.UI.Navigation.NavNodeOptions.MarkPendingNodes
            End If
            If Me.PopulateNodesFromClient AndAlso Control.SupportsPopulateOnDemand = False Then
                ExpandDepth = -1
            End If

            'End If

            If StartTabId <> -1 Then intRootParent = StartTabId

            If Not objNode Is Nothing Then
                'we are in a POD request
                intRootParent = CInt(objNode.ID)
                intNavNodeOptions = DotNetNuke.UI.Navigation.NavNodeOptions.MarkPendingNodes
                'other options for this don't apply, but we do know we want to mark pending nodes
                objNodes = _
                    Navigation.GetNavigationNodes(objNode, eToolTips, intRootParent, intDepth, intNavNodeOptions)
            Else
                objNodes = _
                    Navigation.GetNavigationNodes(Control.ClientID, eToolTips, intRootParent, intDepth, _
                                                   intNavNodeOptions)
            End If

            Return objNodes

        End Function
    End Class
End Namespace