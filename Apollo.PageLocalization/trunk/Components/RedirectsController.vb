Imports System.ComponentModel

Namespace Apollo.DNN.Modules.PageLocalization.Components


    <DataObject(True)> _
    Public Class RedirectsController

#Region "Private Methods"

        Private Shared Function GetNull(ByVal Field As Object) As Object
            Return Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

#Region "Public Methods"

        Public Shared Function [Get](ByVal SrcLang As String, ByVal SrcTabId As Integer) As RedirectsInfo
            Return _
                CType( _
                    CBO.FillObject( _
                                   CType( _
                                      DataProvider.Instance().ExecuteReader("Apollo_TabLocalization_RedirectsGet", _
                                                                            SrcLang, SrcTabId), IDataReader), _
                                   GetType(RedirectsInfo)), RedirectsInfo)
        End Function


        <DataObjectMethod(DataObjectMethodType.Insert, True)> _
        Public Shared Sub Add(ByVal objApollo_TabLocalization_Redirects As RedirectsInfo)

            DataProvider.Instance().ExecuteNonQuery("Apollo_TabLocalization_RedirectsAdd", _
                                                    GetNull(objApollo_TabLocalization_Redirects.SrcTabId), _
                                                    GetNull(objApollo_TabLocalization_Redirects.SrcLang), _
                                                    GetNull(objApollo_TabLocalization_Redirects.ReTabId), _
                                                    GetNull(objApollo_TabLocalization_Redirects.ReLang))

        End Sub

        <DataObjectMethod(DataObjectMethodType.Update, True)> _
        Public Shared Sub Update(ByVal objApollo_TabLocalization_Redirects As RedirectsInfo)

            DataProvider.Instance().ExecuteNonQuery("Apollo_TabLocalization_RedirectsUpdate", _
                                                    GetNull(objApollo_TabLocalization_Redirects.ItemId), _
                                                    GetNull(objApollo_TabLocalization_Redirects.SrcTabId), _
                                                    GetNull(objApollo_TabLocalization_Redirects.SrcLang), _
                                                    GetNull(objApollo_TabLocalization_Redirects.ReTabId), _
                                                    GetNull(objApollo_TabLocalization_Redirects.ReLang))

        End Sub

        <DataObjectMethod(DataObjectMethodType.Delete, True)> _
        Public Shared Sub Delete(ByVal SrcLang As String, ByVal SrcTabId As Integer)

            DataProvider.Instance().ExecuteNonQuery("Apollo_TabLocalization_RedirectsDelete", SrcLang, SrcTabId)

        End Sub

#End Region
    End Class
End Namespace
