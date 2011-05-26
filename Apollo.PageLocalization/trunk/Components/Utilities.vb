Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Host


Namespace Apollo.DNN.Modules.PageLocalization.Components
    Module Utilities

#Region "host settings"


        Public Function GetHostSetting(ByVal key As String) As String
            Dim setting As String = Null.NullString
            Host.GetHostSettingsDictionary.TryGetValue(key, setting)
            Return setting
        End Function

        Public Function GetHostSetting(ByVal key As String, ByVal setting As String) As String
            Dim newSetting As String = Null.NullString
            Host.GetHostSettingsDictionary.TryGetValue(key, newSetting)
            If newSetting Is Nothing Then Return setting Else Return newSetting
        End Function

        Public Sub UpdateHostSetting(ByVal SettingName As String, ByVal SettingValue As String)
            Dim hsc As New HostSettingsController
            hsc.UpdateHostSetting(SettingName, SettingValue)
        End Sub
#End Region



        Public Sub PermanentRedirect(ByVal redirUrl As String)
            Dim Response As HttpResponse = HttpContext.Current.Response
            Response.Clear()
            Response.Status = "301 Moved Permanently"
            Response.AddHeader("Location", redirUrl)
            Response.Flush()
            Response.End()
        End Sub

    End Module
End Namespace