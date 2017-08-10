<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="~/UserControl/UCSlidingContents.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.UCSlidingContents" %>
<%=sContentsHTML %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#newFeaturesSlider").easySlider({
            controlsBefore: '<p id="controls">',
            controlsAfter: '</p>',
            prevId: 'prevBtn',
            nextId: 'nextBtn',
            auto: true,
            continuous: true
        });
    });	
</script>
