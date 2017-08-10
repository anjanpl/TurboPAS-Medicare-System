
Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports VisitelCommon

Namespace Visitel.UserControl.EDICentral

    Public Class RenderingProviderControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "RenderingProviderControl"
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

        Private Sub ButtonRenderingProviderClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonRenderingProviderSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objRenderingProviderDataObject As New BLEDICentral.RenderingProviderDataObject()

                objRenderingProviderDataObject.EntityIdCode = Convert.ToString(DropDownListEntityIdCode.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.EntityTypeQualifier = Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.LastOrOrganizationName = Convert.ToString(TextBoxLastOrOrgName.Text, Nothing).Trim()
                objRenderingProviderDataObject.FirstName = Convert.ToString(TextBoxFirst.Text, Nothing).Trim()
                objRenderingProviderDataObject.MiddleName = Convert.ToString(TextBoxMiddle.Text, Nothing).Trim()
                objRenderingProviderDataObject.Prefix = Convert.ToString(TextBoxPrefix.Text, Nothing).Trim()
                objRenderingProviderDataObject.Suffix = Convert.ToString(TextBoxSuffix.Text, Nothing).Trim()
                objRenderingProviderDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objRenderingProviderDataObject.City = Convert.ToString(TextBoxCity.Text, Nothing).Trim()
                objRenderingProviderDataObject.State = Convert.ToString(TextBoxState.Text, Nothing).Trim()
                objRenderingProviderDataObject.Zip = Convert.ToString(TextBoxZip.Text, Nothing).Trim()
                objRenderingProviderDataObject.ReferenceIdQualifier = Convert.ToString(DropDownListReferenceIdQualifier.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.EINOrSSN = Convert.ToString(TextBoxEINOrSSN.Text, Nothing).Trim()
                objRenderingProviderDataObject.IdCodeQualifier = Convert.ToString(DropDownListIdCodeQualifier.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.RenderingProviderIdentifier = Convert.ToString(TextBoxRenderingProviderIdentifier.Text, Nothing).Trim()
                objRenderingProviderDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()
                objRenderingProviderDataObject.ProviderCode = Convert.ToString(DropDownListProviderCode.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.ReferenceIdQualifier = Convert.ToString(DropDownListRefIdQualifier.SelectedValue, Nothing).Trim()
                objRenderingProviderDataObject.TaxonomyCode = Convert.ToString(TextBoxTaxonomyCode.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objRenderingProviderDataObject.UserId = objShared.UserId
                objRenderingProviderDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertRenderingProviderInfo(objShared.ConVisitel, objRenderingProviderDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objRenderingProviderDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonRenderingProviderDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()
            Try
                If (String.IsNullOrEmpty(LabelRenderingProviderId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteRenderingProviderInfo(objShared.ConVisitel, Convert.ToInt64(LabelRenderingProviderId.Text), objShared.UserId)
                LabelRenderingProviderId.Text = String.Empty
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
                         & " var LoaderImagePath4 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton4 ='ButtonRenderingProviderDelete'; " _
                         & " var DeleteDialogHeader4 ='EDI Central: Rendering Provider'; " _
                         & " var DeleteDialogConfirmMsg4 ='Are you sure to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress4); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteRenderingProvider();" _
                         & "     prm.add_endRequest(DeleteRenderingProvider); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        Private Sub InitializeControl()

            AddHandler ButtonRenderingProviderClear.Click, AddressOf ButtonRenderingProviderClear_Click
            AddHandler ButtonRenderingProviderSave.Click, AddressOf ButtonRenderingProviderSave_Click
            AddHandler ButtonRenderingProviderDelete.Click, AddressOf ButtonRenderingProviderDelete_Click

            ButtonRenderingProviderClear.ClientIDMode = ClientIDMode.Static
            ButtonRenderingProviderSave.ClientIDMode = ClientIDMode.Static
            ButtonRenderingProviderDelete.ClientIDMode = ClientIDMode.Static

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
            objShared.SetControlTextLength(TextBoxRenderingProviderIdentifier, 80)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
            objShared.SetControlTextLength(TextBoxTaxonomyCode, 30)
        End Sub

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
            TextBoxEINOrSSN.ToolTip = If(String.IsNullOrEmpty(TextBoxEINOrSSN.ToolTip),
                                                          "Reference information as defined for a particular Transaction Set or as specified",
                                                          TextBoxEINOrSSN.ToolTip)

            DropDownListIdCodeQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListIdCodeQualifierToolTip"), Nothing)
            DropDownListIdCodeQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListIdCodeQualifier.ToolTip),
                                                          "Code designating the system/method of code structure used for Identification",
                                                          DropDownListIdCodeQualifier.ToolTip)

            TextBoxRenderingProviderIdentifier.ToolTip = Convert.ToString(ResourceTable("TextBoxRenderingProviderIdentifierToolTip"), Nothing)
            TextBoxRenderingProviderIdentifier.ToolTip = If(String.IsNullOrEmpty(TextBoxRenderingProviderIdentifier.ToolTip),
                                                          "Code identifying a party or other code",
                                                          TextBoxRenderingProviderIdentifier.ToolTip)

            ResourceTable = Nothing
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

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim())) Then
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

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListReferenceIdQualifier.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Reference Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxEINOrSSN.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter EIN Or SSN.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListIdCodeQualifier.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Id Code Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxRenderingProviderIdentifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Rendering Provider Identifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxRenderingProviderIdentifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Rendering Provider Identifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter ContractNo.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListProviderCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Provider Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListRefIdQualifier.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Ref Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxTaxonomyCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Taxonomy Code.")
                Return False
            End If

            Return True

        End Function

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

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelRenderingProviderIdentifier.Text = Convert.ToString(ResourceTable("LabelRenderingProviderIdentifier"), Nothing)
            LabelRenderingProviderIdentifier.Text = If(String.IsNullOrEmpty(LabelRenderingProviderIdentifier.Text),
                                                       "Rendering Provider Identifier:", LabelRenderingProviderIdentifier.Text)

            LabelProviderCode.Text = Convert.ToString(ResourceTable("LabelProviderCode"), Nothing)
            LabelProviderCode.Text = If(String.IsNullOrEmpty(LabelProviderCode.Text), "Provider Code:", LabelProviderCode.Text)

            LabelRefIdQualifier.Text = Convert.ToString(ResourceTable("LabelRefIdQualifier"), Nothing)
            LabelRefIdQualifier.Text = If(String.IsNullOrEmpty(LabelRefIdQualifier.Text), "Ref Id Qualifier:", LabelRefIdQualifier.Text)

            LabelTaxonomyCode.Text = Convert.ToString(ResourceTable("LabelTaxonomyCode"), Nothing)
            LabelTaxonomyCode.Text = If(String.IsNullOrEmpty(LabelTaxonomyCode.Text), "Taxonomy Code:", LabelTaxonomyCode.Text)

            ButtonRenderingProviderClear.Text = Convert.ToString(ResourceTable("ButtonRenderingProviderClear"), Nothing)
            ButtonRenderingProviderClear.Text = If(String.IsNullOrEmpty(ButtonRenderingProviderClear.Text), "Clear", ButtonRenderingProviderClear.Text)

            ButtonRenderingProviderSave.Text = Convert.ToString(ResourceTable("ButtonRenderingProviderSave"), Nothing)
            ButtonRenderingProviderSave.Text = If(String.IsNullOrEmpty(ButtonRenderingProviderSave.Text), "Save", ButtonRenderingProviderSave.Text)

            ButtonRenderingProviderDelete.Text = Convert.ToString(ResourceTable("ButtonRenderingProviderDelete"), Nothing)
            ButtonRenderingProviderDelete.Text = If(String.IsNullOrEmpty(ButtonRenderingProviderDelete.Text), "Delete", ButtonRenderingProviderDelete.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "BillingProvider", ValidationGroup)

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
                                                     objShared.InlineAssignHelper(DropDownListProviderCode.SelectedIndex,
                                                     objShared.InlineAssignHelper(DropDownListRefIdQualifier.SelectedIndex,
                                                     objShared.InlineAssignHelper(DropDownListIdCodeQualifier.SelectedIndex, 0)))))


            TextBoxLastOrOrgName.Text = objShared.InlineAssignHelper(TextBoxFirst.Text, objShared.InlineAssignHelper(TextBoxMiddle.Text,
                                        objShared.InlineAssignHelper(TextBoxPrefix.Text, objShared.InlineAssignHelper(TextBoxSuffix.Text,
                                        objShared.InlineAssignHelper(TextBoxReferringProviderUpdateDate.Text, objShared.InlineAssignHelper(TextBoxReferringProviderUpdateBy.Text,
                                        objShared.InlineAssignHelper(TextBoxAddress.Text, objShared.InlineAssignHelper(TextBoxCity.Text,
                                        objShared.InlineAssignHelper(TextBoxState.Text, objShared.InlineAssignHelper(TextBoxZip.Text,
                                        objShared.InlineAssignHelper(TextBoxEINOrSSN.Text, objShared.InlineAssignHelper(TextBoxRenderingProviderIdentifier.Text,
                                        objShared.InlineAssignHelper(TextBoxContractNo.Text, objShared.InlineAssignHelper(TextBoxTaxonomyCode.Text, String.Empty))))))))))))))

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

            BindProviderCodeDropDownList()
            BindRefIdQualifierDropDownList()
        End Sub

        Private Sub BindProviderCodeDropDownList()

            Dim objBLEDICentral As New BLEDICentral

            Try

                objBLEDICentral.SelectProviderCodeInfo(objShared.VisitelConnectionString, SqlDataSourceProviderCode)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Provider Code information")
            Finally
                objBLEDICentral = Nothing
            End Try


            DropDownListProviderCode.DataSource = SqlDataSourceProviderCode
            DropDownListProviderCode.DataTextField = "PRV01_PROVIDER_CODE"
            DropDownListProviderCode.DataValueField = "PRV01_PROVIDER_CODE"
            DropDownListProviderCode.DataBind()

            DropDownListProviderCode.Items.Insert(0, New ListItem(" ", "-1"))

        End Sub

        Private Sub BindRefIdQualifierDropDownList()

            Dim objBLEDICentral As New BLEDICentral

            Try

                objBLEDICentral.SelectReferenceIdQualifierInfo(objShared.VisitelConnectionString, SqlDataSourceRefIdQualifier)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Ref Id Qualifier information")
            Finally
                objBLEDICentral = Nothing
            End Try


            DropDownListRefIdQualifier.DataSource = SqlDataSourceRefIdQualifier
            DropDownListRefIdQualifier.DataTextField = "PRV02_REF_ID_QUALIFIER"
            DropDownListRefIdQualifier.DataValueField = "PRV02_REF_ID_QUALIFIER"
            DropDownListRefIdQualifier.DataBind()

            DropDownListRefIdQualifier.Items.Insert(0, New ListItem(" ", "-1"))

        End Sub

        Public Sub LoadRenderingProvider()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objRenderingProviderDataObject As BLEDICentral.RenderingProviderDataObject

            objRenderingProviderDataObject = objBLEDICentral.SelectRenderingProviderInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelRenderingProviderId.Text = objRenderingProviderDataObject.Id

            DropDownListEntityIdCode.SelectedIndex = DropDownListEntityIdCode.Items.IndexOf(DropDownListEntityIdCode.Items.FindByValue(
                                                                          objRenderingProviderDataObject.EntityIdCode))

            DropDownListEntityTypeQualifier.SelectedIndex = DropDownListEntityTypeQualifier.Items.IndexOf(DropDownListEntityTypeQualifier.Items.FindByValue(
                                                                          objRenderingProviderDataObject.EntityTypeQualifier))

            TextBoxLastOrOrgName.Text = objRenderingProviderDataObject.LastOrOrganizationName
            TextBoxFirst.Text = objRenderingProviderDataObject.FirstName
            TextBoxMiddle.Text = objRenderingProviderDataObject.MiddleName
            TextBoxPrefix.Text = objRenderingProviderDataObject.Prefix
            TextBoxSuffix.Text = objRenderingProviderDataObject.Suffix

            TextBoxAddress.Text = objRenderingProviderDataObject.Address
            TextBoxCity.Text = objRenderingProviderDataObject.City
            TextBoxState.Text = objRenderingProviderDataObject.State
            TextBoxZip.Text = objRenderingProviderDataObject.Zip

            DropDownListReferenceIdQualifier.SelectedIndex = DropDownListReferenceIdQualifier.Items.IndexOf(DropDownListReferenceIdQualifier.Items.FindByValue(
                                                                          objRenderingProviderDataObject.ReferenceIdQualifier))

            TextBoxEINOrSSN.Text = objRenderingProviderDataObject.EINOrSSN

            DropDownListIdCodeQualifier.SelectedIndex = DropDownListIdCodeQualifier.Items.IndexOf(DropDownListIdCodeQualifier.Items.FindByValue(
                                                                          objRenderingProviderDataObject.IdCodeQualifier))

            TextBoxRenderingProviderIdentifier.Text = objRenderingProviderDataObject.RenderingProviderIdentifier
            TextBoxContractNo.Text = objRenderingProviderDataObject.ContractNo

            DropDownListProviderCode.SelectedIndex = DropDownListProviderCode.Items.IndexOf(DropDownListProviderCode.Items.FindByValue(
                                                                          objRenderingProviderDataObject.ProviderCode))

            DropDownListRefIdQualifier.SelectedIndex = DropDownListRefIdQualifier.Items.IndexOf(DropDownListRefIdQualifier.Items.FindByValue(
                                                                          objRenderingProviderDataObject.ProviderReferenceIdQualifier))

            TextBoxTaxonomyCode.Text = objRenderingProviderDataObject.TaxonomyCode
            TextBoxReferringProviderUpdateDate.Text = objRenderingProviderDataObject.UpdateDate
            TextBoxReferringProviderUpdateBy.Text = objRenderingProviderDataObject.UpdateBy

            objRenderingProviderDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub
    End Class
End Namespace