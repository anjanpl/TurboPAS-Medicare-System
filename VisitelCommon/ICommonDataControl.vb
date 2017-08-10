Imports System.Data.SqlClient
Imports System.Web.UI.WebControls

Namespace VisitelCommon
    Public Interface ICommonDataControl

        Sub BindEDICodesDropDownList(ByRef ConVisitel As SqlConnection, ByRef EDICodesDropDownList As DropDownList, CodeTable As String, CodeColumn As String,
                                     EDICodesFor As String, EdiCodesType As String)

    End Interface
End Namespace
