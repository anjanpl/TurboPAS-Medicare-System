Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports VisitelCommon

Namespace Visitel.UserControl.EDICentral

    Public Class ReferringProviderControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ReferringProviderControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDICentral/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonReferringProviderClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonReferringProviderSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objReferringProviderDataObject As New BLEDICentral.ReferringProviderDataObject()

                objReferringProviderDataObject.EntityIdCode = Convert.ToString(DropDownListEntityIdCode.SelectedValue, Nothing).Trim()
                objReferringProviderDataObject.EntityTypeQualifier = Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim()
                objReferringProviderDataObject.LastOrOrginizationName = Convert.ToString(TextBoxLastOrOrgName.Text, Nothing).Trim()
                objReferringProviderDataObject.FirstName = Convert.ToString(TextBoxFirst.Text, Nothing).Trim()
                objReferringProviderDataObject.MiddleName = Convert.ToString(TextBoxMiddle.Text, Nothing).Trim()
                objReferringProviderDataObject.Prefix = Convert.ToString(TextBoxPrefix.Text, Nothing).Trim()
                objReferringProviderDataObject.Suffix = Convert.ToString(TextBoxSuffix.Text, Nothing).Trim()
                objReferringProviderDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objReferringProviderDataObject.City = Convert.ToString(TextBoxCity.Text, Nothing).Trim()
                objReferringProviderDataObject.State = Convert.ToString(TextBoxState.Text, Nothing).Trim()
                objReferringProviderDataObject.Zip = Convert.ToString(TextBoxZip.Text, Nothing).Trim()
                objReferringProviderDataObject.ReferenceIdQualifier = Convert.ToString(DropDownListReferenceIdQualifier.SelectedValue, Nothing).Trim()
                objReferringProviderDataObject.EINOrSSN = Convert.ToString(TextBoxEINOrSSN.Text, Nothing).Trim()
                objReferringProviderDataObject.IdCodeQualifier = Convert.ToString(DropDownListIdCodeQualifier.SelectedValue, Nothing).Trim()
                objReferringProviderDataObject.ReferringProviderIdentifier = Convert.ToString(TextBoxReferringProviderIdentifier.Text, Nothing).Trim()
                objReferringProviderDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objReferringProviderDataObject.UserId = objShared.UserId
                objReferringProviderDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertReferringProviderInfo(objShared.ConVisitel, objReferringProviderDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objReferringProviderDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonReferringProviderDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()
            Try
                If (String.IsNullOrEmpty(LabelReferringProviderId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteReferringProviderInfo(objShared.ConVisitel, Convert.ToInt64(LabelReferringProviderId.Text), objShared.UserId)
                LabelReferringProviderId.Text = String.Empty
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                ClearControl()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Delete {0}", ex.Message))
            Finally
                objBLEDICentral = Nothing
            End Try
        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                         & " var LoaderImagePath2 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton2 ='ButtonReferringProviderDelete'; " _
                         & " var DeleteDialogHeader2 ='EDI Central: Referring Provider'; " _
                         & " var DeleteDialogConfirmMsg2 ='Are you sure to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress2); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteReferringProvider();" _
                         & "     prm.add_endRequest(DeleteReferringProvider); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        Private Sub InitializeControl()

            AddHandler ButtonReferringProviderClear.Click, AddressOf ButtonReferringProviderClear_Click
            AddHandler ButtonReferringProviderSave.Click, AddressOf ButtonReferringProviderSave_Click
            AddHandler ButtonReferringProviderDelete.Click, AddressOf ButtonReferringProviderDelete_Click

            ButtonReferringProviderClear.ClientIDMode = ClientIDMode.Static
            ButtonReferringProviderSave.ClientIDMode = ClientIDMode.Static
            ButtonReferringProviderDelete.ClientIDMode = ClientIDMode.Static

            TextBoxReferringProviderUpdateBy.Enabled = False
            TextBoxReferringProviderUpdateDate.Enabled = False

            SetToolTip()
            ControlTextLength()
        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxLastOrOrgName, 35)
            objShared.SetControlTextLength(TextBoxFirst, 25)
            objShared.SetControlTextLength(TextBoxMiddle, 25)
            objShared.SetControlTextLength(TextBoxPrefix, 10)
            objShared.SetControlTextLength(TextBoxSuffix, 10)
            objShared.SetControlTextLength(TextBoxAddress, 55)
            objShared.SetControlTextLength(TextBoxCity, 30)
            objShared.SetControlTextLength(TextBoxState, 2)
            objShared.SetControlTextLength(TextBoxZip, 15)
            objShared.SetControlTextLength(TextBoxEINOrSSN, 30)
            objShared.SetControlTextLength(TextBoxReferringProviderIdentifier, 80)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListEntityIdCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Entity Id Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Entity Type Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxLastOrOrgName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Last Or Org. Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxFirst.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter First Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxMiddle.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Middle Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxPrefix.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Prefix.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxSuffix.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Suffix.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxAddress.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Address.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxCity.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter City.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxState.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter State.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxZip.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Zip.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListReferenceIdQualifier.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Reference Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxEINOrSSN.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter EIN Or SSN.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListIdCodeQualifier.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Id Code Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxReferringProviderIdentifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Referring Provider Identifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Contract No.")
                Return False
            End If

            Return True

        End Function

        Private Sub SetToolTip()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            DropDownListEntityIdCode.ToolTip = Convert.ToString(ResourceTable("DropDownListEntityIdCodeToolTip"), Nothing)
            DropDownListEntityIdCode.ToolTip = If(String.IsNullOrEmpty(DropDownListEntityIdCode.ToolTip),
                                                  "Code identifying an organizational entity, a physical location, property or an",
                                                  DropDownListEntityIdCode.ToolTip)

            DropDownListEntityTypeQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListEntityTypeQualifierToolTip"), Nothing)
            DropDownListEntityTypeQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListEntityTypeQualifier.ToolTip),
                                                  "Code qualifying the type of entity",
                                                  DropDownListEntityTypeQualifier.ToolTip)

            TextBoxLastOrOrgName.ToolTip = Convert.ToString(ResourceTable("TextBoxLastOrOrgNameToolTip"), Nothing)
            TextBoxLastOrOrgName.ToolTip = If(String.IsNullOrEmpty(TextBoxLastOrOrgName.ToolTip), "Individual last name or organizational name", TextBoxLastOrOrgName.ToolTip)

            TextBoxFirst.ToolTip = Convert.ToString(ResourceTable("TextBoxFirstToolTip"), Nothing)
            TextBoxFirst.ToolTip = If(String.IsNullOrEmpty(TextBoxFirst.ToolTip), "Individual first name", TextBoxFirst.ToolTip)

            TextBoxMiddle.ToolTip = Convert.ToString(ResourceTable("TextBoxMiddleToolTip"), Nothing)
            TextBoxMiddle.ToolTip = If(String.IsNullOrEmpty(TextBoxMiddle.ToolTip), "Individual middle name or initial", TextBoxMiddle.ToolTip)

            TextBoxPrefix.ToolTip = Convert.ToString(ResourceTable("TextBoxPrefixToolTip"), Nothing)
            TextBoxPrefix.ToolTip = If(String.IsNullOrEmpty(TextBoxPrefix.ToolTip), "Prefix to individual name", TextBoxPrefix.ToolTip)

            TextBoxSuffix.ToolTip = Convert.ToString(ResourceTable("TextBoxSuffixToolTip"), Nothing)
            TextBoxSuffix.ToolTip = If(String.IsNullOrEmpty(TextBoxSuffix.ToolTip), "Suffix to individual name", TextBoxSuffix.ToolTip)

            DropDownListReferenceIdQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListReferenceIdQualifierToolTip"), Nothing)
            DropDownListReferenceIdQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListReferenceIdQualifier.ToolTip),
                                                          "Code qualifying the Reference Identification",
                                                          DropDownListReferenceIdQualifier.ToolTip)

            TextBoxEINOrSSN.ToolTip = Convert.ToString(ResourceTable("TextBoxEINOrSSNToolTip"), Nothing)
            TextBoxEINOrSSN.ToolTip = If(String.IsNullOrEmpty(TextBoxEINOrSSN.ToolTip), "Referring Provider Secondary Identifier", TextBoxEINOrSSN.ToolTip)

            DropDownListIdCodeQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListIdCodeQualifierToolTip"), Nothing)
            DropDownListIdCodeQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListIdCodeQualifier.ToolTip),
                                                     "Code designating the system/method of code structure used for Identification",
                                                     DropDownListIdCodeQualifier.ToolTip)

            TextBoxReferringProviderIdentifier.ToolTip = Convert.ToString(ResourceTable("TextBoxReferringProviderIdentifierToolTip"), Nothing)
            TextBoxReferringProviderIdentifier.ToolTip = If(String.IsNullOrEmpty(TextBoxReferringProviderIdentifier.ToolTip),
                                                     "Code identifying a party or other code",
                                                     TextBoxReferringProviderIdentifier.ToolTip)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            LabelEntityIDCode.Text = Convert.ToString(ResourceTable("LabelEntityIDCode"), Nothing)
            LabelEntityIDCode.Text = If(String.IsNullOrEmpty(LabelEntityIDCode.Text), "Entity ID Code:", LabelEntityIDCode.Text)

            LabelEntityTypeQualifier.Text = Convert.ToString(ResourceTable("LabelEntityTypeQualifier"), Nothing)
            LabelEntityTypeQualifier.Text = If(String.IsNullOrEmpty(LabelEntityTypeQualifier.Text), "Entity Type Qualifier:", LabelEntityTypeQualifier.Text)

            LabelLastOrOrgName.Text = Convert.ToString(ResourceTable("LabelLastOrOrgName"), Nothing)
            LabelLastOrOrgName.Text = If(String.IsNullOrEmpty(LabelLastOrOrgName.Text), "Last or Org. Name:", LabelLastOrOrgName.Text)

            LabelFirst.Text = Convert.ToString(ResourceTable("LabelFirst"), Nothing)
            LabelFirst.Text = If(String.IsNullOrEmpty(LabelFirst.Text), "First:", LabelFirst.Text)

            LabelMiddle.Text = Convert.ToString(ResourceTable("LabelMiddle"), Nothing)
            LabelMiddle.Text = If(String.IsNullOrEmpty(LabelMiddle.Text), "Middle:", LabelMiddle.Text)

            LabelPrefix.Text = Convert.ToString(ResourceTable("LabelPrefix"), Nothing)
            LabelPrefix.Text = If(String.IsNullOrEmpty(LabelPrefix.Text), "Prefix:", LabelPrefix.Text)

            LabelSuffix.Text = Convert.ToString(ResourceTable("LabelSuffix"), Nothing)
            LabelSuffix.Text = If(String.IsNullOrEmpty(LabelSuffix.Text), "Suffix:", LabelSuffix.Text)

            LabelReferringProviderUpdateDate.Text = Convert.ToString(ResourceTable("LabelReferringProviderUpdateDate"), Nothing)
            LabelReferringProviderUpdateDate.Text = If(String.IsNullOrEmpty(LabelReferringProviderUpdateDate.Text), "Update Date:", LabelReferringProviderUpdateDate.Text)

            LabelReferringProviderUpdateBy.Text = Convert.ToString(ResourceTable("LabelReferringProviderUpdateBy"), Nothing)
            LabelReferringProviderUpdateBy.Text = If(String.IsNullOrEmpty(LabelReferringProviderUpdateBy.Text), "Update By:", LabelReferringProviderUpdateBy.Text)

            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing)
            LabelAddress.Text = If(String.IsNullOrEmpty(LabelAddress.Text), "Address:", LabelAddress.Text)

            LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing)
            LabelCity.Text = If(String.IsNullOrEmpty(LabelCity.Text), "City:", LabelCity.Text)

            LabelState.Text = Convert.ToString(ResourceTable("LabelState"), Nothing)
            LabelState.Text = If(String.IsNullOrEmpty(LabelState.Text), "State:", LabelState.Text)

            LabelZip.Text = Convert.ToString(ResourceTable("LabelZip"), Nothing)
            LabelZip.Text = If(String.IsNullOrEmpty(LabelZip.Text), "Zip:", LabelZip.Text)

            LabelReferenceIDQualifier.Text = Convert.ToString(ResourceTable("LabelReferenceIDQualifier"), Nothing)
            LabelReferenceIDQualifier.Text = If(String.IsNullOrEmpty(LabelReferenceIDQualifier.Text), "Reference ID Qualifier:", LabelReferenceIDQualifier.Text)

            LabelEINOrSSN.Text = Convert.ToString(ResourceTable("LabelEINOrSSN"), Nothing)
            LabelEINOrSSN.Text = If(String.IsNullOrEmpty(LabelEINOrSSN.Text), "EIN or SSN:", LabelEINOrSSN.Text)

            LabelIDCodeQualifier.Text = Convert.ToString(ResourceTable("LabelIDCodeQualifier"), Nothing)
            LabelIDCodeQualifier.Text = If(String.IsNullOrEmpty(LabelIDCodeQualifier.Text), "ID Code Qualifier:", LabelIDCodeQualifier.Text)

            LabelReferringProviderIdentifier.Text = Convert.ToString(ResourceTable("LabelReferringProviderIdentifier"), Nothing)
            LabelReferringProviderIdentifier.Text = If(String.IsNullOrEmpty(LabelReferringProviderIdentifier.Text),
                                                       "Referring Provider Identifier:", LabelReferringProviderIdentifier.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            ButtonReferringProviderClear.Text = Convert.ToString(ResourceTable("ButtonReferringProviderClear"), Nothing)
            ButtonReferringProviderClear.Text = If(String.IsNullOrEmpty(ButtonReferringProviderClear.Text), "Clear", ButtonReferringProviderClear.Text)

            ButtonReferringProviderSave.Text = Convert.ToString(ResourceTable("ButtonReferringProviderSave"), Nothing)
            ButtonReferringProviderSave.Text = If(String.IsNullOrEmpty(ButtonReferringProviderSave.Text), "Save", ButtonReferringProviderSave.Text)

            ButtonReferringProviderDelete.Text = Convert.ToString(ResourceTable("ButtonReferringProviderDelete"), Nothing)
            ButtonReferringProviderDelete.Text = If(String.IsNullOrEmpty(ButtonReferringProviderDelete.Text), "Delete", ButtonReferringProviderDelete.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "ReferringProvider", ValidationGroup)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            DropDownListEntityIdCode.SelectedIndex = objShared.InlineAssignHelper(DropDownListEntityTypeQualifier.SelectedIndex,
                                                     objShared.InlineAssignHelper(DropDownListReferenceIdQualifier.SelectedIndex,
                                                     objShared.InlineAssignHelper(DropDownListIdCodeQualifier.SelectedIndex, 0)))


            TextBoxLastOrOrgName.Text = objShared.InlineAssignHelper(TextBoxFirst.Text, objShared.InlineAssignHelper(TextBoxMiddle.Text,
                                        objShared.InlineAssignHelper(TextBoxPrefix.Text, objShared.InlineAssignHelper(TextBoxSuffix.Text,
                                        objShared.InlineAssignHelper(TextBoxReferringProviderUpdateDate.Text, objShared.InlineAssignHelper(TextBoxReferringProviderUpdateBy.Text,
                                        objShared.InlineAssignHelper(TextBoxAddress.Text, objShared.InlineAssignHelper(TextBoxCity.Text,
                                        objShared.InlineAssignHelper(TextBoxState.Text, objShared.InlineAssignHelper(TextBoxZip.Text,
                                        objShared.InlineAssignHelper(TextBoxEINOrSSN.Text, objShared.InlineAssignHelper(TextBoxReferringProviderIdentifier.Text,
                                        objShared.InlineAssignHelper(TextBoxContractNo.Text, String.Empty)))))))))))))

        End Sub

        Private Sub GetData()
            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListEntityIdCode,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.ReferringProvider),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.NM101),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityIdCode),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListEntityTypeQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Provider),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.NM102),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityTypeQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListReferenceIdQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Provider),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.REF01),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.ReferenceIdQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListIdCodeQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Provider),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.NM108),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.IdCodeQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))
        End Sub

        Public Sub LoadReferringProvider()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objReferringProviderDataObject As BLEDICentral.ReferringProviderDataObject

            objReferringProviderDataObject = objBLEDICentral.SelectReferringProviderInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelReferringProviderId.Text = objReferringProviderDataObject.Id

            DropDownListEntityIdCode.SelectedIndex = DropDownListEntityIdCode.Items.IndexOf(DropDownListEntityIdCode.Items.FindByValue(
                                                                          objReferringProviderDataObject.EntityIdCode))

            DropDownListEntityTypeQualifier.SelectedIndex = DropDownListEntityTypeQualifier.Items.IndexOf(DropDownListEntityTypeQualifier.Items.FindByValue(
                                                                            objReferringProviderDataObject.EntityTypeQualifier))

            TextBoxLastOrOrgName.Text = objReferringProviderDataObject.LastOrOrginizationName
            TextBoxFirst.Text = objReferringProviderDataObject.FirstName
            TextBoxMiddle.Text = objReferringProviderDataObject.MiddleName
            TextBoxPrefix.Text = objReferringProviderDataObject.Prefix
            TextBoxSuffix.Text = objReferringProviderDataObject.Suffix
            TextBoxAddress.Text = objReferringProviderDataObject.Address
            TextBoxCity.Text = objReferringProviderDataObject.City
            TextBoxState.Text = objReferringProviderDataObject.State
            TextBoxZip.Text = objReferringProviderDataObject.Zip

            DropDownListReferenceIdQualifier.SelectedIndex = DropDownListReferenceIdQualifier.Items.IndexOf(DropDownListReferenceIdQualifier.Items.FindByValue(
                                                                            objReferringProviderDataObject.ReferenceIdQualifier))

            TextBoxEINOrSSN.Text = objReferringProviderDataObject.EINOrSSN

            DropDownListIdCodeQualifier.SelectedIndex = DropDownListIdCodeQualifier.Items.IndexOf(DropDownListIdCodeQualifier.Items.FindByValue(
                                                                            objReferringProviderDataObject.IdCodeQualifier))

            TextBoxReferringProviderIdentifier.Text = objReferringProviderDataObject.ReferringProviderIdentifier
            TextBoxContractNo.Text = objReferringProviderDataObject.ContractNo

            TextBoxReferringProviderUpdateDate.Text = objReferringProviderDataObject.UpdateDate
            TextBoxReferringProviderUpdateBy.Text = objReferringProviderDataObject.UpdateBy

            objReferringProviderDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub

    End Class

End Namespace