Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class EVVInfoControl
        Inherits BaseUserControl

        Private ControlName As String

        Private TextBoxEVVBillCode As TextBox, TextBoxEVVProcCodeQualifier As TextBox, TextBoxEVVLandPhone As TextBox

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "EVVInfoControl"
            objShared = New SharedWebControls()
            GetCaptionFromResource()
            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If (Not IsPostBack) Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            TextBoxEVVBillCode = DirectCast(Session(StaticSettings.SessionField.TEXTBOX_EVV_BILL_CODE), TextBox)
            TextBoxEVVProcCodeQualifier = DirectCast(Session(StaticSettings.SessionField.TEXTBOX_EVV_PROC_CODE_QUALIFIER), TextBox)
            TextBoxEVVLandPhone = DirectCast(Session(StaticSettings.SessionField.TEXTBOX_EVV_LAND_PHONE), TextBox)

            DropDownListProgramOrService.ClientIDMode = objShared.InlineAssignHelper(DropDownListServiceGroup.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListServiceCode.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListServiceCodeDescription.ClientIDMode, UI.ClientIDMode.Static)))

            DropDownListProgramOrService.AutoPostBack = objShared.InlineAssignHelper(DropDownListServiceGroup.AutoPostBack,
                                                         objShared.InlineAssignHelper(DropDownListServiceCode.AutoPostBack,
                                                         objShared.InlineAssignHelper(DropDownListServiceCodeDescription.AutoPostBack, True)))

            AddHandler DropDownListProgramOrService.SelectedIndexChanged, AddressOf DropDownListProgramOrService_OnSelectedIndexChanged
            AddHandler DropDownListServiceGroup.SelectedIndexChanged, AddressOf DropDownListServiceGroup_OnSelectedIndexChanged
            AddHandler DropDownListServiceCode.SelectedIndexChanged, AddressOf DropDownListServiceCode_OnSelectedIndexChanged
            AddHandler DropDownListServiceCodeDescription.SelectedIndexChanged, AddressOf DropDownListServiceCodeDescription_OnSelectedIndexChanged

            TextBoxEVVLandPhone.ClientIDMode = UI.ClientIDMode.Static

            SetControlTabIndex()

        End Sub

        Private Sub SetControlTabIndex()
            DropDownListServiceCode.TabIndex = 57
            DropDownListServiceCodeDescription.TabIndex = 58
            DropDownListServiceGroup.TabIndex = 59
            DropDownListProgramOrService.TabIndex = 60
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelEVVInformation.Text = Convert.ToString(ResourceTable("LabelEVVInformation"), Nothing).Trim()
            LabelEVVInformation.Text = If(String.IsNullOrEmpty(LabelEVVInformation.Text), "EVV Information", LabelEVVInformation.Text)

            LabelProgramOrService.Text = Convert.ToString(ResourceTable("LabelProgramOrService"), Nothing).Trim()
            LabelProgramOrService.Text = If(String.IsNullOrEmpty(LabelProgramOrService.Text), "Select Program/Service", LabelProgramOrService.Text)

            LabelServiceGroup.Text = Convert.ToString(ResourceTable("LabelServiceGroup"), Nothing).Trim()
            LabelServiceGroup.Text = If(String.IsNullOrEmpty(LabelServiceGroup.Text), "Service Group", LabelServiceGroup.Text)

            LabelServiceCode.Text = Convert.ToString(ResourceTable("LabelServiceCode"), Nothing).Trim()
            LabelServiceCode.Text = If(String.IsNullOrEmpty(LabelServiceCode.Text), "Service Code", LabelServiceCode.Text)

            LabelServiceCodeDescription.Text = Convert.ToString(ResourceTable("LabelServiceCodeDescription"), Nothing).Trim()
            LabelServiceCodeDescription.Text = If(String.IsNullOrEmpty(LabelServiceCodeDescription.Text), "Service Code Description", LabelServiceCodeDescription.Text)

            LabelHCPCS.Text = Convert.ToString(ResourceTable("LabelHCPCS"), Nothing).Trim()
            LabelHCPCS.Text = If(String.IsNullOrEmpty(LabelHCPCS.Text), "HCPCS", LabelHCPCS.Text)

            LabelModifiers.Text = Convert.ToString(ResourceTable("LabelModifiers"), Nothing).Trim()
            LabelModifiers.Text = If(String.IsNullOrEmpty(LabelModifiers.Text), "Modifiers", LabelModifiers.Text)

            ResourceTable = Nothing
        End Sub

        Private Sub DropDownListProgramOrService_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            FillEVVInfo(DropDownListProgramOrService)

        End Sub

        Private Sub DropDownListServiceGroup_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            FillEVVInfo(DropDownListServiceGroup)

        End Sub

        Private Sub DropDownListServiceCode_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            FillEVVInfo(DropDownListServiceCode)

        End Sub

        Private Sub DropDownListServiceCodeDescription_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            FillEVVInfo(DropDownListServiceCodeDescription)

        End Sub

        ''' <summary>
        ''' This is for filling EVV Information
        ''' </summary>
        ''' <param name="DropDownListEVV"></param>
        ''' <remarks></remarks>
        Private Sub FillEVVInfo(DropDownListEVV As DropDownList)

            If Not DropDownListEVV.SelectedValue.Equals("-1") Then

                Dim EVVInfoBillCodeList As New List(Of EVVDataObject)()
                Dim EVVProcedureCodeQualifierList As New List(Of EVVDataObject)()
                Dim EVVLandPhoneList As New List(Of EVVDataObject)()

                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetEVV(objShared.ConVisitel, objShared.CompanyId, Nothing, Nothing, Nothing, Nothing, EVVInfoBillCodeList,
                                           EVVProcedureCodeQualifierList, EVVLandPhoneList)
                objBLClientInfo = Nothing

                Dim objEVVDataObject As New EVVDataObject
                objEVVDataObject = (From p In EVVInfoBillCodeList Where p.ID = DropDownListEVV.SelectedValue).SingleOrDefault

                If (Not TextBoxEVVBillCode Is Nothing) Then
                    TextBoxEVVBillCode.Text = objEVVDataObject.BillCode
                End If

                EVVInfoBillCodeList = Nothing

                objEVVDataObject = (From p In EVVProcedureCodeQualifierList Where p.ID = DropDownListEVV.SelectedValue).SingleOrDefault
                If (Not TextBoxEVVProcCodeQualifier Is Nothing) Then
                    TextBoxEVVProcCodeQualifier.Text = objEVVDataObject.ProcedureCodeQualifier
                End If

                EVVProcedureCodeQualifierList = Nothing

                objEVVDataObject = (From p In EVVLandPhoneList Where p.ID = DropDownListEVV.SelectedValue).SingleOrDefault
                If (Not TextBoxEVVLandPhone Is Nothing) Then
                    TextBoxEVVLandPhone.Text = objEVVDataObject.HCPCS
                End If

                EVVLandPhoneList = Nothing

            Else

                'If (Not TextBoxEVVBillCode Is Nothing) Then
                '    TextBoxEVVBillCode.Text = String.Empty
                'End If

                'If (Not TextBoxEVVProcCodeQualifier Is Nothing) Then
                '    TextBoxEVVProcCodeQualifier.Text = String.Empty
                'End If

                'If (Not TextBoxEVVLandPhone Is Nothing) Then
                '    TextBoxEVVLandPhone.Text = String.Empty
                'End If

            End If

        End Sub

        Private Sub GetData()
            BindEVVDropDownLists()
        End Sub

        Public Sub LoadData()
            GetData()
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseEVVInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByText(
                                                            Convert.ToString(objClientCaseEVVInfoDataObject.ServiceCode, Nothing)))
            DropDownListServiceCode_OnSelectedIndexChanged(DropDownListServiceCode, Nothing)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByText(
                                                         Convert.ToString(objClientCaseEVVInfoDataObject.ServiceCode, Nothing)))
            DropDownListProgramOrService_OnSelectedIndexChanged(DropDownListProgramOrService, Nothing)

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByText(
                                                               Convert.ToString(objClientCaseEVVInfoDataObject.ServiceCodeDescription, Nothing)))
            DropDownListServiceCodeDescription_OnSelectedIndexChanged(DropDownListServiceCodeDescription, Nothing)

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByText(
                                                     Convert.ToString(objClientCaseEVVInfoDataObject.ServiceGroup, Nothing)))
            DropDownListServiceGroup_OnSelectedIndexChanged(DropDownListServiceGroup, Nothing)

        End Sub

        ''' <summary>
        ''' Binding EVV Drop Down Lists
        ''' </summary>
        Private Sub BindEVVDropDownLists()

            Dim ProgramServiceList As New List(Of EVVDataObject)()
            Dim ServiceGroupList As New List(Of EVVDataObject)()
            Dim ServiceCodeList As New List(Of EVVDataObject)()
            Dim ServiceCodeDescriptionList As New List(Of EVVDataObject)()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetEVV(objShared.ConVisitel, objShared.CompanyId, ProgramServiceList, ServiceGroupList, ServiceCodeList, ServiceCodeDescriptionList,
                                       Nothing, Nothing, Nothing)
            objBLClientInfo = Nothing

            DropDownListProgramOrService.DataSource = ProgramServiceList
            DropDownListProgramOrService.DataTextField = "ProgramService"
            DropDownListProgramOrService.DataValueField = "ID"
            DropDownListProgramOrService.DataBind()

            DropDownListProgramOrService.Items.Insert(0, New ListItem("", "-1"))
            ProgramServiceList = Nothing

            DropDownListServiceGroup.DataSource = ServiceGroupList
            DropDownListServiceGroup.DataTextField = "ServiceGroup"
            DropDownListServiceGroup.DataValueField = "ID"
            DropDownListServiceGroup.DataBind()

            DropDownListServiceGroup.Items.Insert(0, New ListItem("", "-1"))
            ServiceGroupList = Nothing

            DropDownListServiceCode.DataSource = ServiceCodeList
            DropDownListServiceCode.DataTextField = "ServiceCode"
            DropDownListServiceCode.DataValueField = "ID"
            DropDownListServiceCode.DataBind()

            DropDownListServiceCode.Items.Insert(0, New ListItem("", "-1"))
            ServiceCodeList = Nothing

            DropDownListServiceCodeDescription.DataSource = ServiceCodeDescriptionList
            DropDownListServiceCodeDescription.DataTextField = "ServiceCodeDescription"
            DropDownListServiceCodeDescription.DataValueField = "ID"
            DropDownListServiceCodeDescription.DataBind()

            DropDownListServiceCodeDescription.Items.Insert(0, New ListItem("", "-1"))
            ServiceCodeDescriptionList = Nothing

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            DropDownListServiceCode.SelectedIndex = objShared.InlineAssignHelper(DropDownListProgramOrService.SelectedIndex,
                                                    objShared.InlineAssignHelper(DropDownListServiceCodeDescription.SelectedIndex,
                                                    objShared.InlineAssignHelper(DropDownListServiceGroup.SelectedIndex, 0)))
        End Sub

        Public Sub DataFoSave(ByRef objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            objClientCaseEVVInfoDataObject.ServiceCode = Convert.ToString(DropDownListServiceCode.SelectedItem, Nothing).Trim()
            objClientCaseEVVInfoDataObject.ServiceCodeDescription = Convert.ToString(DropDownListServiceCodeDescription.SelectedItem, Nothing).Trim()
            objClientCaseEVVInfoDataObject.ServiceGroup = Convert.ToString(DropDownListServiceGroup.SelectedItem, Nothing).Trim()

        End Sub

    End Class
End Namespace