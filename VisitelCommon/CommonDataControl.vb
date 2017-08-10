Imports System.Web.UI.WebControls
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelCommon.VisitelCommon

Public Class CommonDataControl
    Inherits BaseUserControl
    Implements ICommonDataControl

    Private Sub BindEDICodesDropDownList(ByRef ConVisitel As SqlConnection, ByRef EDICodesDropDownList As DropDownList, CodeTable As String, CodeColumn As String, _
                                         EDICodesFor As String, EdiCodesType As String) Implements ICommonDataControl.BindEDICodesDropDownList

        Dim objBLEDICodes As New BLEDICodes

        Dim EDICodesList As New List(Of EDICodesDataObject)

        Try
            EDICodesList = objBLEDICodes.SelectEDICodes(ConVisitel, CodeTable, CodeColumn, EdiCodesType)
        Catch ex As SqlException

            Select Case EDICodesFor

                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.AcknowledgementRequested)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.AuthorizationInformationQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.ClaimFilingIndicatorCode)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityIdCode)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityIdentifierCode)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityTypeQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.IdCodeQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.InterchangeIdQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.PayerResponsibilitySequenceNumberCode)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.ReferenceIdQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.RelationshipCode)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.SecurityInformationQualifier)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.UsageIndicator)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch " & EDICodesFor & " data")
                    Exit Select
            End Select

        Finally
            objBLEDICodes = Nothing
        End Try

        EDICodesDropDownList.DataSource = EDICodesList
        EDICodesDropDownList.DataTextField = "CodeDefinition"
        EDICodesDropDownList.DataValueField = "Code"
        EDICodesDropDownList.DataBind()

        If (EdiCodesType.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))) Then
            EDICodesDropDownList.Items.Insert(0, New ListItem(" ", "-1"))
        End If

        EDICodesList = Nothing

    End Sub

End Class
