$(document).ready(() => {

    $(document).on('change', '.toggle-active', function () {
        var userId = $(this).data('id');
        var isChecked = $(this).is(':checked');

        $.ajax({
            url: '/Admin/ToggleIsActive',
            type: 'POST',
            data: {
                userId: userId,
                isActive: isChecked
            },
            success: function (response) {
                if (response.success) {
                    alert('User status updated');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while updating the user.');
            }
        });
    });

 
    $(document).on('change', '.toggle-admin', function () {
        var userId = $(this).data('id');
        var isChecked = $(this).is(':checked');

        $.ajax({
            url: '/Admin/ToggleIsAdmin',
            type: 'POST',
            data: {
                userId: userId,
                isAdmin: isChecked
            },
            success: function (response) {
                if (response.success) {
                    alert('User status updated');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while updating the user.');
            }
        });
    });
});
function loadItems() {
    $.ajax({
        url: "/Staff/ContactDetails",
        type: "GET",
        success: function (data) {
            $("#tblBody").empty();

            if (data.success === false) {
                alert(data.message);  
            } else {
                $.each(data, function (index, item) {
                    var row = `<tr>
                        <td>${item.FName}</td>
                        <td>${item.LName}</td>
                        <td>${item.IsActive}</td>
                        <td>
                            <button onclick="editItem('${item.ContactId}')" class="btn btn-success">Edit</button>
                        </td>
                        <td>
                            <button onclick="deleteRecord('${item.ContactId}')" class="btn btn-danger">Delete</button>
                        </td>
                    </tr>`;
                    $("#tblBody").append(row);
                });
            }
        },
        error: function (err) {
            $("#tblBody").empty();
            alert("No Data Available");
        }
    });
}

function addNewContact(newItem) {
    $.ajax({
        url: "/Staff/CreateContact",
        type: "POST",
        data: newItem,

        success: function (item) {
            alert("New Item Added Successfully")
            loadItems()
        },
        error: function (err) {
            alert("Error adding new record")
        }
    })
}


$("#btnAdd").click(() => {
    $("#contactList").hide();
    $("#newContact").show();
})