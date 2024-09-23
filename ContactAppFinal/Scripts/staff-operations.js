$(document).ready(function () {

    loadContacts();
    
    $('#contactModal').on('submit', '#createContactForm', function (e) {
        e.preventDefault();

        let contactData = {
            FName: $('#FName').val(),
            LName: $('#LName').val()
        };

        $.ajax({
            url: '/Staff/CreateContact',
            type: 'POST',
            data: JSON.stringify(contactData),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    hidePartialView();
                    loadContacts(); 
                } else {
                    alert(response.message);
                }
            }
        });
    });

  
    $('#contactModal').on('submit', '#editContactForm', function (e) {
        e.preventDefault();

        let contactData = {
            ContactId: $('#ContactId').val(),
            FName: $('#FName').val(),
            LName: $('#LName').val()
        };

        $.ajax({
            url: '/Staff/EditContact',
            type: 'POST',
            data: JSON.stringify(contactData),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    hidePartialView();
                    loadContacts(); 
                } else {
                    alert(response.message);
                }
            }
        });
    });
});


function loadContacts() {
    let userId = $('#UserId').val();

    $.ajax({
        url: '/Staff/GetContactList',
        type: 'GET',
        data: { userId: userId }, 
        success: function (data) {
            $('#contactListContainer').html(data);
            $('#contactListContainer').show(); 
            $('#contactModal').hide(); 

      
            $('.isActiveToggle').each(function () {
                $(this).prop('checked', $(this).data('active'));
            });
        }
    });
}


function createContact() {
    $.ajax({
        url: '/Staff/CreateContact',
        type: 'GET',
        success: function (data) {
            loadContacts();
            $('#contactListContainer').hide(); 
            $('#contactModal').html(data);
            $('#contactModal').show();
            
        }
    });
}

function editContact(contactId) {
    $.ajax({
        url: '/Staff/EditContact',
        type: 'GET',
        data: { contactId: contactId },
        success: function (data) {
            $('#contactListContainer').hide(); 
            $('#contactModal').html(data);
            $('#contactModal').show();
            loadContacts();
        }
    });
}

function hidePartialView() {
    $('#contactModal').hide(); 
    $('#contactListContainer').show();
    loadContacts();
}


function deleteContact(contactId) {
    if (confirm('Are you sure?')) {
        $.ajax({
            url: '/Staff/DeleteContact',
            type: 'POST',
            data: { id: contactId },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    loadContacts(); 
                } else {
                    alert(response.message);
                }
            }
        });
    }
}

$.ajax({
    url: '/Staff/GetContactList',
    type: 'GET',
    data: { userId: userId }, 
    success: function (data) {
        $('#contactListContainer').html(data);
        $('#contactListContainer').show(); 
            $('#contactModal').hide(); 
            
        $('.isActiveToggle').each(function () {
            $(this).prop('checked', $(this).data('active'));
        });
    }
});