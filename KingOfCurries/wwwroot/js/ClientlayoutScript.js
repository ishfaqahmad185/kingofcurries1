
function SetTableOrderHeader() {
    var html = '';
 
    html += '<tr>';
    html += '<th>Actions</th>';
    html += '<th >Order Info</th>';
    html += '<th>Name</th>';
 
  
    html += '<th>Type</th>';
    html += '<th>Status</th>';
    html += '<th>Notes</th>';
   
    html += ' </tr>';

    $(".OrderTableHeader").html(html);
        
   
}

function SetTableReservationHeader() {
    var html = '';

    html += '<tr>';

    html += '<th>Action</th>';
    html += '<th >Customer Info</th>';
   


    html += '<th>Reservation Info</th>';

  
    html += '<th>Remarks</th>';

    html += ' </tr>';

    $(".tblReservationData").html(html);


}


function SetOrderAllTableData(item) {

    var html = ''; var OrderText = ''; var StatusText = '';
    if (item.deliveryType === 'delivery') {
        OrderText = 'Delivery Time';

    }
    else {
        OrderText = 'Collection Time';
    }

    if (item.orderStatus === 'Order Delivered' && item.deliveryType === 'collection') {
        StatusText = 'Order Collected';
    } else {
        StatusText = item.orderStatus;
    }
    html += '<td class="text-left" >Order Time <br />' + item.systime + '<br /> <br /> ' + OrderText + '<br />' + item.deliveryTime + '' + '</td>';
    html += '<td class="text-wrap text-left" >' + item.userName + '</br>' + item.contactNo + '</td>';
    // html += '<td class="text-wrap text-center" >' + item.contactNo + '</td>';

    
    html += '<td class="text-wrap text-left" >' + item.deliveryType + '<br/>' + item.paymentType + '<br /> <br /> Total<br />' + item.grandTotal + '</td>';
    html += '<td class="text-wrap text-left" >' + StatusText + '</td>';
    html += '<td class="text-wrap text-left" >' + item.remarks + '</td>';
    

    

    return html;
}


function loadOrdersMainAll() {
    $.ajax({
        url: "/Clients/OrdersMainAll",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '', htmlCounts = '';
            htmlCounts = result.jsonText.length;
            $('#mainId').html(htmlCounts);
            if (result.code === true) {
                
                $.each(result.jsonText, function (key, item) {



                    html += '<tr>';
                    html += ' <td>';
                    html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn  shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>&nbsp;';

                    html += '<a onclick="PrintReceipt(' + item.orderId + ',' + 1 + ')" class="btn  shadow btn-xs sharp btn-warning btn-sm" data-toggle="tooltip" data-placement="bottom" title="Print"><i class="fa fa-print text-white"></i></a>&nbsp;';
                    if (item.statusId == 1) {

                        html += '<a onclick="AllTodayOrderJson(' + item.orderId + ')" class="btn shadow btn-xs sharp btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Accept"><i class="fa fa-check-double text-white"></i></a>&nbsp;';
                        // html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 7 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Reject"><i class="fa fa-trash text-white"></i></a>';

                    } else if (item.statusId == 7) {

                    }
                    // else if (item.statusId == 2) {
                    //    html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 3 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Preparing"><i class="fa fa-utensils text-white"></i></a>';

                    //}

                    html += ' </td>';
                    html += SetOrderAllTableData(item);
                  
                   

                    html += '</tr>';
                });
                $('.ordersMainAll').html(html);


            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}


function AllTodayOrderJson(data) {

    $.ajax({
        url: "/Clients/GetOrderById/" + data,
        type: "POST",

        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.success) {

                BindNewOrderData(result.responseText);
            }
            else {



            }


        },
        error: function (errormessage) {
            // alert(errormessage.responseText);
        }
    });
}

function loadOrdersPreparing() {
    $.ajax({
        url: "/Clients/OrdersPreparing",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '', htmlCounts = '';
            htmlCounts = result.jsonText.length;
            $('#prepareId').html(htmlCounts);
            if (result.code === true) {

                $.each(result.jsonText, function (key, item) {

                    html += '<tr>';
                    html += ' <td>';
                    html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn  shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>&nbsp;';

                    html += '<a onclick="PrintReceipt(' + item.orderId + ',' + 1 + ')" class="btn  shadow btn-xs sharp btn-warning btn-sm" data-toggle="tooltip" data-placement="bottom" title="Print"><i class="fa fa-print text-white"></i></a>&nbsp;';
                    html += '<a onclick="IncreaseOrderTime(' + item.orderId + ')" class="btn  shadow btn-xs sharp btn-success btn-sm" data-toggle="tooltip" data-placement="bottom" title="Increase"><i class="fa fa-clock text-white"></i></a>&nbsp;';

                    if (item.deliveryType == "delivery") {
                        html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 4 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="delivery"><i class="fa fa-truck text-white"></i></a>';

                    } else {
                        html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 8 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Collections"><i class="fa fa-handshake text-white"></i></a>';

                    }



                    html += ' </td>';
                    html += SetOrderAllTableData(item);
                   
              

                    html += '</tr>';
                });
                $('.ordersMainAllPreparing').html(html);
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}

function loadOrdersDone() {
    $.ajax({
        url: "/Clients/OrdersDone",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '', htmlCounts = '';
            htmlCounts = result.jsonText.length;
            $('#doneId').html(htmlCounts);


            if (result.code === true) {

                $.each(result.jsonText, function (key, item) {

                    html += '<tr>';
                    html += '<td class="text-wrap text-center" >'
                    html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn  shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>&nbsp;';

                    html += '<a onclick="PrintReceipt(' + item.orderId + ',' + 1 + ')" class="btn  shadow btn-xs sharp btn-warning btn-sm" data-toggle="tooltip" data-placement="bottom" title="Print"><i class="fa fa-print text-white"></i></a>&nbsp;';
                    html += '</td>';
                    html += SetOrderAllTableData(item);

                   

                    html += '</tr>';
                });
                $('.ordersDone').html(html);

                console.log("---------------------------------------");
                console.log(result);
                console.log("---------------------------------------");

                $('#h4TotalCollectionPayment').html(result.summary.totalCollectionPayment);
                $('#h4TotalCollectionOrders').html(result.summary.totalCollectionOrders);
                $('#h4TotalDeliveryPayment').html(result.summary.totalDeliveryPayment);
                $('#h4TotalDeliveryOrders').html(result.summary.totalDeliveryOrders);
                $('#h4CashPayment').html(result.summary.cashPayment);
                $('#h4CardPayment').html(result.summary.cardPayment);
                $('#h4TotalPayment').html(result.summary.totalPayment);
                $('#h4DeliveryCharges').html(result.summary.deliveryCharges);

            } else {
                
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function loadOrdersCollection() {
    $.ajax({
        url: "/Clients/OrdersCollection",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '', htmlCounts = '';
            htmlCounts = result.jsonText.length;
            $('#collectionId').html(htmlCounts);
            if (result.code === true) {

                $.each(result.jsonText, function (key, item) {

                    html += '<tr>';
                    html += ' <td>';
                    html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn  shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>&nbsp;';

                    html += '<a onclick="PrintReceipt(' + item.orderId + ',' + 1 + ')" class="btn  shadow btn-xs sharp btn-warning btn-sm" data-toggle="tooltip" data-placement="bottom" title="Print"><i class="fa fa-print text-white"></i></a>&nbsp;';
                    html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 6 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Order Delivered"><i class="fa fa-check-double text-white"></i></a>';





                    html += ' </td>';
                    html += SetOrderAllTableData(item);
                  

                    html += '</tr>';
                });
                $('.ordersMainAllCollection').html(html);
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}
function loadOrdersDelievring() {
    $.ajax({
        url: "/Clients/OrdersDelivering",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '', htmlCounts = '';
            htmlCounts = result.jsonText.length;
            $('#deliveryId').html(htmlCounts);
            if (result.code === true) {


                console.log(result.jsonText);


                $.each(result.jsonText, function (key, item) {




                    html += '<tr>';
                    html += ' <td>';
                    html += '<a onclick="LoadInvoiceData(' + "'" + item.orderNo + "'" + ')" class="btn  shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Details"><i class="fa fa-receipt text-white"></i></a>&nbsp;';

                    html += '<a onclick="PrintReceipt(' + item.orderId + ',' + 1 + ')" class="btn  shadow btn-xs sharp btn-warning btn-sm" data-toggle="tooltip" data-placement="bottom" title="Print"><i class="fa fa-print text-white"></i></a>&nbsp;';
                    if (item.statusId != 5) {
                        html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 5 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Out for Delivery"><i class=" fa  fa-truck text-white"></i></a>';

                    } else {
                        html += '<a onclick="ConfirmOrder(' + item.orderId + ',' + 6 + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Order Delivered"><i class=" fa fa-check-double text-white"></i></a>';

                    }

                    html += ' </td>';
                    html += SetOrderAllTableData(item);
                   

                    html += '</tr>';
                });
                $('.ordersMainAllDelivering').html(html);
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}


function ConfirmOrder(id, status) {

    var ans = confirm("Do you want to Confirm this  Order?");
    if (ans) {
        $.ajax({
            url: "/ConfirmOrderStatusChange/" + id + "/" + status,
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    loadOrdersMainAll();
                    loadOrdersPreparing();
                    loadOrdersDelievring();
                    loadOrdersCollection();
                    loadOrdersDone();
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
function loadContacts() {
    $.ajax({
        url: "/Clients/ListOfContacts",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '';
            if (result.code === true) {





                $.each(result.jsonText, function (key, item) {



                    html += '<tr>';
                    html += '<td class="text-wrap text-center" >' + item.contactUsId + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.name + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.email + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.number + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.subject + '</td>';

                    html += '<td class="text-wrap text-center" ></td>';
                    html += ' <td>';
                    html += '<a onclick="DeleteContactUs(' + item.contactUsId + ')" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash text-white"></i></a>';

                    html += ' </td>';
                    html += '</tr>';
                });
                $('.contactList').html(html);
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}


function loadSubcriptions() {
    $.ajax({
        url: "/Clients/ListOfSubcriptions",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '';
            if (result.code === true) {





                $.each(result.jsonText, function (key, item) {



                    html += '<tr>';
                    html += '<td class="text-wrap text-center" >' + item.subcriptionsId + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.emailAddress + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.date + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.type + '</td>';
                    html += '<td class="text-wrap text-center" >' + item.status + '</td>';

                    html += '<td class="text-wrap text-center" ></td>';
                    html += ' <td>';
                    html += '<a onclick="DeleteSubscriptions(' + item.subcriptionsId + ')" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash text-white"></i></a>';

                    html += ' </td>';
                    html += '</tr>';
                });
                $('.allSubscriptions').html(html);
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}


function DeleteContactUs(id) {

    var ans = confirm("Are you sure you want to delete this  Contact?");
    if (ans) {
        $.ajax({
            url: "/Clients/DeleteContactUs/" + id,
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    loadContacts();



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


function DeleteSubscriptions(id) {

    var ans = confirm("Are you sure you want to delete this   Subcription?");
    if (ans) {
        $.ajax({
            url: "/Clients/DeleteSubcriptions/" + id,
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    loadSubcriptions();



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


//

function loadAllReservations() {
    $.ajax({
        url: "/Clients/ListOfReservations",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
          
            htmlCounts = result.jsonText.length;
            $('#allReservationId').html(htmlCounts);
            if (result.code === true) {
                
                
                $('.allReservations').html(SetReservationData(result.jsonText));
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}

function loadTodayReservations() {
    $.ajax({
        url: "/Clients/ListOfTodayReservations",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '';
            var Status = '';
            htmlCounts = result.jsonText.length;
            $('#todayReservationId').html(htmlCounts);
            if (result.code === true) {
                                                            
                $('.todayReservations').html(SetReservationData(result.jsonText));
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}


function SetReservationData(result) {
    var html = '';
    var Status = '';
    $.each(result, function (key, item) {

        if (item.status === 1) {
            Status = '<span class="badge light badge-warning">Pending</span>';

        }
        else if (item.status === 2) {
            Status = '<span class="badge light badge-success">Confirm</span>';

        }
        else {
            Status = '<span class="badge light badge-danger">Rejected</span>';

        }

        html += '<tr>';
      
        if (item.status == 1) {
            html += ' <td class="text-center">';
            html += Status + '<br/><br/>';
            html += '<a onclick="ConfirmReservation(' + item.reservationId + ')" class="btn shadow btn-xs sharp btn-primary btn-sm" data-toggle="tooltip" data-placement="bottom" title="Confirm"><i class="fa fa-thumbs-up text-white"></i></a>&nbsp;';
            html += '<a onclick="RejectReservation(' + item.reservationId + ')" class="btn shadow btn-xs sharp btn-info btn-sm" data-toggle="tooltip" data-placement="bottom" title="Reject"><i class="fa fa-thumbs-down text-white"></i></a>&nbsp;';
            html += '<a onclick="DeleteReservation(' + item.reservationId + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash text-white"></i></a>';

            html += ' </td>';
        }
        else {
            html += ' <td class="text-center">';
            html += Status + '<br/><br/>';

            html += '<a onclick="DeleteReservation(' + item.reservationId + ')" class="btn  shadow btn-xs sharp btn-danger btn-sm" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash text-white"></i></a>';

            html += ' </td>';
        }
        html += '<td class="text-wrap" >' + item.userName + '<br/>' + item.phoneNo + '<br/>' + item.email + '</td>';
        html += '<td class="text-wrap" >Created on <br/>' + item.sysDateCreatedAt + '<br/><br/>Reservation for<br/>' + item.sysDate + '<br />' + item.reservationTime + '<br /><br />Total Guest : ' + item.totalGuest  +'</td>';
     
      

       

        html += '<td class="text-wrap" >' + item.message + '</td>';
      
        

        html += '</tr>';

    });
   

    return html;
}

function loadNewReservations() {
    $.ajax({
        url: "/Clients/ListOfNewReservations",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result)
            var html = '';
            var Status = '';
            htmlCounts = result.jsonText.length;
            $('#newReservationId').html(htmlCounts);
            if (result.code === true) {
                        


              
    $('.newReservations').html(SetReservationData(result.jsonText));
            } else {
                alert(false);
            }
        },
        error: function (errormessage) {
            //alert(errormessage.responseText);
        }
    });
}



function DeleteReservation(id) {

    var ans = confirm("Are you sure you want to delete this Reservation?");
    if (ans) {
        $.ajax({
            url: "/Clients/DeleteReservation/" + id,
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    loadAllReservations();
                    loadTodayReservations();



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

function ConfirmReservation(id) {

    var ans = confirm("Are you sure you want to Confirm this Reservation?");
    if (ans) {
        $.ajax({
            url: "/Clients/ConfirmReservation/" + id,
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    loadAllReservations();
                    loadTodayReservations();



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
                    loadTodayReservations();



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

$(function () {

    setInterval(function () {

        $.ajax({
            url: "/Clients/GetAllNewOrders/",
            type: "POST",

            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.success) {

                    if (result.newOrder) {
                        var x = document.getElementById("myAudio");

                        x.play();

                        if ($("#NewOrderModel").hasClass("show")) {

                        } else {
                            BindNewOrderData(result.orderMain);
                            loadOrdersMainAll();

                        }


                    }




                }
                else {



                }


            },
            error: function (errormessage) {
                // alert(errormessage.responseText);
            }
        });

    }, 20000);

});


function IncreaseOrderTime(orderId) {



    var modal = $("#ChangePreparationDuration");

    modal.find('.modal-body #hdnOrderId').val(orderId);

    $("#ChangePreparationDuration").modal('show');
}


function BindNewOrderData(data) {

    console.log(data);

    var modalData = $("#NewOrderModel");

    modalData.find('.modal-header #spnUserName').html(data.userName);
    modalData.find('.modal-body #hdnNewOrderId').val(data.orderId);
    modalData.find('.modal-body #txtNewInvoice').val(data.orderNo);
    modalData.find('.modal-body #txtTotalAmount').val('€ ' + data.grandTotal);
    modalData.find('.modal-body #txtDeliveryType').val(data.deliveryType);
    modalData.find('.modal-body #txtpaymentType').val(data.paymentType);
    modalData.find('.modal-body #txtNewOrderDate').val(data.systime);
    modalData.find('.modal-body #txtNewOrderNotes').val(data.remarks);
    modalData.find('.modal-body #txtNewOrderDuration').val(data.deliveryTime);
    if (data.deliveryType == "delivery") {
        modalData.find('.modal-body #lblDeliveryTime').text("Delivery Time");
    } else {
        modalData.find('.modal-body #lblDeliveryTime').text("Collection Time");

    }


    $("#NewOrderModel").modal('show');

}


$(function () {
    $("#btnNewOrderAccept").on("click", function (e) {

        NewOrderConfirm(3, 0, true);
        CallAllFunctions();
    });
});


$(function () {
    $("#btnNewOrderReject").on("click", function (e) {
        var ans = confirm("Are you sure you want to reject this Order");
        if (ans) {

            NewOrderConfirm(7, 0, false);
            CallAllFunctions();

        }


    });
});

function NewOrderConfirm(statusId, duration,isPrint) {
    ShowLoader();
    $("#NewOrderModel").modal('hide');
    var orderId = $("#hdnNewOrderId").val();
    var duration = $('#txtNewOrderDuration').val();

    var formData = new FormData();
    formData.append("orderId", orderId);
    formData.append("statusId", statusId);
    formData.append("duration", duration);

    $.ajax({

        method: "Post",
        url: "/Clients/NewOrderConfirmation",

        data: formData,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (result) {

            if (result.success) {
                CallAllFunctions();
                HideLoader();

                if (isPrint === true) {
                    PrintReceipt(orderId, 2);
                }
              



            } else {
                $(".alert-new-order").html(result.responseText);
                $(".alert-new-order").addClass("alert-danger");
                $(".alert-new-order").removeAttr("hidden");
                $(".alert-new-order").show();
            }

        },

        error: function (errorResponse) {


            $(".alert-new-order").html(errorResponse.Message);
            $(".alert-new-order").addClass("alert-danger");
        },

    })

}

$(function () {

    $("#btnPlusTime").click(function () {


        ChangeTimeData(2);

    });

    $("#btnMinusTime").click(function () {

        ChangeTimeData(1);


    });

    function ChangeTimeData(type) {
        var dataId = $("#txtNewOrderDuration").val();
        var formData = new FormData();
        formData.append("type", type);
        formData.append("dataId", dataId);
        $.ajax({
            url: "/Clients/ChangeTimeData/",
            type: "POST",
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,

            success: function (result) {
                if (result.success) {
                    $("#txtNewOrderDuration").val(result.responseText);

                }
                else {

                    console.log(result.responseText);


                }


            },
            error: function (errormessage) {
                // alert(errormessage.responseText);
            }
        });

    }
});


function PrintReceipt(id, type) {
  
        window.open("/PrintPreview/" + id,  "_blank","width=500");
       
    

}

function ShowLoader() {
    $('.spinner').css('display', 'block');
}

function HideLoader() {
    $('.spinner').css('display', 'none');
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
                $("#spnInvoiceNotes").html(result.invoiceMain.remarks);
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
                    html += '<td data-title="Name">€' + item.price + '</td>';
                    html += '<td data-title="Name">' + item.quantity + '</td>';
                    html += '<td data-title="Name">€' + item.totalPrice + '</td>';


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






