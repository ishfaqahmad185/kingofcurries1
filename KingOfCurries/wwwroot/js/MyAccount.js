

$(function () {
    $("#ChangePassUser").on("click", function (e) {

        var form = $("#ChangeAdminUserForm")[0];
        var isValid = form.checkValidity();
        var passwordCheck = CheckValidation();
        var ConfirmPassword = Checkconfirm_password();

        if (!isValid) {

            e.preventDefault();
            e.stopPropagation();
        }
        else if (!passwordCheck) {

            e.preventDefault();
            e.stopPropagation();
        }
        else if (!ConfirmPassword) {

            e.preventDefault();
            e.stopPropagation();
        }
        else {
            ChangeUserPasswordMain();
        }
        form.classList.add('was-validated');
        return false; // For testing only to stay on this page
    });
});

function ChangeUserPasswordMain() {

    var Password = $("#new_password").val();
    var ConfirmPassword = $("#confirm_password").val();
    var currentpassword = $("#current_password").val();
    
    var formData = new FormData();
    formData.append("Password", Password);
    formData.append("ConfirmPassword", ConfirmPassword);
    formData.append("currentPassword", currentpassword);
    
    $.ajax({
        type: "Post",
        url: "/UserLogin/ChangeUserPassword/",
        data: formData,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (result) {
            if (result.success) {
                $(window).scrollTop(0);

                $('#successAccountMessage').removeAttr('hidden');
                $(".alertmsg").attr("style", "display block !important");
                $('#successAccountText').html("success : User Password Updated Successfully");
                $('#successAccountMessage').fadeIn();

                $('#successAccountMessage').fadeTo(5000, 500).slideUp(500,
                    function () {
                        $("#successAccountMessage").slideUp(500);


                    });
                $('#successAccountMessage').attr('hidden');
                setTimeout(function () {
                    window.location.href = "/logout/";
                }, 3000);



            }
            else {

                $(window).scrollTop(0);
                $('#errorAccountMessage').removeAttr('hidden');
                $(".alertmsg").attr("style", "display block !important");
                $('#errorAccountText').html('Danger :' +result.responseText);
                $('#errorAccountMessage').fadeIn();

                $('#errorAccountMessage').fadeTo(5000, 500).slideUp(500,
                    function () {
                        $("#errorAccountMessage").slideUp(500);


                    });
                $('#errorAccountMessage').attr('hidden');

            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}



$('#new_password').keyup(function () {
    var Check = CheckValidation();
    if (check) {

        $('#PasswordAlertLabel').attr('class', 'invalid-feedback');
        $('#new_password').attr('class', 'form-control');
    }
});

function CheckValidation() {
    var check = true;

    var Passwordcheck = document.getElementById('new_password').value;

    if ($("#new_password").val().length < 8) {


        check = false;
    }
    else if (!/\d/.test($("#new_password").val())) {

        check = false;
    }
    else if
        (!/[a-z]/.test($("#new_password").val())) {


        check = false;
    }
    else if (!/[A-Z]/.test($("#new_password").val())) {


        check = false;
    } else if (!/([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/.test($("#new_password").val())) {


        check = false;
    }



    if (check == false) {

        $('#PasswordAlertLabel').attr('class', 'invalid-feedback2');
        $('#new_password').attr('class', 'form-control textboxstyle');

        return false;
        $('#AddUserPopUp').modal('show');
    }
    else {

        $('#PasswordAlertLabel').attr('class', 'invalid-feedback');
        $('#new_password').attr('class', 'form-control');
        return true;
    }

}
function Checkconfirm_password() {

    var Password = document.getElementById('new_password').value;
    var confirm_password = document.getElementById('confirm_password').value;

    if (!(Password === confirm_password)) {

        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');

        return false;


    }
    if ($("#confirm_password").val().length < 8) {


        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');
        return false;
    }
    else if (!/\d/.test($("#confirm_password").val())) {

        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');
        return false;
    }
    else if
        (!/[a-z]/.test($("#confirm_password").val())) {


        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');
        return false;
    }
    else if (!/[A-Z]/.test($("#confirm_password").val())) {


        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');
        return false;
    } else if (!/([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/.test($("#confirm_password").val())) {


        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback2');
        $('#confirm_password').attr('class', 'form-control  textboxstyle');
        return false;
    }
    else {

        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback');
        $('#confirm_password').attr('class', 'form-control');
        return true;
    }
}
$('#confirm_password').keyup(function () {
    var Check = Checkconfirm_password();
    if (check) {

        $('#confirm_passwordAlertLabel').attr('class', 'invalid-feedback');
        $('#confirm_password').attr('class', 'form-control');
        return true;
    }
    else {
        return false;
    }
});


function loadAllReservations() {
	$.ajax({
		url: "/UserLogin/ListOfReservations",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {

			console.log(result)


			var html = '';
			if (result.code === true) {





				$.each(result.jsonText, function (key, item) {



					html += '<tr>';
					html += '<td class="text-wrap" >' + (key+1) + '</td>';
					html += '<td class="text-wrap" >' + item.userName + '</td>';
					html += '<td class="text-wrap" >' + item.sysDate + '<br/>' + item.reservationTime + '</td>';
					html += '<td class="text-wrap" >' + item.phoneNo + '</td>';
					html += '<td class="text-wrap" >' + item.email + '</td>';
					html += '<td class="text-wrap" >' + item.totalGuest + '</td>';
				
				

					html += '</tr>';
				});
				$('.allReservations').html(html);
			} else {
				alert(false);
			}
		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});
}



function loadTodayOrderList() {






	$.ajax({
		url: "/UserLogin/TodayOrdersGet",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {

			console.log(result)
			var html = '';
			if (result.code === true) {

				$.each(result.jsonText, function (key, item) {

					html += '<tr onclick="loadOrderStatusInvoice(' + "'" + item.orderNo + "'" + ')">';

					html += '<td class="text-wrap" >' + (key + 1) + '</td>';
					html += '<td class="text-wrap" >' + item.orderNo + '</td>';
					html += '<td class="text-wrap" >' + item.systime + '</td>';
					html += '<td class="text-wrap" >' + item.orderStatus + '</td>';
					html += '<td class="text-wrap" >€' + item.grandTotal + '</td>';
					html += '<td class="text-wrap" >' + item.paymentType + '</td>';
					html += '<td class="text-wrap" >' + item.deliveryType + '</td>';


					html += '</tr>';

				});
				$('.todayordersMainAll').html(html);
			} else {
				alert(false);
			}
		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});
}


function loadOrderStatusInvoice(inv) {


	window.location.href = "/Home/OrderComplete/" + inv;
}


function loadOrderList() {






	$.ajax({
		url: "/UserLogin/GetAllOrders",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {

			console.log(result)
			var html = '';
			if (result.code === true) {

				$.each(result.jsonText, function (key, item) {

					html += '<tr>';
					html += '<td class="text-wrap" >' + (key + 1) + '</td>';
					html += '<td class="text-wrap" >' + item.orderNo + '</td>';
					html += '<td class="text-wrap" >' + item.systime + '</td>';
					html += '<td class="text-wrap" >' + item.orderStatus + '</td>';
					html += '<td class="text-wrap" >€' + item.grandTotal + '</td>';
					html += '<td class="text-wrap" >' + item.paymentType + '</td>';
					html += '<td class="text-wrap" >' + item.deliveryType + '</td>';

					html += ' <td>';

					//  html += '<div class="custom-control custom-switch lg"><input type="checkbox" class="custom-control-input control" name="ShowOnWebsite" id="'+showonWebsiteId+'" onchange="tableCheckBoxChange('+item.announcementID+')" data-val="true"  data-val-required="Do you want to show on Website" value="'+ item.showOnWebsite+'" '+classChecked+'><label class="custom-control-label" for="'+showonWebsiteId+'"></label></div><br/><a onclick="EditAnnouncement(' + item.announcementID + ')" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit text-white"></i></a>&nbsp;';
					//html += '<a onclick="EditAddress(' + item.id + ')" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit text-white"></i></a>&nbsp;';
					html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>';

					html += ' </td>';
					html += '</tr>';

				});
				$('.customerOrders').html(html);
			} else {
				alert(false);
			}
		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});
}




function loadListList() {



	ApendOption();


	$.ajax({
		url: "/UserLogin/ListAddres",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {

			console.log(result)
			var html = '';
			if (result.code === true) {


				if (result.jsonText.length >= 3) {

					$("#divAddAddess").attr("hidden");
					$("#divAddAddess").hide();
				}
				else {
					$("#divAddAddess").removeAttr("hidden");
					$("#divAddAddess").show();
				}

				$.each(result.jsonText, function (key, item) {

					$("#addressType option[value='" + item.addreessType + "']").each(function () {
						$(this).remove();
					});



					html += '<tr>';
					html += '<td class="text-wrap" >' + item.country + '</td>';
					html += '<td class="text-wrap" >' + item.town + '</td>';
					html += '<td class="text-wrap" >' + item.state + '</td>';
					html += '<td class="text-wrap" >' + item.addreessType + '</td>';
					html += '<td class="text-wrap" >' + item.addresss + '</td>';
					html += '<td class="text-wrap" >' + item.postalCode + '</td>';
					html += '<td class="text-wrap" >' + item.contactNo + '</td>';
					html += ' <td>';

					//  html += '<div class="custom-control custom-switch lg"><input type="checkbox" class="custom-control-input control" name="ShowOnWebsite" id="'+showonWebsiteId+'" onchange="tableCheckBoxChange('+item.announcementID+')" data-val="true"  data-val-required="Do you want to show on Website" value="'+ item.showOnWebsite+'" '+classChecked+'><label class="custom-control-label" for="'+showonWebsiteId+'"></label></div><br/><a onclick="EditAnnouncement(' + item.announcementID + ')" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit text-white"></i></a>&nbsp;';
					html += '<a onclick="EditAddress(' + item.id + ')" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit text-white"></i></a>&nbsp;';
					html += '<a onclick="DeleteAddress(' + item.id + ')" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash text-white"></i></a>';

					html += ' </td>';
					html += '</tr>';

				});
				$('.addresList').html(html);
			} else {
				alert(false);
			}
		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});
}


function DeleteAddress(id) {

	var ans = confirm("Are you sure you want to delete this Addres?");
	if (ans) {
		$.ajax({
			url: "/UserLogin/DeleteAddres/" + id,
			type: "POST",

			contentType: "application/json;charset=UTF-8",
			dataType: "json",
			success: function (result) {
				if (result.success) {
					loadListList();
					$(window).scrollTop(0);

				}
				else {



				}


			},
			error: function (errormessage) {
				// alert(errormessage.responseText);
			}
		});
	}
}


function AddAddress() {



	var Country = document.getElementById('country').value;
	var PostalCode = document.getElementById('postalCode').value;
	var ContactNo = document.getElementById('contactNo').value;
	var Town = document.getElementById('town').value;
	var State = document.getElementById('state').value;
	var AddreessType = document.getElementById('addressType').value;
	var Address = document.getElementById('address').value;



	var addre = new FormData();
	addre.append("Country", Country);
	addre.append("PostalCode", PostalCode);
	addre.append("ContactNo", ContactNo);
	addre.append("Town", Town);
	addre.append("State", State);
	addre.append("AddreessType", AddreessType);
	addre.append("Addresss", Address);




	$.ajax({
		type: "POST",
		url: "/UserLogin/AddAddress1/",

		data: addre,
		processData: false,  // tell jQuery not to process the data
		contentType: false,  // tell jQuery not to set contentType




		success: function (result) {
			if (result.code) {
				loadListList();


				$('#staticBackdropp').modal('hide');

			}
			else {




			}
		},
		error: function (errormessage) {

			alert(errormessage.responseText)

		}
	});


}
function UpdateAddress() {



	var Country = document.getElementById('ucountry').value;
	var PostalCode = document.getElementById('upostalCode').value;
	var ContactNo = document.getElementById('ucontactNo').value;
	var Town = document.getElementById('utown').value;
	var State = document.getElementById('ustate').value;
	var AddreessType = document.getElementById('uaddressType').value;
	var Address = document.getElementById('uaddress').value;
	var AddressId = document.getElementById('addressId').value;



	var addre = new FormData();
	addre.append("Id", AddressId);
	addre.append("Country", Country);
	addre.append("PostalCode", PostalCode);
	addre.append("ContactNo", ContactNo);
	addre.append("Town", Town);
	addre.append("State", State);
	addre.append("AddreessType", AddreessType);
	addre.append("Addresss", Address);




	$.ajax({
		type: "POST",
		url: "/UserLogin/UpdateAddress/",

		data: addre,
		processData: false,  // tell jQuery not to process the data
		contentType: false,  // tell jQuery not to set contentType




		success: function (result) {
			if (result.code) {
				loadListList();


				$('#EditAddressModel').modal('hide');

			}
			else {




			}
		},
		error: function (errormessage) {

			alert(errormessage.responseText)

		}
	});


}
function ApendOption() {
	$('#addressType').append('<option value="Default">Default</option>');
	$('#addressType').append('<option value="Home">Home</option>');
	$('#addressType').append('<option value="Office">Office</option>');

	$("#addressType option").each(function () {
		$(this).siblings('[value="' + this.value + '"]').remove();
	});
}

function EditAddress(id) {
	$.ajax({
		type: "Get",
		url: '@Url.Action("EditUserAddress", "UserLogin")',
		data: { id: id },
		success: function (data) {
			console.log("asd");
			console.log(data);

			$('#EditAddressPreview').html(data);
			$('#EditAddressModel').modal('show');

		}
	})
}


function RejectReservation(id) {

	var ans = confirm("Are you sure you want to Reject this Reservation?");
	if (ans) {
		$.ajax({
			url: "/Clients/RejectReservation/" + id,
			type: "POST",

			contentType: "application/json;charset=UTF-8",
			dataType: "json",
			success: function (result) {
				if (result.success) {
					loadAllReservations();



				}
				else {



				}


			},
			error: function (errormessage) {
				// alert(errormessage.responseText);
			}
		});
	}
}




function LoadInvoiceData(id) {



	$.ajax({
		method: "Get",
		url: "/home/CustomerInvoice/" + id,
		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.code === -1) {

				window.location.href = "/";
			}

			if (result.success) {

				$("#spnInvoiceNo").html(result.invoiceMain.orderNo);
				$("#spnInvoiceDate").html(result.invoiceMain.systime2);
				$("#spnInvoiceStatus").html(result.invoiceMain.orderStatus);
				$("#spnInvoiceUser").html(' ' + result.invoiceMain.userName);
				if (result.invoiceMain.deliveryType === "delivery") {
					$("#spnInvoiceAddress").html('Location: ' + result.invoiceMain.deliveryLocation);
					$("#spnInvoiceTown").html('Eir Code: ' + result.invoiceMain.postalCode);
				}
				$("#spnInvoiceEmail").html(': ' + result.invoiceMain.email);
				$("#spnInvoicePhone").html(': ' + result.invoiceMain.contactNo);
				$("#spnInvoiceSubTotal").html('€ ' + result.invoiceMain.totalAmount);
				$("#spnInvoiceDelivery").html('€ ' + result.invoiceMain.deliveryCharges);
				$("#spnInvoiceGrandTotal").html('€ ' + result.invoiceMain.grandTotal);
				$("#spnInvoicePayment").html(':  ' + result.invoiceMain.paymentType);
				$("#spnInvoiceType").html(': ' + result.invoiceMain.deliveryType);

				var html = '';

				$.each(result.invoiceDetail, function (key, item) {


					html += '<tr class="cart_item">';
					html += '<td>' + (key + 1) + '</td>';
					html += ' <td data-title="Product">';

					html += '<img width="91" height="91" src="' + item.itemImage + '" alt="Image">';
					html += '</td>';
					html += '<td data-title="Name">' + item.itemName + '<br/>' + item.subName + '<br/>' + item.freeName + '</td>';
					html += '<td data-title="Name">' + item.price + '</td>';
					html += '<td data-title="Name">' + item.quantity + '</td>';
					html += '<td data-title="Name">' + item.totalPrice + '</td>';


					html += '</tr>';


				});

				$("#tblInvoiceTBody").html(html);
				$("#ShowCustomerInvoice").modal('show');
			}

		},
		error: function (errorText) {

		}
	});
}