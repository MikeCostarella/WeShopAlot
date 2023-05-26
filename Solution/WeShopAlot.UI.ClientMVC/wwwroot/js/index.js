$(document).ready(ApplicationManagement_ReadyDom);

const ApplicationManagement_ReadyDom = function () {
    $('#btnSave').click(ApplicationManagementService.SaveContinueApplication());
};

var ApplicationManagementService = {
    SaveContinueApplication: function (applicationManagementToSave) {
        return $.ajax({
            type: 'POST',
            url: '/Product/SaveAndContinue',
            dataType: "json",
            data: applicationManagementToSave
        });
    },
}