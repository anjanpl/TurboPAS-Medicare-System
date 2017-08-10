Imports VisitelCommon.VisitelCommon
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient

Namespace Visitel.UserControl.EDICentral

    Public Class BillOrPayToProviderControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "BillOrPayToProviderControl"
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

        Private Sub ButtonBillOrPayToProviderClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonBillOrPayToProviderSave_Click(sender As Object, e As EventArgs)
            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objBillOrPayToProviderDataObject As New BLEDICentral.BillOrPayToProviderDataObject()

                objBillOrPayToProviderDataObject.HierarchicalIdNumber = Convert.ToString(TextBoxHierarchicalIDNumber.Text, Nothing).Trim()
                objBillOrPayToProviderDataObject.HierarchicalLevelCode = Convert.ToString(TextBoxHierarchicalLevelCode.Text, Nothing).Trim()
                objBillOrPayToProviderDataObject.ReferenceIdQualifier = Convert.ToString(TextBoxReferenceIdQualifier.Text, Nothing).Trim()
                objBillOrPayToProviderDataObject.TaxonomyCode = Convert.ToString(TextBoxTaxonomyCode.Text, Nothing).Trim()
                objBillOrPayToProviderDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()
                objBillOrPayToProviderDataObject.HierarchicalChildCode = Convert.ToInt64(TextBoxHierarchicalChildCode.Text)
                objBillOrPayToProviderDataObject.ProviderCode = Convert.ToString(DropDownListProviderCode.SelectedValue, Nothing)

                Dim objBLEDICentral As New BLEDICentral()

                objBillOrPayToProviderDataObject.UserId = objShared.UserId
                objBillOrPayToProviderDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertBillOrPayToProviderInfo(objShared.ConVisitel, objBillOrPayToProviderDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objBillOrPayToProviderDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonBillOrPayToProviderDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()

            Try
                If (String.IsNullOrEmpty(LabelBillOrPayToProviderId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteBillOrPayToProviderInfo(objShared.ConVisitel, Convert.ToInt64(LabelBillOrPayToProviderId.Text), objShared.UserId)
                LabelBillOrPayToProviderId.Text = String.Empty
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                ClearControl()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Delete {0}", ex.Message))
            Finally
                objBLEDICentral = Nothing
            End Try
        End Sub

        Private Sub InitializeControl()
            AddHandler ButtonBillOrPayToProviderClear.Click, AddressOf ButtonBillOrPayToProviderClear_Click
            AddHandler ButtonBillOrPayToProviderSave.Click, AddressOf ButtonBillOrPayToProviderSave_Click
            AddHandler ButtonBillOrPayToProviderDelete.Click, AddressOf ButtonBillOrPayToProviderDelete_Click

            ButtonBillOrPayToProviderClear.ClientIDMode = ClientIDMode.Static
            ButtonBillOrPayToProviderSave.ClientIDMode = ClientIDMode.Static
            ButtonBillOrPayToProviderDelete.ClientIDMode = ClientIDMode.Static

            TextBoxBillOrPayToProviderControlUpdateBy.Enabled = False
            TextBoxBillOrPayToProviderControlUpdateDate.Enabled = False

            ControlTextLength()
        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxHierarchicalIDNumber, 50)
            objShared.SetControlTextLength(TextBoxHierarchicalLevelCode, 255)
            objShared.SetControlTextLength(TextBoxReferenceIdQualifier, 25)
            objShared.SetControlTextLength(TextBoxTaxonomyCode, 30)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
            objShared.SetControlTextLength(TextBoxHierarchicalChildCode, 15)
        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                         & " var LoaderImagePath6 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton6 ='ButtonBillOrPayToProviderDelete'; " _
                         & " var DeleteDialogHeader6 ='EDI Central: Bill/Pay-To Provider'; " _
                         & " var DeleteDialogConfirmMsg6 ='Are you sure you want to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress6); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteBillOrPayToProvider();" _
                         & "     prm.add_endRequest(DeleteBillOrPayToProvider); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            LabelHierarchicalIDNumber.Text = Convert.ToString(ResourceTable("LabelHierarchicalIDNumber"), Nothing)
            LabelHierarchicalIDNumber.Text = If(String.IsNullOrEmpty(LabelHierarchicalIDNumber.Text), "Hierarchical ID Number:", LabelHierarchicalIDNumber.Text)

            LabelHierarchicalLevelCode.Text = Convert.ToString(ResourceTable("LabelHierarchicalLevelCode"), Nothing)
            LabelHierarchicalLevelCode.Text = If(String.IsNullOrEmpty(LabelHierarchicalLevelCode.Text), "Hierarchical Level Code:", LabelHierarchicalLevelCode.Text)

            LabelHierarchicalChildCode.Text = Convert.ToString(ResourceTable("LabelHierarchicalChildCode"), Nothing)
            LabelHierarchicalChildCode.Text = If(String.IsNullOrEmpty(LabelHierarchicalChildCode.Text), "Hierarchical Child Code:", LabelHierarchicalChildCode.Text)

            LabelProviderCode.Text = Convert.ToString(ResourceTable("LabelProviderCode"), Nothing)
            LabelProviderCode.Text = If(String.IsNullOrEmpty(LabelProviderCode.Text), "Provider Code:", LabelProviderCode.Text)

            LabelReferenceIdQualifier.Text = Convert.ToString(ResourceTable("LabelReferenceIdQualifier"), Nothing)
            LabelReferenceIdQualifier.Text = If(String.IsNullOrEmpty(LabelReferenceIdQualifier.Text), "Reference Id Qualifier:", LabelReferenceIdQualifier.Text)

            LabelTaxonomyCode.Text = Convert.ToString(ResourceTable("LabelTaxonomyCode"), Nothing)
            LabelTaxonomyCode.Text = If(String.IsNullOrEmpty(LabelTaxonomyCode.Text), "Taxonomy Code:", LabelTaxonomyCode.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelBillOrPayToProviderControlUpdateDate.Text = Convert.ToString(ResourceTable("LabelBillOrPayToProviderControlUpdateDate"), Nothing)
            LabelBillOrPayToProviderControlUpdateDate.Text = If(String.IsNullOrEmpty(LabelBillOrPayToProviderControlUpdateDate.Text),
                                                                "Update Date:", LabelBillOrPayToProviderControlUpdateDate.Text)

            LabelBillOrPayToProviderControlUpdateBy.Text = Convert.ToString(ResourceTable("LabelBillOrPayToProviderControlUpdateBy"), Nothing)
            LabelBillOrPayToProviderControlUpdateBy.Text = If(String.IsNullOrEmpty(LabelBillOrPayToProviderControlUpdateBy.Text),
                                                                "Update By:", LabelBillOrPayToProviderControlUpdateBy.Text)

            ButtonBillOrPayToProviderClear.Text = Convert.ToString(ResourceTable("ButtonBillOrPayToProviderClear"), Nothing)
            ButtonBillOrPayToProviderClear.Text = If(String.IsNullOrEmpty(ButtonBillOrPayToProviderClear.Text), "Clear", ButtonBillOrPayToProviderClear.Text)

            ButtonBillOrPayToProviderSave.Text = Convert.ToString(ResourceTable("ButtonBillOrPayToProviderSave"), Nothing)
            ButtonBillOrPayToProviderSave.Text = If(String.IsNullOrEmpty(ButtonBillOrPayToProviderSave.Text), "Save", ButtonBillOrPayToProviderSave.Text)

            ButtonBillOrPayToProviderDelete.Text = Convert.ToString(ResourceTable("ButtonBillOrPayToProviderDelete"), Nothing)
            ButtonBillOrPayToProviderDelete.Text = If(String.IsNullOrEmpty(ButtonBillOrPayToProviderDelete.Text), "Delete", ButtonBillOrPayToProviderDelete.Text)

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

            TextBoxHierarchicalIDNumber.Text = objShared.InlineAssignHelper(TextBoxHierarchicalLevelCode.Text, objShared.InlineAssignHelper(TextBoxReferenceIdQualifier.Text,
                                               objShared.InlineAssignHelper(TextBoxTaxonomyCode.Text, objShared.InlineAssignHelper(TextBoxContractNo.Text,
                                               objShared.InlineAssignHelper(TextBoxBillOrPayToProviderControlUpdateDate.Text,
                                               objShared.InlineAssignHelper(TextBoxBillOrPayToProviderControlUpdateBy.Text,
                                               objShared.InlineAssignHelper(TextBoxHierarchicalChildCode.Text, String.Empty)))))))

            DropDownListProviderCode.SelectedIndex = 0

        End Sub

        Private Sub GetData()
            BindProviderCodeDropDownList()
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxHierarchicalIDNumber.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Hierarchical ID Number.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxHierarchicalLevelCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Hierarchical Level Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxReferenceIdQualifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Reference Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxTaxonomyCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Reference Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Contract No.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxHierarchicalChildCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Hierarchical Child Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListProviderCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Provider Code.")
                Return False
            End If

            Return True

        End Function

        Private Sub BindProviderCodeDropDownList()
            DropDownListProviderCode.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.ProviderCode)()
            DropDownListProviderCode.DataTextField = "Value"
            DropDownListProviderCode.DataValueField = "Value"
            DropDownListProviderCode.DataBind()

            'DropDownListProviderCode.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        Public Sub LoadBillOrPayToProvider()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objBillOrPayToProviderDataObject As BLEDICentral.BillOrPayToProviderDataObject

            objBillOrPayToProviderDataObject = objBLEDICentral.SelectBillOrPayToProviderInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelBillOrPayToProviderId.Text = objBillOrPayToProviderDataObject.Id

            TextBoxHierarchicalIDNumber.Text = objBillOrPayToProviderDataObject.HierarchicalIdNumber
            TextBoxHierarchicalLevelCode.Text = objBillOrPayToProviderDataObject.HierarchicalLevelCode

            TextBoxHierarchicalChildCode.Text = If(objBillOrPayToProviderDataObject.HierarchicalChildCode >= 0, objBillOrPayToProviderDataObject.HierarchicalChildCode, String.Empty)

            DropDownListProviderCode.SelectedIndex = DropDownListProviderCode.Items.IndexOf(DropDownListProviderCode.Items.FindByValue(
                                                                         objBillOrPayToProviderDataObject.ProviderCode))

            TextBoxReferenceIdQualifier.Text = objBillOrPayToProviderDataObject.ReferenceIdQualifier
            TextBoxTaxonomyCode.Text = objBillOrPayToProviderDataObject.TaxonomyCode
            TextBoxContractNo.Text = objBillOrPayToProviderDataObject.ContractNo

            TextBoxBillOrPayToProviderControlUpdateDate.Text = objBillOrPayToProviderDataObject.UpdateDate
            TextBoxBillOrPayToProviderControlUpdateBy.Text = objBillOrPayToProviderDataObject.UpdateBy

            objBillOrPayToProviderDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub
    End Class
End Namespace