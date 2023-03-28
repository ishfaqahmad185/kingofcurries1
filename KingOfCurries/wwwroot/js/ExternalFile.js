



$(function () {
    $("#btnRegistrationClick").on("click", function (e) {
        var form = $("#AddRegistrationForm")[0];
        var isValid = form.checkValidity();
        if (!isValid) {
            e.preventDefault();
            e.stopPropagation();
        }
        else {
            
            addRegistraionForm();
        }
        form.classList.add('was-validated');
        return false; // For testing only to stay on this page
    });
});

$(function () {
    $("#btnNewsLetter").on("click", function (e) {
        var form = $(".newsletter-form")[0];
        var isValid = form.checkValidity();
        if (!isValid) {
            e.preventDefault();
            e.stopPropagation();
         
        }
        else {
            var emailAddress = $('#txtNewsLetter').val();
            AddNewsLetter(emailAddress, "News Letter");
           
        }
        form.classList.add('was-validated');
        return false; // For testing only to stay on this page
    });
});

$(function () {
    $("#btnFoodTest").on("click", function (e) {
        var form = $("#formFoodTest")[0];
        var isValid = form.checkValidity();
        if (!isValid) {
            e.preventDefault();
            e.stopPropagation();
         
        }
        else {
            var emailAddress = $('#txtFoodTest').val();
            AddNewsLetter(emailAddress, "Food Test");
           
        }
        form.classList.add('was-validated');
        return false; // For testing only to stay on this page
    });
});




function AddNewsLetter(email,type) {

    var newLetters = new FormData();
    newLetters.append("emailAddress", email);
    newLetters.append("subType", type);
   

    $.ajax({
        type: "Post",
        url: "/Subscription/Subscribed/",
        data: newLetters,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (response) {
            if (response.isSuccess) {



                swal("success!", response.message, "success");

            }
            else {


                swal("Error!", response.message, "error");
            }

        },
        error: function (errormessage) {
          swal("Error!", errormessage.responseText, "error");
            
        }

    });
}




$(function () {
   
    $("#btnSignInSubmit").on("click", function (e) {
        var form = $("#signInForm")[0];
        var isValid = form.checkValidity();
        if (!isValid) {
            e.preventDefault();
            e.stopPropagation();
        }
        else {
            var UserName = $("#singinEmail").val();
            var Password = $("#singinPassword").val();
            
            const KeepLoginIn = $('#rememberMe').val();
        
            addSigninForm(UserName, Password,KeepLoginIn);
        }
        form.classList.add('was-validated');
        return false; // For testing only to stay on this page
    });

});


$("#togglePassword").on("click", function () {
    const password = document.querySelector("#singinPassword");
    const type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);
    // toggle the eye icon
    this.classList.toggle('fa-eye');
    this.classList.toggle('fa-eye-slash');


});

function OpenMyAccount() {
    
    window.location.href = "/MyAccount";
}

function LogOut() {
    window.location.href = "/Logout/";
}
function addRegistraionForm() {
   
    var UserName = $("#registerName").val();
    var PhoneNumber = $("#phoneNumber").val();

    var registerEmail = $("#registerEmail").val();
    var registerPassword = $("#registerPassword").val();
    var customers = new FormData();
    customers.append("CustomerName", UserName);
    customers.append("CustomerPhone", PhoneNumber);
    customers.append("CustomerEmailAddress", registerEmail);
    customers.append("CustomerPassword", registerPassword);

    $.ajax({
        type: "Post",
        url: "/Customer/Registration/",
        data: customers,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (response) {
            if (response.success) {

             
                

                window.location.reload();

            }
            else {
              

                $('#errorRegistrationMessage').removeAttr('hidden');
                $(".alertmsg").attr("style", "display block !important");
                $('#errorRegistrationText').html(response.responseText);
                $('#errorRegistrationMessage').fadeIn();

                $('#errorRegistrationMessage').fadeTo(5000, 500).slideUp(500,
                    function () {
                        $("#errorRegistrationMessage").slideUp(500);


                    });
                $('#errorRegistrationMessage').attr('hidden');
            }

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
}

function addSigninForm(UserName, Password, KeepLoginIn) {




    var formData = new FormData();
    formData.append("UserName", UserName);
    formData.append("Password", Password);
    formData.append("KeepLoginIn", KeepLoginIn);
    
    $.ajax({
        type: "Post",
        url: "/login/",
        data: formData,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (response) {
            if (response.success) {
                window.location.reload();
            }
            else {

              
                $('#errorSignInMessage').removeAttr('hidden');
                $(".alertmsg").attr("style", "display block !important");
                $('#errorSignInText').html(response.responseText);
                $('#errorSignInMessage').fadeIn();

                $('#errorSignInMessage').fadeTo(5000, 500).slideUp(500,
                    function () {
                        $("#errorSignInMessage").slideUp(500);


                    });
                $('#errorSignInMessage').attr('hidden');
            }

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
}
function OpenLoginPopUp(id) {

    
        $(".register").addClass("active");
        $(".signin").removeClass("active");
        $("#register").addClass("active in");
        $("#signin").removeClass("active in");

   
    $("#exampleModalCenter").modal("show");
}

function WishListAdd(id) {

    var text = $('#wishlist-' + id).attr('title');

    if (text == 'Add to wishlist') {
        $('#wishlist-' + id).attr("title", "Remove From WishList");
        $('#wishlist-' + id).html("<i class='d-icon-heart-full'></i>");
        var formData = new FormData();
        formData.append("id", id);

        $.ajax({
            type: "POST",
            url: "/Website/AddWishList/",

            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    swal("Success!", "Item added to your Favourite list", "success");
                }
                else {
                    swal("Error!", result.responseText, "error");
                }


            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            }
        });
    }
    else {
        $('#wishlist-' + id).attr("title", "Add to wishlist");
        $('#wishlist-' + id).html("<i class='d-icon-heart'></i>");
        var formData = new FormData();
        formData.append("id", id);
        $.ajax({
            type: "POST",
            url: "/Website/RemoveWishList/",

            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    swal("Success!", "Item Successfully Removed from your Favourite list", "success");
                }
                else {
                    swal("Error!", result.responseText, "error");
                }


            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            }
        });
    }



}

