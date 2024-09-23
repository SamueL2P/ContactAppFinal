$(document).ready(function () {
    var contactId = $('#contactId').val();

    console.log("Contact ID:", contactId);

    $("#grid").jqGrid({
        url: '/Staff/GetData?contactId=' + contactId,
        mtype: "GET",
        datatype: "json",
        colNames: ["ContactDetailId", "Type", "Value"],
        colModel: [
            { name: "ContactDetailId", key: true, hidden: true, editable: true },
            { name: "Type", editable: true, searchoptions: { sopt: ['eq'] } },
            { name: "Value", editable: true, searchoptions: { sopt: ['cn'] } }
        ],
        pager: '#pager',    
        rowNum: 5,
        rowList: [5, 10, 15],   
        sortname: 'ContactDetailId',
        sortorder: 'asc',
        viewrecords: true,
        height: 250,
        width: 700,
        caption: "Manage Contact Details",
        jsonReader: {
            root: function (obj) { return obj.rows; },
            page: function (obj) { return obj.page; },
            total: function (obj) { return obj.total; },
            records: function (obj) { return obj.records; },
            repeatitems: false,
            id: "ContactDetailId"
        },
        loadComplete: function (data) {
            console.log("Data loaded into jqGrid:", data); 
        },
        gridComplete: function () {
            console.log("Grid complete!");

            $("#grid").jqGrid('navGrid', '#pager',
                { edit: true, add: true, del: true, search: true, refresh: true }, 
                {
                    
                    url: '/Staff/Edit', 
                    mtype: "POST", 
                    closeAfterEdit: true,
                    width: 400,
                    afterSubmit: handleResponse,
                    serializeEditData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxEditOptions: {
                        contentType: "application/json" 
                    }

                },
                {
            
                    url: '/Staff/Add',
                    closeAfterAdd: true,
                    width: 400,
                    afterSubmit: handleResponse
                },
                {
                   
                    url: '/Staff/Delete',
                    afterSubmit: handleResponse
                },
                {
               
                    multipleSearch: false,
                    closeAfterSearch: true
                }
            );
        }
    });

    function handleResponse(response, postdata) {
        var result = JSON.parse(response.responseText);
        if (result.success) {
            alert(result.message);
            return [true];
        } else {
            alert(result.message);
            return [false];
        }
    }
});
