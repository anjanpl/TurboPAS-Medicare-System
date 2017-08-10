Namespace VisitelBusiness.DataObject.Settings
    Public Class CitySettingDataObject
        Inherits BaseDataObject

        Public Property CityId As Integer
        
        Public Property CityName As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.CityId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))
            Me.CityName = String.Empty
        End Sub
    End Class
End Namespace

